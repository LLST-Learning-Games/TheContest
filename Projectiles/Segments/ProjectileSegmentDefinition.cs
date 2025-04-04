using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentDefinition : Node
{
    [Export] private ProjectileSegmentData _segmentData;
    [Export] private Array<ProjectileSegmentDefinition> _children = new ();
    
    private Array<ProjectileSegmentInstance> _instancesQueuedToAdd = new ();
    private bool _isEnemy;
    private Node2D _currentMap;
    
    public void SetData(ProjectileSegmentData data) => _segmentData = data;
    public ProjectileSegmentData GetData() => _segmentData;
    public void SetChildren(Array<ProjectileSegmentDefinition> children)
    {
        _children = children;
    }

    public void SetIsEnemy(bool isEnemy)
    {
        _isEnemy = isEnemy;
        PassIsEnemyToChildren();
    }

    private void PassIsEnemyToChildren()
    {
        if(_children == null)
        {
            return;
        }

        foreach (var child in _children)
        {
            child.SetIsEnemy(_isEnemy);
        }
    }
    
    public void Fire(Vector2 globalPosition, float facing, Node inheritedCollision = null)
    {
        if (!IsInstanceValid(this))
        {
            return;
        }
        
        var instance = _segmentData.InstancePrefab.Instantiate<ProjectileSegmentInstance>();

        _instancesQueuedToAdd.Add(instance);
        instance.GlobalPosition = globalPosition;
        instance.Rotation = facing;
        _segmentData.OnInitialize(instance, GetTree());
        if (inheritedCollision != null && _segmentData.ShouldInheritCollisions)
        {
            instance.OnCollide(inheritedCollision);
        }
    }

    private Node2D GetCurrentMap()
    {
        var mapGroup = GetTree().GetNodesInGroup("CurrentMap");
        if (mapGroup.Count == 0)
        {
            return null;
        }
        return mapGroup[0] as Node2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_instancesQueuedToAdd.Count == 0)
        {
            return;
        }

        _currentMap ??= GetCurrentMap();
        
        foreach (var instance in _instancesQueuedToAdd)
        {
            if(IsInstanceValid(this) && IsInstanceValid(instance))
            {
                instance.Initialize(_segmentData, _children);
                instance.SetCollisionLayers(_isEnemy);
                var oldPosition = instance.GlobalPosition;
                _currentMap.AddChild(instance);
                instance.GlobalPosition = oldPosition;     //this is a bit of a sloppy hack but it gets us there
                
            }
        }
        
        _instancesQueuedToAdd.Clear();
    }

    // public async void AddChildToTreeDeferred(ProjectileSegmentInstance instance)
    // {
    //     await ToSignal(GetTree(), "process_frame");
    //     if(IsInstanceValid(this) && IsInstanceValid(instance))
    //     {
    //         GetTree().CurrentScene.AddChild(instance);
    //     }
    // }
    
    // public void AddChildToTreeDeferred(ProjectileSegmentInstance instance)
    // {
    //     CallDeferred(nameof(AddChildToTreeDeferred), instance);
    // }
    //
    // private void AddChildToTree(ProjectileSegmentInstance instance)
    // {
    //     if(IsInstanceValid(instance))
    //     {
    //         GetTree().CurrentScene.AddChild(instance);
    //     }
    // }




}