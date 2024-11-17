using Godot;
using Godot.Collections;

public partial class WarriorSlash : Card {
	private int _baseDamage = 4;

	protected override void ApplyEffect(Array<Node2D> targets, ModifierHandler modifierHandler) {
		DamageEffect damageEffect = new();
		damageEffect.Amount = modifierHandler.GetModifierValue(_baseDamage, Modifier.EType.DmgDealt);
		damageEffect.Sound = Sound;
		damageEffect.Execute(targets);
	}

	public override string GetDefaultTooltip() {
		return string.Format(ToolTipTxt, _baseDamage);
	}

	public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers) {
		var modifiedDmg = playerModifiers.GetModifierValue(_baseDamage, Modifier.EType.DmgDealt);
		if (enemyModifiers != null) {
			modifiedDmg = enemyModifiers.GetModifierValue(modifiedDmg, Modifier.EType.DmgTaken);
		}

		return string.Format(ToolTipTxt, modifiedDmg);
	}
}