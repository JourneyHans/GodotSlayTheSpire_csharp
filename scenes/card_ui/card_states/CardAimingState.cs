using framework.events;
using Godot;

public partial class CardAimingState : CardState {
    private const int MouseYSnapBackThreshold = 138;

    public override void Enter() {
        CardUI.Targets.Clear();

        Vector2 offset = new(CardUI.Parent.Size.X / 2, -CardUI.Parent.Size.Y / 2);
        offset.X -= CardUI.Size.X / 2;
        CardUI.AnimateToPosition(CardUI.Parent.GlobalPosition + offset, 0.2f);
        CardUI.DropPointDetector.Monitoring = false;
        EventDispatcher.TriggerEvent(Event.CardAimStarted, CardUI);
    }

    public override void Exit() {
        EventDispatcher.TriggerEvent(Event.CardAimEnded, CardUI);
    }

    public override void OnInput(InputEvent inputEvent) {
        bool isMouseMotion = inputEvent is InputEventMouseMotion;
        bool isMouseAtBottom = CardUI.GetGlobalMousePosition().Y > MouseYSnapBackThreshold;

        if (isMouseMotion && isMouseAtBottom || inputEvent.IsActionPressed(InputKey.RightMouse)) {
            EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Base);
        }
        else if (inputEvent.IsActionReleased(InputKey.LeftMouse) ||
                 inputEvent.IsActionPressed(InputKey.LeftMouse)) {
            EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Released);
        }
    }
}