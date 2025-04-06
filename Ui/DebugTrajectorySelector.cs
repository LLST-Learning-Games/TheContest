using Godot;
using Godot.Collections;
using Systems;
using TheContest.Projectiles;

public partial class DebugTrajectorySelector : Control
{
    [Export] private ItemList _trajectoryList;
    [Export] private ItemList _collisionList;
    
    private NeuroPulse _playerWeapon;
    private ProjectileLibrary Library => _library ??= SystemLoader.GetSystem<ProjectileLibrary>();
    private ProjectileLibrary _library;
    
    public override void _Ready()
    {
        _trajectoryList.FocusMode = Control.FocusModeEnum.None;
        _collisionList.FocusMode = Control.FocusModeEnum.None;
        if (SystemLoader.IsSystemLoadComplete)
        {
            OnSystemLoadComplete();
        }
        else
        {
            SystemLoader.OnSystemLoadComplete += OnSystemLoadComplete;
        }
    }

    private void OnSystemLoadComplete()
    {
        PopulateList();
    }
    
    private void PopulateList()
    {
        _trajectoryList.Clear();
        var itemNames = Library.GetTrajectoryIds();
        foreach (var name in itemNames)
        {
            _trajectoryList.AddItem(name);
        }
        _trajectoryList.Select(0);
        
        _collisionList.Clear();
        itemNames = Library.GetCollisionIds();
        foreach (var name in itemNames)
        {
            _collisionList.AddItem(name);
        }
        _collisionList.Select(0);
    }
    
    public void OnConfirmSelection()
    {
        var children = Library.GetChildren();
        foreach (var child in children)
        {
            child.QueueFree();
            return;
        }

        int trajectoryIndex = _trajectoryList.GetSelectedItems()[0];
        var trajectoryId = _trajectoryList.GetItemText(trajectoryIndex);
        ProjectileSegmentDefinition trajectoryDefinition = Library.Factory.TryAddPulse(trajectoryId);
        
        int collisionIndex = _collisionList.GetSelectedItems()[0];
        var collisionId = _collisionList.GetItemText(collisionIndex);
        Library.Factory.TryAddPulse(collisionId,trajectoryDefinition);
        
        _playerWeapon = Library.Factory.ExportNeuroPulse();
        Library.AddChild(_playerWeapon);
        
        GetParent().QueueFree();        // clear the selection UI
    }
}
