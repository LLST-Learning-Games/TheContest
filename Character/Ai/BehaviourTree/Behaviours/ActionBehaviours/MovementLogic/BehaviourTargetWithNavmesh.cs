using System.Collections.Generic;
using Godot;

namespace Behaviours;

public partial class BehaviourTargetWithNavmesh : BehaviourActionBase
{
    [Export] private double _updateTime = 0.1;
    private NavigationAgent2D _navAgent;
    private Node2D _navMeshTarget;
    private Vector2 _nextPosition = Vector2.Zero;
    private double _currentTime = 0;
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        if (_navAgent == null)
        {
            _navAgent = GetNavAgent(blackboard);
        }

        if (_navMeshTarget == null)
        {
            _navMeshTarget = GetNavMeshTarget();
            if (_navMeshTarget == null)
            {
                return BehaviourState.Failure;
            }
            _navAgent.SetTargetPosition(_navMeshTarget.GlobalPosition);
        }
        
        _currentTime += delta;
        if(_currentTime >= _updateTime || _nextPosition == Vector2.Zero)
        {
            _nextPosition = _navAgent.GetNextPathPosition();
            blackboard.TreeData[BehaviourDataKeys.LOCATION] = _nextPosition;
            if (_nextPosition != Vector2.Zero)
            {
                _state = BehaviourState.Success;
            }
            else
            {
                _state = BehaviourState.Failure;
            }
        }
        
        
        return _state;
    }

    private Node2D GetNavMeshTarget()
    {
        var playerGroup = GetTree().GetNodesInGroup("Player");
        if (playerGroup.Count == 0)
        {
            return null;
        }
        Node2D target = playerGroup[0] as Node2D;
        if (target == null)
        {
            _state = BehaviourState.Failure;
        }
        return target;
    }

    private NavigationAgent2D GetNavAgent(BehaviourTreeBlackboard blackboard)
    {
        if (blackboard.Actor is not Enemy actor)
        {
            GD.PrintErr($"[{GetType().Name}] Actor in blackboard is not an enemy.");
            _state = BehaviourState.Failure;
            return null;
        }

        if (actor.NavAgent is null)
        {
            GD.PrintErr($"[{GetType().Name}] Enemy in blackboard has no NavAgent.");
            _state = BehaviourState.Failure;
            return null;
        }
        
        return actor.NavAgent;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _navAgent = null;
        _nextPosition = Vector2.Zero;
        _currentTime = 0;
        _navMeshTarget = null;
    }
}