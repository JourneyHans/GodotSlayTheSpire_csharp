using Godot;

public partial class BattleReward : Control
{
    private Button _button;
    private Label _label;

    public override void _Ready() {
        _button = GetNode<Button>("VBoxContainer/Button");
        _button.Pressed += OnButtonPressed;
        _label = GetNode<Label>("VBoxContainer/Label");
    }

    private void OnButtonPressed() {
        EventDispatcher.TriggerEvent(Event.BattleRewardExited);
    }
}

public partial class BattleReward {
    public static class Event {
        public const string BattleRewardExited = "BattleRewardExited";
    }
}