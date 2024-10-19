using framework.events;
using Godot;

public partial class GoldUI : HBoxContainer {
    [Export]
    public RunStats RunStats {
        get => _runStats;
        set {
            _runStats = value;
            EventDispatcher.SafeRegEventListener(RunStats.Event.GoldChanged, UpdateGold);
            UpdateGold();
        }
    }

    private Label _label;
    private RunStats _runStats;

    public override void _Ready() {
        _label = GetNode<Label>("Label");
        _label.Text = "0";
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(RunStats.Event.GoldChanged, UpdateGold);
    }

    private void UpdateGold() {
        _label.Text = RunStats.Gold.ToString();
    }
}