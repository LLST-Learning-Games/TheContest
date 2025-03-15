using Godot;

public partial class TrajectoryYoyo : BaseProjectileTrajectory
{
    private const float SECONDS_TO_CHECK_RETURN_TIME = 0.4f;
    [Export] private float _startSpeed;
    [Export] private float _yoyoStrength;
    [Export] private float _despawnRangeSquared;
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
        var distanceBetweenSelfAndStart = (body.Position - _startPosition.Position);
        body.ApplyForce(distanceBetweenSelfAndStart.Normalized() * -(float)delta * _yoyoStrength);
        
        _timeSinceShot += (float)delta;
        GD.Print("Time Since Shot: " + _timeSinceShot);
        GD.Print("Distance: " + distanceBetweenSelfAndStart.LengthSquared());
        if (_timeSinceShot < SECONDS_TO_CHECK_RETURN_TIME)
        {
            return;
        }

        if (distanceBetweenSelfAndStart.LengthSquared() < _despawnRangeSquared)
        {
            body.QueueFree();
        }
    }
}
