using Godot;

public partial class CollisionSimpleDamage : BaseProjectileCollision
{
    [Export] private int _damageToDealOnCollision;
    
    private const string HEALTH_COMPONENT = "HealthComponent";
        
    public override void OnCollide(Node body)
    {
        if (body.FindChild(HEALTH_COMPONENT) is not HealthComponent healthComponent)
        {
            return;
        }
        
        healthComponent.UpdateHealth(-_damageToDealOnCollision);
    }
}