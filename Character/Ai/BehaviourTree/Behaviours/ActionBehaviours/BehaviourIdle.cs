using System.Collections.Generic;
using Godot;

namespace Behaviours;

public partial class BehaviourIdle : BehaviourBase
{
    [Export] private double _idleTime;
    
    private double _currentTime;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        if (blackboard.IsVerbose && _currentTime == 0)
        {
            GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Begin idle!");
        }
        _currentTime += delta;
        //GD.Print($"[{GetType().Name}] Idling for {_currentTime} seconds!");
        if (_currentTime < _idleTime)
        {
            return BehaviourState.Running;
        }
        
        if(blackboard.IsVerbose)
        {
            GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Idle complete after {_currentTime} seconds!");
        }
        return BehaviourState.Success;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _currentTime = 0;
    }
}