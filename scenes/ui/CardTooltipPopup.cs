using Godot;
using framework.debug;
using framework.extension;
using framework.utils;

public partial class CardTooltipPopup : Control {
    private PackedScene _cardMenuUIPackedScene = SimpleLoader.LoadPackedScene("res://scenes/ui/card_menu_ui");

    [Export] public Color BackgroundColor = new("000000b0");

    #region onready

    private ColorRect _background;
    private CenterContainer _tooltipCard;
    private RichTextLabel _cardDescription;

    #endregion
    
    public override void _Ready() {
        _background = GetNode<ColorRect>("Background");
        _tooltipCard = GetNode<CenterContainer>("%TooltipCard");
        _cardDescription = GetNode<RichTextLabel>("%CardDescription");
        
        GuiInput += OnGuiInput;
        
        _tooltipCard.QueueFreeAllChildren();
        
        _background.Color = BackgroundColor;
    }

    public void ShowTooltip(Card card) {
        var newCard = _cardMenuUIPackedScene.Instantiate<CardMenuUI>();
        _tooltipCard.AddChild(newCard);
        newCard.Card = card;
        newCard.SetClickCall(_ => HideTooltip());
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

    private void OnGuiInput(InputEvent @event) {
        if (Input.IsActionPressed(InputKey.LeftMouse)) {
            HideTooltip();
        }
    }
}