using framework.events;
using Godot;

public partial class Campfire : Control
{
    #region export

    [Export] public CharacterStats CharacterStats;

    #endregion
    
    private Button _resetButton;
    private AnimationPlayer _animationPlayer;
    private bool _isExiting;

    public override void _Ready() {
        _resetButton = GetNode<Button>("UILayer/UI/RestButton");
        _resetButton.Pressed += OnResetButtonPressed;
        
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private void OnResetButtonPressed() {
        if (_isExiting) {
            return;
        }

        _isExiting = true;
        CharacterStats.Heal(Mathf.CeilToInt(CharacterStats.MaxHealth * 0.3f));
        _animationPlayer.Play("fade_out");
    }
    
    // This is called from the AnimationPlayer
    // at the end of 'fade-out'
    private void OnFadeOutFinished() {
        EventDispatcher.TriggerEvent(Event.CampfireExited);
    }
}

public partial class Campfire {
    public static class Event {
        public const string CampfireExited = "CampfireExited";
    }
}