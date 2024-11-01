using System.Linq;
using framework.debug;
using framework.extension;
using Godot;
using Godot.Collections;

public partial class MapGenerator : Node {
    private FinchLogger _logger;

    #region const

    public const int XDist = 30; // 列间距
    public const int YDist = 25; // 行间距
    private const int PlacementRandomness = 5; // 地图点会有5像素的随机偏移
    public const int Floors = 15; // 15行
    public const int MapWidth = 7; // 7列
    private const int Paths = 6; // 最大起点数量

    // 各个房间的随机权重
    private const float MonsterRoomWeight = 10f;
    private const float ShopRoomWeight = 2.5f;
    private const float CampfireRoomWeight = 4.0f;

    [Export] public BattleStatsPool BattleStatsPool;

    #endregion

    private Dictionary<Room.EType, float> _roomTypeToWeight = new() {
        { Room.EType.Monster, 0f },
        { Room.EType.Campfire, 0f },
        { Room.EType.Shop, 0f },
    };

    private float _randomRomTypeTotalWeight;
    private Array<Array<Room>> _mapData;

    public Room BossRoom => _mapData[Floors - 1][MapWidth / 2];

    public override void _Ready() {
        _logger = new FinchLogger(this);
    }

    public Array<Array<Room>> GenerateMap() {
        _mapData = GenerateInitialGrid();
        // for (int i = 0; i < _mapData.Count; i++) {
        //     _logger.Log($"floor {i}\t:\t{_mapData[i]}");
        // }

        Array<int> startingPoints = GetRandomStartingPoints();
        // _logger.Log(startingPoints.ToString());

        foreach (int col in startingPoints) {
            int currentCol = col;
            for (int row = 0; row < Floors - 1; row++) {
                currentCol = SetupConnection(row, currentCol);
            }
        }

        BattleStatsPool.Setup();

        SetupBossRoom();
        SetupRandomRoomWeights();
        SetupRoomTypes();
        
        // StringBuilder log = new();
        // log.AppendLine();
        // for (int i = 0; i < _mapData.Count; i++) {
        //     log.AppendLine($"floor {i}");
        //     var usedRooms = new Array<Room>(_mapData[i].Where(room => !room.NextRooms.IsNullOrEmpty()));
        //     log.AppendLine(usedRooms.ToString());
        // }
        // _logger.Log(log.ToString());

        return _mapData;
    }

    private Array<Array<Room>> GenerateInitialGrid() {
        Array<Array<Room>> result = new();
        for (int i = 0; i < Floors; i++) {
            Array<Room> floorRooms = new();
            for (int j = 0; j < MapWidth; j++) {
                Room room = new Room();
                Vector2 offset = new Vector2(GD.Randf(), GD.Randf()) * PlacementRandomness;
                room.Position = new Vector2(j * XDist, i * -YDist) + offset;
                room.Row = i;
                room.Column = j;
                room.NextRooms = new Array<Room>();

                // Boss房间
                if (i == Floors - 1) {
                    // i + 1 是为了让最后的Boss房间在视觉上更突出点
                    // 注意：实际上这里只是修改了Y值，并没有修改Column
                    room.Position = room.Position with { Y = (i + 1) * -YDist };
                }

                floorRooms.Add(room);
            }

            result.Add(floorRooms);
        }

        return result;
    }

    private Array<int> GetRandomStartingPoints() {
        Array<int> yCoords = new();
        int uniquePoints = 0;
        while (uniquePoints < 2) {
            uniquePoints = 0;
            yCoords = new Array<int>();

            for (int i = 0; i < Paths; i++) {
                int startingPoint = GD.RandRange(0, MapWidth - 1);
                if (!yCoords.Contains(startingPoint)) {
                    uniquePoints++;
                }

                yCoords.Add(startingPoint);
            }
        }

        return yCoords;
    }

    private int SetupConnection(int row, int col) {
        Room nextRoom = null;
        Room currentRoom = _mapData[row][col];

        while (nextRoom == null || WouldCrossExistingPath(row, col, nextRoom)) {
            // 随机下一层的列坐标，相对于当前层 (-1, 0, 1)
            int randomCol = Mathf.Clamp(GD.RandRange(col - 1, col + 1), 0, MapWidth - 1);
            nextRoom = _mapData[row + 1][randomCol];
        }
        
        currentRoom.NextRooms.Add(nextRoom);
        return nextRoom.Column;
    }
    
    // 检测是否交叉跨越下一个房间
    private bool WouldCrossExistingPath(int row, int col, Room room) {
        Room leftNeighbor = null;
        Room rightNeighbor = null;
        
        // if col == 0, there's no left neighbor
        if (col > 0) {
            leftNeighbor = _mapData[row][col - 1];
        }
        // if col === MapWidth - 1, there's no right neighbor
        if (col < MapWidth - 1) {
            rightNeighbor = _mapData[row][col + 1];
        }

        if (rightNeighbor != null && room.Column > col) {
            foreach (Room nextRoom in rightNeighbor.NextRooms) {
                if (nextRoom.Column < room.Column) {
                    return true;
                }
            }
        }

        if (leftNeighbor != null && room.Column < col) {
            foreach (Room nextRoom in leftNeighbor.NextRooms) {
                if (nextRoom.Column > room.Column) {
                    return true;
                }
            }
        }

        return false;
    }

