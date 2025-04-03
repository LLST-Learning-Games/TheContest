using System.Collections.Generic;
using Behaviours;
using Godot;
using Godot.Collections;

public partial class EntityBrain : Node
{
    private BehaviourTree _behaviourTree;
    [Export] private BehaviourCompositeBase _root;
    [Export] private Node2D _actor;
    [Export] private float _resetLogicTimer;
    [Export] private bool _isVerbose;

    public override void _Ready()
    {
        _behaviourTree = GetNewBehaviourTree();
    }

    public override void _PhysicsProcess(double delta)
    {
        _behaviourTree.UpdateBehaviour(delta);
    }

    // todo - consider moving this into a factory if it gets complicated
    private BehaviourTree GetNewBehaviourTree()
    {
        GD.Print($"[{GetType().Name}] New BehaviourTree created!");

        return new BehaviourTree(_root, _actor, _resetLogicTimer, _isVerbose);
    }
}