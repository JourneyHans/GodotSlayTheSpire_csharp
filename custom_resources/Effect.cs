using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class Effect : RefCounted {
    public AudioStream Sound;
    
    public abstract void Execute(Array<Node2D> targets);
}