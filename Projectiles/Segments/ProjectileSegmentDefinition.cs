using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentDefinition : Node
{
    [Export] private ProjectileSegmentData _segmentData;
    [Export] private Array<ProjectileSegmentDefinition> _children = new ();

    public Array<ProjectileSegmentDefinition> Children => _children;
    
    private bool _isEnemy;
    
    public void SetData(ProjectileSegmentData data) => _segmentData = data;
    public ProjectileSegmentData GetData() => _segmentData;
    public void SetChildren(Array<ProjectileSegmentDefinition> children)
    {
        _children = children;
    }

    public bool TryAddChild(ProjectileSegmentDefinition candidateChild)
    {
        _children ??= new Array<ProjectileSegmentDefinition>();
        if (_children.Count < _segmentData.AllowedChildCount)
        {
            _children.Add(candidateChild);
            return true;
        }

        return false;
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
        var instance = _segmentData.InstancePrefab.Instantiate<ProjectileSegmentInstance>();

        AddChildToTreeDeferred(instance);
        instance.GlobalPosition = globalPosition;
        instance.Rotation = facing;
        instance.Initialize(_segmentData, _children);
        instance.SetCollisionLayers(_isEnemy);
        _segmentData.OnInitialize(instance, GetTree());
        if (inheritedCollision != null && _segmentData.ShouldInheritCollisions)
        {
            instance.OnTriggerEntered(inheritedCollision);
        }
    }

    public async void AddChildToTreeDeferred(ProjectileSegmentInstance instance)
    {
        await ToSignal(GetTree(), "process_frame");
        if(IsInstanceValid(instance))
        {
            GetTree().CurrentScene.AddChild(instance);
        }
    }




}