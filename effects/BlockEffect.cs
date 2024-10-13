using Godot;
using Godot.Collections;

public partial class BlockEffect : Effect {
    public int Amount;

    public override void Execute(Array<Node2D> targets) {
        foreach (Node2D target in targets) {
            switch (target) {
                case Enemy enemy:
                    enemy.Stats.Block += Amount;
                    break;
                case Player player:
                    player.Stats.Block += Amount;
                    break;
                default:
                    Logger.Error($"未定义实现: {target.GetType().Name}");
                    return;
            }

            AudioPlayer.PlaySFX(Sound);
        }
    }
}