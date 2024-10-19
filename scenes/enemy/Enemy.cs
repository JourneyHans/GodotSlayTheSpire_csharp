using framework.events;
using framework.extension;
using framework.utils;
using Godot;

public partial class Enemy : Area2D {
    private const int ArrowOffset = 5;
    private Material _whiteSpriteMatRes;

    private EnemyStats _stats;
    private Sprite2D _sprite;
    private Sprite2D _arrow;
    private StatsUI _statsUI;
    private IntentUI _intentUI;

    [Export]
    public EnemyStats Stats {
        get => _stats;
        private set {
            EventDispatcher.SafeRegEventListener(global::Stats.Event.StatsChanged, OnUpdateStats);
            _stats = value.CreateInstance();
            UpdateEnemy();
        }
    }

    private EnemyActionPicker _enemyActionPicker;
    private EnemyAction _currentAction;
    public EnemyAction CurrentAction {
        get => _currentAction;
        set {
            _currentAction = value;
            if (_currentAction != null) {
                _intentUI.UpdateIntent(_currentAction.Intent);
            }
        }
    }

    public override void _Ready() {
        _whiteSpriteMatRes = SimpleLoader.LoadResource<Material>("res://art/white_sprite_material");
        
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _arrow = GetNode<Sprite2D>("Arrow");
        _statsUI = GetNode<StatsUI>("StatsUI");
        _intentUI = GetNode<IntentUI>("IntentUI");

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(global::Stats.Event.StatsChanged, OnUpdateStats);
    }

    private void SetUpAI() {
        _enemyActionPicker?.QueueFree();
        _enemyActionPicker = Stats.AI.Instantiate<EnemyActionPicker>();
        AddChild(_enemyActionPicker);
        _enemyActionPicker.Enemy = this;
    }

    private async void UpdateEnemy() {
        if (!IsInsideTree()) {
            await this.WhenReady();
        }

        _sprite.Texture = Stats.Art;
        _arrow.Position = Vector2.Right * (_sprite.GetRect().Size.X / 2 + ArrowOffset);
        SetUpAI();
        OnUpdateStats();
    }

    public void DoTurn() {
        Stats.Block = 0;
        _currentAction.PerformAction();
    }

    public void TakeDamage(int damage) {
        if (Stats.Health <= 0) {
            return;
        }
        
        _sprite.Material = _whiteSpriteMatRes;

        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(() => { this.Shake(16, 0.15f); }));
        tween.TweenCallback(Callable.From(() => { Stats.TakeDamage(damage); }));
        tween.TweenInterval(0.17);
        tween.Finished += () => {
            _sprite.Material = null;
            if (Stats.Health <= 0) {
                QueueFree();
            }
        };
    }

    private async void OnUpdateStats() {
        if (!IsInsideTree()) {
            await this.WhenReady();
        }
        _statsUI.UpdateStats(Stats);

        UpdateAction();
    }

    public void UpdateAction() {
        if (_enemyActionPicker == null) {
            return;
        }

        if (CurrentAction == null) {
            CurrentAction = _enemyActionPicker.GetAction();
            return;
        }
        
        // 当状态改变时，可能会导致敌人修改动作
        var newConditionalAction = _enemyActionPicker.GetFirstConditionalAction();
        if (newConditionalAction != null && CurrentAction != newConditionalAction) {
            CurrentAction = newConditionalAction;
        }
    }

    private void OnAreaEntered(Area2D area) {
        _arrow.Show();
    }

    private void OnAreaExited(Area2D area) {
        _arrow.Hide();
    }
}