// meta-name: Status
// meta-description: Create a Status which can be applied to a target

using framework.debug;
using Godot;

[GlobalClass]
public partial class _CLASS_Status : Status {
    private FinchLogger _logger;

    private int _memberVar = 0;

    public override void InitializeStatus(Node2D target) {
        _logger = new FinchLogger(this);
        _logger.Log($"Initialize my status for target: {target}");
    }

    public override void ApplyStatus(Node2D target) {
        _logger.Log($"My status target: {target}");
        _logger.Log($"It does {_memberVar} something");
        base.ApplyStatus(target);
    }
}