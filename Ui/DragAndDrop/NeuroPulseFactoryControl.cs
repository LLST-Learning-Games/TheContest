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
    private Draggable _rootPulse;
    
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
        _rootPulse = RecursivelyInitializePulseUi(startingSegment);
    }

    private Draggable RecursivelyInitializePulseUi(ProjectileSegmentDefinition currentSegment)
    {
        var currentDraggable = CreateNewPulseUiInstance(currentSegment.GetData().Id);
        
        for(int i = 0; i < currentSegment.GetData().AllowedChildCount; i++)
        {
            Draggable childDraggable;
            if (i < currentSegment.Children.Count)
            {
                var child = currentSegment.Children[i];
                childDraggable = RecursivelyInitializePulseUi(child);
            }
            else
            {
                childDraggable = CreateNewPulseUiInstance(Draggable.IS_EMPTY);
                childDraggable.OnUpdated += UpdatePulseUi;
            }
            currentDraggable.AddTreeChild(childDraggable);
        }

        currentDraggable.OnUpdated += UpdatePulseUi;
        return currentDraggable;
    }

    private void UpdatePulseUi(Draggable draggable)
    {
        draggable.ClearChildren();
        if (draggable.IsEmpty)
        {
            return;
        }
        
        var currentSegment = draggable.Data;

        for(int i = 0; i < currentSegment.AllowedChildCount; i++)
        {
            var childDraggable = CreateNewPulseUiInstance(Draggable.IS_EMPTY);
            draggable.AddTreeChild(childDraggable);
        }
    }
    
    private Draggable CreateNewPulseUiInstance(string currentSegmentId)
    {
        Draggable newPulse = _pulsePrefab.Instantiate() as Draggable;
        newPulse.SetDescriptionLabel(_descriptionLabel);
        newPulse.InitializeId(currentSegmentId);
        _pulses.Add(newPulse);
        _container.AddChild(newPulse);
        return newPulse;
    }

    public void OnConfirmSelection()
    {
        ProjectileSegmentDefinition trajectoryDefinition = Library.Factory.TryAddPulse(_rootPulse._projectileId);
        RecursivelyGeneratePulseTree(trajectoryDefinition, _rootPulse);
        
        var newWeapon = Library.Factory.ExportNeuroPulse();
        Library.SetPlayerPulse(newWeapon);
        
        GetParent().QueueFree();        // clear the selection UI
    }

    private void RecursivelyGeneratePulseTree(ProjectileSegmentDefinition definition, Draggable draggable)
    {
        if (draggable.IsEmpty || draggable.Children is null || draggable.Children.Count == 0)
        {
            return;
        }
        
        foreach (var child in draggable.Children)
        {
            var newDefinition = Library.Factory.TryAddPulse(child._projectileId, definition);
            RecursivelyGeneratePulseTree(newDefinition, child);
        }
    }
}
