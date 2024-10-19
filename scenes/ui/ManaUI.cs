using framework.events;
using framework.extension;
using Godot;

public partial class ManaUI : Panel {
    private CharacterStats _characterStats;
    private Label _manaLabel;

    [Export]
    public CharacterStats CharacterStats {
        get => _characterStats;
        set => SetCharacterStats(value);
    }

    public override void _Ready() {
        _manaLabel = GetNode<Label>("ManaLabel");
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(Stats.Event.StatsChanged, OnStatsChanged);
    }

    private void SetCharacterStats(CharacterStats stats) {
        _characterStats = stats;
        EventDispatcher.SafeRegEventListener(Stats.Event.StatsChanged, OnStatsChanged);
        
        OnStatsChanged();
    }

    private async void OnStatsChanged() {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _manaLabel.Text = $"{_characterStats.Mana}/{_characterStats.MaxMana}";
    }
}