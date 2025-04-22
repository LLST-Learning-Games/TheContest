using System;
using Godot;

namespace TheContest.Projectiles;

public partial class NeuroPulse : Node
{
    [Export] private ProjectileSegmentDefinition _startingSegment;
    [Export] private float _pulseDelay = 0.5f;
    [Export] private float _maxEnergy = 250f;
    [Export] private float _rechargeRate = 50f;
    private float _currentEnergy;
    private bool _isEnemy = false;

    public ProjectileSegmentDefinition StartingSegment => _startingSegment;
    public float MaxEnergy => _maxEnergy;
    public bool CanFire => _currentEnergy > 0;
    public Action<float> OnEnergyUpdated;

    public override void _Ready()
    {
        _currentEnergy = _maxEnergy;
    }

    public override void _Process(double delta)
    {
        if (_currentEnergy < _maxEnergy)
        {
            UpdateEnergyByDelta((float)(delta * _rechargeRate));
        }
    }


    public void Fire(Vector2 globalPosition, float direction) => _startingSegment.Fire(globalPosition, direction, this);

    public void UpdateEnergyByDelta(float delta, bool clampEnergy = true)
    {
        _currentEnergy += delta;
        if (clampEnergy && _currentEnergy > _maxEnergy)
        {
            _currentEnergy = _maxEnergy;
        }
        
        OnEnergyUpdated?.Invoke(_currentEnergy);
    }

    public float GetDelay() => _pulseDelay;
    public void SetDelay(float delay) => _pulseDelay = delay;

    public void SetIsEnemy(bool isEnemy)
    {
        _isEnemy = isEnemy;
        _startingSegment.SetIsEnemy(isEnemy);
    }
    
    public void InjectStartingSegment(ProjectileSegmentDefinition startingSegment, bool isEnemy = false)
    {
        SetDelay(startingSegment.GetData().Delay);
        _startingSegment = startingSegment;
        SetIsEnemy(isEnemy);
    }
}