using framework.extension;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Card : Resource {
    public enum EType {
        Attack,
        Skill,
        Power,
    }

    public enum ETarget {
        Self,
        SingleEnemy,
        AllEnemies,
        Everyone,
    }

    [ExportGroup("Card Attributes")]
    [Export]
    public string Id { get; private set; }

    [Export] public EType Type { get; private set; }
    [Export] public ETarget Target { get; private set; }
    [Export] public int Cost { get; private set; }

    [ExportGroup("Card Visuals")] [Export] public Texture2D Icon { get; private set; }
    [Export(PropertyHint.MultilineText)] public string ToolTipTxt { get; private set; }
    [Export] public AudioStream Sound;

    public bool IsSingleTarget => Target == ETarget.SingleEnemy;

    private Array<Node2D> GetTargets(Array<Node2D> targets) {
        if (targets.IsNullOrEmpty()) {
            return null;
        }

        SceneTree tree = targets[0].GetTree();
        return Target switch {
            ETarget.Self => tree.Get2DNodesInGroup("player"),
            ETarget.AllEnemies => tree.Get2DNodesInGroup("enemies"),
            ETarget.Everyone => tree.Get2DNodesInGroup("player") + tree.Get2DNodesInGroup("enemies"),
            _ => null
        };
    }

    public void Play(Array<Node2D> targets, CharacterStats characterStats) {
        EventDispatcher.TriggerEvent(Event.CardPlayed, this);
        characterStats.Mana -= Cost;

        ApplyEffect(IsSingleTarget ? targets : GetTargets(targets));
    }

    protected virtual void ApplyEffect(Array<Node2D> targets) {
        
    }
}