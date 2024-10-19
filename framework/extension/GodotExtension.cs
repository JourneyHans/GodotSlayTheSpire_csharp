using Godot;
using Godot.Collections;

namespace framework.extension;

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

public static class ResourceExtension {
    public static T Duplicate<T>(this Resource resource, bool subresource = false) where T : Resource {
        return (T)resource.Duplicate(subresource);
    }
}

public static class TweenExtension {
    public static PropertyTweener DoMove(this Tween tween, GodotObject target, Variant end, float duration) {
        return tween.TweenProperty(target, "global_position", end, duration);
    }
}

public static class SceneTreeExtension {
    public static Array<Node2D> Get2DNodesInGroup(this SceneTree tree, StringName group) {
        Array<Node> result = tree.GetNodesInGroup(group);
        Array<Node2D> nodes = new Array<Node2D>();
        foreach (Node node in result) {
            nodes.Add((Node2D)node);
        }

        return nodes;
    }
}