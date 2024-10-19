using System;
using framework.extension;
using Godot;

public partial class CardMenuUI : CenterContainer {

    private Card _card;

    [Export]
    public Card Card {
        get => _card;
        set => SetCard(value);
    }

    private CardVisuals _visuals;
    
    private Action<Card> _clickCall;

    public override void _Ready() {
        _visuals = GetNode<CardVisuals>("Visuals");
        _visuals.MouseEntered += OnMouseEntered;
        _visuals.MouseExited += OnMouseExited;
        _visuals.GuiInput += OnGuiInput;
    }

    public void SetClickCall(Action<Card> clickCall) {
        _clickCall = clickCall;
    }

    private async void SetCard(Card card) {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _card = card;
        _visuals.Card = Card;
    }

    private void OnMouseEntered() {
        _visuals.SetPanelStyle(CardVisuals.EPanelStyle.Hover);
    }

    private void OnMouseExited() {
        _visuals.SetPanelStyle(CardVisuals.EPanelStyle.Base);
    }

    private void OnGuiInput(InputEvent @event) {
        if (@event.IsActionPressed(InputKey.LeftMouse)) {
            _clickCall?.Invoke(Card);
        }
    }
}