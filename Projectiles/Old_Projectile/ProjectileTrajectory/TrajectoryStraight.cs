using Godot;
using System;

public partial class TrajectoryStraight : BaseProjectileTrajectory
{
    [Export] private float _speed;
    private Vector2 _direction;
    public override void SetTarget(Vector2 target)
    {
        _direction = target;
    }

    public override void UpdatePosition(RigidBody2D body, double delta)
    {
        if (_direction == Vector2.Zero)
        {
            return;
        }
		
        body.ApplyForce(_direction * _speed);
        _direction = Vector2.Zero;
    }
}
