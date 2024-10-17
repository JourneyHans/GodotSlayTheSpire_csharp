using Godot;
using System;
using framework.utils;

public partial class CardTooltipPopup : Control {
    private PackedScene _cardMenuUIPackedScene = SimpleLoader.LoadPackedScene("res://scenes/ui/card_menu_ui");
    private CenterContainer _tooltipCard;
    private RichTextLabel _cardDescription;

    public override void _Ready() {
        _tooltipCard = GetNode<CenterContainer>("%TooltipCard");
        _cardDescription = GetNode<RichTextLabel>("%CardDescription");
        
        GuiInput += OnGuiInput;

        foreach (CardMenuUI cardMenuUI in _tooltipCard.GetChildren()) {
            cardMenuUI.QueueFree();
        }

        HideTooltip();
        
        EventDispatcher.RegEventListener<Card>(Event.TooltipRequested, OnTooltipRequested);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Card>(Event.TooltipRequested, OnTooltipRequested);
    }

    public void ShowTooltip(Card card) {
        var newCard = _cardMenuUIPackedScene.Instantiate<CardMenuUI>();
        _tooltipCard.AddChild(newCard);
        newCard.Card = card;
        _cardDescription.Text = card.ToolTipTxt;
        Show();
    }

    public void HideTooltip() {
        if (!Visible) {
            return;
        }

        foreach (CardMenuUI cardMenuUI in _tooltipCard.GetChildren()) {
            cardMenuUI.QueueFree();
        }

        Hide();
    }

    private void OnTooltipRequested(Card card) {
        HideTooltip();
    }

    private void OnGuiInput(InputEvent @event) {
        if (Input.IsActionPressed(InputKey.LeftMouse)) {
            HideTooltip();
        }
    }
}

public partial class CardTooltipPopup {
    public static class Event {
        /// <summary>
        /// 请求打开卡牌信息界面
        /// 参数1：Card
        /// </summary>
        public const string TooltipRequested = "TooltipRequested";
    }
}