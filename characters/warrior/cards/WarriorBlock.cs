using Godot;
using Godot.Collections;

public partial class WarriorBlock : Card {
    private int _blockValue = 5;
    
    protected override void ApplyEffect(Array<Node2D> targets, ModifierHandler modifierHandler) {
        BlockEffect blockEffect = new();
        blockEffect.Amount = _blockValue;
        blockEffect.Sound = Sound;
        blockEffect.Execute(targets);
    }

    public override string GetDefaultTooltip() {
        return string.Format(ToolTipTxt, _blockValue);
    }

    public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers) {
        return GetDefaultTooltip();
    }
}