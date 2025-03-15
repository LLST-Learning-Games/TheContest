using System;
using System.Collections.Generic;

namespace Behaviours
{

    public abstract class BehaviourTreeNodeBase
    {
        protected IList<BehaviourTreeNodeBase> _children;
        protected BehaviourNodeState _state;

        public virtual void Initialize(IList<BehaviourTreeNodeBase> children, Dictionary<BehaviourDataKeys, Object> data)
        {
            _children = children;
        }

        public abstract BehaviourNodeState UpdateNode(double delta, Dictionary<BehaviourDataKeys, object> treeData);
        public Action<BehaviourTreeNodeBase> OnSuccess;
        public Action<BehaviourTreeNodeBase> OnFail;
    }
}