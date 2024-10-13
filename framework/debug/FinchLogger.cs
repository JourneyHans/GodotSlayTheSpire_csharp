using System;
using Godot;

namespace framework.debug;

public class FinchLogger {
    private readonly string _tag;
    private readonly bool _enabled;

    private static string StackTrace => System.Environment.StackTrace;

    public FinchLogger(object parent, bool enabled = true) : this(parent.GetType().Name, enabled) {
    }

    public FinchLogger(string tag, bool enabled = true) {
        _tag = tag;
        _enabled = enabled;
    }

    public void Log(string log, bool stackTrace = false) {
        if (!_enabled) {
            return;
        }

        GD.Print(stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"[{_tag}] {log}");
    }

    public void Warning(string log, bool stackTrace = false) {
        if (!_enabled) {
            return;
        }

        GD.PushWarning(stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"[{_tag}] {log}");
    }

    public void Error(string log, bool stackTrace = true) {
        if (!_enabled) {
            return;
        }

        GD.PrintErr(stackTrace ? $"[{_tag}] {log}\n{StackTrace}" : $"[{_tag}] {log}");
    }
}