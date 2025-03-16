using System;
using System.Collections.Generic;
using Godot;

namespace Behaviours
{
    public class BehaviourTree
    {
        private double _resetLogicTimer;
        private BehaviourBase _rootNode;
        private BehaviourTreeBlackboard _treeData;
        private double _currentTime = 0;

        public BehaviourTree(BehaviourCompositeBase rootNode, Node2D actor, double resetLogicTimer = 0)
        {
            _rootNode = rootNode;
            _resetLogicTimer = resetLogicTimer;
            _treeData = new BehaviourTreeBlackboard 
            {      
                Actor = actor,
                Tree = this
            };
        }

        public void UpdateBehaviour(double delta)
        {
            BehaviourState state = _rootNode.UpdateNode(delta, _treeData);
            if (state != BehaviourState.Running)
            {
                GD.PrintErr($"[{GetType().Name}] Could not find behaviour node to run. Check the logic in your tree!");
            }

            if (_resetLogicTimer <= 0)
            {
                return;
            }
            
            _currentTime += delta;
            if (_currentTime >= _resetLogicTimer)
            {
                _rootNode.ResetBehaviour(_treeData);
                _currentTime = 0;
            }
        }

    }
}