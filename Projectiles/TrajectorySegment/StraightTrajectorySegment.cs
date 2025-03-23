using Godot;

namespace TheContest.Projectiles;

public partial class StraightTrajectorySegment : ProjectileSegmentData
{
    [Export] private float _speed;
    public override void OnPhysicsProcess(double delta, RigidBody2D body)
    {
        body.ApplyForce(Vector2.Right * _speed);
    }

    public override void OnCollide(Node otherBody)
    {
        throw new System.NotImplementedException();
    }
}