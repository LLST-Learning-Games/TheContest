using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Systems;

public partial class PulseLibraryControl : Control
{
    [Export] private PackedScene _draggablePrefab;
    [Export] private Control _container;

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
        var ids = _library.GetAllPulseIds();
        foreach (var id in ids)
        {
            var newDraggable = _draggablePrefab.Instantiate<Draggable>();
            newDraggable.SetId(id);
            _draggables.Add(newDraggable);
            _container.AddChild(newDraggable);
        }
    }
}
