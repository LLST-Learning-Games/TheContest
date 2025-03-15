using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Behaviours
{

    public abstract partial class BehaviourTreeNodeBase : Node
    {
        [Export] protected Array<BehaviourTreeNodeBase> _children;
        protected BehaviourNodeState _state;

        public virtual void Initialize(Array<BehaviourTreeNodeBase> children, System.Collections.Generic.Dictionary<BehaviourDataKeys, Object> data)
        {
            _children = children;
        }

        public abstract BehaviourNodeState UpdateNode(double delta, System.Collections.Generic.Dictionary<BehaviourDataKeys, object> treeData);
        public Action<BehaviourTreeNodeBase> OnSuccess;
        public Action<BehaviourTreeNodeBase> OnFail;
    }
}