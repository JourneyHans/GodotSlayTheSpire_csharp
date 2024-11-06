using Godot;
using Godot.Collections;

public partial class StatusEffect : Effect {
	public Status Status;
	
	public override void Execute(Array<Node2D> targets) {
		foreach (Node2D target in targets) {
			if (target == null) {
				continue;
			}

			if (target is Enemy enemy) {
				enemy.StatusHandler.AddStatus(Status);
			}
			else if (target is Player player) {
				player.StatusHandler.AddStatus(Status);
			}
		}
	}
}
