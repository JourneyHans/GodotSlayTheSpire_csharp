using System;
using Godot;

public partial class CardClickedState : CardState {
	public override void Enter() {
		CardUI.DropPointDetector.Monitoring = true;
		CardUI.OriginalIndex = CardUI.GetIndex();
	}

	public override void OnInput(InputEvent inputEvent) {
		if (inputEvent is InputEventMouseMotion motionEvent && motionEvent.Relative != Vector2.Zero) {
			EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Dragging);
		}
	}
}