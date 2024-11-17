using framework.extension;
using Godot;

public partial class IntentUI : HBoxContainer {
    private TextureRect _icon;
    private Label _label;

    public override void _Ready() {
        _icon = GetNode<TextureRect>("Icon");
        _label = GetNode<Label>("Label");
    }

    public void UpdateIntent(Intent intent) {
        if (intent == null) {
            Hide();
            return;
        }

        _icon.Texture = (Texture2D)intent.Icon;
        _icon.Visible = _icon.Texture != null;
        _label.Text = intent.CurrentText;
        _label.Visible = !intent.CurrentText.IsNullOrEmpty();
        Show();
    }
}