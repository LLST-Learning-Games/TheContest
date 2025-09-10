using System.Collections.Generic;
using Godot;

namespace Behaviours;

public partial class BehaviourFindSafeLocation : BehaviourBase
{
    [Export] private BehaviourDataKeys _safeFromTypeKey = BehaviourDataKeys.TARGET;
    [Export] private BehaviourDataKeys _destinationTypeKey = BehaviourDataKeys.LOCATION;
    [Export] private float _safeDistance = 200f;
    
    private RigidBody2D _actorBody;
    private Node2D _safeFromAgent;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        _state = BehaviourState.Running;
        GetRigidBodyFromBlackboard(blackboard);
        GetSafeFromAgentFromBlackboard(blackboard);
        if (_state != BehaviourState.Failure)
        {
            GetSafeLocation(blackboard);
        }
        
        return _state;
    }

    private void GetSafeLocation(BehaviourTreeBlackboard blackboard)
    {
        Vector2 direction = _safeFromAgent.GlobalPosition - _actorBody.GlobalPosition;
        direction = direction.Normalized() * _safeDistance;
        blackboard.TreeData[_destinationTypeKey] = direction;        
        if(blackboard.IsVerbose)
        {
            GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Found safe location {direction} and added it to blackboard!");
        }
        _state = BehaviourState.Success;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        blackboard.TreeData.Remove(_destinationTypeKey);
    }
    
    private void GetRigidBodyFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (blackboard.Actor is not RigidBody2D actor)
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] [{blackboard.Actor.Name}] Actor in blackboard has no RigidBody2D.");
            }
            _state = BehaviourState.Failure;
            return;
        }
        _actorBody = actor;
    }
    
    private void GetSafeFromAgentFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (!blackboard.TreeData.TryGetValue(_safeFromTypeKey, out var target))
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] [{blackboard.Actor.Name}] No SafeFromAgent in blackboard. Try adding a GetTarget behaviour before this one.");
            }
            _state = BehaviourState.Failure;
        }
        
        _safeFromAgent = target as Node2D;

        if (_safeFromAgent is null)
        {
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Invalid SafeFromAgent in Blackboard.");
            }
            _state = BehaviourState.Failure;
        }
        
    }
}