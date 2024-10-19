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
    /// 按类型加载资源(.tres)，不需要传后缀
    /// </summary>
    public static T LoadResource<T>(string resPath, string ext = ".tres") where T : Resource {
        return ResourceLoader.Load<T>($"{resPath}{ext}");
    }

    /// <summary>
    /// 加载纹理资源(.tres)，后缀默认为.png
    /// </summary>
    public static Texture LoadTexture(string resPath, string ext = ".png") {
        return LoadResource<Texture>(resPath, ext);
    }

    /// <summary>
    /// 加载资源(.tres)
    /// </summary>
    public static Resource LoadResource(string resPath, string ext) {
        return ResourceLoader.Load($"{resPath}{ext}");
    }
}