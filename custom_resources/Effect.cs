using framework.debug;
using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class Effect : RefCounted {
    public AudioStream Sound;
    
    public abstract void Execute(Array<Node2D> targets);
    
    protected FinchLogger Logger;

    public Effect() {
        Logger = new FinchLogger(this);
    }
}