using framework.debug;
using framework.events;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class CardRewards : ColorRect {
    private FinchLogger _logger;
    private static readonly PackedScene CardMenuUI = SimpleLoader.LoadPackedScene("res://scenes/ui/card_menu_ui");
    private Array<Card> _rewards;

    [Export]
    public Array<Card> Rewards {
        get => _rewards;
        set => SetRewards(value);
    }

    #region onready

    private HBoxContainer _cards;
    private Button _skipCardReward;
    private CardTooltipPopup _cardTooltipPopup;
    private Button _takeButton;

    #endregion
    
    private Card _selectedCard;

    public override void _Ready() {
        _logger = new FinchLogger(this);

        #region onready

        _cards = GetNode<HBoxContainer>("%Cards");
        _skipCardReward = GetNode<Button>("%SkipCardReward");
        _cardTooltipPopup = GetNode<CardTooltipPopup>("CardTooltipPopup");
        _takeButton = GetNode<Button>("%TakeButton");

        #endregion

        ClearRewards();

        _takeButton.Pressed += TakeRewardCardCall;
        _skipCardReward.Pressed += () => {
            _selectedCard = null;
            TakeRewardCardCall();
        };
    }

    private async void SetRewards(Array<Card> rewards) {
        _rewards = rewards;
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        ClearRewards();
        foreach (Card card in rewards) {
            CardMenuUI newCard = CardMenuUI.Instantiate<CardMenuUI>();
            _cards.AddChild(newCard);
            newCard.Card = card;
            newCard.SetClickCall(ShowTooltip);
        }
    }

    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed(InputKey.Esc)) {
            _cardTooltipPopup.HideTooltip();
        }
    }

    private void ClearRewards() {
        _cards.QueueFreeAllChildren();
        _cardTooltipPopup.HideTooltip();
        _selectedCard = null;
    }

    private void ShowTooltip(Card card) {
        _selectedCard = card;
        _cardTooltipPopup.ShowTooltip(_selectedCard);
    }

    private void TakeRewardCardCall() {
        EventDispatcher.TriggerEvent(Event.CardRewardSelected, _selectedCard);
        _logger.Log(_selectedCard == null ? "skipped card reward" : $"drafted {_selectedCard.Id}");
        QueueFree();
    }
}

public partial class CardRewards {
    public static class Event {
        /// <summary>
        /// 奖励卡片被选择
        /// 参数1：Card
        /// </summary>
        public const string CardRewardSelected = "card_reward_selected";
    }
}