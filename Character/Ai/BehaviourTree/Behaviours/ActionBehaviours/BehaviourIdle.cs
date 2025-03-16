using System.Collections.Generic;
using Godot;

namespace Behaviours;

public partial class BehaviourIdle : BehaviourBase
{
    [Export] private double _idleTime;
    
    private double _currentTime;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        _currentTime += delta;
        GD.Print($"[{GetType().Name}] Idling for {_currentTime} seconds!");
        if (_currentTime < _idleTime)
        {
            return BehaviourState.Running;
        }
        return BehaviourState.Success;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _currentTime = 0;
    }
}