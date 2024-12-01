using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace framework.extension;

#region GodotObjectExtension

public static class GodotObjectExtension {
    public static string Info(this GodotObject target) {
        return target switch {
            null => "null",
            Node node => $"{node.Name}{node}",
            _ => target.ToString()
        };
    }

    public static SignalAwaiter WhenReady(this GodotObject target) {
        return target.ToSignal(target, Node.SignalName.Ready);
    }

    public static void QueueFreeAllChildren(this Node parent) {
        foreach (Node child in parent.GetChildren()) {
            child.QueueFree();
        }
    }

    public static void FreeAllChildren(this Node parent) {
        foreach (Node child in parent.GetChildren()) {
            child.Free();
        }
    }

    public static void Shake(this Node2D node2D, float strength, float duration, int shakeCount = 10) {
        if (node2D == null) {
            return;
        }

        Vector2 originPos = node2D.Position;
        Tween tween = node2D.CreateTween();
        for (int i = 0; i < shakeCount; i++) {
            Vector2 shakeOffset = new((float)GD.RandRange(-1.0, 1.0), (float)GD.RandRange(1.0, 1.0));
            Vector2 target = originPos + strength * shakeOffset;
            if (i % 2 == 0) {
                target = originPos;
            }

            tween.DoMove(node2D, target, duration / shakeCount);
            strength *= 0.75f;
        }
        
        tween.Finished += () => { node2D.Position = originPos; };
    }
}

#endregion

#region NodeExtension

public static class NodeExtension {
    public static IEnumerable<T> GetChildren<T>(this Node node, bool includeInternal = false) {
        return node.GetChildren(includeInternal).Cast<T>();
    }
}

#endregion

#region ResourceExtension

public static class ResourceExtension {
    public static T Duplicate<T>(this Resource resource, bool subresource = false) where T : Resource {
        return (T)resource.Duplicate(subresource);
    }
}

#endregion

#region TweenExtion

public static class TweenExtension {
    public static PropertyTweener DoMove(this Tween tween, GodotObject target, Variant end, float duration) {
        return tween.TweenProperty(target, "global_position", end, duration);
    }

    public static PropertyTweener DoMoveX(this Tween tween, GodotObject target, Variant end, float duration) {
        return tween.TweenProperty(target, "position:x", end, duration);
    }
}

#endregion

#region SceneTreeExtension

public static class SceneTreeExtension {
#pragma warning disable GD0302
    public static Array<T> GetNodesInGroup<T>(this SceneTree tree, StringName group) where T : Node {
        Array<Node> result = tree.GetNodesInGroup(group);
        Array<T> nodes = new Array<T>();
        foreach (Node node in result) {
            nodes.Add((T)node);
        }

        return nodes;
    }
#pragma warning restore GD0302
}

#endregion

#region ArrayExtension

#pragma warning disable GD0302

public static class ArrayExtension {
    public static Array<T> Filter<T>(this Array<T> array, Func<T, bool> predicate) {
        return new Array<T>(array.Where(predicate));
    }
}

#pragma warning restore GD0302

#endregion

#region ControlExtension

// TODO: 后续把这种常量放到另一个文件，例如GodotConst
public static class ThemeConstantKey {
    public const string Separation = "separation";
}

public static class ControlExtension {
    
    public static int GetSeparation(this Control control) {
        return control.GetThemeConstant(ThemeConstantKey.Separation);
    }
}

#endregion