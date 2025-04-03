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

        public BehaviourTree(BehaviourCompositeBase rootNode, Node2D actor, double resetLogicTimer = 0, bool isVerbose = false)
        {
            _rootNode = rootNode;
            _resetLogicTimer = resetLogicTimer;
            _treeData = new BehaviourTreeBlackboard 
            {      
                Actor = actor,
                Tree = this,
                IsVerbose = isVerbose
            };
        }

        public void UpdateBehaviour(double delta)
        {
            BehaviourState state = _rootNode.UpdateNode(delta, _treeData);
            switch (state)
            {
                case BehaviourState.Failure:
                    if(_treeData.IsVerbose)
                    {
                        GD.Print($"[{GetType().Name}] Could not find behaviour node to run. Check the logic in your tree!");
                    }
                    break;
                case BehaviourState.Success:
                    if(_treeData.IsVerbose)
                    {
                        GD.Print($"[{GetType().Name}] Reached end of root behaviour. Resetting logic...");
                    }
                    ResetLogic();
                    break;
                case BehaviourState.Running:
                    // all is good here, keep doing what you're doing
                    break;
            }

            if (_resetLogicTimer <= 0)
            {
                return;
            }
            
            _currentTime += delta;
            if (_currentTime >= _resetLogicTimer)
            {
                ResetLogic();
            }
        }

        private void ResetLogic()
        {
            _rootNode.ResetBehaviour(_treeData);
            _currentTime = 0;
        }
    }
}