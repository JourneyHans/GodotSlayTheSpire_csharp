using framework.debug;
using Godot;

[GlobalClass]
public abstract partial class Relic : Resource {
    public enum EType {
        StartOfTurn,
        StartOfCombat,
        EndOfTurn,
        EndOfCombat,
        EventBased
    };

    public enum ECharacterType {
        All,
        Assassin,
        Warrior,
        Wizard
    };

    [Export] public string RelicName;
    [Export] public string Id;
    [Export] public EType Type;
    [Export] public ECharacterType CharacterType;
    [Export] public bool StarterRelic;
    [Export] public Texture2D Icon;
    [Export(PropertyHint.MultilineText)] public string Tooltip;

    #region logger

    private FinchLogger _logger;
    protected FinchLogger Logger => _logger ??= new FinchLogger(this);

    #endregion

    public virtual void InitializeRelic(RelicUI owner) {
        
    }

    public abstract void ActivateRelic(RelicUI owner);

    public virtual void DeactivateRelic(RelicUI owner) {
        
    }

    public virtual string GetTooltip() {
        return Tooltip;
    }

    public bool CanAppearAsReward(CharacterStats character) {
        if (StarterRelic) {
            return false;
        }

        if (CharacterType == ECharacterType.All) {
            return true;
        }

        string relicCharName = CharacterType.ToString().ToLower();
        string charName = character.CharacterName.ToLower();
        return charName == relicCharName;
    }
}