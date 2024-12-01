using Godot;

public partial class HealingPotion : Relic {
	[Export] private int _healingAmount = 6;

	public override void ActivateRelic(RelicUI owner) {
		var player = (Player)owner.GetTree().GetFirstNodeInGroup("player");
		if (player != null) {
			player.Stats.Heal(_healingAmount);
			owner.Flash();
		}
	}
}