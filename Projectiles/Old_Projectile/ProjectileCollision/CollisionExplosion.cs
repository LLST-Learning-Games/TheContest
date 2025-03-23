using Godot;

public partial class CollisionExplosion : BaseProjectileCollision
{
    [Export] private Area2D _explosionArea;
    [Export] private int _damageToDealOnExplosion;
    
    private const string HEALTH_COMPONENT = "HealthComponent";
        
    public override void OnCollide(Node body, RigidBody2D projectileBody)
    {
        projectileBody.LinearVelocity = Vector2.Zero;
        _explosionArea.BodyEntered += OnBodyEntered;
        
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
    }
    
    public override void SetAsEnemyCollision()
    {
        _explosionArea.SetCollisionLayer(0b1000);
        _explosionArea.SetCollisionMask(0b10001);	// player and environment
    }
}