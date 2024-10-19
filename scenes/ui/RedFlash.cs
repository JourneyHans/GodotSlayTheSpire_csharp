using framework.events;
using Godot;

public partial class RedFlash : CanvasLayer {
    private ColorRect _colorRect;
    private Timer _timer;

    public override void _Ready() {
        _colorRect = GetNode<ColorRect>("ColorRect");
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;

        EventDispatcher.RegEventListener(Player.Event.PlayerHit, OnPlayerHit);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(Player.Event.PlayerHit, OnPlayerHit);
    }

    private void OnTimerTimeout() {
        Color original = _colorRect.Color;
        _colorRect.Color = new Color(original.R, original.G, original.B, 0f);
    }

    private void OnPlayerHit() {
        Color original = _colorRect.Color;
        _colorRect.Color = new Color(original.R, original.G, original.B, 0.2f);
        _timer.Start();
    }
}