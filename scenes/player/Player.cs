using System;
using framework.extension;
using Godot;

public partial class Player : Node2D {
    private Material _whiteSpriteMatRes;
    private CharacterStats _stats;
    private Sprite2D _sprite;
    private StatsUI _statsUI;

    [Export]
    public CharacterStats Stats {
        get => _stats;
        set {
            EventDispatcher.UnRegEventListener(global::Stats.Event.StatsChanged, OnUpdateStats);
            EventDispatcher.RegEventListener(global::Stats.Event.StatsChanged, OnUpdateStats);
            _stats = value;
            UpdatePlayer();
        }
    }

    public override void _Ready() {
        _whiteSpriteMatRes = ResourceLoader.Load<Material>("res://art/white_sprite_material.tres");
        
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _statsUI = GetNode<StatsUI>("StatsUI");
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(global::Stats.Event.StatsChanged, OnUpdateStats);
    }

    private async void UpdatePlayer() {
        if (!IsInsideTree()) {
            await this.WhenReady();
        }

        _sprite.Texture = Stats.Art;
        OnUpdateStats();
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
                EventDispatcher.TriggerEvent(Event.PlayerDied);
                QueueFree();
            }
        };
    }

    private async void OnUpdateStats() {
        if (!IsInsideTree()) {
            await this.WhenReady();
        }
        _statsUI.UpdateStats(Stats);
    }
}

public partial class Player {
    public static class Event {
        public const string PlayerDied = "PlayerDied";
        public const string PlayerHit = "PlayerHit";
    }
}