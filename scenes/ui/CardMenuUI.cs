using framework.extension;
using framework.utils;
using Godot;

public partial class CardMenuUI : CenterContainer {
    public StyleBoxFlat BaseStyleBox =
        SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_base_style_box");

    public StyleBoxFlat HoverStyleBox =
        SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_hover_style_box");

    private Card _card;

    [Export]
    public Card Card {
        get => _card;
        set => SetCard(value);
    }

    private Panel _panel;
    public Label Cost { get; private set; }
    public TextureRect Icon { get; private set; }

    public override void _Ready() {
        _panel = GetNode<Panel>("Visuals/Panel");
        Cost = GetNode<Label>("Visuals/Cost");
        Icon = GetNode<TextureRect>("Visuals/Icon");
        
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        GuiInput += OnGuiInput;
    }

    private async void SetCard(Card card) {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _card = card;
        Cost.Text = _card.Cost.ToString();
        Icon.Texture = _card.Icon;
    }

    private void OnMouseEntered() {
        _panel.Set("theme_override_styles/panel", HoverStyleBox);
    }

    private void OnMouseExited() {
        _panel.Set("theme_override_styles/panel", BaseStyleBox);
    }

    private void OnGuiInput(InputEvent @event) {
        if (@event.IsActionPressed(InputKey.LeftMouse)) {
            EventDispatcher.TriggerEvent(CardTooltipPopup.Event.TooltipRequested, Card);
        }
    }
}