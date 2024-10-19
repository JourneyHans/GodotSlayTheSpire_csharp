using System;
using System.Collections.Generic;
using framework.debug;
using framework.events;
using framework.utils;
using Godot;

public partial class Run : Node {
    private FinchLogger _logger;
    
    private PackedScene _battleScene = SimpleLoader.LoadPackedScene("res://scenes/battle/battle");
    private PackedScene _battleRewardScene = SimpleLoader.LoadPackedScene("res://scenes/battle_reward/battle_reward");
    private PackedScene _campfireScene = SimpleLoader.LoadPackedScene("res://scenes/campfire/campfire");
    private PackedScene _mapScene = SimpleLoader.LoadPackedScene("res://scenes/map/map");
    private PackedScene _shopScene = SimpleLoader.LoadPackedScene("res://scenes/shop/shop");
    private PackedScene _treasureScene = SimpleLoader.LoadPackedScene("res://scenes/treasure_room/treasure_room");

    #region onready
    
    private Node _currentView;
    private GoldUI _goldUI;
    private CardPileOpener _deckButton;
    private CardPileView _deckView;
    
    #endregion
    
    private RunStats _runStats;
    private CharacterStats _characterStats;

    private Dictionary<string, Action> _btnNameToPressed;
    
    [Export] public RunStartup RunStartup;

    public override void _Ready() {
        _logger = new FinchLogger(this);
        
        _currentView = GetNode<Node>("CurrentView");
        _goldUI = GetNode<GoldUI>("%GoldUI");
        _deckButton = GetNode<CardPileOpener>("%DeckButton");
        _deckView = GetNode<CardPileView>("%DeckView");

        if (RunStartup == null) {
            _logger.Error("There is no RunStartup");
            return;
        }

        switch (RunStartup.type) {
            case RunStartup.Type.NewRun:
                _characterStats = RunStartup.PickedCharacter.CreateInstance();
                StartRun();
                break;
            case RunStartup.Type.ContinuedRun:
                _logger.Log("TODO: load previous Run");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void Dispose(bool disposing) {
        UnRegAllEventListener();
    }

    private void StartRun() {
        _runStats = new RunStats();
        SetupEventConnections();
        SetupTopBar();
        _logger.Log("TODO: procedurally generate map");
    }

    private Node ChangeView(PackedScene scene) {
        if (_currentView.GetChildCount() > 0) {
            _currentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = scene.Instantiate();
        _currentView.AddChild(newView);
        return newView;
    }

    private void SetupTopBar() {
        _goldUI.RunStats = _runStats;
        _deckButton.CardPile = _characterStats.Deck;
        _deckView.CardPile = _characterStats.Deck;
        _deckButton.Pressed += () => { _deckView.ShowCurrentView("Deck"); };
    }

    private void SetupEventConnections() {
        EventDispatcher.RegEventListener(Battle.Event.BattleWon, OnBattleWon);
        EventDispatcher.RegEventListener(BattleReward.Event.BattleRewardExited, OnBattleRewardExited);
        EventDispatcher.RegEventListener(Campfire.Event.CampfireExited, OnCampfireExited);
        EventDispatcher.RegEventListener(Map.Event.MapExited, OnMapExited);
        EventDispatcher.RegEventListener(Shop.Event.ShopExited, OnShopExited);
        EventDispatcher.RegEventListener(TreasureRoom.Event.TreasureRoomExited, OnTreasureRoomExited);

        _btnNameToPressed = new Dictionary<string, Action> {
            { "%MapButton", () => { ChangeView(_mapScene); } },
            { "%BattleButton", () => { ChangeView(_battleScene); } },
            { "%ShopButton", () => { ChangeView(_shopScene); } },
            { "%TreasureButton", () => { ChangeView(_treasureScene); } },
            { "%RewardsButton", () => { ChangeView(_battleRewardScene); } },
            { "%CampfireButton", () => { ChangeView(_campfireScene); } },
        };
        foreach (var (btnName, func) in _btnNameToPressed) {
            GetNode<Button>(btnName).Pressed += func;
        }
    }

    private void UnRegAllEventListener() {
        EventDispatcher.UnRegEventListener(Battle.Event.BattleWon, OnBattleWon);
        EventDispatcher.UnRegEventListener(BattleReward.Event.BattleRewardExited, OnBattleRewardExited);
        EventDispatcher.UnRegEventListener(Campfire.Event.CampfireExited, OnCampfireExited);
        EventDispatcher.UnRegEventListener(Map.Event.MapExited, OnMapExited);
        EventDispatcher.UnRegEventListener(Shop.Event.ShopExited, OnShopExited);
        EventDispatcher.UnRegEventListener(TreasureRoom.Event.TreasureRoomExited, OnTreasureRoomExited);
    }

    private void OnBattleWon() {
        var rewardScene = (BattleReward)ChangeView(_battleRewardScene);
        rewardScene.RunStats = _runStats;
        rewardScene.CharacterStats = _characterStats;
        
        // TODO: 临时测试代码
        rewardScene.AddGoldReward(77);
        rewardScene.AddCardReward();
    }

    private void OnBattleRewardExited() {
        ChangeView(_mapScene);
    }

    private void OnCampfireExited() {
        ChangeView(_mapScene);
    }

    private void OnMapExited() {
        _logger.Log("TODO: from the MAP, change view based on room type");
    }

    private void OnShopExited() {
        ChangeView(_mapScene);
    }

    private void OnTreasureRoomExited() {
        ChangeView(_mapScene);
    }
}