// meta-name: EnemyAction
// meta-description: An action which can be performed by an enemy during its turn.

using framework.extension;
using Godot;

public partial class _CLASS_Action : EnemyAction {
    public override void PerformAction() {
        Tween tween = CreateTween().SetTrans(Tween.TransitionType.Quint);
        Vector2 start = Enemy.GlobalPosition;
        Vector2 end = Target.GlobalPosition + Vector2.Right * 32;
    }

    // e.g.
    public override async void UpdateIntentText() {
        if (Target is not Player player) {
            return;
        }

        if (!player.IsNodeReady()) {
            await player.WhenReady();
        }

        int modifiedDmg = player.ModifierHandler.GetModifierValue(6, Modifier.EType.DmgTaken);
        Intent.UpdateCurrentText(modifiedDmg);
    }
}