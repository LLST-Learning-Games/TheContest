using Godot;
using System;

public partial class PulseEnergyWorldUi : Node2D
{
    [Export] private Sprite2D _energyBarSprite;
    public float MaxEnergy { get; set; }

    public override void _Ready()
    {
        _energyBarSprite.Scale = Vector2.One;
        _energyBarSprite.Position = Vector2.Zero;
    }
    public void OnEnergyChanged(float currentEnergy)
    {
        float energyPercent = currentEnergy / MaxEnergy;
        _energyBarSprite.Scale = new Vector2(energyPercent, 1f);
        _energyBarSprite.Position = new Vector2((energyPercent - 1f)/ 2f, 0f);
    }
}
