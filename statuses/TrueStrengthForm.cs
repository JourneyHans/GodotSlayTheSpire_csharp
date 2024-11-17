using framework.debug;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TrueStrengthForm : Status {
	private FinchLogger _logger;
	private static readonly Muscle MuscleRes = SimpleLoader.LoadResource<Muscle>("res://statuses/muscle");

	public int StacksPerTurn = 2;
	
	public override void InitializeStatus(Node2D target) {
		_logger = new FinchLogger(this);
	}

	public override void ApplyStatus(Node2D target) {
		_logger.Log("applied true strength form");

		StatusEffect statusEffect = new();
		Muscle muscle = MuscleRes.Duplicate<Muscle>();
		muscle.Stacks = StacksPerTurn;
		statusEffect.Status = muscle;
		statusEffect.Execute(new Array<Node2D> { target });
		
		base.ApplyStatus(target);
	}
}

public partial class TrueStrengthForm {
	public static readonly TrueStrengthForm TrueStrengthFormRes =
		SimpleLoader.LoadResource<TrueStrengthForm>("res://statuses/true_strength_form");

	public static TrueStrengthForm Duplicate() {
		return TrueStrengthFormRes.Duplicate<TrueStrengthForm>();
	}
}