using Godot;

public partial class HealthUI : HBoxContainer {
    [Export] public bool ShowMaxHP;

    private Label _healthLabel;
    private Label _maxHealthLabel;

    public override void _Ready() {
        _healthLabel = GetNode<Label>("%HealthLabel");
        _maxHealthLabel = GetNode<Label>("%MaxHealthLabel");
    }

    public void UpdaeteStates(Stats stats) {
        _healthLabel.Text = stats.Health.ToString();
        _maxHealthLabel.Text = $"/{stats.MaxHealth}";
        _maxHealthLabel.Visible = ShowMaxHP;
    }
}