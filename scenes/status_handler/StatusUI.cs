using framework.debug;
using framework.extension;
using framework.utils;
using Godot;

public partial class StatusUI : Control {
    private FinchLogger _logger;
    
    private Status _status;
    [Export]
    public Status Status {
        get => _status;
        set => SetStatus(value);
    }

    #region onready

    private TextureRect _icon;
    private Label _duration;
    private Label _stacks;

    #endregion

    public override void _Ready() {
        _logger = new FinchLogger(this);
        _icon = GetNode<TextureRect>("Icon");
        _duration = GetNode<Label>("Duration");
        _stacks = GetNode<Label>("Stacks");
    }

    private async void SetStatus(Status status) {
        if (!IsNodeReady()) {
            await this.WhenReady();
        }

        _status = status;
        _icon.Texture = _status.Icon;
        _duration.Visible = _status.StackType == Status.EStackType.Duration;
        _stacks.Visible = _status.StackType == Status.EStackType.Intensity;

        if (_duration.Visible) {
            CustomMinimumSize = _duration.Size + _duration.Position;
        }
        else if (_stacks.Visible) {
            CustomMinimumSize = _stacks.Size + _stacks.Position;
        }
        else {
            CustomMinimumSize = _icon.Size;
        }

        _status.StatusChanged -= OnStatusChanged;
        _status.StatusChanged += OnStatusChanged;
        OnStatusChanged();
    }

    private void OnStatusChanged() {
        if (Status == null) {
            _logger.Error("status is null");
            return;
        }

        if (Status.CanExpire && Status.Duration <= 0) {
            QueueFree();
        }

        if (Status.StackType == Status.EStackType.Intensity && Status.Stacks == 0) {
            QueueFree();
        }
        
        _duration.Text = Status.Duration.ToString();
        _stacks.Text = Status.Stacks.ToString();
    }
}

public partial class StatusUI {
    private static readonly PackedScene Scene = SimpleLoader.LoadPackedScene("res://scenes/status_handler/status_ui");

    public static StatusUI Instantiate() {
        return Scene.Instantiate<StatusUI>();
    }
}