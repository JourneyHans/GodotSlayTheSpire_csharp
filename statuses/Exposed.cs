using framework.debug;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Exposed : Status {
	private FinchLogger _logger;

	private const float ModifierRate = 0.5f;
	private Modifier _dmgTakenModifier;

	public override void InitializeStatus(Node2D target) {
		_logger = new FinchLogger(this);
		ModifierHandler modifierHandler = target.GetNode<ModifierHandler>("ModifierHandler");
		_logger.Assert(modifierHandler != null, $"No modifier on {target}");

		_dmgTakenModifier = modifierHandler.GetModifier(Modifier.EType.DmgTaken);
		_logger.Assert(_dmgTakenModifier != null, $"No dmg taken modifier on {target}");

		ModifierValue exposedModifierValue = _dmgTakenModifier.GetValue("exposed");

		if (exposedModifierValue == null) {
			exposedModifierValue = ModifierValue.CreateNewModifier("exposed", ModifierValue.EType.PercentBased);
			exposedModifierValue.PercentValue = ModifierRate;
			_dmgTakenModifier.AddNewValue(exposedModifierValue);
		}

		StatusChanged -= OnStatusChanged;
		StatusChanged += OnStatusChanged;
	}

	private void OnStatusChanged() {
		if (Duration <= 0 && _dmgTakenModifier != null) {
			_dmgTakenModifier.RemoveValue("exposed");
		}
	}
}
