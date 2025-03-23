using Godot;
using System;

public partial class DebugTrajectorySelector : Control
{
    [Export] private ItemList _trajectoryList;
    [Export] private ItemList _collisionList;
    private PlayerProjectileSpawnComponent _spawnComponent;
    private ProjectileLibrary _library;
    
    public override void _Ready()
    {
        base._Ready();
        _trajectoryList.FocusMode = Control.FocusModeEnum.None;
        _collisionList.FocusMode = Control.FocusModeEnum.None;
        _library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");
        PopulateList();
        _trajectoryList.ItemSelected += OnTrajectorySelected;
        _collisionList.ItemSelected += OnCollisionSelected;
        var character = GetNode<Character>("/root/Scene/Character");
        _spawnComponent = character.GetNode<PlayerProjectileSpawnComponent>("PlayerProjectileSpawnComponent");
    }


    private void PopulateList()
    {
        _trajectoryList.Clear();
        var itemNames = _library.GetTrajectoryIds();
        foreach (var name in itemNames)
        {
            _trajectoryList.AddItem(name.Substring(10));
        }
        
        _collisionList.Clear();
        itemNames = _library.GetCollisionIds();
        foreach (var name in itemNames)
        {
            _collisionList.AddItem(name.Substring(9));
        }
    }

    private void OnTrajectorySelected(long index)
    {
        string itemName = _trajectoryList.GetItemText((int)index);
        if (string.IsNullOrEmpty(itemName))
        {
            return;
        }
        //_spawnComponent.SetCurrentTrajectoryId("Trajectory" + itemName);
    }
    
    private void OnCollisionSelected(long index)
    {
        string itemName = _collisionList.GetItemText((int)index);
        if (string.IsNullOrEmpty(itemName))
        {
            return;
        }
        //_spawnComponent.SetCurrentCollisionId("Collision" + itemName);
    }

}
