using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class WarriorBigSlam : Card {
	private static readonly Status ExposeStatus = SimpleLoader.LoadResource<Status>("res://statuses/exposed");

	private int _baseDamage = 4;
	private int _exposedDuration = 2;
	
	protected override void ApplyEffect(Array<Node2D> targets) {
		DamageEffect damageEffect = new();
		damageEffect.Amount = _baseDamage;
		damageEffect.Sound = Sound;
		damageEffect.Execute(targets);
		
		StatusEffect statusEffect = new();
		Status exposed = ExposeStatus.Duplicate<Status>();
		exposed.Duration = _exposedDuration;
		statusEffect.Status = exposed;
		statusEffect.Execute(targets);
	}
}
