using Godot;

public partial class Campfire : Control
{
    private Button _button;
    private Label _label;

    public override void _Ready() {
        _button = GetNode<Button>("VBoxContainer/Button");
        _button.Pressed += OnButtonPressed;
        _label = GetNode<Label>("VBoxContainer/Label");
    }

    private void OnButtonPressed() {
        EventDispatcher.TriggerEvent(Event.CampfireExited);
    }
}

public partial class Campfire {
    public static class Event {
        public const string CampfireExited = "CampfireExited";
    }
}