using framework.debug;
using framework.events;
using framework.extension;
using framework.utils;
using Godot;

public partial class RelicUI : Control {
    private FinchLogger _logger;
    
    private Relic _relic;

    [Export]
    public Relic Relic {
        get => _relic;
        set => SetRelic(value);
    }

    #region onready

    private TextureRect _icon;
    private AnimationPlayer _animationPlayer;

    #endregion

    public override void _Ready() {
        _logger = new FinchLogger(this);
        GuiInput += OnGuiInput;
        _icon = GetNode<TextureRect>("Icon");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private void OnGuiInput(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed(InputKey.LeftMouse)) {
            EventDispatcher.TriggerEvent(RelicTooltip.Event.RelicTooltipRequest, _relic);
        }
    }

    private async void SetRelic(Relic relic) {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _relic = relic;
        _icon.Texture = relic.Icon;
        _relic.InitializeRelic(this);
    }

    public void Flash() {
        _animationPlayer.Play("flash");
    }
}

public partial class RelicUI {
    private static readonly PackedScene
        PackedScene = SimpleLoader.LoadPackedScene("res://scenes/relic_handler/relic_ui");

    public static RelicUI Instantiate() => PackedScene.Instantiate<RelicUI>();
}