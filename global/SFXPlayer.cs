public partial class SFXPlayer : SoundBasePlayer {
    public override void _Ready() {
        AudioPlayer.SFXPlayer = this;
    }
}