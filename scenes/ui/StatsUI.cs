using Godot;

public partial class StatsUI : HBoxContainer {
    private HBoxContainer _block;
    private Label _blockLabel;
    private HealthUI _healthUI;

    public override void _Ready() {
        _block = GetNode<HBoxContainer>("Block");
        _blockLabel = _block.GetNode<Label>("%BlockLabel");
        _healthUI = GetNode<HealthUI>("Health");
    }

    public void UpdateStats(Stats stats) {
        _blockLabel.Text = stats.Block.ToString();
        _block.Visible  = stats.Block > 0;
        
        _healthUI.UpdaeteStates(stats);
        _healthUI.Visible = stats.Health > 0;
    }
}