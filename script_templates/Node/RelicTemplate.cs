// meta-name: Relic
// meta-description: Create a Relic which can be acuired by the player

public partial class _CLASS_Relic : Relic {
    private int _memberVar = 0;

    public override void InitializeRelic(RelicUI owner) {
        Logger.Log("this happens once when we gain a new relic");
    }

    public override void ActivateRelic(RelicUI owner) {
        Logger.Log("this happens at specific times based on the Relic.Type property");
    }

    public override void DeactivateRelic(RelicUI owner) {
        Logger.Log("this gets called when a RelicUI is exiting the SceneTree i.e. getting deleted");
        Logger.Log("Event-based Relics should disconnect from the EventBus here.");
    }
}