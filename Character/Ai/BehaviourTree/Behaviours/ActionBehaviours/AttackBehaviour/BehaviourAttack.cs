using Godot;

namespace Behaviours;

public partial class BehaviourAttack : BehaviourActionBase
{
    [Export] private double _attackTime = 5;

    private double _currentTime = 0;
    private Node2D _attackTarget;
    private EnemyProjectileSpawnComponent _spawnComponent;
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        if(blackboard.IsVerbose && _currentTime == 0)
        {
            GD.Print($"[{GetType().Name}] Attack starting!");
        }
        _currentTime += delta;
        if (_currentTime > _attackTime)
        {
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] Attack Complete!");
            }
            _state = BehaviourState.Success;
            return _state;
        }
        _state = BehaviourState.Running;
        GetTargetFromBlackboard(blackboard);

        if (_state != BehaviourState.Running)
        {
            return _state;
        }

        GetEnemyProjectileSpawnComponent(blackboard);
        
        _spawnComponent.SetTarget(_attackTarget);
        if (_state == BehaviourState.Failure)
        {
            ResetBehaviour(blackboard);
        }

        return _state;
    }


    private void GetEnemyProjectileSpawnComponent(BehaviourTreeBlackboard blackboard)
    {
        blackboard.TreeData.TryGetValue(BehaviourDataKeys.ENEMY_SPAWN_COMPONENT, out var spawnComponent);
        _spawnComponent = spawnComponent as EnemyProjectileSpawnComponent;
        
        if (_spawnComponent is null)
        {
            var enemy = blackboard.Actor as Enemy;      // todo - this should not have to be an enemy
            if (enemy is null)
            { 
                if(blackboard.IsVerbose)
                {
                    GD.PrintErr($"[{GetType().Name}] Actor in blackboard is not an enemy.");
                }
                _state = BehaviourState.Failure;
                return;
            }

            _spawnComponent = enemy.EnemyProjectileSpawnComponent;
            blackboard.TreeData[BehaviourDataKeys.ENEMY_SPAWN_COMPONENT] = _spawnComponent;
        }
    }

    private void GetTargetFromBlackboard(BehaviourTreeBlackboard blackboard)
    {
        if (!blackboard.TreeData.TryGetValue(BehaviourDataKeys.TARGET, out var target))
        {
            if(blackboard.IsVerbose)
            {
                GD.PrintErr($"[{GetType().Name}] No target in blackboard. Try adding a GetTarget behaviour before this one.");
            }
            _state = BehaviourState.Failure;
        }
        
        _attackTarget = target as Node2D;

        if (_attackTarget is null)
        {
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] No attack target in Blackboard.");
            }
            blackboard.TreeData.Remove(BehaviourDataKeys.ENEMY_SPAWN_COMPONENT);
            _state = BehaviourState.Failure;
        }
        
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        _attackTarget = null;
        _spawnComponent?.SetTarget(null);
        _spawnComponent = null;
        blackboard.TreeData.Remove(BehaviourDataKeys.ENEMY_SPAWN_COMPONENT);
        _currentTime = 0;
    }
}