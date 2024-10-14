using Godot;

public partial class Map : Control {
    private Button _button;
    private Label _label;

    public override void _Ready() {
        _button = GetNode<Button>("VBoxContainer/Button");
        _button.Pressed += OnButtonPressed;
        _label = GetNode<Label>("VBoxContainer/Label");
    }

    private void OnButtonPressed() {
        EventDispatcher.TriggerEvent(Event.MapExited);
    }
}

public partial class Map {
    public static class Event {
        public const string MapExited = "MapExited";
    }
}