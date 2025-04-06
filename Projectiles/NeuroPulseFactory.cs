using Godot;

namespace TheContest.Projectiles;

public partial class NeuroPulseFactory : Node
{
    private NeuroPulse _neuroPulse;
    private ProjectileLibrary _library;

    public void Initialize(ProjectileLibrary library)
    {
        _library = library;
    }
    private void InjectStartingSegment(ProjectileSegmentDefinition startingSegment, bool isEnemy = false)
    {
        _neuroPulse = new NeuroPulse();
        _neuroPulse.SetDelay(startingSegment.GetData().Delay);
        _neuroPulse.InjectStartingSegment(startingSegment, isEnemy);
    }

    public ProjectileSegmentDefinition TryAddPulse(string newPulseId, ProjectileSegmentDefinition parent = null)
    {
        var pulse = _library.GetAnyResource(newPulseId);
        if (pulse == null)
        {
            return null;
        }
        return TryAddPulse(pulse, parent);
    }
    
    public ProjectileSegmentDefinition TryAddPulse(ProjectileSegmentData newPulse, ProjectileSegmentDefinition parent = null)
    {
        var definition = CreateDefinition(newPulse);
        if (parent is null)
        {
            InjectStartingSegment(definition);
            _neuroPulse.AddChild(definition);
            return definition;
        }
        
        _neuroPulse ??= new NeuroPulse();
        if (parent.TryAddChild(definition))
        {
            _neuroPulse.AddChild(definition);
        }
        return definition;
    }
    
    private ProjectileSegmentDefinition CreateDefinition(ProjectileSegmentData data)
    {
        var definition = new ProjectileSegmentDefinition();
        definition.SetData(data);
        return definition;
    }

    public NeuroPulse ExportNeuroPulse()
    {
        return _neuroPulse;
    }

}