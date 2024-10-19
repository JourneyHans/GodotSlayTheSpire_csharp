using framework.events;
using Godot;

public partial class BattleUI : CanvasLayer {
	private CharacterStats _characterStats;
	private Hand _hand;
	private ManaUI _manaUI;
	private Button _endTurnButton;
	private CardPileOpener _drawPileButton;
	private CardPileOpener _discardPileButton;
	private CardPileView _drawPileView;
	private CardPileView _discardPileView;

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
		_drawPileButton = GetNode<CardPileOpener>("%DrawPileButton");
		_discardPileButton = GetNode<CardPileOpener>("%DiscardPileButton");
		_drawPileView = GetNode<CardPileView>("%DrawPileView");
		_discardPileView = GetNode<CardPileView>("%DiscardPileView");
		
		_endTurnButton.Pressed += OnEndTurnButtonPressed;
		_drawPileButton.Pressed += OnDrawPileButtonPressed;
		_discardPileButton.Pressed += OnDiscardPileButtonPressed;

		EventDispatcher.RegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
	}

	public void InitializedCardPileUI() {
		_drawPileButton.CardPile = CharacterStats.DrawPile;
		_drawPileView.CardPile = CharacterStats.DrawPile;
		_discardPileButton.CardPile = CharacterStats.Discard;
		_discardPileView.CardPile = CharacterStats.Discard;
	}

	protected override void Dispose(bool disposing) {

		EventDispatcher.UnRegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
	}

	private void OnEndTurnButtonPressed() {
		_endTurnButton.Disabled = true;
		EventDispatcher.TriggerEvent(PlayerHandler.Event.PlayerTurnEnded);
	}

	private void OnDrawPileButtonPressed() {
		_drawPileView.ShowCurrentView("Draw Pile", true);
	}

	private void OnDiscardPileButtonPressed() {
		_discardPileView.ShowCurrentView("Discard Pile");
	}

	private void OnPlayerHandDrawn() {
		_endTurnButton.Disabled = false;
	}
}