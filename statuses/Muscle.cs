using framework.debug;
using Godot;

[GlobalClass]
public partial class Muscle : Status {
	private FinchLogger _logger;

	public override void InitializeStatus(Node2D target) {
		_logger = new FinchLogger(this);
		StatusChanged += OnStatusChanged;
	}

	private void OnStatusChanged() {
		_logger.Log($"Muscle status: +{Stacks} damage");
	}
}
