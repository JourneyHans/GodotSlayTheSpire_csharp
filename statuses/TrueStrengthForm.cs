using framework.debug;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TrueStrengthForm : Status {
	private FinchLogger _logger;
	private static readonly Muscle MuscleRes = SimpleLoader.LoadResource<Muscle>("res://statuses/muscle");

	private int _stacksPerTurn = 2;
	
	public override void InitializeStatus(Node2D target) {
		_logger = new FinchLogger(this);
	}

	public override void ApplyStatus(Node2D target) {
		_logger.Log("applied true strength form");

		StatusEffect statusEffect = new();
		Muscle muscle = MuscleRes.Duplicate<Muscle>();
		muscle.Stacks = _stacksPerTurn;
		statusEffect.Status = muscle;
		statusEffect.Execute(new Array<Node2D> { target });
		
		base.ApplyStatus(target);
	}
}