    private void SetupBossRoom() {
        for (int col = 0; col < MapWidth; col++) {
            Room currentRoom = _mapData[Floors - 2][col];
            if (!currentRoom.NextRooms.IsNullOrEmpty()) {
                currentRoom.NextRooms = new Array<Room> { BossRoom };
            }
        }
        BossRoom.Type = Room.EType.Boss;
        BossRoom.BattleStats = BattleStatsPool.GetRandomBattleForTier(2);
    }

    private void SetupRandomRoomWeights() {
        _roomTypeToWeight[Room.EType.Monster] = MonsterRoomWeight;
        _roomTypeToWeight[Room.EType.Campfire] = MonsterRoomWeight + CampfireRoomWeight;
        _roomTypeToWeight[Room.EType.Shop] = MonsterRoomWeight + CampfireRoomWeight + ShopRoomWeight;
        _randomRomTypeTotalWeight = _roomTypeToWeight[Room.EType.Shop];
    }

    private void SetupRoomTypes() {
        Dictionary<int, Room.EType> fixedRoomTypes = new() {
            // first floor is always a battle
            { 0, Room.EType.Monster },
            // middle (9th, idx = 8) floor is always a treasure
            { Mathf.CeilToInt(Floors * 0.5f), Room.EType.Treasure },
            // last floor before the boss is always a campfire
            { Floors - 2, Room.EType.Campfire },
        };
        
        // fixed floor rooms types
        foreach (var (idx, type) in fixedRoomTypes) {
            foreach (Room room in _mapData[idx]) {
                if (!room.NextRooms.IsNullOrEmpty()) {
                    room.Type = type;
                    if (type == Room.EType.Monster) {
                        room.BattleStats = BattleStatsPool.GetRandomBattleForTier(0);
                    }
                }
            }
        }
        
        // rest of rooms
        foreach (Array<Room> currentFloor in _mapData) {
            foreach (Room room in currentFloor) {
                foreach (Room nextRoom in room.NextRooms) {
                    if (nextRoom.Type == Room.EType.NotAssigned) {
                        SetRoomRandomly(nextRoom);
                    }
                }
            }
        }
    }

    private void SetRoomRandomly(Room room) {
        bool campfireBelow4 = true;
        bool consecutiveCampfire = true;
        bool consecutiveShop = true;
        bool campfireOn13 = true;   // 因为倒数第二层必然是篝火，所以倒数第三层不能是篝火

        Room.EType typeCandidate = Room.EType.NotAssigned;
        while (campfireBelow4 || consecutiveCampfire || consecutiveShop || campfireOn13) {
            typeCandidate = GetRandomRoomTypeByWeight();
            bool isCampfire = typeCandidate == Room.EType.Campfire;
            bool hasCampfireParent = RoomHasParentOfType(room, Room.EType.Campfire);
            bool isShop = typeCandidate == Room.EType.Shop;
            bool hasShopParent = RoomHasParentOfType(room, Room.EType.Shop);

            campfireBelow4 = isCampfire && room.Row < 3;
            consecutiveCampfire = isCampfire && hasCampfireParent;
            consecutiveShop = isShop && hasShopParent;
            campfireOn13 = isCampfire && room.Row == Floors - 3;
        }

        room.Type = typeCandidate;

        if (typeCandidate == Room.EType.Monster) {
            int tierForMonsterRooms = room.Row > 2 ? 1 : 0;
            room.BattleStats = BattleStatsPool.GetRandomBattleForTier(tierForMonsterRooms);
        }
    }

    private bool RoomHasParentOfType(Room room, Room.EType type) {
        Array<Room> parentRooms = new();

        int[] colOffsets = { -1, 0, 1 };
        foreach (int colOffset in colOffsets) {
            if (room.Row <= 0) {
                // 第一行没有父节点
                return false;
            }

            int parentCol = room.Column + colOffset;
            if (parentCol is >= 0 and <= MapWidth - 1) {
                Room parentCandidate = _mapData[room.Row - 1][parentCol];
                if (parentCandidate.NextRooms.Contains(room)) {
                    parentRooms.Add(parentCandidate);
                }
            }
        }

        return parentRooms.Any(parent => parent.Type == type);
    }

    private Room.EType GetRandomRoomTypeByWeight() {
        double roll = GD.RandRange(0, _randomRomTypeTotalWeight);
        foreach (var (type, weight) in _roomTypeToWeight) {
            if (weight > roll) {
                return type;
            }
        }

        return Room.EType.Monster;
    }
}