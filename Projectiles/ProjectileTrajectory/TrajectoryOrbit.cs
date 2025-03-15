using Godot;

namespace TheContest.Projectiles.ProjectileTrajectory;

public partial class TrajectoryOrbit :  BaseProjectileTrajectory
{
    [Export] private float _startSpeed;
    [Export] private float _yoyoStrength;
    private Vector2 _direction;
    private bool _hasStarted;
    private Node2D _startPosition;
    public override void SetTarget(Vector2 target)
    {
        _direction = target;
        _hasStarted = false;
        _startPosition = GetNode<Character>("/root/Scene/Character");
    }

    public override void UpdatePosition(RigidBody2D body, double delta)
    {
        if (!_hasStarted)
        {
            body.ApplyForce(_direction * _startSpeed);
            _hasStarted = true;
        }
        _direction = (body.Position - _startPosition.Position).Normalized();
        GD.Print("direction: " + _direction);
        body.ApplyForce(_direction * -(float)delta * _yoyoStrength);
    }
}