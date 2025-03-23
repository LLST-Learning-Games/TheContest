using Godot;

public partial class CollisionSimpleDamage : BaseProjectileCollision
{
    [Export] private int _damageToDealOnCollision;
    
    private const string HEALTH_COMPONENT = "HealthComponent";
        
    public override void OnCollide(Node body, RigidBody2D projectileBody)
    {
        projectileBody.LinearVelocity = Vector2.Zero;
        if (body.FindChild(HEALTH_COMPONENT) is not HealthComponent healthComponent)
        {
            return;
        }
        
        healthComponent.UpdateHealth(-_damageToDealOnCollision);
    }
}