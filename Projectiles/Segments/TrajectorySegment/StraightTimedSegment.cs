using Godot;

namespace TheContest.Projectiles;

public partial class StraightTimedSegment : StraightTrajectoryDamageSegment, ITimedSegment
{
    [Export] private double _lifetime = 0.3f;
    
    public override string GetDescription()
    {
        var description = base.GetDescription();
        description += "\nTrigger Time: " + _lifetime + "s";
        return description;
    }

    public double GetLifetime()
    {
        return _lifetime;
    }
}