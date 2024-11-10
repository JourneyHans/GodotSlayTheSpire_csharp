using Godot;
using Godot.Collections;

public partial class WarriorAxeAttack : Card {
    private int _baseDamage = 6;
    
    protected override void ApplyEffect(Array<Node2D> targets, ModifierHandler modifierHandler) {
        DamageEffect damageEffect = new();
        damageEffect.Amount = modifierHandler.GetModifierValue(_baseDamage, Modifier.EType.DmgDealt);
        damageEffect.Sound = Sound;
        damageEffect.Execute(targets);
    }
}