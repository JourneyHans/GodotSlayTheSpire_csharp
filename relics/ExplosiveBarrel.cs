using framework.extension;
using Godot;

public partial class ExplosiveBarrel : Relic {
	[Export] private int _damage = 2;
	
	public override void ActivateRelic(RelicUI owner) {
		var enemies = owner.GetTree().GetNodesInGroup<Node2D>("enemies");
		var damageEffect = new DamageEffect();
		damageEffect.Amount = _damage;
		damageEffect.ReceiverModifierType = Modifier.EType.NoModifier;	// 让爆炸桶圣物的伤害类型不受修改器的影响
		damageEffect.Execute(enemies);
		
		owner.Flash();
	}
}
