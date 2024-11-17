using Godot;

[GlobalClass]
public partial class Intent : Resource {
    [Export] public string BaseText;
    [Export] public Texture Icon;

    public string CurrentText { get; private set; }

    public void UpdateCurrentText(int value) {
        CurrentText = string.Format(BaseText, value);
    }
}