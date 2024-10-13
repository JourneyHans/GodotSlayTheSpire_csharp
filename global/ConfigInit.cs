using framework.debug;
using Godot;

public partial class ConfigInit : Node {
	public override void _Ready() {
		FinchLogger logger = new(this);
		
		ConfigFile config = new();
		Error err = config.Load("res://config.init");
		if (err != Error.Ok) {
			logger.Error("无法加载配置文件, 请重命名根目录下的'config_backup.init'为'config.init'后运行");
			return;
		}

		int overrideWidth = (int)config.GetValue("display", "override_width", 1920);
		int overrideHeight = (int)config.GetValue("display", "override_height", 1080);
		logger.Log($"窗口大小修改为: {overrideWidth}x{overrideHeight}");
		Vector2I overrideSize = new(overrideWidth, overrideHeight);
		
		// 居中显示
		Window window = GetWindow();
		window.Size = overrideSize;
		int screen = window.CurrentScreen;
		Rect2I screenRect = DisplayServer.ScreenGetUsableRect(screen);
		var windowSize = window.GetSizeWithDecorations();
		window.Position = screenRect.Position + (screenRect.Size / 2 - windowSize / 2);
	}
}
