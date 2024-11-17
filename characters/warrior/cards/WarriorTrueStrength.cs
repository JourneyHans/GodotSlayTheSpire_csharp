using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class WarriorTrueStrength : Card {
	protected override void ApplyEffect(Array<Node2D> targets, ModifierHandler modifierHandler) {
		StatusEffect statusEffect = new();
		TrueStrengthForm trueStrengthForm = TrueStrengthForm.Duplicate();
		statusEffect.Status = trueStrengthForm;
		statusEffect.Execute(targets);
	}

	public override string GetDefaultTooltip() {
		return string.Format(ToolTipTxt, TrueStrengthForm.TrueStrengthFormRes.StacksPerTurn);
	}

	public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers) {
		return GetDefaultTooltip();
	}
}
