using System.Collections.Generic;
using Godot;

namespace Behaviours;

public partial class BehaviourTargetWithNavmesh : BehaviourActionBase
{
    [Export] private double _updateTime = 0.1;
    [Export] public float SightDistance = 1000f;
    [Export] public uint CollisionMask = 1 << 4;
    [Export] private BehaviourDataKeys _targetTypeKey = BehaviourDataKeys.TARGET;
    [Export] private BehaviourDataKeys _destinationTypeKey = BehaviourDataKeys.LOCATION;
    
    private NavigationAgent2D _navAgent;
    private Node2D _navMeshTarget;
    private Vector2 _nextPosition = Vector2.Zero;
    private double _currentTime = double.MaxValue;
    private RigidBody2D _actorBody;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        if (_navAgent == null)
        {
            _navAgent = GetNavAgent(blackboard);
        }

        GetRigidBodyFromBlackboard(blackboard);
        
        if (_navMeshTarget == null)
        {
            _navMeshTarget = GetNavMeshTarget(blackboard);
            if (_navMeshTarget == null)
            {
                return BehaviourState.Failure;
            }
            _navAgent.SetTargetPosition(_navMeshTarget.GlobalPosition);
        }
        
        if(_currentTime >= _updateTime || _nextPosition == Vector2.Zero)
        {
            _currentTime = 0;
            _nextPosition = _navAgent.GetNextPathPosition();
            blackboard.TreeData[_destinationTypeKey] = _nextPosition;
            if (_nextPosition != Vector2.Zero)
            {
                _state = BehaviourState.Success;
            }
            else
            {
                _state = BehaviourState.Failure;
            }
        }
        _currentTime += delta;
        
        
        return _state;
    }

    private Node2D GetNavMeshTarget(BehaviourTreeBlackboard blackboard)
    {
        if (!blackboard.TreeData.TryGetValue(_targetTypeKey, out var targetData))
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] No data of type {_targetTypeKey} found.");
            }
            return null;
        }
        
        if (targetData is Node2D target)
        {
            //if (HasLineOfSightToTarget(target))
            {
                return target;
            }
        }
        return null;
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
    
    private NavigationAgent2D GetNavAgent(BehaviourTreeBlackboard blackboard)
    {
        if (blackboard.Actor is not Enemy actor)
        {
            
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] Actor in blackboard is not an enemy.");
            }
            _state = BehaviourState.Failure;
            return null;
        }

        if (actor.NavAgent is null)
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] Enemy in blackboard has no NavAgent.");
            }
            _state = BehaviourState.Failure;
            return null;
        }
        
        return actor.NavAgent;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _navAgent = null;
        _nextPosition = Vector2.Zero;
        _navMeshTarget = null;
        _currentTime = double.MaxValue;
    }
}