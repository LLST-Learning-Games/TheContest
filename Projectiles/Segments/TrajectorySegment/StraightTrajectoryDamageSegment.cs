using Godot;

namespace TheContest.Projectiles;

public partial class StraightTrajectoryDamageSegment : ProjectileSegmentData
{
    private const string HEALTH_COMPONENT = "HealthComponent";
    
    [Export] private float _speed;
    [Export] private int _damageToDealOnCollision = 25;
    [Export] private bool _stopMotionOnCollision = true;
    
    public override void OnInitialize(RigidBody2D instanceBody, SceneTree tree)
    {
        var globalForce = Vector2.FromAngle(instanceBody.Rotation);
        instanceBody.ApplyForce(globalForce * _speed);
    }

    public override void OnPhysicsProcess(double delta, RigidBody2D instanceBody)
    {
        // ..
    }

    public override void OnTriggerEntered(Node otherBody, RigidBody2D instanceBody)
    {
        if(!IsInstanceValid(instanceBody))
        {
            return;
        }
        
        if (otherBody?.FindChild(HEALTH_COMPONENT) is HealthComponent healthComponent)
        {
            healthComponent.UpdateHealth(-_damageToDealOnCollision);
        }

        if (_stopMotionOnCollision)
        {
            instanceBody.LinearVelocity = Vector2.Zero;
        }
        //instanceBody.QueueFree();
    }

    public override string GetDescription()
    {
        var description = base.GetDescription();
        description += $"\nDamage: {_damageToDealOnCollision}";
        description += $"\nSpeed: {_speed / 100}";
        return description;
    }
}