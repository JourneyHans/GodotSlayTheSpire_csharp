using framework.debug;
using Godot;

public abstract partial class SoundBasePlayer : Node {
    private FinchLogger _logger;

    public override void _Ready() {
        _logger = new FinchLogger(this);
    }

    public void Play(AudioStream audio, bool single = false) {
        if (audio == null) {
            _logger.Error("audio为空");
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