using framework.tag;
using Godot;

public partial class ConfigInit : TagNode {
	public override void _Ready() {
		ConfigFile config = new();
		Error err = config.Load("res://config.init");
		if (err != Error.Ok) {
			PrintErr($"无法加载配置文件, err: {err}");
			return;
		}

		int overrideWidth = (int)config.GetValue("display", "override_width", 1920);
		int overrideHeight = (int)config.GetValue("display", "override_height", 1080);
		Print($"窗口大小修改为: {overrideWidth}x{overrideHeight}");
		Vector2I overrideSize = new(overrideWidth, overrideHeight);
		
		// 居中显示
		Window window = GetWindow();
		window.Size = overrideSize;
		int screen = window.CurrentScreen;
		Rect2I screenRect = DisplayServer.ScreenGetUsableRect(screen);
		var windowSize = window.GetSizeWithDecorations();
		window.Position = screenRect.Position + (screenRect.Size / 2 - windowSize / 2);
	}

	// public override void _Process(double delta) {
	// 	if (Input.IsKeyPressed(Key.A)) {
	// 		GD.Print("[ConfigInit:_Process] Key A pressed");
	// 	}
	// }
}
