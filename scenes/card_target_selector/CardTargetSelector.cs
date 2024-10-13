using System.Collections.Generic;
using framework.math;
using Godot;

public partial class CardTargetSelector : Node2D {
    private const int ArcPointsCount = 8;
    
    public Area2D Area2D { get; private set; }
    public Line2D CardArc { get; private set; }
    
    private CardUI _currentCardUI;
    private bool _targeting;
    private readonly List<Vector2> _arcPoints = new();
    
    public override void _Ready() {
        Area2D = GetNode<Area2D>("Area2D");
        CardArc = GetNode<Line2D>("CanvasLayer/CardArc");
        
        Area2D.AreaEntered += Area2DOnAreaEntered;
        Area2D.AreaExited += Area2DOnAreaExited;
        EventDispatcher.RegEventListener<CardUI>(CardState.Event.CardAimStarted, OnCardAimStarted);
        EventDispatcher.RegEventListener<CardUI>(CardState.Event.CardAimEnded, OnCardAimEnded);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<CardUI>(CardState.Event.CardAimStarted, OnCardAimStarted);
        EventDispatcher.UnRegEventListener<CardUI>(CardState.Event.CardAimEnded, OnCardAimEnded);
    }

    public override void _Process(double delta) {
        if (!_targeting) {
            return;
        }

        Area2D.Position = GetLocalMousePosition();
        CardArc.Points = GetPoints();
    }

    private Vector2[] GetPoints() {
        _arcPoints.Clear();

        Vector2 startPos = _currentCardUI.GlobalPosition;
        startPos.X += _currentCardUI.Size.X / 2;
        Vector2 targetPos = GetLocalMousePosition();
        for (int i = 0; i < ArcPointsCount; i++) {
            float t = 1f / ArcPointsCount * i;
            float x = Mathf.Lerp(startPos.X, targetPos.X, t);
            float y = Mathf.Lerp(startPos.Y, targetPos.Y, t.EaseOutCubic());
            _arcPoints.Add(new Vector2(x, y));
        }

        return _arcPoints.ToArray();
    }

    private void OnCardAimStarted(CardUI cardUI) {
        if (!cardUI.Card.IsSingleTarget) {
            return;
        }

        _targeting = true;
        Area2D.Monitoring = true;
        Area2D.Monitorable = true;
        _currentCardUI = cardUI;
    }

    private void OnCardAimEnded(CardUI cardUI) {
        _targeting = false;
        CardArc.ClearPoints();
        Area2D.Position = Vector2.Zero;
        Area2D.Monitoring = false;
        Area2D.Monitorable = false;
        _currentCardUI = null;
    }

    private void Area2DOnAreaEntered(Area2D area) {
        if (_currentCardUI == null || !_targeting) {
            return;
        }

        if (!_currentCardUI.Targets.Contains(area)) {
            _currentCardUI.Targets.Add(area);
        }
    }

    private void Area2DOnAreaExited(Area2D area) {
        if (_currentCardUI == null || !_targeting) {
            return;
        }

        _currentCardUI.Targets.Remove(area);
    }
}
