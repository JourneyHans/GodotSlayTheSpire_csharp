using Godot;

public partial class Tooltip : PanelContainer {
    [Export] public float FadeSeconds = 0.2f;
    public TextureRect TooltipIcon;
    public RichTextLabel TooltipTextLabel;

    private Tween _tween;
    private bool _isVisible = false;

    public override void _Ready() {
        TooltipIcon = GetNode<TextureRect>("%TooltipIcon");
        TooltipTextLabel = GetNode<RichTextLabel>("%TooltipText");
        Modulate = Colors.Transparent;
        Hide();

        EventDispatcher.RegEventListener<Texture2D, string>(Event.ShowTips, ShowTooltip);
        EventDispatcher.RegEventListener(Event.HideTips, HideTooltip);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Texture2D, string>(Event.ShowTips, ShowTooltip);
        EventDispatcher.UnRegEventListener(Event.HideTips, HideTooltip);
    }

    public void ShowTooltip(Texture2D icon, string text) {
        _isVisible = true;
        _tween?.Kill();

        TooltipIcon.Texture = icon;
        TooltipTextLabel.Text = text;

        _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenCallback(Callable.From(Show));
        _tween.TweenProperty(this, "modulate", Colors.White, FadeSeconds);
    }

    public void HideTooltip() {
        _isVisible = false;
        _tween?.Kill();
        
        // 延迟0.2s再播放隐藏动画
        GetTree().CreateTimer(FadeSeconds, false).Timeout += HideAnimation;
    }

    private void HideAnimation() {
        if (_isVisible) {
            return;
        }

        _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenProperty(this, "modulate", Colors.Transparent, FadeSeconds);
        _tween.TweenCallback(Callable.From(Hide));
    }
}

// event
public partial class Tooltip {
    public static class Event {
        /// <summary>
        /// 显示提示框
        /// 参数1 Card
        /// </summary>
        public const string ShowTips = "ShowTooltips";
        public const string HideTips = "HideTooltips";
    }
}