using Godot;

[GlobalClass]
public partial class EnemyStats : Stats {
    [Export] public PackedScene AI { get; private set; }

    public new EnemyStats CreateInstance() {
        return (EnemyStats)base.CreateInstance();
    }
}