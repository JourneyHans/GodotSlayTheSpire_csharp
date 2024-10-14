using framework.debug;
using Godot;

public partial class GDLogHandler : Node, ILogHandler {
    public override void _Ready() {
        FinchLogger.LogHandler = this;
    }

    public void Log(string log) {
        GD.Print(log);
    }

    public void Warning(string log) {
        GD.PushWarning(log);
    }

    public void Error(string log) {
        GD.PushError(log);
    }
}