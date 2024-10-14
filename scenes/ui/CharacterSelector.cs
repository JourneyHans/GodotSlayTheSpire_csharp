using framework.debug;
using framework.utils;
using Godot;

public partial class CharacterSelector : Control {
    private FinchLogger _logger;

    private PackedScene RunScene = SimpleLoader.LoadPackedScene("res://scenes/run/run");
    
    private CharacterStats WarriorStats = SimpleLoader.LoadResource<CharacterStats>("res://characters/warrior/warrior");
    private CharacterStats WizardStats = SimpleLoader.LoadResource<CharacterStats>("res://characters/wizard/wizard");
    private CharacterStats AssassinStats = SimpleLoader.LoadResource<CharacterStats>("res://characters/assassin/assassin");

    [Export] public RunStartup RunStartup;

    private Label _title;
    private Label _description;
    private TextureRect _characterPortrait;
    
    private Button _startBtn;
    private Button _warriorBtn;
    private Button _wizardBtn;
    private Button _assassinBtn;

    private CharacterStats _currentCharacter;

    public override void _Ready() {
        _logger = new FinchLogger(this);
        
        _title = GetNode<Label>("%Title");
        _description = GetNode<Label>("%Description");
        _characterPortrait = GetNode<TextureRect>("%CharacterPortrait");
        
        _startBtn = GetNode<Button>("StartButton");
        _startBtn.Pressed += OnStartBtnPressed;
        
        _warriorBtn = GetNode<Button>("CharacterButtons/WarriorButton");
        _warriorBtn.Pressed += OnWarriorBtnPressed;
        
        _wizardBtn = GetNode<Button>("CharacterButtons/WizardButton");
        _wizardBtn.Pressed+= OnWizardBtnPressed;
        
        _assassinBtn = GetNode<Button>("CharacterButtons/AssassionButton");
        _assassinBtn.Pressed+= OnAssassinBtnPressed;

        SetCurrentCharacter(WarriorStats);
    }

    private void SetCurrentCharacter(CharacterStats character) {
        _currentCharacter = character;
        _title.Text = _currentCharacter.CharacterName;
        _description.Text = _currentCharacter.Description;
        _characterPortrait.Texture = (Texture2D)_currentCharacter.Portrait;
    }

    private void OnStartBtnPressed() {
        _logger.Log($"Start new run with {_currentCharacter.CharacterName}");
        RunStartup.type = RunStartup.Type.NewRun;
        RunStartup.PickedCharacter = _currentCharacter;
        GetTree().ChangeSceneToPacked(RunScene);
    }

    private void OnWarriorBtnPressed() {
        SetCurrentCharacter(WarriorStats);
    }

    private void OnWizardBtnPressed() {
        SetCurrentCharacter(WizardStats);
    }

    private void OnAssassinBtnPressed() {
        SetCurrentCharacter(AssassinStats);
    }
}