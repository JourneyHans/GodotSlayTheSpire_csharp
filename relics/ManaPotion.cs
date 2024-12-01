using framework.events;

public partial class ManaPotion : Relic {
	private RelicUI _owner;

	public override void ActivateRelic(RelicUI owner) {
		_owner = owner;
		EventDispatcher.RegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
	}

	private void OnPlayerHandDrawn() {
		EventDispatcher.UnRegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
		AddMana(_owner);
	}

	private void AddMana(RelicUI owner) {
		owner.Flash();
		var player = (Player)owner.GetTree().GetFirstNodeInGroup("player");
		if (player != null) {
			player.Stats.Mana += 1;
		}
	}
}