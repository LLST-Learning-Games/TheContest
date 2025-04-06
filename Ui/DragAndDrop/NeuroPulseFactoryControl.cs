using Godot;
using Godot.Collections;
using Systems;
using TheContest.Projectiles;

public partial class NeuroPulseFactoryControl : Control
{
    [Export] private Array<Draggable> _pulses; 
    
    private ProjectileLibrary Library => _library ??= SystemLoader.GetSystem<ProjectileLibrary>();
    private ProjectileLibrary _library;
    
    public void OnConfirmSelection()
    {
        var children = Library.Factory.GetChildren();
        foreach (var child in children)
        {
            child.QueueFree();
            return;
        }

        ProjectileSegmentDefinition trajectoryDefinition = Library.Factory.TryAddPulse(_pulses[0].ProjectileId);
        Library.Factory.TryAddPulse(_pulses[1].ProjectileId,trajectoryDefinition);
        
        var newWeapon = Library.Factory.ExportNeuroPulse();
        Library.Factory.AddChild(newWeapon);
        
        GetParent().QueueFree();        // clear the selection UI
    }
}
