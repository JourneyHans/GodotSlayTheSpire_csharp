using framework.events;
using Godot;

[GlobalClass]
public partial class CharacterStats : Stats {
    [ExportGroup("Visuals")] 
    [Export] public string CharacterName;
    [Export(PropertyHint.MultilineText)] public string Description;
    [Export] public Texture Portrait;
    
    [ExportGroup("Gameplay Data")]
    [Export] public CardPile StartingDeck;
    [Export] public CardPile DraftableCards;
    [Export] public int CardsPerTurn;
    [Export] public int MaxMana;

    private int _mana;

    public CardPile Deck { get; private set; }
    public CardPile Discard { get; set; }
    public CardPile DrawPile { get; set; }

    public int Mana {
        get => _mana;
        set {
            if (_mana == value) {
                return;
            }

            _mana = value;
            OnStatsChanged();
        }
    }

    public void ResetMana() {
        Mana = MaxMana;
    }

    public override void TakeDamage(int damage) {
        int beforeHealth = Health;
        base.TakeDamage(damage);
        if (beforeHealth > Health) {
            // 说明扣血了
            EventDispatcher.TriggerEvent(Player.Event.PlayerHit);
        }
    }

    public bool CanPlayCard(Card card) {
        return Mana >= card.Cost;
    }

    public new CharacterStats CreateInstance() {
        CharacterStats instance = (CharacterStats)Duplicate();
        instance.InitValues();
        instance.ResetMana();
        instance.Deck = (CardPile)instance.StartingDeck.Duplicate();
        instance.DrawPile = new CardPile();
        instance.Discard = new CardPile();
        return instance;
    }
}