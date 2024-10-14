using Godot;

[GlobalClass]
public partial class RunStartup : Resource {
    public enum Type {
        NewRun,
        ContinuedRun,
    }

    [Export] public Type type;
    [Export] public CharacterStats PickedCharacter;
}