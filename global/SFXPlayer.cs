public partial class SFXPlayer : SoundBasePlayer {
    public override void _Ready() {
        base._Ready();
        AudioPlayer.SFXPlayer = this;
    }
}