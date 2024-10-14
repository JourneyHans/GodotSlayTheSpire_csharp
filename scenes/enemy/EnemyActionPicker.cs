using System;
using Godot;

public partial class EnemyActionPicker : Node {
    private Enemy _enemy;
    private Node2D _target;

    [Export]
    public Enemy Enemy {
        get => _enemy;
        set {
            _enemy = value;
            foreach (EnemyAction action in GetChildren()) {
                action.Enemy = _enemy;
            }
        }
    }

    [Export]
    public Node2D Target {
        get => _target;
        private set {
            _target = value;
            foreach (Node child in GetChildren()) {
                var action = (EnemyAction)child;
                action.Target = _target;
            }
        }
    }

    private float _totalWeight;

    public override void _Ready() {
        Target = (Node2D)GetTree().GetFirstNodeInGroup("player");
        SetupChances();
    }

    private void SetupChances() {
        foreach (EnemyAction action in GetChildren()) {
            if (action.Type != EnemyAction.EType.ChanceBased) {
                continue;
            }

            _totalWeight += action.ChanceWeight;
            action.AccumulatedWeight = _totalWeight;
        }
    }

    public EnemyAction GetAction() {
        var action = GetFirstConditionalAction();
        
        // 条件性行动优先于基于几率的行动
        if (action != null) {
            return action;
        }
        
        return GetChanceBasedAction();
    }

    public EnemyAction GetFirstConditionalAction() {
        foreach (Node child in GetChildren()) {
            if (child is not EnemyAction { Type: EnemyAction.EType.Conditional } action) {
                continue;
            }

            if (action.IsPerformable()) {
                return action;
            }
        }

        return null;
    }

    private EnemyAction GetChanceBasedAction() {
        float roll = GD.Randf() * _totalWeight;
        foreach (Node child in GetChildren()) {
            if (child is not EnemyAction { Type: EnemyAction.EType.ChanceBased } action) {
                continue;
            }

            if (action.AccumulatedWeight > roll) {
                return action;
            }
        }
        
        return null;
    }
}