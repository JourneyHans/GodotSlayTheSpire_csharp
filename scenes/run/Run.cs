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
    private PackedScene _shopScene = SimpleLoader.LoadPackedScene("res://scenes/shop/shop");
    private PackedScene _treasureScene = SimpleLoader.LoadPackedScene("res://scenes/treasure_room/treasure_room");

    #region onready

    private Map _map;
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

        _map = GetNode<Map>("Map");
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

        _map.GenerateNewMap();
        _map.UnlockFloor(0);
    }

    private Node ChangeView(PackedScene scene) {
        if (_currentView.GetChildCount() > 0) {
            _currentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = scene.Instantiate();
        _currentView.AddChild(newView);

        _map.HideMap();

        return newView;
    }

    private void ShowMap() {
        if (_currentView.GetChildCount() > 0) {
            _currentView.GetChild(0).QueueFree();
        }

        _map.ShowMap();
        _map.UnlockNextRooms();
    }

    private void SetupTopBar() {
        _goldUI.RunStats = _runStats;
        _deckButton.CardPile = _characterStats.Deck;
        _deckView.CardPile = _characterStats.Deck;
        _deckButton.Pressed += () => { _deckView.ShowCurrentView("Deck"); };
    }

    private void SetupEventConnections() {
        EventDispatcher.RegEventListener(Battle.Event.BattleWon, OnBattleWon);
        EventDispatcher.RegEventListener(BattleReward.Event.BattleRewardExited, ShowMap);
        EventDispatcher.RegEventListener(Campfire.Event.CampfireExited, ShowMap);
        EventDispatcher.RegEventListener<Room>(Map.Event.MapExited, OnMapExited);
        EventDispatcher.RegEventListener(Shop.Event.ShopExited, ShowMap);
        EventDispatcher.RegEventListener(TreasureRoom.Event.TreasureRoomExited, ShowMap);

        _btnNameToPressed = new Dictionary<string, Action> {
            { "%MapButton", ShowMap },
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
        EventDispatcher.UnRegEventListener(BattleReward.Event.BattleRewardExited, ShowMap);
        EventDispatcher.UnRegEventListener(Campfire.Event.CampfireExited, ShowMap);
        EventDispatcher.UnRegEventListener<Room>(Map.Event.MapExited, OnMapExited);
        EventDispatcher.UnRegEventListener(Shop.Event.ShopExited, ShowMap);
        EventDispatcher.UnRegEventListener(TreasureRoom.Event.TreasureRoomExited, ShowMap);
    }

    private void OnBattleRoomEntered(Room room) {
        var battleScene = (Battle)ChangeView(_battleScene);
        battleScene.CharacterStats = _characterStats;
        battleScene.BattleStats = room.BattleStats;
        battleScene.StartBattle();
    }

    private void OnBattleWon() {
        var rewardScene = (BattleReward)ChangeView(_battleRewardScene);
        rewardScene.RunStats = _runStats;
        rewardScene.CharacterStats = _characterStats;

        rewardScene.AddGoldReward(_map.LastRoom.BattleStats.RollGoldReward());
        rewardScene.AddCardReward();
    }

    private void OnMapExited(Room room) {
        switch (room.Type) {
            case Room.EType.Monster:
                OnBattleRoomEntered(room);
                break;
            case Room.EType.Treasure:
                ChangeView(_treasureScene);
                break;
            case Room.EType.Campfire:
                ChangeView(_campfireScene);
                break;
            case Room.EType.Shop:
                ChangeView(_shopScene);
                break;
            case Room.EType.Boss:
                OnBattleRoomEntered(room);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}