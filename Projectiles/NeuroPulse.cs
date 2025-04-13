using Godot;

namespace TheContest.Projectiles;

public partial class NeuroPulse : Node
{
    [Export] private ProjectileSegmentDefinition _startingSegment;
    [Export] private float _pulseDelay = 0.5f;
    [Export] private float _maxEnergy = 100f;
    [Export] private float _rechargeRate = 10f;
    private float _currentEnergy;
    private bool _isEnemy = false;

    public ProjectileSegmentDefinition StartingSegment => _startingSegment;
    public bool CanFire => _currentEnergy > 0;

    public override void _Ready()
    {
        _currentEnergy = _maxEnergy;
    }

    public override void _Process(double delta)
    {
        if(!_isEnemy)
            GD.Print($"[NeuroPulse] EndlessEnergyUpdate: {_currentEnergy}");
        
        if (_currentEnergy < _maxEnergy)
        {
            _currentEnergy += (float)(delta * _rechargeRate);
            if (_currentEnergy > _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
                if(!_isEnemy)
                    GD.Print($"[NeuroPulse] Bzzzt! Energy is full! Energy: {_currentEnergy}");
            }
        }
    }


    public void Fire(Vector2 globalPosition, float direction) => _startingSegment.Fire(globalPosition, direction, this);

    public void UpdateEnergyByDelta(float delta)
    {
        _currentEnergy += delta;
        if(!_isEnemy)
            GD.Print($"[NeuroPulse] Zap! Energy Spent: {delta} New Energy: {_currentEnergy}");
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