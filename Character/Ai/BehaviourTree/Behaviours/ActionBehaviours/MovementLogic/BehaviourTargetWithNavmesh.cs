using System.Collections.Generic;
using Godot;

namespace Behaviours;

public partial class BehaviourTargetWithNavmesh : BehaviourActionBase
{
    [Export] private double _updateTime = 0.1;
    [Export] public float SightDistance = 1000f;
    [Export] public uint CollisionMask = 1 << 4;
    private NavigationAgent2D _navAgent;
    private Node2D _navMeshTarget;
    private Vector2 _nextPosition = Vector2.Zero;
    private double _currentTime = 0;
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
        
        if (HasLineOfSightToTarget(target))
        {
            return target;
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

    private bool HasLineOfSightToTarget(Node2D target)
    {
        if (_actorBody.GlobalPosition.DistanceTo(target.GlobalPosition) >= SightDistance)
        {
            return false;
        }
        
        var spaceState = target.GetWorld2D().DirectSpaceState;

        var query = new PhysicsRayQueryParameters2D
        {
            From = _actorBody.GlobalPosition,
            To = target.GlobalPosition,
            CollisionMask = CollisionMask,
            HitFromInside = true
        };

        var result = spaceState.IntersectRay(query);

        if (result.Count == 0)
        {
            // Nothing in the way — clear line of sight
            return true;
        }
        
        return false;
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
        _currentTime = 0;
        _navMeshTarget = null;
    }
}