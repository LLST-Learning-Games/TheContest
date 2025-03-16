using Godot;

namespace Behaviours;

public partial class BehaviourMoveTo : BehaviourActionBase
{
    [Export] private float _successDistance = 10f;
    [Export] private float _moveSpeed = 50f;
    
    private Vector2 _moveDestination = Vector2.Zero;
    private RigidBody2D _actorBody;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        _state = BehaviourState.Running;
        GetLocationFromBlackboard(blackboard);
        GetRigidBodyFromBlackboard(blackboard);
        MoveToLocation(delta);
        return _state;
    }


    private void MoveToLocation(double delta)
    {
        if ((_moveDestination - _actorBody.Position).Length() < _successDistance)
        {
            _state = BehaviourState.Success;
            return;
        }
        _actorBody.Position = _actorBody.Position.Lerp(_moveDestination,  _moveSpeed * (float)delta);
    }

    private void GetLocationFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (!blackboard.TreeData.TryGetValue(BehaviourDataKeys.LOCATION, out var locationData))
        {
            GD.PrintErr(
                $"[{GetType().Name}] No location in blackboard. Try adding a GetLocation behaviour before this one.");
            _state = BehaviourState.Failure;
        }

        _moveDestination = (Vector2)locationData;

        if (_moveDestination == Vector2.Zero)
        {
            GD.PrintErr($"[{GetType().Name}] Invalid object marked as location in Blackboard.");
            _state = BehaviourState.Failure;
        }
    }
    
    private void GetRigidBodyFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (blackboard.Actor is not RigidBody2D actor)
        {
            GD.PrintErr($"[{GetType().Name}] Actor in blackboard has no RigidBody2D.");
            _state = BehaviourState.Failure;
            return;
        }
        _actorBody = actor;
    }

    

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _actorBody = null;
        _moveDestination = Vector2.Zero;
    }
}