using framework.extension;
using Godot;
using Godot.Collections;

public partial class ReinforcedArmor : Relic {
	[Export] private int _blockBonus = 3;

	public override void ActivateRelic(RelicUI owner) {
		var player = owner.GetTree().GetNodesInGroup<Node2D>("player");	// easier to pass player to blockEffect
		BlockEffect blockEffect = new();
		blockEffect.Amount = _blockBonus;
		blockEffect.Execute(player);
		owner.Flash();
	}
}