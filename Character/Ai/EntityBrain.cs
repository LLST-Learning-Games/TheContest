using System.Collections.Generic;
using Behaviours;
using Godot;
using Godot.Collections;

public partial class EntityBrain : Node
{
    private BehaviourTree _behaviourTree;
    [Export] private BehaviourTreeNodeBase _root;

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
        Array<BehaviourTreeNodeBase> children = new Array<BehaviourTreeNodeBase>();
        children.Add(_root);
        var treeData = new System.Collections.Generic.Dictionary<BehaviourDataKeys, object>();
        rawTree.Initialize(children, treeData);
        return new BehaviourTree(rawTree, treeData);
    }
}