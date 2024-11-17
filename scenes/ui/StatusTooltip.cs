using Godot;
using framework.extension;
using framework.utils;

public partial class StatusTooltip : HBoxContainer {
    [Export]
    public Status Status {
        get => _status;
        set => SetStatus(value);
    }

    #region onready

    private TextureRect _icon;
    private Label _label;
    private Status _status;

    #endregion

    public override void _Ready() {
        _icon = GetNode<TextureRect>("Icon");
        _label = GetNode<Label>("Label");
    }

    private async void SetStatus(Status newStatus) {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _status = newStatus;
        _icon.Texture = Status.Icon;
        _label.Text = Status.GetToolTip();
    }
}

public partial class StatusTooltip {
    private static readonly PackedScene StatusTooltipScene =
        SimpleLoader.LoadPackedScene("res://scenes/ui/status_tooltip");

    public static StatusTooltip Instantiate() {
        return StatusTooltipScene.Instantiate<StatusTooltip>();
    }
}