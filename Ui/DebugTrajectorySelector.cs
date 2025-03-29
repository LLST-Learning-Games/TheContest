using Godot;
using System;
using Godot.Collections;
using Systems;
using TheContest.Projectiles;

public partial class DebugTrajectorySelector : Control
{
    [Export] private ItemList _trajectoryList;
    [Export] private ItemList _collisionList;
    private NeuroPulse _playerWeapon;
    private ProjectileLibrary _library => SystemLoader.GetSystem<ProjectileLibrary>();
    
    public override void _Ready()
    {
        _trajectoryList.FocusMode = Control.FocusModeEnum.None;
        _collisionList.FocusMode = Control.FocusModeEnum.None;
        var character = GetNode<Character>("../../Character");
        _playerWeapon = character.GetNode<NeuroPulse>("PlayerProjectileSpawnComponent/Weapon_1");
        SystemLoader.OnSystemLoadComplete += OnSystemLoadComplete;
    }

    private void OnSystemLoadComplete()
    {
        PopulateList();
        _trajectoryList.Select(0);
        _collisionList.Select(0);
        _trajectoryList.ItemSelected += OnTrajectorySelected;
        _collisionList.ItemSelected += OnCollisionSelected;
    }


    private void PopulateList()
    {
        _trajectoryList.Clear();
        var itemNames = _library.GetTrajectoryIds();
        foreach (var name in itemNames)
        {
            _trajectoryList.AddItem(name);
        }
        
        _collisionList.Clear();
        itemNames = _library.GetCollisionIds();
        foreach (var name in itemNames)
        {
            _collisionList.AddItem(name);
        }
    }

    private void OnTrajectorySelected(long index)
    {
        string itemName = _trajectoryList.GetItemText((int)index);
        if (string.IsNullOrEmpty(itemName))
        {
            return;
        }

        ProjectileSegmentDefinition definitions = CreateNewDefinitions();
        _playerWeapon.InjectStartingSegment(definitions, false);
    }

    private ProjectileSegmentDefinition CreateNewDefinitions()
    {
        foreach (var child in _playerWeapon.GetChildren())
        {
            child.QueueFree();
        }
        
        int trajectoryIndex = _trajectoryList.GetSelectedItems()[0];
        var trajectoryData = _library.GetTrajectoryResource(_trajectoryList.GetItemText(trajectoryIndex));
        ProjectileSegmentDefinition trajectoryDefinition = CreateDefinition(trajectoryData);
        
        int collisionIndex = _collisionList.GetSelectedItems()[0];
        var collisionData = _library.GetCollisionResource(_collisionList.GetItemText(collisionIndex));
        ProjectileSegmentDefinition collisionDefinition = CreateDefinition(collisionData);
        
        trajectoryDefinition.SetChildren(new Array<ProjectileSegmentDefinition>{collisionDefinition});
        return trajectoryDefinition;
    }

    private ProjectileSegmentDefinition CreateDefinition(ProjectileSegmentData data)
    {
        var definition = new ProjectileSegmentDefinition();
        definition.SetData(data);
        _playerWeapon.AddChild(definition);
        return definition;
    }

    private void OnCollisionSelected(long index)
    {
        string itemName = _collisionList.GetItemText((int)index);
        if (string.IsNullOrEmpty(itemName))
        {
            return;
        }

        ProjectileSegmentDefinition definitions = CreateNewDefinitions();
        _playerWeapon.InjectStartingSegment(definitions, false);
    }

}
