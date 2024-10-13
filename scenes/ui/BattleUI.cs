using Godot;

public partial class BattleUI : CanvasLayer {
	private CharacterStats _characterStats;
	private Hand _hand;
	private ManaUI _manaUI;
	private Button _endTurnButton;

	[Export]
	public CharacterStats CharacterStats {
		get => _characterStats;
		set {
			_characterStats = value;
			_hand.CharacterStats = _characterStats;
			_manaUI.CharacterStats = _characterStats;
		}
	}

	public override void _Ready() {
		_hand = GetNode<Hand>("Hand");
		_manaUI = GetNode<ManaUI>("ManaUI");
		_endTurnButton = GetNode<Button>("%EndTurnButton");
		_endTurnButton.Pressed += OnEndTurnButtonPressed;

		EventDispatcher.RegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
	}

	protected override void Dispose(bool disposing) {

		EventDispatcher.UnRegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
	}

	private void OnEndTurnButtonPressed() {
		_endTurnButton.Disabled = true;
		EventDispatcher.TriggerEvent(PlayerHandler.Event.PlayerTurnEnded);
	}

	private void OnPlayerHandDrawn() {
		_endTurnButton.Disabled = false;
	}
}