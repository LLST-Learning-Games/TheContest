using Behaviours.Emotions;
using Godot;

namespace Behaviours
{
    public record BehaviourTreeBlackboard
    {
        public LimbicSystem LimbicSystem;
        public Node2D Actor;
        public System.Collections.Generic.Dictionary<BehaviourDataKeys, object> TreeData = new();
        public BehaviourTree Tree;
        public bool IsVerbose;
    }
}