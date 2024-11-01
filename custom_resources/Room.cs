using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Room : Resource {
    public enum EType {
        NotAssigned,
        Monster,
        Treasure,
        Campfire,
        Shop,
        Boss
    };

    #region export

    [Export] public EType Type { get; set; }
    [Export] public int Row { get; set; }
    [Export] public int Column { get; set; }
    [Export] public Vector2 Position { get; set; }
    [Export] public Array<Room> NextRooms { get; set; }
    [Export] public bool Selected { get; set; }
    
    /* This is only used by the Monster and Boss types */
    [Export] public BattleStats BattleStats { get; set; }

    #endregion

    public override string ToString() {
        return $"{Column} ({Type.ToString()[0]})";
    }
}