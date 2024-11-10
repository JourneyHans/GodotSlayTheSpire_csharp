using Godot;

public partial class ModifierValue : Node {
    public enum EType {
        PercentBased,
        Flat
    };

    [Export] public EType Type { get; private set; }
    [Export] public float PercentValue { get; set; }
    [Export] public float FlatValue { get; set; }
    [Export] public string Source { get; private set; }

    public static ModifierValue CreateNewModifier(string source, EType type) {
        ModifierValue modifierValue = new();
        modifierValue.Source = source;
        modifierValue.Type = type;
        return modifierValue;
    }
}