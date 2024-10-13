using Godot;
using Godot.Collections;

public partial class BatBlockAction : EnemyAction {
	[Export] public int Block = 4;
	
	public override void PerformAction() {
		BlockEffect blockEffect = new();
		blockEffect.Amount = Block;
		blockEffect.Sound = Sound;
		blockEffect.Execute(new Array<Node2D> { Enemy });

		GetTree().CreateTimer(0.6, false).Timeout += EnemyActionCompleted;
	}
}
