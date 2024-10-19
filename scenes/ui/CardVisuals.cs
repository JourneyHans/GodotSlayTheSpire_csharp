using Godot;
using System.Collections.Generic;
using framework.extension;
using framework.utils;

public partial class CardVisuals : Control {
    public enum EPanelStyle {
        Base,
        Hover,
        Dragging,
    }

    private static readonly Dictionary<EPanelStyle, StyleBoxFlat> Styles = new() {
        {
            EPanelStyle.Base,
            SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_base_style_box")
        }, {
            EPanelStyle.Hover,
            SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_hover_style_box")
        }, {
            EPanelStyle.Dragging,
            SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_dragging_style_box")
        },
    };
    
    private Card _card;

    [Export]
    public Card Card {
        get => _card;
        set => SetCard(value);
    }

    #region onReady

    private Panel _panel;
    public Label Cost { get; private set; }
    public TextureRect Icon { get; private set; }
    private TextureRect _rarity;

    #endregion

    public override void _Ready() {
        _panel = GetNode<Panel>("Panel");
        Cost = GetNode<Label>("Cost");
        Icon = GetNode<TextureRect>("Icon");
        _rarity = GetNode<TextureRect>("Rarity");
    }

    // Card Setter
    private async void SetCard(Card card) {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _card = card;
        Cost.Text = Card.Cost.ToString();
        Icon.Texture = Card.Icon;
        _rarity.Modulate = Card.RarityToColor[card.Rarity];
    }

    public void SetPanelStyle(EPanelStyle style) {
        _panel.Set("theme_override_styles/panel", Styles[style]);
    }
}