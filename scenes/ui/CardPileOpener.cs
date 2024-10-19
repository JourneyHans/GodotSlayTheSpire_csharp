using framework.events;
using Godot;

public partial class CardPileOpener : TextureButton {
    private CardPile _cardPile;
    
    [Export] public Label Counter;

    [Export]
    public CardPile CardPile {
        get => _cardPile;
        set {
            _cardPile = value;
            EventDispatcher.SafeRegEventListener<CardPile>(CardPile.Event.CardPileSizeChanged, OnCardPileSizeChanged);
            OnCardPileSizeChanged(_cardPile);
        }
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<CardPile>(CardPile.Event.CardPileSizeChanged, OnCardPileSizeChanged);
    }

    private void OnCardPileSizeChanged(CardPile _) {
        Counter.Text = _cardPile.Cards.Count.ToString();
    }
}