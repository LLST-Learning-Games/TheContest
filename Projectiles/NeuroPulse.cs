using Godot;

namespace TheContest.Projectiles;

public partial class NeuroPulse : Node
{
    [Export] private ProjectileSegmentDefinition _startingSegment;
    [Export] private float _pulseDelay = 0.5f;
    
    public void Fire(Vector2 globalPosition, float direction) => _startingSegment.Fire(globalPosition, direction);
    
    public float GetDelay() => _pulseDelay;
    public void SetDelay(float delay) => _pulseDelay = delay;

    public void SetIsEnemy(bool isEnemy)
    {
        _startingSegment.SetIsEnemy(isEnemy);
    }
    
    public void InjectStartingSegment(ProjectileSegmentDefinition startingSegment, bool isEnemy = false)
    {
        SetDelay(startingSegment.GetData().Delay);
        _startingSegment = startingSegment;
        SetIsEnemy(isEnemy);
    }
}