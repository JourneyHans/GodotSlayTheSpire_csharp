using Godot;
using Godot.Collections;

public partial class WarriorBigSlam : Card {
	protected override void ApplyEffect(Array<Node2D> targets) {
		DamageEffect damageEffect = new();
		damageEffect.Amount = 10;
		damageEffect.Sound = Sound;
		damageEffect.Execute(targets);
	}
}
