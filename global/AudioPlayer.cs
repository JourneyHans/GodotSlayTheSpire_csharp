using Godot;

public static class AudioPlayer {
    public static MusicPlayer MusicPlayer;
    public static SFXPlayer SFXPlayer;

    public static void PlayMusic(AudioStream audio, bool single = false) {
        MusicPlayer.Play(audio, single);
    }

    public static void PlaySFX(AudioStream audio, bool single = false) {
        SFXPlayer.Play(audio, single);
    }

    public static void StopMusic() {
        MusicPlayer.Stop();
    }

    public static void StopSFX() {
        SFXPlayer.Stop();
    }
}