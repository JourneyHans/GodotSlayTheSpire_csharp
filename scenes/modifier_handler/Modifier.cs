using framework.extension;
using Godot;

public partial class Modifier : Node {
    public enum EType {
        DmgDealt,
        DmgTaken,
        CardCost,
        ShopCost,
        NoModifier,
    }
    
    [Export] public EType Type { get; private set; }

    public ModifierValue GetValue(string source) {
        foreach (ModifierValue value in GetChildren()) {
            if (value.Source == source) {
                return value;
            }
        }

        return null;
    }

    public void AddNewValue(ModifierValue value) {
        var modifierValue = GetValue(value.Source);
        if (modifierValue == null) {
            AddChild(value);
        }
        else {
            modifierValue.FlatValue = value.FlatValue;
            modifierValue.PercentValue = value.PercentValue;
        }
    }

    public void RemoveValue(string source) {
        foreach (ModifierValue value in GetChildren()) {
            if (value.Source == source) {
                value.QueueFree();
            }
        }
    }

    public void ClearValue() {
        this.QueueFreeAllChildren();
    }

    public int GetModifiedValue(int baseValue) {
        float flatResult = baseValue;
        float percentResult = 1.0f;
        
        // Apply flat modifiers first
        foreach (ModifierValue value in GetChildren()) {
            if (value.Type == ModifierValue.EType.Flat) {
                flatResult += value.FlatValue;
            }
        }

        // Apply % modifiers next
        foreach (ModifierValue value in GetChildren()) {
            if (value.Type == ModifierValue.EType.PercentBased) {
                percentResult += value.PercentValue;
            }
        }

        return Mathf.FloorToInt(flatResult * percentResult);
    }
}