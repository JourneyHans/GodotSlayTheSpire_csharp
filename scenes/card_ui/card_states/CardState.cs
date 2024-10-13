using framework.tag;
using Godot;

[GlobalClass]
public abstract partial class CardState : TagNode {
    public enum EState {
        Base,
        Clicked,
        Dragging,
        Aiming,
        Released
    }

    [Signal]
    public delegate void TransitionRequestedEventHandler(CardState from, EState to);

    [Export] public EState State;

    public CardUI CardUI;

    public abstract void Enter();

    public virtual void Exit() {

    }

    public virtual void OnInput(InputEvent inputEvent) {

    }

    public virtual void OnGUIInput(InputEvent inputEvent) {

    }

    public virtual void OnMouseEntered() {

    }

    public virtual void OnMouseExited() {

    }
}