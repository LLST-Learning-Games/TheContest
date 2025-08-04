using Godot;

namespace Behaviours;

public partial class BehaviourGetTarget : BehaviourActionBase
{
    [Export] public float SightDistance = 1000f;
    [Export] public uint CollisionMask = 1 << 4;
    
    private RigidBody2D _actorBody;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        GetRigidBodyFromBlackboard(blackboard);
        if(blackboard.IsVerbose)
        {
            GD.Print($"[{GetType().Name}] Getting target!");
        }
        object target = null;
        if (!blackboard.TreeData.TryGetValue(BehaviourDataKeys.TARGET, out target))
        {
            target = GetTarget(blackboard);
            blackboard.TreeData[BehaviourDataKeys.TARGET] = target;
        }

        if (target is null || target is not Node2D)
        {
            if (blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] No valid target. Returning failure.");
            }

            return BehaviourState.Failure;
        }

        return BehaviourState.Success;
    }

    private Node2D GetTarget(BehaviourTreeBlackboard blackboard)
    {
        var targetGroup = GetTree().GetNodesInGroup("Player");
        if (targetGroup.Count == 0)
        {
            return null;
        }
        Node2D target = targetGroup[0] as Node2D;
        if (HasLineOfSightToTarget(target))
        {
            return target;
        }
        
        if (blackboard.IsVerbose)
        {
            GD.Print($"[{GetType().Name}] Found target, but no line of sight.");
        }
        
        return null;
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

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        blackboard.TreeData.Remove(BehaviourDataKeys.TARGET);
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
}