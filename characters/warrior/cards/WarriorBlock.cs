using Godot;
using Godot.Collections;

public partial class WarriorBlock : Card {
    protected override void ApplyEffect(Array<Node2D> targets) {
        BlockEffect blockEffect = new();
        blockEffect.Amount = 5;
        blockEffect.Sound = Sound;
        blockEffect.Execute(targets);
    }
}