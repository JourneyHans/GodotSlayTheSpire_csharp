public partial class MusicPlayer : SoundBasePlayer {
    public override void _Ready() {
        base._Ready();
        AudioPlayer.MusicPlayer = this;
    }
}