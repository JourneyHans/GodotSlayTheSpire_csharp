using framework.events;
using Godot;

public partial class CardDraggingState : CardState {

	private const float DragMinimumThreshold = 0.05f;
	private bool _minimumDragTimeElapsed;

	public override void Enter() {
		Node uiLayer = GetTree().GetFirstNodeInGroup("ui_layer");
		if (uiLayer != null) {
			CardUI.Reparent(uiLayer);
		}

		CardUI.CardVisuals.SetPanelStyle(CardVisuals.EPanelStyle.Dragging);
		EventDispatcher.TriggerEvent(Event.CardDragStarted, CardUI);

		_minimumDragTimeElapsed = false;
		SceneTreeTimer thresholdTimer = GetTree().CreateTimer(DragMinimumThreshold, false);
		thresholdTimer.Timeout += () => _minimumDragTimeElapsed = true;
	}

	public override void Exit() {
		EventDispatcher.TriggerEvent(Event.CardDragEnded, CardUI);
	}

	public override void OnInput(InputEvent inputEvent) {
		bool isSingleTargeted = CardUI.Card.IsSingleTarget;
		bool isMouseMotion = inputEvent is InputEventMouseMotion;
		bool cancel = inputEvent.IsActionPressed(InputKey.RightMouse);
		bool confirm = inputEvent.IsActionReleased(InputKey.LeftMouse) ||
		               inputEvent.IsActionPressed(InputKey.LeftMouse);

		if (isSingleTargeted && isMouseMotion && CardUI.Targets.Count > 0) {
			EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Aiming);
			return;
		}
		
		if (isMouseMotion) {
			CardUI.GlobalPosition = CardUI.GetGlobalMousePosition() - CardUI.PivotOffset;
		}

		if (cancel) {
			EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Base);
		}
		else if (_minimumDragTimeElapsed && confirm) {
			GetViewport().SetInputAsHandled();
			EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Released);
		}
	}
}