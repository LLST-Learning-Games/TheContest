using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentDefinition : Node
{
    [Export] private ProjectileSegmentData _segmentData;
    [Export] Array<ProjectileSegmentDefinition> _children;
    
    public void Fire(Vector2 globalPosition, float facing, Node inheritedCollision = null)
    {
        var instance = _segmentData.InstancePrefab.Instantiate<ProjectileSegmentInstance>();
        AddChildToTreeDeferred(instance);
        instance.GlobalPosition = globalPosition;
        instance.Rotation = facing;
        instance.Initialize(_segmentData, _children);
        _segmentData.OnInitialize(instance, GetTree());
        if (inheritedCollision != null)
        {
            instance.OnCollide(inheritedCollision);
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