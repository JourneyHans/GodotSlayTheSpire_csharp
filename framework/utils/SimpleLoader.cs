using Godot;

namespace framework.utils;

public static class SimpleLoader {
    /// <summary>
    /// 加载场景(.tscn)，不需要传后缀
    /// </summary>
    public static PackedScene LoadPackedScene(string resPath) {
        return ResourceLoader.Load<PackedScene>($"{resPath}.tscn");
    }

    /// <summary>
    /// 加载资源(.tres)，不需要传后缀
    /// </summary>
    public static T LoadResource<T>(string resPath) where T : Resource {
        return ResourceLoader.Load<T>($"{resPath}.tres");
    }
}