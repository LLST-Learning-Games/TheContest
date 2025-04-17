using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentTimedInstance : ProjectileSegmentInstance
{
   private double _lifetime = 0.7f;
   private double _timeSinceInit;

   private ITimedSegment _timedSegment;
   
   public override void Initialize(ProjectileSegmentData data, Array<ProjectileSegmentDefinition> children)
   {
      base.Initialize(data, children);
      _timeSinceInit = 0;
      _timedSegment = _segmentData as ITimedSegment;
      
      if (_timedSegment is null)
      {
         GD.PrintErr($"[{GetType().Name}] ProjectileSegmentTimedInstance must be used with a data type that implements ITimedSegment");
         return;
      }
      
      _lifetime = _timedSegment.GetLifetime();
   }

   public override void _Process(double delta)
   {
      _timeSinceInit += delta;
      if (_timeSinceInit >= _lifetime)
      {
         OnTriggerEntered(null);
      }
   }
}