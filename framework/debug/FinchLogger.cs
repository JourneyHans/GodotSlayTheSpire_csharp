using System.Diagnostics.CodeAnalysis;
namespace framework.debug;

public class FinchLogger {
    public static ILogHandler LogHandler;
    public static bool Enabled = true;

    private readonly string _tag;

    private static string StackTrace => System.Environment.StackTrace;

    public FinchLogger(object parent, bool enabled = true) : this(parent.GetType().Name, enabled) {
    }

    public FinchLogger(string tag, bool enabled = true) {
        _tag = tag;
        Enabled = enabled;
    }

    public void Log(string log, bool stackTrace = false) {
        if (!Enabled) {
            return;
        }

        LogHandler?.Log(stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"[{_tag}] {log}");
    }

    public void Warning(string log, bool stackTrace = false) {
        if (!Enabled) {
            return;
        }

        LogHandler?.Warning(stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"[{_tag}] {log}");
    }

    public void Error(string log, bool stackTrace = true) {
        if (!Enabled) {
            return;
        }

        LogHandler?.Error(stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"[{_tag}] {log}");
    }

    public void Assert([DoesNotReturnIf(false)] bool condition, string log, bool stackTrace = false) {
        if (!Enabled) {
            return;
        }

        LogHandler?.Assert(condition, stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"{log}");
    }
}