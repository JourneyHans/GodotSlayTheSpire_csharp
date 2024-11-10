using framework.debug;
using Godot;

[GlobalClass]
public partial class Muscle : Status {
	private FinchLogger _logger;

	public override void InitializeStatus(Node2D target) {
		_logger = new FinchLogger(this);
		StatusChanged += () => { OnStatusChanged(target); };
		OnStatusChanged(target);
	}

	private void OnStatusChanged(Node2D target) {
		ModifierHandler modifierHandler = target.GetNode<ModifierHandler>("ModifierHandler");
		_logger.Assert(modifierHandler != null, $"No modifier on {target}");

		Modifier dmgDealtModifier = modifierHandler.GetModifier(Modifier.EType.DmgDealt);
		_logger.Assert(dmgDealtModifier != null, $"No dmg dealt modifier on {target}");

		ModifierValue modifierValue = dmgDealtModifier.GetValue("muscle") ??
		                              ModifierValue.CreateNewModifier("muscle", ModifierValue.EType.Flat);
		modifierValue.FlatValue = Stacks;
		dmgDealtModifier.AddNewValue(modifierValue);
	}
}
