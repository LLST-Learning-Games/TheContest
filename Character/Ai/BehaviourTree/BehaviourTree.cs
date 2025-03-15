using System;
using System.Collections.Generic;
using Godot;

namespace Behaviours
{
    public class BehaviourTree
    {
        private BehaviourTreeNodeBase _rootNode;
        private Dictionary<BehaviourDataKeys, Object> _treeData;

        public BehaviourTree(BehaviourTreeNodeBase rootNode, Dictionary<BehaviourDataKeys, Object> treeData)
        {
            _rootNode = rootNode;
            _treeData = treeData;
        }

        public void UpdateBehaviour(double delta)
        {
            // todo - cache the current running node
            if (_rootNode.UpdateNode(delta, _treeData) != BehaviourNodeState.Running)
            {
                GD.PrintErr($"[{GetType().Name}] Could not find behaviour node to run. Check the logic in your tree!");
            }
        }

    }
}