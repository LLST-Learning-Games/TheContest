using Godot;

namespace Behaviours;

public partial class BehaviourGetTarget : BehaviourActionBase
{
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        GD.Print($"[{GetType().Name}] Getting target!");
        object target = null;
        if (!blackboard.TreeData.TryGetValue(BehaviourDataKeys.TARGET, out target))
        {
            target = GetTarget();
            blackboard.TreeData[BehaviourDataKeys.TARGET] = target;
        }

        if (target is null || target is not Node2D)
        {
            return BehaviourState.Failure;
        }
        else
        {
            return BehaviourState.Success;
        }
    }

    public Node2D GetTarget()
    {
        var playerGroup = GetTree().GetNodesInGroup("Player");
        if (playerGroup.Count == 0)
        {
            return null;
        }
        return playerGroup[0] as Node2D;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        blackboard.TreeData.Remove(BehaviourDataKeys.TARGET);
    }
}