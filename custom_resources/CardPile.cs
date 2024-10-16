using System.Linq;
using System.Text;
using framework.extension;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CardPile : Resource {
    public const string CardPileSizeChanged = "CardPileSizeChanged";

    [Export] public Array<Card> Cards { get; private set; } = new();

    public bool IsEmpty => Cards.IsNullOrEmpty();

    public Card DrawCard() {
        Card card = Cards.PopFront();
        OnSizeChanged();
        return card;
    }

    public void AddCard(Card card) {
        Cards.Add(card);
        OnSizeChanged();
    }

    public void Shuffle() {
        Cards.Shuffle();
    }

    public void Clear() {
        Cards.Clear();
        OnSizeChanged();
    }

    private void OnSizeChanged() {
        EventDispatcher.TriggerEvent(CardPileSizeChanged, Cards.Count);
    }

    public override string ToString() {
        StringBuilder sb = new();

        for (int i = 0; i < Cards.Count; i++) {
            sb.AppendLine($"{i + 1}: {Cards[i].Id}");
        }

        return sb.ToString();
    }
}