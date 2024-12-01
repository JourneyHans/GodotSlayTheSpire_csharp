using Godot;

public partial class RelicTooltip : Control {

    #region onready

    private TextureRect _relicIcon;
    private RichTextLabel _relicTooltip;
    private Button _backButton;

    #endregion

    public override void _Ready() {
        GuiInput += OnGuiInput;

        _relicIcon = GetNode<TextureRect>("%RelicIcon");
        _relicTooltip = GetNode<RichTextLabel>("%RelicTooltip");
        _backButton = GetNode<Button>("%BackButton");

        _backButton.Pressed += Hide;
    }

    public override void _Input(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed(InputKey.Esc) && Visible) {
            Hide();
        }
    }

    public void ShowTooltip(Relic relic) {
        _relicIcon.Texture = relic.Icon;
        _relicTooltip.Text = relic.GetTooltip();
        Show();
    }

    private void OnGuiInput(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed(InputKey.LeftMouse)) {
            Hide();
        }
    }
}

// Events
public partial class RelicTooltip {
    public static class Event {
        /// <summary>
        /// 参数1: Relic
        /// </summary>
        public const string RelicTooltipRequest = "RelicTooltip.Event.RelicTooltipRequest";
    }
}