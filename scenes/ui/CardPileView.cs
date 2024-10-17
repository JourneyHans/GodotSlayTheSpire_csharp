using framework.utils;
using Godot;
using Godot.Collections;

public partial class CardPileView : Control {
    private PackedScene _cardMenuUIScene = SimpleLoader.LoadPackedScene("res://scenes/ui/card_menu_ui");

    [Export] public CardPile CardPile { get; set; }

    private Label _title;
    private GridContainer _cards;
    private CardTooltipPopup _cardTooltipPopup;
    private Button _backButton;
    
    public override void _Ready() {
        _title = GetNode<Label>("%Title");
        _cards = GetNode<GridContainer>("%Cards");
        _cardTooltipPopup = GetNode<CardTooltipPopup>("%CardTooltipPopup");
        _backButton = GetNode<Button>("%BackButton");
        _backButton.Pressed += Hide;

        foreach (Node card in _cards.GetChildren()) {
            card.QueueFree();
        }

        _cardTooltipPopup.HideTooltip();
        
        EventDispatcher.RegEventListener<Card>(CardTooltipPopup.Event.TooltipRequested, OnTooltipRequested);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Card>(CardTooltipPopup.Event.TooltipRequested, OnTooltipRequested);
    }

    public void ShowCurrentView(string title, bool randomized = false) {
        foreach (Node card in _cards.GetChildren()) {
            card.QueueFree();
        }

        _cardTooltipPopup.HideTooltip();
        _title.Text = title;
        Callable.From(() => { UpdateView(randomized); }).CallDeferred();
    }

    private void UpdateView(bool randomized) {
        if (CardPile == null) {
            return;
        }

        Array<Card> allCards = CardPile.Cards.Duplicate();
        if (randomized) {
            allCards.Shuffle();
        }

        foreach (Card card in allCards) {
            var newCard = _cardMenuUIScene.Instantiate<CardMenuUI>();
            _cards.AddChild(newCard);
            newCard.Card = card;
        }
        
        Show();
    }

    private void OnTooltipRequested(Card card) {
        _cardTooltipPopup.ShowTooltip(card);
    }

    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed("ui_cancel")) {
            if (_cardTooltipPopup.Visible) {
                _cardTooltipPopup.HideTooltip();
            }
            else {
                Hide();
            }
        }
    }
}