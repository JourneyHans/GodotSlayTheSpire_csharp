using Godot;
using Godot.Collections;

public partial class DamageEffect : Effect {
    public int Amount;
    public Modifier.EType ReceiverModifierType = Modifier.EType.DmgTaken;
    
    public override void Execute(Array<Node2D> targets) {
        foreach (Node2D target in targets) {
            switch (target) {
                case Enemy enemy:
                    enemy.TakeDamage(Amount, ReceiverModifierType);
                    break;
                case Player player:
                    player.TakeDamage(Amount, ReceiverModifierType);
                    break;
                default:
                    Logger.Error($"未定义实现: {target.GetType().Name}");
                    return;
            }

            AudioPlayer.PlaySFX(Sound);
        }
    }
}