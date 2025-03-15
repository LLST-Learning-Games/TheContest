using Godot;
using Godot.Collections;

namespace Behaviours;

public partial class BehaviourGetTarget : BehaviourTreeNodeBase
{
    public override void Initialize(Array<BehaviourTreeNodeBase> children, System.Collections.Generic.Dictionary<BehaviourDataKeys, object> treeData)
    {
        base.Initialize(children, treeData);
    }

    public override BehaviourNodeState UpdateNode(double delta, System.Collections.Generic.Dictionary<BehaviourDataKeys, object> treeData)
    {
        object target = null;
        if (!treeData.TryGetValue(BehaviourDataKeys.TARGET, out target))
        {
            target = GetTarget();
            treeData[BehaviourDataKeys.TARGET] = target;
        }

        if (target is null)
        {
            return BehaviourNodeState.Failure;
        }
        else
        {
            return BehaviourNodeState.Success;
        }
    }

    public string GetTarget()
    {
        return "testing";
    }
    
}