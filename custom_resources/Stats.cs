using framework.events;
using Godot;

[GlobalClass]
public partial class Stats : Resource {
    [Export] public int MaxHealth = 1;
    [Export] public Texture2D Art;

    protected int health;
    protected int block;

    public int Health {
        get => health;
        private set {
            int newHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (health == newHealth) {
                return;
            }

            health = newHealth;
            OnStatsChanged();
        }
    }

    public int Block {
        get => block;
        set {
            int newBlock = Mathf.Clamp(value, 0, 999);
            if (block == newBlock) {
                return;
            }

            block = newBlock;
            OnStatsChanged();
        }
    }

    public virtual void TakeDamage(int damage) {
        if (damage <= 0) {
            return;
        }

        var initialDamage = damage;
        damage = Mathf.Clamp(damage - block, 0, damage);
        Block = Mathf.Clamp(Block - initialDamage, 0, Block);
        Health -= damage;
    }

    public void Heal(int heal) {
        Health += heal;
    }

    public Stats CreateInstance() {
        Stats instance = (Stats)Duplicate();
        instance.InitValues();
        return instance;
    }

    protected void InitValues() {
        health = MaxHealth;
        block = 0;
    }

    protected void OnStatsChanged() {
        EventDispatcher.TriggerEvent(Event.StatsChanged);
    }
}

public partial class Stats {
    public static class Event {
        public const string StatsChanged = "StatsChanged";
    }
}