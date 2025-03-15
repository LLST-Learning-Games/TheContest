using System.Collections.Generic;
using Behaviours;
using Godot;

public partial class EntityBrain : Node
{
    private BehaviourTree _behaviourTree;

    public override void _Ready()
    {
        _behaviourTree = GetNewBehaviourTree();
    }

    public override void _Process(double delta)
    {
        _behaviourTree.UpdateBehaviour(delta);
    }

    // todo - consider moving this into a factory if it gets complicated
    private BehaviourTree GetNewBehaviourTree()
    {
        GD.Print($"[{GetType().Name}] New BehaviourTree created!");
        BehaviourTreeNodeBase rawTree = new BehaviourSequence();
        List<BehaviourTreeNodeBase> children = new List<BehaviourTreeNodeBase>();
        children.Add(new BehaviourIdle());
        var treeData = new Dictionary<BehaviourDataKeys, object>();
        rawTree.Initialize(children, treeData);
        return new BehaviourTree(rawTree, treeData);
    }
}