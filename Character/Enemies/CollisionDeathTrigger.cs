
using Godot;

public partial class CollisionDeathTrigger : Node
{
    [Export] private HealthComponent _healthComponent;
    [Export] private RigidBody2D _rb;

    public override void _Ready()
    {
        _rb.BodyEntered += OnCollide;
        base._Ready();
    }

    private void OnCollide(Node body)
    {
        if(body is CharacterBody2D character)
        {
            _healthComponent.TriggerDeath();
        }
    }
}