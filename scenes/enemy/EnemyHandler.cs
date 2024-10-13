using framework.extension;
using Godot;

public partial class EnemyHandler : Node2D {
    public override void _Ready() {
        EventDispatcher.RegEventListener<Enemy>(EnemyAction.Event.EnemyActionCompleted, OnEnemyActionCompleted);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Enemy>(EnemyAction.Event.EnemyActionCompleted, OnEnemyActionCompleted);
    }

    public void ResetEnemyActions() {
        foreach (Node child in GetChildren()) {
            Enemy enemy = (Enemy)child;
            enemy.CurrentAction = null;
            enemy.UpdateAction();
        }
    }

    public void StartTurn() {
        if (GetChildren().IsNullOrEmpty()) {
            return;
        }

        // 从第一个敌人开始，这里的写法需要保证，EnemyHandler下所有的子节点都是Enemy
        Enemy firstEnemy = (Enemy)GetChildren()[0];
        firstEnemy.DoTurn();
    }

    private void OnEnemyActionCompleted(Enemy enemy) {
        int index = enemy.GetIndex();
        if (index == GetChildCount() - 1) {
            // 最后一个敌人回合完毕
            EventDispatcher.TriggerEvent(Event.EnemyTurnEnded);
            return;
        }

        Enemy nextEnemy = GetChild<Enemy>(index + 1);
        nextEnemy.DoTurn();
    }
}

public partial class EnemyHandler {
    public static class Event {
        public const string EnemyTurnEnded = "EnemyTurnEnded";
    }
}