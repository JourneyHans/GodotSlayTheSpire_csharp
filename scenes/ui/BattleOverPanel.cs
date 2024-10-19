using framework.events;
using Godot;

public partial class BattleOverPanel : Panel {
    public enum Type {
        Win,
        Lose
    };

    public Label Label;
    public Button ContinueButton;
    public Button RestartButton;

    public override void _Ready() {
        Label = GetNode<Label>("%Label");
        ContinueButton = GetNode<Button>("%ContinueButton");
        RestartButton = GetNode<Button>("%RestartButton");

        ContinueButton.Pressed += () => EventDispatcher.TriggerEvent(Battle.Event.BattleWon);
        RestartButton.Pressed += () => GetTree().ReloadCurrentScene();

        EventDispatcher.RegEventListener<string, Type>(Event.BattleOverScreenRequested, ShowScreen);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<string, Type>(Event.BattleOverScreenRequested, ShowScreen);
    }

    private void ShowScreen(string text, Type type) {
        Label.Text = text;
        ContinueButton.Visible = type == Type.Win;
        RestartButton.Visible = type == Type.Lose;
        Show();
        GetTree().Paused = true;
    }
}

public partial class BattleOverPanel {
    public static class Event {
        /// <summary>
        /// 请求战斗结束界面
        /// 参数1：string
        /// 参数2：BattleOverPanel.Type
        /// </summary>
        public const string BattleOverScreenRequested = "BattleOverScreenRequested";
    }
}