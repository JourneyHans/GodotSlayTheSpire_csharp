using framework.extension;
using Godot;

public partial class CardBaseState : CardState {
    public override async void Enter() {
        if (!CardUI.IsNodeReady()) {
            await this.WhenReady(CardUI);
        }

        if (CardUI.Tween != null && CardUI.Tween.IsRunning()) {
            CardUI.Tween.Kill();
        }

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        EventDispatcher.TriggerEvent(CardUI.ReparentRequested, CardUI);

        // 重置PivotOffset，因为拖拽状态会通过修改这个属性达到拖拽时鼠标在卡牌中央的效果
        CardUI.PivotOffset = Vector2.Zero;
        EventDispatcher.TriggerEvent(Tooltip.Event.HideTips);
    }

    public override void OnGUIInput(InputEvent inputEvent) {
        if (!CardUI.Playable || CardUI.Disabled) {
            return;
        }

        if (!inputEvent.IsActionPressed(InputKey.LeftMouse)) {
            return;
        }

        CardUI.PivotOffset = CardUI.GetGlobalMousePosition() - CardUI.GlobalPosition;
        EmitSignal(CardState.SignalName.TransitionRequested, this, (int)EState.Clicked);
    }

    public override void OnMouseEntered() {
        if (!CardUI.Playable || CardUI.Disabled) {
            return;
        }

        CardUI.SetPanelStyleBox(CardUI.HoverStyleBox);
        EventDispatcher.TriggerEvent(Tooltip.Event.ShowTips, CardUI.Card.Icon, CardUI.Card.ToolTipTxt);
    }

    public override void OnMouseExited() {
        if (!CardUI.Playable || CardUI.Disabled) {
            return;
        }

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        EventDispatcher.TriggerEvent(Tooltip.Event.HideTips);
    }
}