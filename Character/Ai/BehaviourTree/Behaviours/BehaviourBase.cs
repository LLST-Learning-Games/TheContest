using System;
using Godot;

namespace Behaviours
{
    public abstract partial class BehaviourBase : Node
    {
        protected BehaviourState _state;

        public abstract BehaviourState UpdateNode(double delta,
            BehaviourTreeBlackboard blackboard);
        
        public abstract void ResetBehaviour(BehaviourTreeBlackboard blackboard);

        public Action<BehaviourCompositeBase> OnSuccess;
        public Action<BehaviourCompositeBase> OnFail;
    }
}