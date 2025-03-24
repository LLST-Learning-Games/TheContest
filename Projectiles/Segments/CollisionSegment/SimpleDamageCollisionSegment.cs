using Godot;
using System;
using TheContest.Projectiles;

public partial class SimpleDamageCollisionSegment : ProjectileSegmentData
{    
    [Export] private int _damageToDealOnCollision = 25;
    
    private const string HEALTH_COMPONENT = "HealthComponent";
    
    public override void OnInitialize(RigidBody2D instanceBody)
    {
        instanceBody.LinearVelocity = Vector2.Zero;
    }

    public override void OnPhysicsProcess(double delta, RigidBody2D instanceBody)
    {
        // ..
    }

    public override void OnCollide(Node otherBody, RigidBody2D instanceBody)
    {
        if (otherBody.FindChild(HEALTH_COMPONENT) is not HealthComponent healthComponent)
        {
            return;
        }
        
        healthComponent.UpdateHealth(-_damageToDealOnCollision);
    }
}
