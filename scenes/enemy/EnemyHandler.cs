using System;
using framework.events;
using framework.extension;
using Godot;
using Godot.Collections;

public partial class EnemyHandler : Node2D {
    private readonly Array<Enemy> _actingEnemies = new();
    
    public override void _Ready() {
        EventDispatcher.RegEventListener<Enemy>(Enemy.Event.EnemyDied, OnEnemyDied);
        EventDispatcher.RegEventListener<Enemy>(EnemyAction.Event.EnemyActionCompleted, OnEnemyActionCompleted);
        EventDispatcher.RegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Enemy>(Enemy.Event.EnemyDied, OnEnemyDied);
        EventDispatcher.UnRegEventListener<Enemy>(EnemyAction.Event.EnemyActionCompleted, OnEnemyActionCompleted);
        EventDispatcher.UnRegEventListener(PlayerHandler.Event.PlayerHandDrawn, OnPlayerHandDrawn);
    }

    public void SetupEnemies(BattleStats battleStats) {
        if (battleStats == null) {
            return;
        }

        foreach (Enemy enemy in GetChildren()) {
            enemy.QueueFree();
        }

        Node2D allNewEnemies = battleStats.Enemies.Instantiate<Node2D>();
        foreach (Node2D newEnemy in allNewEnemies.GetChildren()) {
            var newEnemyChild = (Enemy)newEnemy.Duplicate();
            AddChild(newEnemyChild);
            newEnemyChild.StatusHandler.StatusesApplied += type => OnEnemyStatusesApplied(type, newEnemyChild);
        }

        allNewEnemies.QueueFree();
    }

    public void ResetEnemyActions() {
        foreach (Node child in GetChildren()) {
            Enemy enemy = (Enemy)child;
            enemy.CurrentAction = null;
            enemy.UpdateAction();
        }
    }

    #region 敌方行动流程

    public void StartTurn() {
        if (GetChildren().IsNullOrEmpty()) {
            return;
        }

        _actingEnemies.Clear();
        _actingEnemies.AddRange(this.GetChildren<Enemy>());
        StartNextEnemyTurn();
    }

    private void StartNextEnemyTurn() {
        if (_actingEnemies.IsNullOrEmpty()) {
            EventDispatcher.TriggerEvent(Event.EnemyTurnEnded);
            return;
        }

        _actingEnemies[0].StatusHandler.ApplyStatusesByType(Status.EType.StartOfTurn);
    }

    private void OnEnemyStatusesApplied(Status.EType type, Enemy enemy) {
        switch (type) {
            case Status.EType.StartOfTurn:
                enemy.DoTurn();
                break;
            case Status.EType.EndOfTurn:
                _actingEnemies.Remove(enemy);
                StartNextEnemyTurn();
                break;
        }
    }

    private void OnEnemyActionCompleted(Enemy enemy) {
        enemy.StatusHandler.ApplyStatusesByType(Status.EType.EndOfTurn);
    }

    #endregion

    private void OnPlayerHandDrawn() {
        foreach (Enemy enemy in GetChildren()) {
            enemy.UpdateIntent();
        }
    }

    private void OnEnemyDied(Enemy enemy) {
        bool isEnemyTurn = !_actingEnemies.IsNullOrEmpty();
        _actingEnemies.Remove(enemy);
        if (isEnemyTurn) {
            StartNextEnemyTurn();
        }
    }
}

public partial class EnemyHandler {
    public static class Event {
        public const string EnemyTurnEnded = "EnemyTurnEnded";
    }
}