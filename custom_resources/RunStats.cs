using framework.events;
using Godot;

public partial class RunStats : Resource {
    private const int StartingGold = 70;
    private const int BaseCardRewards = 3;
    
    public const float BaseCommonWeight = 6.0f;
    public const float BaseUncommonWeight = 3.7f;
    public const float BaseRareWeight = 0.3f;

    private int _gold = StartingGold;

    [Export]
    public int Gold {
        get => _gold;
        set {
            _gold = value;
            EventDispatcher.TriggerEvent(Event.GoldChanged);
        }
    }

    [Export] public int CardRewards = BaseCardRewards;
    [Export(PropertyHint.Range)] public float CommonWeight = BaseCommonWeight;
    [Export(PropertyHint.Range)] public float UncommonWeight = BaseUncommonWeight;
    [Export(PropertyHint.Range)] public float RareWeight = BaseRareWeight;

    public float CurrentTotalWeight => CommonWeight + UncommonWeight + RareWeight;

    public void ResetWeights() {
        CommonWeight = BaseCommonWeight;
        UncommonWeight = BaseUncommonWeight;
        RareWeight = BaseRareWeight;
    }
}

public partial class RunStats {
    public static class Event {
        public const string GoldChanged = "GoldChanged";
    }
}