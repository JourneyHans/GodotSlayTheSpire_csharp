using framework.extension;
using Godot;

public partial class CardReleasedState : CardState {
    private bool _played;
    
    public override void Enter() {
        _played = false;
        if (!CardUI.Targets.IsNullOrEmpty()) {
            EventDispatcher.TriggerEvent(Tooltip.Event.HideTips);
            _played = true;
            CardUI.Play();
        }
    }

    public override void OnInput(InputEvent inputEvent) {
        if (_played) {
            return;
        }

        EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Base);
    }
}