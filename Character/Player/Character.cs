using Godot;
using System;

public partial class Character : CharacterBody2D
{
    [Export] private Camera2D _camera;
    [Export] private HealthComponent _healthComponent;
    
    public override void _Ready()
    {
        _healthComponent.OnDeath += OnDeath;
    }
    
    private void OnDeath()
    {
        _camera.PositionSmoothingEnabled = false;
        _camera.Reparent(GetTree().Root);
        QueueFree();
    }
}
