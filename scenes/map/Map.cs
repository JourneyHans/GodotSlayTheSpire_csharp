using framework.debug;
using framework.events;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class Map : Node2D {
    private FinchLogger _logger;
    
    private const float ScrollSpeed = 15f;
    private static readonly PackedScene MapRoom = SimpleLoader.LoadPackedScene("res://scenes/map/map_room");
    private static readonly PackedScene MapLine = SimpleLoader.LoadPackedScene("res://scenes/map/map_line");

    #region onready

    private MapGenerator _mapGenerator;
    private Node2D _lines;
    private Node2D _rooms;
    private Node2D _visuals;
    private Camera2D _camera;

    #endregion

    private Array<Array<Room>> _mapData;
    private int _floorsClimbed;
    public Room LastRoom { get; private set; }
    private float _cameraEdgeY;
    
    public override void _Ready() {
        _logger = new FinchLogger(this);
        
        _mapGenerator = GetNode<MapGenerator>("MapGenerator");
        _lines = GetNode<Node2D>("%Lines");
        _rooms = GetNode<Node2D>("%Rooms");
        _visuals = GetNode<Node2D>("Visuals");
        _camera = GetNode<Camera2D>("Camera2D");

        _cameraEdgeY = MapGenerator.YDist * (MapGenerator.Floors - 1);
        
        EventDispatcher.RegEventListener<Room>(global::MapRoom.Event.Selected, OnMapRoomSelected);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Room>(global::MapRoom.Event.Selected, OnMapRoomSelected);
    }

    public void GenerateNewMap() {
        _floorsClimbed = 0;
        _mapData = _mapGenerator.GenerateMap();
        CreateMap();
    }

    private void CreateMap() {
        foreach (Array<Room> floor in _mapData) {
            foreach (Room room in floor) {
                if (!room.NextRooms.IsNullOrEmpty()) {
                    SpawnMap(room);
                }
            }
        }
        
        // Boss Room
        SpawnMap(_mapGenerator.BossRoom);
        
        // center the Visuals Node
        float mapWidthPixels = MapGenerator.XDist * (MapGenerator.MapWidth - 1);
        Vector2 screenSize = GetViewportRect().Size;
        _visuals.Position = new Vector2((screenSize.X - mapWidthPixels) / 2, screenSize.Y / 2);
    }

    public void UnlockFloor(int floor) {
        foreach (MapRoom mapRoom in _rooms.GetChildren()) {
            if (mapRoom.Room.Row == floor) {
                mapRoom.Available = true;
            }
        }
    }

    public void UnlockNextRooms() {
        foreach (MapRoom mapRoom in _rooms.GetChildren()) {
            if (LastRoom.NextRooms.Contains(mapRoom.Room)) {
                mapRoom.Available = true;
            }
        }
    }

    public void ShowMap() {
        Show();
        _camera.Enabled = true;
    }

    public void HideMap() {
        Hide();
        _camera.Enabled = false;
    }

    private void SpawnMap(Room room) {
        MapRoom mapRoom = MapRoom.Instantiate<MapRoom>();
        _rooms.AddChild(mapRoom);
        mapRoom.Room = room;
        ConnectLines(room);
        if (room.Selected && room.Row < _floorsClimbed) {
            mapRoom.ShowSelected();
        }
    }

    private void ConnectLines(Room room) {
        if (room.NextRooms.IsNullOrEmpty()) {
            return;
        }

        foreach (Room nextRoom in room.NextRooms) {
            Line2D mapLine = MapLine.Instantiate<Line2D>();
            mapLine.AddPoint(room.Position);
            mapLine.AddPoint(nextRoom.Position);
            _lines.AddChild(mapLine);
        }
    }

    private void OnMapRoomSelected(Room room) {
        foreach (MapRoom mapRoom in _rooms.GetChildren()) {
            if (mapRoom.Room.Row == room.Row) {
                mapRoom.Available = false;
            }
        }

        LastRoom = room;
        _floorsClimbed++;

        EventDispatcher.TriggerEvent(Event.MapExited, LastRoom);
    }

    public override void _Input(InputEvent @event) {
        if (!@event.IsAction(InputKey.ScrollUp) && !@event.IsAction(InputKey.ScrollDown)) {
            return;
        }

        float yDelta = _camera.Position.Y;
        if (@event.IsAction(InputKey.ScrollUp)) {
            yDelta -= ScrollSpeed;
        }
        else if (@event.IsAction(InputKey.ScrollDown)) {
            yDelta += ScrollSpeed;
        }

        yDelta = Mathf.Clamp(yDelta, -_cameraEdgeY, 0);
        _camera.Position = _camera.Position with { Y = yDelta };
    }
}

public partial class Map {
    public static class Event {
        public const string MapExited = "MapExited";
    }
}