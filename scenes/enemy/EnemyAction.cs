using Godot;

[GlobalClass]
public abstract partial class EnemyAction : Node {
    public enum EType {
        Conditional,
        ChanceBased,
    }

    [Export] public Intent Intent;
    [Export] public AudioStream Sound;
    [Export] public EType Type { get; private set; }
    [Export(PropertyHint.Range, "0.0, 10.0")] public float ChanceWeight;

    public float AccumulatedWeight;

    public Enemy Enemy;
    public Node2D Target;

    public virtual bool IsPerformable() {
        return Enemy != null && Target != null;
    }

    public abstract void PerformAction();

    protected virtual void EnemyActionCompleted() {
        EventDispatcher.TriggerEvent(Event.EnemyActionCompleted, Enemy);
    }
}

public abstract partial class EnemyAction {
    public static class Event {
        /// <summary>
        /// 敌人行动结束
        /// 参数1：Enemy
        /// </summary>
        public const string EnemyActionCompleted = "EnemyActionCompleted";
    };
}