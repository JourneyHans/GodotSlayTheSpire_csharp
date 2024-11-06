using framework.debug;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Exposed : Status {
	private FinchLogger _logger;

	private const float Modifier = 0.5f;

	public override void InitializeStatus(Node2D target) {
		_logger = new FinchLogger(this);
		_logger.Log($"Initialize my status for target: {target}");
	}

	public override void ApplyStatus(Node2D target) {
		_logger.Log($"{target} should take {Modifier * 100}% more damage");

		DamageEffect damageEffect = new();
		damageEffect.Amount = 12;
		damageEffect.Execute(new Array<Node2D> { target });
		
		base.ApplyStatus(target);
	}
}
