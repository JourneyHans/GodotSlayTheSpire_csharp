using System;
using System.Linq;
using System.Threading.Tasks;
using framework.debug;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class RelicHandler : HBoxContainer {
    private FinchLogger _logger;
    public event Action<Relic.EType> RelicsActivated;

    private const float RelicApplyInterval = 0.5f;

    #region onready

    private RelicsControl _relicsControl;
    private HBoxContainer _relics;

    #endregion

    public override void _Ready() {
        _logger = new FinchLogger(this);
        
        _relicsControl = GetNode<RelicsControl>("RelicsControl");
        _relics = GetNode<HBoxContainer>("%Relics");
        
        _relics.ChildExitingTree += OnRelicsChildExitingTree;
    }

    public void ActivateRelicsByType(Relic.EType type) {
        if (type == Relic.EType.EventBased) {
            return;
        }

        Array<RelicUI> relicQueue = GetAllRelicUINodes().Filter(relicUI => relicUI.Relic.Type == type);
        
        if (relicQueue.IsNullOrEmpty()) {
            Finish();
            return;
        }

        Tween tween = CreateTween();
        foreach (RelicUI relicUI in relicQueue) {
            tween.TweenCallback(Callable.From(() => {
                relicUI.Relic.ActivateRelic(relicUI);
            }));
            tween.TweenInterval(RelicApplyInterval);
        }

        tween.Finished += Finish;
        return;
        
        void Finish() {
            RelicsActivated?.Invoke(type);
        }
    }

    public void AddRelics(Array<Relic> relics) {
        foreach (Relic relic in relics) {
            AddRelic(relic);
        }
    }

    public void AddRelic(Relic relic) {
        if (HasRelic(relic.Id)) {
            return;
        }

        RelicUI newRelicUI = RelicUI.Instantiate();
        newRelicUI.Relic = relic;
        _relics.AddChild(newRelicUI);
    }

    private bool HasRelic(string id) {
        foreach (RelicUI relicUI in _relics.GetChildren()) {
            if (IsInstanceValid(relicUI) && relicUI.Relic.Id == id) {
                return true;
            }
        }

        return false;
    }

    private Array<Relic> GetAllRelics() {
        Array<RelicUI> relicUINodes = GetAllRelicUINodes();
        Array<Relic> relics = new();
        foreach (RelicUI relicUI in relicUINodes) {
            relics.Add(relicUI.Relic);
        }

        return relics;
    }

    private Array<RelicUI> GetAllRelicUINodes() {
        Array<RelicUI> allRelicUIs = new();
        allRelicUIs.AddRange(_relics.GetChildren<RelicUI>());
        return allRelicUIs;
    }

    private void OnRelicsChildExitingTree(Node node) {
        if (node is not RelicUI relicUI) {
            return;
        }

        relicUI.Relic?.DeactivateRelic(relicUI);
    }
}