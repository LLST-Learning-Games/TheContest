using Godot;

namespace TheContest.Projectiles;

public partial class NeuroPulse : Node
{
    [Export] private ProjectileSegmentDefinition _startingSegment;
    [Export] private float _pulseDelay = 0.5f;
    
    public void Fire(Vector2 globalPosition, float direction) => _startingSegment.Fire(globalPosition, direction);
    
    public float GetDelay() => _pulseDelay;
    public void SetDelay(float delay) => _pulseDelay = delay;
    
    public void InjectStartingSegment(ProjectileSegmentDefinition startingSegment, bool isEnemy)
    {
        _startingSegment = startingSegment;
        _startingSegment.SetIsEnemy(isEnemy);
    }
}