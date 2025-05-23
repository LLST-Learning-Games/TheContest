using Godot;

namespace Behaviours;

public partial class BehaviourMoveTo : BehaviourActionBase
{
    [Export] private float _successDistance = 10f;
    [Export] private float _moveSpeed = 50f;
    [Export] private double _failTimeout = 0.5;
    
    private Vector2 _moveDestination = Vector2.Zero;
    private Vector2 _positionLastTick = Vector2.Zero;
    private RigidBody2D _actorBody;
    private float _dampingCache;
    private double _currentTime = 0f;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {

        _state = BehaviourState.Running;
        GetLocationFromBlackboard(blackboard);
        if (_state == BehaviourState.Failure)
        {
            return _state;
        }
        GetRigidBodyFromBlackboard(blackboard);
        
        if (blackboard.IsVerbose && _currentTime == 0)
        {
            GD.Print($"[{GetType().Name}] Begin move to {_moveDestination}!");
        }
        _currentTime += delta;
        MoveToLocation(delta, blackboard);
        return _state;
    }


    private void MoveToLocation(double delta, BehaviourTreeBlackboard blackboard)
    {
        var moveVector = _moveDestination - _actorBody.GlobalPosition;
        if (moveVector.Length() < _successDistance)
        {
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] Arrived at {_moveDestination}!");
            }
            _state = BehaviourState.Success;
            return;
        }

        if (_positionLastTick == _actorBody.GlobalPosition && _currentTime >= _failTimeout)
        {
            
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] Could not reach {_moveDestination}!");
            }
            _state = BehaviourState.Failure;
            return;
        }
        
        _positionLastTick = _actorBody.GlobalPosition;
        _actorBody.ApplyForce(moveVector.Normalized() * _moveSpeed * (float)delta);
    }

    private void GetLocationFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (!blackboard.TreeData.TryGetValue(BehaviourDataKeys.LOCATION, out var locationData))
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] No location in blackboard. Try adding a GetLocation behaviour before this one.");
            }
            _state = BehaviourState.Failure;
            return;
        }

        _moveDestination = (Vector2)locationData;

        if (_moveDestination == Vector2.Zero)
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] Invalid object marked as location in Blackboard.");
            }
            _state = BehaviourState.Failure;
        }
    }
    
    private void GetRigidBodyFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (blackboard.Actor is not RigidBody2D actor)
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] Actor in blackboard has no RigidBody2D.");
            }
            _state = BehaviourState.Failure;
            return;
        }
        _actorBody = actor;
    }

    

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _currentTime = 0f;
        _actorBody = null;
        _moveDestination = Vector2.Zero;
    }
}