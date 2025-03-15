using Godot;
using System;

public partial class DebugTrajectorySelector : Control
{
    [Export] private ItemList _itemList;
    private PlayerProjectileSpawnComponent _spawnComponent;
    private ProjectileLibrary _library;
    
    public override void _Ready()
    {
        base._Ready();
        _itemList.FocusMode = Control.FocusModeEnum.None;
        _library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");
        PopulateList();
        _itemList.ItemSelected += OnItemSelected;
        var character = GetNode<Character>("/root/Scene/Character");
        _spawnComponent = character.GetNode<PlayerProjectileSpawnComponent>("PlayerProjectileSpawnComponent");
    }

    private void PopulateList()
    {
        _itemList.Clear();
        var itemNames = _library.GetTrajectoryIds();
        foreach (var name in itemNames)
        {
            _itemList.AddItem(name.Substring(10));
        }
    }

    private void OnItemSelected(long index)
    {
        string itemName = _itemList.GetItemText((int)index);
        if (string.IsNullOrEmpty(itemName))
        {
            return;
        }
        _spawnComponent.SetCurrentTrajectoryId("Trajectory" + itemName);
    }

}
