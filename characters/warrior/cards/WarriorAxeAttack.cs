using Godot;
using Godot.Collections;

public partial class WarriorAxeAttack : Card {
    protected override void ApplyEffect(Array<Node2D> targets) {
        DamageEffect damageEffect = new();
        damageEffect.Amount = 6;
        damageEffect.Sound = Sound;
        damageEffect.Execute(targets);
    }
}