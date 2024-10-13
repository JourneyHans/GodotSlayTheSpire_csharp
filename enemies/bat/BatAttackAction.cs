using framework.extension;
using Godot;
using Godot.Collections;

public partial class BatAttackAction : EnemyAction {
	[Export] public int Damage = 4;
	
	public override void PerformAction() {
		Tween tween = CreateTween().SetTrans(Tween.TransitionType.Quint);
		Vector2 start = Enemy.GlobalPosition;
		Vector2 end = Target.GlobalPosition + Vector2.Right * 32;

		DamageEffect damageEffect = new();
		damageEffect.Amount = Damage;
		damageEffect.Sound = Sound;

		Array<Node2D> targets = new() { Target };
		tween.DoMove(Enemy, end, 0.4f);
		tween.TweenCallback(Callable.From(() => { damageEffect.Execute(targets); }));
		tween.TweenInterval(0.35f);
		tween.TweenCallback(Callable.From(() => { damageEffect.Execute(targets); }));
		tween.TweenInterval(0.25f);
		tween.DoMove(Enemy, start, 0.4f);
		tween.Finished += EnemyActionCompleted;
	}
}
