using framework.events;
using framework.utils;
using Godot;

public partial class Hand : HBoxContainer {
	private static readonly PackedScene CardUIScene = SimpleLoader.LoadPackedScene("res://scenes/card_ui/card_ui");

	[Export] public Player Player;
	[Export] public CharacterStats CharacterStats { get; set; }

	public override void _Ready() {
		EventDispatcher.RegEventListener<CardUI>(CardUI.ReparentRequested, OnReparentRequested);
	}

	protected override void Dispose(bool disposing) {
		EventDispatcher.UnRegEventListener<CardUI>(CardUI.ReparentRequested, OnReparentRequested);
	}

	public void AddCard(Card card) {
		CardUI cardUI = CardUIScene.Instantiate<CardUI>();
		AddChild(cardUI);
		cardUI.Card = card;
		cardUI.Parent = this;
		cardUI.CharacterStats = CharacterStats;
		cardUI.ModifierHandler = Player.ModifierHandler;
	}

	private void OnReparentRequested(CardUI cardUI) {
		cardUI.Reparent(this);
		MoveChild(cardUI, cardUI.OriginalIndex);
	}

	public void DiscardCard(CardUI cardUI) {
		cardUI.QueueFree();
	}

	public void DisableHand() {
		foreach (Node child in GetChildren()) {
			var cardUI = (CardUI)child;
			cardUI.Disabled = true;
		}
	}
}