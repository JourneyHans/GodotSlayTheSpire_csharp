using Godot;
using Godot.Collections;

public partial class WarriorSlash : Card {
	protected override void ApplyEffect(Array<Node2D> targets) {
		DamageEffect damageEffect = new();
		damageEffect.Amount = 4;
		damageEffect.Sound = Sound;
		damageEffect.Execute(targets);
	}
}
