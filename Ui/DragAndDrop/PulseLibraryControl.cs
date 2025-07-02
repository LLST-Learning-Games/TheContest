using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Systems;

public partial class PulseLibraryControl : Control
{
    [Export] private PackedScene _draggablePrefab;
    [Export] private Control _container;
    [Export] private Label _descriptionLabel;

    private List<Control> _draggables = new();
    private ProjectileLibrary _library => SystemLoader.GetSystem<ProjectileLibrary>();

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
        ClearDraggableUiList();
        var ids = _library.GetAllUnlockedPulseIds();
        PopulateDraggables(ids);
    }
    
    private void DebugShowProjectileDataIgnoringLocks()
    {
        ClearDraggableUiList();
        var ids = _library.GetAllPulseIds().ToList();
        PopulateDraggables(ids);
    }

    private void PopulateDraggables(List<string> ids)
    {
        foreach (var id in ids)
        {
            var newDraggable = _draggablePrefab.Instantiate<Draggable>();
            newDraggable.InitializeId(id);
            newDraggable.IsDraggableSource = true;
            newDraggable.SetDescriptionLabel(_descriptionLabel);
            _draggables.Add(newDraggable);
            _container.AddChild(newDraggable);
        }
    }

    private void ClearDraggableUiList()
    {
        foreach(var draggable in _draggables)
        {
            draggable.QueueFree();
        }

        _draggables.Clear();
    }
}
