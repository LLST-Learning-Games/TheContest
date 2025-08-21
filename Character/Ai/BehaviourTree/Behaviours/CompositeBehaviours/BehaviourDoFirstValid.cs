using System.Collections.Generic;
using Godot;


namespace Behaviours
{
    public partial class BehaviourDoFirstValid : BehaviourCompositeBase
    {
        private BehaviourBase _currentBehaviour;
        public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
        {
            if (_currentBehaviour != null)
            {
                _state = _currentBehaviour.UpdateNode(delta, blackboard);
                if (_state == BehaviourState.Running)
                {
                    return _state;
                }
            }
            
            foreach (var child in _children)
            {
                if (blackboard.IsVerbose)
                {
                    GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Current behaviour: {child.Name}.");
                }
                _state = child.UpdateNode(delta, blackboard);
                if (blackboard.IsVerbose)
                {
                    GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Current state of {child.Name}: {_state}.");
                }
                if (_state == BehaviourState.Running || _state == BehaviourState.Success)
                {
                    _currentBehaviour = child;
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
            _currentBehaviour = null;
        }
    }
}