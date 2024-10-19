using Godot;

public partial class InitScene : Node {
    [Export] public PackedScene FirstScene;

    public override void _Ready() {
        // 不能同一帧调用
        Callable.From(() => { GetTree().ChangeSceneToPacked(FirstScene); }).CallDeferred();
    }
}