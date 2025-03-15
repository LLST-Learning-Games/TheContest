using Godot;

namespace TheContest.Projectiles.ProjectileTrajectory;

public partial class TrajectoryOrbit :  BaseProjectileTrajectory
{
    [Export] private float _startSpeed;
    [Export] private float _yoyoStrength;
    [Export] private double _lifetime;
    private Vector2 _direction;
    private bool _hasStarted;
    private Node2D _startPosition;
    private float _timeSinceShot;
    
    public override void SetTarget(Vector2 target)
    {
        _direction = target;
        _hasStarted = false;
        _startPosition = GetNode<Character>("/root/Scene/Character");
        _timeSinceShot = 0;
    }

    public override void UpdatePosition(RigidBody2D body, double delta)
    {
        if (!_hasStarted)
        {
            body.ApplyForce(_direction * _startSpeed);
            _hasStarted = true;
        }
        _direction = (body.Position - _startPosition.Position).Normalized();
        body.ApplyForce(_direction * -(float)delta * _yoyoStrength);
        _timeSinceShot += (float)delta;
        if (_timeSinceShot > _lifetime)
        {
            body.QueueFree();
        }
    }
}