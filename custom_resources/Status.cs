using System;
using Godot;

[GlobalClass]
public partial class Status : Resource {
    public event Action<Status> StatusApplied;
    public event Action StatusChanged;

    public enum EType {
        StartOfTurn,
        EndOfTurn,
        EventBased,
    }
    
    public enum EStackType {
        Node,
        Intensity,
        Duration
    }

    [ExportGroup("Status Data")]
    [Export] public string ID;
    [Export] public EType Type;
    [Export] public EStackType StackType;
    [Export] public bool CanExpire;
    
    private int _duration;
    [Export]
    public int Duration {
        get => _duration;
        set {
            _duration = value;
            StatusChanged?.Invoke();
        }
    }

    private int _stacks;
    [Export]
    public int Stacks {
        get => _stacks;
        set {
            _stacks = value;
            StatusChanged?.Invoke();
        }
    }

    [ExportGroup("Status Visuals")]
    [Export]
    public Texture2D Icon;

    [Export(PropertyHint.MultilineText)] 
    public string ToolTip;

    public virtual void InitializeStatus(Node2D target) {
        
    }

    public virtual void ApplyStatus(Node2D target) {
        StatusApplied?.Invoke(this);
    }

    public virtual string GetToolTip() {
        return ToolTip;
    }
}