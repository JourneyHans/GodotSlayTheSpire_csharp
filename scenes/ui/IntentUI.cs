using framework.extension;
using Godot;

public partial class IntentUI : HBoxContainer {
    private TextureRect _icon;
    private Label _number;

    public override void _Ready() {
        _icon = GetNode<TextureRect>("Icon");
        _number = GetNode<Label>("Number");
    }

    public void UpdateIntent(Intent intent) {
        if (intent == null) {
            Hide();
            return;
        }

        _icon.Texture = (Texture2D)intent.Icon;
        _icon.Visible = _icon.Texture != null;
        _number.Text = intent.Number;
        _number.Visible = !intent.Number.IsNullOrEmpty();
        Show();
    }
}