using framework.debug;
using framework.utils;
using Godot;

public partial class MainMenu : Control {
    private FinchLogger _logger;

    private PackedScene _characterSelectorScene = SimpleLoader.LoadPackedScene("res://scenes/ui/character_selector");
    
    private Button _continueBtn;
    private Button _newRunBtn;
    private Button _exitBtn;

    public override void _Ready() {
        _logger = new FinchLogger(this);
        
        GetTree().Paused = false;
        _continueBtn = GetNode<Button>("%Continue");
        _continueBtn.Pressed += OnContinueBtnPressed;
        
        _newRunBtn = GetNode<Button>("VBoxContainer/NewRun");
        _newRunBtn.Pressed += OnNewRunBtnPressed;
        
        _exitBtn = GetNode<Button>("VBoxContainer/Exit");
        _exitBtn.Pressed += OnExitBtnPressed;
    }

    private void OnContinueBtnPressed() {
        _logger.Log("continue");
    }

    private void OnNewRunBtnPressed() {
        GetTree().ChangeSceneToPacked(_characterSelectorScene);
    }

    private void OnExitBtnPressed() {
        GetTree().Quit();
    }
}