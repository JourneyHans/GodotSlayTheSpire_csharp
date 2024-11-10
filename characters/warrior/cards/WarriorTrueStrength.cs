using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class WarriorTrueStrength : Card {
	private static readonly TrueStrengthForm TrueStrengthForm =
		SimpleLoader.LoadResource<TrueStrengthForm>("res://statuses/true_strength_form");
	
	protected override void ApplyEffect(Array<Node2D> targets, ModifierHandler modifierHandler) {
		StatusEffect statusEffect = new();
		var trueStrengthForm = TrueStrengthForm.Duplicate<TrueStrengthForm>();
		statusEffect.Status = trueStrengthForm;
		statusEffect.Execute(targets);
	}
}
