using System.Collections.Generic;


namespace Behaviours
{
    public class BehaviourSequence : BehaviourTreeNodeBase
    {
        public override BehaviourNodeState UpdateNode(double delta, Dictionary<BehaviourDataKeys, object> treeData)
        {
            foreach (var child in _children)
            {
                _state = child.UpdateNode(delta, treeData);
                if (_state == BehaviourNodeState.Running)
                {
                    break;
                }
            }

            return _state;
        }
    }
}