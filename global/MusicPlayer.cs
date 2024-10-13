public partial class MusicPlayer : SoundBasePlayer {
    public override void _Ready() {
        AudioPlayer.MusicPlayer = this;
    }
}