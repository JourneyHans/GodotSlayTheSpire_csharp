using Godot;

public partial class TreasureRoom : Control
{
    private Button _button;
    private Label _label;

    public override void _Ready() {
        _button = GetNode<Button>("VBoxContainer/Button");
        _button.Pressed += OnButtonPressed;
        _label = GetNode<Label>("VBoxContainer/Label");
    }

    private void OnButtonPressed() {
        EventDispatcher.TriggerEvent(Event.TreasureRoomExited);
    }
}

public partial class TreasureRoom {
    public static class Event {
        public const string TreasureRoomExited = "TreasureRoomExited";
    }
}