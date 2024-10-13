using Godot;
using Godot.Collections;

public partial class CrabBlockAction : EnemyAction {
    [Export] public int Block = 6;

    public override void PerformAction() {
        BlockEffect blockEffect = new();
        blockEffect.Amount = Block;
        blockEffect.Sound = Sound;
        blockEffect.Execute(new Array<Node2D> { Enemy });

        GetTree().CreateTimer(0.6f, false).Timeout += EnemyActionCompleted;
    }
}