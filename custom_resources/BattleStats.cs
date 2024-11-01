using Godot;

[GlobalClass]
public partial class BattleStats : Resource {
    [Export(PropertyHint.Range, "0, 2")] public int BattleTier;

    [Export(PropertyHint.Range, "0.0, 10.0")]
    public float Weight;

    [Export] public int GoldRewardMin;
    [Export] public int GoldRewardMax;
    [Export] public PackedScene Enemies;

    public float AccumulatedWeight;

    public int RollGoldReward() {
        return GD.RandRange(GoldRewardMin, GoldRewardMax);
    }
}