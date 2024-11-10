using System.Linq;
using Godot;

public partial class ModifierHandler : Node {
    public bool HasModifier(Modifier.EType type) {
        return GetChildren().Cast<Modifier>().Any(modifier => modifier.Type == type);
    }

    public Modifier GetModifier(Modifier.EType type) {
        return GetChildren().Cast<Modifier>().FirstOrDefault(modifier => modifier.Type == type);
    }

    public int GetModifierValue(int baseValue, Modifier.EType type) {
        Modifier modifier = GetModifier(type);
        return modifier?.GetModifiedValue(baseValue) ?? baseValue;
    }
}