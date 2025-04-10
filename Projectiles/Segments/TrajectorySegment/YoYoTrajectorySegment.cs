using Godot;

namespace TheContest.Projectiles;

public partial class YoYoTrajectorySegment : ProjectileSegmentData
{
    private const float SECONDS_TO_CHECK_RETURN_TIME = 0.4f;
    [Export] private float _startSpeed;
    [Export] private float _yoyoStrength;
    [Export] private float _despawnRangeSquared;
    
    private Node2D _homingTarget;
    private float _timeSinceShot;

    public override void OnInitialize(RigidBody2D instanceBody, SceneTree tree)
    {
        _homingTarget = tree.GetFirstNodeInGroup("Player") as Node2D;
        var direction = Vector2.FromAngle(instanceBody.Rotation);
        instanceBody.ApplyForce(direction * _startSpeed);
        _timeSinceShot = 0;
    }


    public override void OnPhysicsProcess(double delta, RigidBody2D instanceBody)
    {
        float distanceBetweenSelfAndStart = float.MaxValue;
        if(IsInstanceValid(_homingTarget))
        {
            var vectorBetweenSelfAndStart = (instanceBody.Position - _homingTarget.Position);
            instanceBody.ApplyForce(vectorBetweenSelfAndStart.Normalized() * -(float)delta * _yoyoStrength);
            distanceBetweenSelfAndStart = vectorBetweenSelfAndStart.Length();
        }

        
        if (_timeSinceShot < SECONDS_TO_CHECK_RETURN_TIME)
        {
            _timeSinceShot += (float)delta;
            return;
        }
        
        if (distanceBetweenSelfAndStart < _despawnRangeSquared)
        {
            instanceBody.QueueFree();
        }

    }

    public override void OnDraw(RigidBody2D instanceBody)
    {
        instanceBody.DrawDashedLine(
            from: instanceBody.Position, 
            to: _homingTarget.Position,
            width: 3f,
            color: Colors.DarkGreen,
            dash: 1f
        );
    }
    
    public override void OnCollide(Node otherBody, RigidBody2D instanceBody)
    {
        if(!IsInstanceValid(instanceBody))
        {
            return;
        }
        instanceBody.QueueFree();
    }
}