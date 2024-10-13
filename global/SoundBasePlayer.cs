using framework.tag;
using Godot;

public abstract partial class SoundBasePlayer : TagNode {
    public void Play(AudioStream audio, bool single = false) {
        if (audio == null) {
            PrintErr("audio为空");
            return;
        }

        if (single) {
            Stop();
        }

        foreach (Node child in GetChildren()) {
            var player = (AudioStreamPlayer)child;
            if (!player.Playing) {
                player.Stream = audio;
                player.Play();
                break;
            }
        }
    }

    public void Stop() {
        foreach (Node child in GetChildren()) {
            var player = (AudioStreamPlayer)child;
            player.Stop();
        }
    }
}