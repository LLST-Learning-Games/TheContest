using Godot;
using Godot.Collections;
using Systems;
using TheContest.Projectiles;

public partial class NeuroPulseFactoryControl : Control
{
    [Export] private Array<Draggable> _pulses = new(); 
    [Export] private PackedScene _pulsePrefab;
    
    private ProjectileLibrary Library => _library ??= SystemLoader.GetSystem<ProjectileLibrary>();
    private ProjectileLibrary _library;
    
    public override void _Ready()
    {
        if (SystemLoader.IsSystemLoadComplete)
        {
            LookupProjectileData();
        }
        else
        {
            SystemLoader.OnSystemLoadComplete += LookupProjectileData;
        }
    }

    private void LookupProjectileData()
    {
        var startingSegment = Library.PlayerPulse.StartingSegment;
        _pulses[0].InitializeId(startingSegment.GetData().Id);
        var nextSegment = startingSegment.Children[0];
        _pulses[1].InitializeId(nextSegment.GetData().Id);
    }


    public void OnConfirmSelection()
    {
        ProjectileSegmentDefinition trajectoryDefinition = Library.Factory.TryAddPulse(_pulses[0].ProjectileId);
        Library.Factory.TryAddPulse(_pulses[1].ProjectileId,trajectoryDefinition);
        
        var newWeapon = Library.Factory.ExportNeuroPulse();
        Library.SetPlayerPulse(newWeapon);
        
        GetParent().QueueFree();        // clear the selection UI
    }
}
