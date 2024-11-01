using System.Linq;
using framework.events;
using Godot;
using framework.utils;
using Godot.Collections;

public partial class MapRoom : Area2D {
    private static readonly Dictionary<Room.EType, Array> RoomTypeToIconInfos = new() {
        {
            Room.EType.NotAssigned,
            new Array { new Texture() /* null */, Vector2.One }
        }, {
            Room.EType.Monster,
            new Array { SimpleLoader.LoadTexture("res://art/tile_0103"), Vector2.One }
        }, {
            Room.EType.Treasure,
            new Array { SimpleLoader.LoadTexture("res://art/tile_0089"), Vector2.One }
        }, {
            Room.EType.Campfire,
            new Array { SimpleLoader.LoadTexture("res://art/player_heart"), Vector2.One * 0.6f }
        }, {
            Room.EType.Shop,
            new Array { SimpleLoader.LoadTexture("res://art/gold"), Vector2.One * 0.6f }
        }, {
            Room.EType.Boss,
            new Array { SimpleLoader.LoadTexture("res://art/tile_0105"), Vector2.One * 1.25f }
        },
    };

    #region onready

    private Sprite2D _sprite2D;
    private Line2D _line2D;
    private AnimationPlayer _animationPlayer;

    #endregion

    private bool _available;

    public bool Available {
        get => _available;
        set {
            _available = value;
            if (_available) {
                _animationPlayer.Play("highlight");
            }
            else if (!Room.Selected) {
                _animationPlayer.Play("RESET");
            }
        }
    }

    private Room _room;
    public Room Room {
        get => _room;
        set {
            _room = value;
            Position = _room.Position;
            _line2D.RotationDegrees = GD.RandRange(0, 360);
            _sprite2D.Texture = (Texture2D)RoomTypeToIconInfos[_room.Type][0];
            _sprite2D.Scale = (Vector2)RoomTypeToIconInfos[_room.Type][1];
        }
    }

    public override void _Ready() {
        _sprite2D = GetNode<Sprite2D>("Visuals/Sprite2D");
        _line2D = GetNode<Line2D>("Visuals/Line2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        InputEvent += OnInputEvent;
    }

    public void ShowSelected() {
        _line2D.Modulate = Colors.White;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx) {
        if (!_available || !@event.IsActionPressed(InputKey.LeftMouse)) {
            return;
        }

        _room.Selected = true;
        _animationPlayer.Play("select");
        
        // 点击房间的那一刻就禁用地图滚动
        EventDispatcher.TriggerEvent(Map.Event.SetMapScrollEnabled, false);
    }

    // Called by the AnimationPlayer when the
    // 'select' animation finishes.
    private void OnMapRoomSelected() {
        EventDispatcher.TriggerEvent(Event.Selected, Room);
    }
}

public partial class MapRoom {
    public static class Event {
        /// <summary>
        /// 选中事件
        /// 参数1：Room
        /// </summary>
        public const string Selected = "MapRoom.Event.Selected";
    }
}