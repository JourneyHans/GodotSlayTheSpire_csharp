using framework.debug;
using Godot;

namespace framework.tag;

[GlobalClass]
public abstract partial class TagControl : Control, ITag, ILog {
    public string Tag => GetType().Name;

    public void Print(string log) {
        GD.Print($"[{Tag}] {log}");
    }

    public void PrintWarning(string log) {
        GD.PushWarning($"[{Tag}] {log}");
    }

    public void PrintErr(string log) {
        GD.PushError($"[{Tag}] {log}");
    }
}