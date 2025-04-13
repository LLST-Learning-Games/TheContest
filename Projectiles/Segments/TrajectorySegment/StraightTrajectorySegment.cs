using Godot;

namespace TheContest.Projectiles;

public partial class StraightTrajectorySegment : ProjectileSegmentData
{
    [Export] private float _speed;
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
        instanceBody.QueueFree();
    }

    public override string GetDescription()
    {
        return base.GetDescription() + $"\n Speed: {_speed / 100}";
    }
}