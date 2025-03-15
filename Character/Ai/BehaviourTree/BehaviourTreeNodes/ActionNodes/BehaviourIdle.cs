using System.Collections.Generic;
using Godot;

namespace Behaviours;

public class BehaviourIdle : BehaviourTreeNodeBase
{
    public override BehaviourNodeState UpdateNode(double delta, Dictionary<BehaviourDataKeys, object> treeData)
    {
        GD.Print($"[{GetType().Name}] Idling!");
        return BehaviourNodeState.Running;
    }
}