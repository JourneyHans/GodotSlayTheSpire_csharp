using System;
using Godot;

public partial class StatsUI : HBoxContainer {
    private HBoxContainer _block;
    private Label _blockLabel;
    private HBoxContainer _health;
    private Label _healthLabel;

    public override void _Ready() {
        _block = GetNode<HBoxContainer>("Block");
        _blockLabel = _block.GetNode<Label>("%BlockLabel");
        _health = GetNode<HBoxContainer>("Health");
        _healthLabel = _health.GetNode<Label>("%HealthLabel");
    }

    public void UpdateStats(Stats stats) {
        _blockLabel.Text = stats.Block.ToString();
        _healthLabel.Text = stats.Health.ToString();
        
        _block.Visible  = stats.Block > 0;
        _health.Visible = stats.Health > 0;
    }
}