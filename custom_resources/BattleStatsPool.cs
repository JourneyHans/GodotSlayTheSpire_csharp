using System.Linq;
using framework.extension;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BattleStatsPool : Resource {
    [Export] public Array<BattleStats> Pool;

    private Array<float> _totalWeightsByTier = new() { 0f, 0f, 0f };

    public Array<BattleStats> GetAllBattlesForTier(int tier) {
        return Pool.Filter(battle => battle.BattleTier == tier);
    }

    public void Setup() {
        for (int i = 0; i < 3; i++) {
            SetupWeightForTier(i);
        }
    }

    private void SetupWeightForTier(int tier) {
        Array<BattleStats> battles = GetAllBattlesForTier(tier);
        _totalWeightsByTier[tier] = 0f;

        foreach (BattleStats battle in battles) {
            _totalWeightsByTier[tier] += battle.Weight;
            battle.AccumulatedWeight = _totalWeightsByTier[tier];
        }
    }

    public BattleStats GetRandomBattleForTier(int tier) {
        double roll = GD.RandRange(0f, _totalWeightsByTier[tier]);
        Array<BattleStats> battles = GetAllBattlesForTier(tier);
        return battles.FirstOrDefault(battle => battle.AccumulatedWeight > roll);
    }
}