using Godot;

namespace TheContest.Projectiles.SpawnableEvents;

public partial class Explosion : Node2D
{
    [Export] private AnimatedSprite2D _sprite;
    [Export] private Area2D _explosionArea;
    [Export] private int _damageToDealOnExplosion;
    
    private const string HEALTH_COMPONENT = "HealthComponent";

    public override void _Ready()
    {
        _sprite.Play("collide");
        _explosionArea.BodyEntered += OnBodyEntered;
        _sprite.AnimationFinished += QueueFree;
        var bodiesInExplosionRadius = _explosionArea.GetOverlappingBodies();
        foreach (var inExplosionRadius in bodiesInExplosionRadius)
        {
            OnBodyEntered(inExplosionRadius);
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.FindChild(HEALTH_COMPONENT) is not HealthComponent healthComponent)
        {
            return;
        }
        
        healthComponent.UpdateHealth(-_damageToDealOnExplosion);;
        
        if (body is RigidBody2D rigidBody2D)
        {
            Vector2 forceVector = rigidBody2D.GlobalPosition - GlobalPosition ;
            forceVector = forceVector.Normalized();
            forceVector *= _damageToDealOnExplosion * 5000f;
            rigidBody2D.ApplyForce(forceVector);
        }

    }
    
    public void SetAsEnemyCollision()
    {
        _explosionArea.SetCollisionLayer(0b1000);
        _explosionArea.SetCollisionMask(0b10001);	// player and environment
    } 
    
    public override void _ExitTree()
    {
        _explosionArea.BodyEntered -= OnBodyEntered;
        _sprite.AnimationFinished -= QueueFree;
    }
}