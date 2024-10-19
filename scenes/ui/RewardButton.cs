using System.Threading.Tasks;
using framework.extension;
using Godot;

public partial class RewardButton : Button {
    private Texture _rewardIcon;
    private string _rewardText;

    [Export]
    public Texture RewardIcon {
        get => _rewardIcon;
        set => SetRewardIcon(value);
    }

    [Export]
    public string RewardText {
        get => _rewardText;
        set => SetRewardText(value);
    }

    private TextureRect _customIcon;
    private Label _customText;

    public override void _Ready() {
        _customIcon = GetNode<TextureRect>("%CustomIcon");
        _customText = GetNode<Label>("%CustomText");
        Pressed += OnPressed;
    }

    private async void SetRewardIcon(Texture icon) {
        _rewardIcon = icon;
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _customIcon.Texture = (Texture2D)_rewardIcon;
    }

    private async void SetRewardText(string text) {
        _rewardText = text;
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _customText.Text = text;
    }

    private void OnPressed() {
        QueueFree();
    }
}