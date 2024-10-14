using framework.utils;
using Godot;

public partial class Hand : HBoxContainer {
	[Export] public CharacterStats CharacterStats { get; set; }

	private PackedScene _cardUIScene = SimpleLoader.LoadPackedScene("res://scenes/card_ui/card_ui");

	public override void _Ready() {
		EventDispatcher.RegEventListener<CardUI>(CardUI.ReparentRequested, OnReparentRequested);
	}

	protected override void Dispose(bool disposing) {
		EventDispatcher.UnRegEventListener<CardUI>(CardUI.ReparentRequested, OnReparentRequested);
	}

	public void AddCard(Card card) {
		CardUI cardUI = _cardUIScene.Instantiate<CardUI>();
		AddChild(cardUI);
		cardUI.Card = card;
		cardUI.Parent = this;
		cardUI.CharacterStats = CharacterStats;
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