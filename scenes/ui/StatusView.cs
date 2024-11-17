using framework.events;
using Godot;
using Godot.Collections;

public partial class StatusView : Control {
    #region onready

    private Button _backButton;
    private VBoxContainer _statusTooltips;

    #endregion

    public override void _Ready() {
        GuiInput += OnGuiInput;
        
        _backButton = GetNode<Button>("BackButton");
        _backButton.Pressed += OnBackButtonPressed;
        
        _statusTooltips = GetNode<VBoxContainer>("%StatusTooltips");

        foreach (StatusTooltip tooltip in _statusTooltips.GetChildren()) {
            tooltip.QueueFree();
        }

        EventDispatcher.RegEventListener<Array<Status>>(Event.StatusTooltipRequested, ShowView);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Array<Status>>(Event.StatusTooltipRequested, ShowView);
    }

    public override void _Input(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed(InputKey.Esc) && Visible) {
            HideView();
        }
    }

    public void ShowView(Array<Status> statuses) {
        foreach (Status status in statuses) {
            var newStatusTooltip = StatusTooltip.Instantiate();
            _statusTooltips.AddChild(newStatusTooltip);
            newStatusTooltip.Status = status;
        }

        Show();
    }

    public void HideView() {
        foreach (StatusTooltip tooltip in _statusTooltips.GetChildren()) {
            tooltip.QueueFree();
        }

        Hide();
    }

    private void OnGuiInput(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed(InputKey.LeftMouse) && Visible) {
            HideView();
        }
    }

    private void OnBackButtonPressed() {
        HideView();
    }
}

public partial class StatusView {
    public class Event {
        /// <summary>
        /// 显示状态详情界面
        /// @1 Array[Status]
        /// </summary>
        public const string StatusTooltipRequested = "StatusView.StatusTooltipRequested";
    }
}