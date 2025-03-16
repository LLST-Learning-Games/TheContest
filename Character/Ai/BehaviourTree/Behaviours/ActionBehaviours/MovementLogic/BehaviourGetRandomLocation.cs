using System;
using Godot;

namespace Behaviours;

public partial class BehaviourGetRandomLocation : BehaviourActionBase
{
    [Export] private float _range = 500f;
    private readonly RandomNumberGenerator _rng = new();
    private Vector2 _randomLocation;
    
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        if(_randomLocation == Vector2.Zero)
        {
            _randomLocation = GetRandomLocationInRange(blackboard);
            blackboard.TreeData[BehaviourDataKeys.LOCATION] = _randomLocation;
            GD.Print($"[{GetType().Name}] Found RandomLocation: {_randomLocation}");
        }

        return BehaviourState.Success;
    }

    public Vector2 GetRandomLocationInRange(BehaviourTreeBlackboard blackboard)
    {
        var directionX = _rng.RandfRange(-_range, _range);
        var directionY = _rng.RandfRange(-_range, _range);
        return new Vector2(directionX, directionY) + blackboard.Actor.Position;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _randomLocation = Vector2.Zero;
        blackboard.TreeData[BehaviourDataKeys.LOCATION] = _randomLocation;
    }
}