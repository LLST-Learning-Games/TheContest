using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Systems;
using TheContest.Projectiles;

public partial class NeuroPulseFactoryControl : Control
{
    [Export] private Control _container;
    [Export] private PackedScene _pulsePrefab;
    [Export] private Label _descriptionLabel;
    private Array<Draggable> _pulses = new(); 
    
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
        CreateNewPulseUi(startingSegment);
    }

    private void CreateNewPulseUi(ProjectileSegmentDefinition currentSegment)
    {
        var newPulses = new List<Draggable>();
        for(int i = 0; i < currentSegment.GetData().AllowedChildCount; i++)
        {
            Draggable newPulse = _pulsePrefab.Instantiate() as Draggable;
            newPulse.SetDescriptionLabel(_descriptionLabel);
            newPulse.InitializeId(currentSegment.GetData().Id);
            newPulses.Add(newPulse);
            _pulses.Add(newPulse);
            _container.AddChild(newPulse);
            if (i < currentSegment.Children.Count)
            {
                var child = currentSegment.Children[i];
                CreateNewPulseUi(child);
            }
            else
            {
                newPulses[i].InitializeId(Draggable.IS_EMPTY);
            }
        }
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
