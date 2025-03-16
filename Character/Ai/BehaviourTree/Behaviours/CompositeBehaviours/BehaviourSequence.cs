using System.Collections.Generic;


namespace Behaviours
{
    public partial class BehaviourSequence : BehaviourCompositeBase
    {
        private BehaviourBase currentBehaviour;
        public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
        {
            if (currentBehaviour != null)
            {
                _state = currentBehaviour.UpdateNode(delta, blackboard);
                if (_state == BehaviourState.Running)
                {
                    return _state;
                }
            }
            
            foreach (var child in _children)
            {
                _state = child.UpdateNode(delta, blackboard);
                if (_state == BehaviourState.Running)
                {
                    currentBehaviour = child;
                    break;
                }
            }

            return _state;
        }

        public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
        {
            foreach (var child in _children)
            {
                child.ResetBehaviour(blackboard);
            }
        }
    }
}