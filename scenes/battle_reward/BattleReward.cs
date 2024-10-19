using System.Linq;
using framework.debug;
using framework.events;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class BattleReward : Control {
    private FinchLogger _logger;

    private static readonly PackedScene CardRewards = SimpleLoader.LoadPackedScene("res://scenes/ui/card_rewards");
    private static readonly PackedScene RewardButtonScene = SimpleLoader.LoadPackedScene("res://scenes/ui/reward_button");
    private static readonly Texture GoldIcon = SimpleLoader.LoadTexture("res://art/gold");
    private const string GoldText = "{0} gold";
    private static readonly Texture CardIcon = SimpleLoader.LoadTexture("res://art/rarity");
    private const string CardText = "Add New Card";

    [Export] public RunStats RunStats;
    [Export] public CharacterStats CharacterStats;
    
    private Button _backButton;
    private VBoxContainer _rewards;

    private float _cardRewardTotalWeight;
    private Dictionary<Card.ERarity, float> _rarityToWeight = new() {
        { Card.ERarity.Common, 0f },
        { Card.ERarity.UnCommon, 0f },
        { Card.ERarity.Rare, 0f },
    };

    public override void _Ready() {
        _logger = new FinchLogger(this);
        
        EventDispatcher.RegEventListener(RunStats.Event.GoldChanged, OnGoldChanged);
        EventDispatcher.RegEventListener<Card>(global::CardRewards.Event.CardRewardSelected, OnCardRewardTaken);
        
        _backButton = GetNode<Button>("VBoxContainer/BackButton");
        _rewards = GetNode<VBoxContainer>("%Rewards");
        
        Reset();
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(RunStats.Event.GoldChanged, OnGoldChanged);
        EventDispatcher.UnRegEventListener<Card>(global::CardRewards.Event.CardRewardSelected, OnCardRewardTaken);
    }

    public void AddGoldReward(int amount) {
        RewardButton goldReward = RewardButtonScene.Instantiate<RewardButton>();
        goldReward.RewardIcon = GoldIcon;
        goldReward.RewardText = string.Format(GoldText, amount);
        goldReward.Pressed += () => { OnGoldRewardTaken(amount); };
        Callable.From(() => { _rewards.AddChild(goldReward); }).CallDeferred();
    }

    public void AddCardReward() {
        RewardButton cardReward = RewardButtonScene.Instantiate<RewardButton>();
        cardReward.RewardIcon = CardIcon;
        cardReward.RewardText = CardText;
        cardReward.Pressed += ShowCardRewards;
        Callable.From(() => { _rewards.AddChild(cardReward); }).CallDeferred();
    }

    private void ShowCardRewards() {
        if (RunStats == null || CharacterStats == null) {
            _logger.Error("RunStats or CharacterStats is null!");
            return;
        }

        CardRewards cardRewards = CardRewards.Instantiate<CardRewards>();
        AddChild(cardRewards);
        
        Array<Card> cardRewardArray = new();
        Array<Card> availableCards = CharacterStats.DraftableCards.Cards.Duplicate(true);

        for (int i = 0; i < RunStats.CardRewards; i++) {
            SetupChances();
            var roll = GD.RandRange(0.0, _cardRewardTotalWeight);

            foreach (var (rarity, weight) in _rarityToWeight) {
                if (weight > roll) {
                    ModifyWeights(rarity);
                    Card pickedCard = GetRandomAvailableCard(availableCards, rarity);
                    cardRewardArray.Add(pickedCard);
                    availableCards.Remove(pickedCard);
                    break;
                }
            }
        }
        
        cardRewards.Rewards = cardRewardArray;
        cardRewards.Show();
    }

    private void SetupChances() {
        _cardRewardTotalWeight = RunStats.CurrentTotalWeight;
        _rarityToWeight[Card.ERarity.Common] = RunStats.CommonWeight;
        _rarityToWeight[Card.ERarity.UnCommon] = RunStats.CommonWeight + RunStats.UncommonWeight;
        _rarityToWeight[Card.ERarity.Rare] = _cardRewardTotalWeight;
    }

    private void ModifyWeights(Card.ERarity rarity) {
        if (rarity == Card.ERarity.Rare) {
            RunStats.RareWeight = RunStats.BaseRareWeight;
        }
        else {
            RunStats.RareWeight = (float)Mathf.Clamp(RunStats.RareWeight + 0.3, RunStats.BaseRareWeight, 5.0);
        }
    }

    private Card GetRandomAvailableCard(Array<Card> cards, Card.ERarity rarity) {
        Array<Card> allPossibleCards = new Array<Card>(cards.Where(card => card.Rarity == rarity));
        return allPossibleCards.PickRandom();
    }

    private void OnGoldRewardTaken(int amount) {
        if (RunStats == null) {
            _logger.Error("RunStats is null");
            return;
        }

        RunStats.Gold += amount;
    }

    private void OnCardRewardTaken(Card card) {
        if (CharacterStats == null) {
            _logger.Error("CharacterStats is null!");
            return;
        }

        if (card == null) {
            // _logger.Log("Skip taken a card");
            return;
        }

        // _logger.Log($"Deck Before: \n{CharacterStats.Deck}\n");
        CharacterStats.Deck.AddCard(card);
        // _logger.Log($"Deck After: \n{CharacterStats.Deck}\n");
    }

    private void OnGoldChanged() {
        _logger.Log($"gold: {RunStats.Gold}");
    }

    private void Reset() {
        _rewards.QueueFreeAllChildren();
    }

    private void OnBackButtonPressed() {
        EventDispatcher.TriggerEvent(Event.BattleRewardExited);
    }
}

public partial class BattleReward {
    public static class Event {
        public const string BattleRewardExited = "BattleRewardExited";
    }
}