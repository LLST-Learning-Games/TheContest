using Godot;
using System;

public partial class HealthBarWorldUi : Node2D
{
    [Export] private Sprite2D _healthBarSprite;
    [Export] private HealthComponent _healthComponent;
    
    public override void _Ready()
    {
        _healthComponent.OnHealthChanged += OnHealthChanged;
        _healthBarSprite.Scale = Vector2.One;
        _healthBarSprite.Position = Vector2.Zero;
    }

    private void OnHealthChanged(int currentHealth)
    {
        float healthPercentage = (float)currentHealth / _healthComponent.MaxHealth;
        _healthBarSprite.Scale = new Vector2(healthPercentage, 1f);
        _healthBarSprite.Position = new Vector2((healthPercentage - 1f)/ 2f, 0f);
    }
}
