using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class WarriorBigSlam : Card {
	private static readonly Status ExposeStatus = SimpleLoader.LoadResource<Status>("res://statuses/exposed");

	private int _baseDamage = 4;
	private int _exposedDuration = 2;
	
	protected override void ApplyEffect(Array<Node2D> targets, ModifierHandler modifierHandler) {
		DamageEffect damageEffect = new();
		damageEffect.Amount = modifierHandler.GetModifierValue(_baseDamage, Modifier.EType.DmgDealt);
		damageEffect.Sound = Sound;
		damageEffect.Execute(targets);
		
		StatusEffect statusEffect = new();
		Status exposed = ExposeStatus.Duplicate<Status>();
		exposed.Duration = _exposedDuration;
		statusEffect.Status = exposed;
		statusEffect.Execute(targets);
	}

	public override string GetDefaultTooltip() {
		return string.Format(ToolTipTxt, _baseDamage, _exposedDuration);
	}

	public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers) {
		var modifiedDmg = playerModifiers.GetModifierValue(_baseDamage, Modifier.EType.DmgDealt);
		if (enemyModifiers != null) {
			modifiedDmg = enemyModifiers.GetModifierValue(modifiedDmg, Modifier.EType.DmgTaken);
		}

		return string.Format(ToolTipTxt, modifiedDmg, _exposedDuration);
	}
}
