using Godot;
using Godot.Collections;

public partial class CrabMegaBlockAction : EnemyAction {
    [Export] public int Block = 15;
    [Export] public int HPThreshold = 6;

    private bool _alreadyUsed;

    public override bool IsPerformable() {
        if (Enemy == null || _alreadyUsed) {
            return false;
        }
        
        return Enemy.Stats.Health <= HPThreshold;
    }

    public override void PerformAction() {
        _alreadyUsed = true;
        BlockEffect blockEffect = new();
        blockEffect.Amount = Block;
        blockEffect.Sound = Sound;
        blockEffect.Execute(new Array<Node2D> { Enemy });
        
        GetTree().CreateTimer(0.6f, false).Timeout += EnemyActionCompleted;
    }
}