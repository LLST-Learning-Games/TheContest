using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentDefinition : Node
{
    [Export] private ProjectileSegmentData _segmentData;
    [Export] Array<ProjectileSegmentDefinition> _children;

    private ProjectileSegmentInstance _instance;
    private Vector2 _facing;
    
    public void Fire(Vector2 globalPosition, float facing)
    {
        _instance = _segmentData.InstancePrefab.Instantiate<ProjectileSegmentInstance>();
        GetTree().CurrentScene.AddChild(_instance);
        _instance.GlobalPosition = globalPosition;
        _instance.Rotation = facing;
        _instance.Initialize(_segmentData);
        _segmentData.OnInitialize(_instance);
        _instance.BodyEntered += OnCollide;
    }

    public override void _PhysicsProcess(double delta)
    {
        _segmentData.OnPhysicsProcess(delta, _instance);
    }

    private void OnCollide(Node body)
    {
        SpawnChildren();
        _segmentData.OnCollide(body, _instance);
    }

    private void SpawnChildren()
    {
        if (_children == null || _children.Count == 0)
        {
            return;
        }

        foreach (var child in _children)
        {
            child.Fire(_instance.GlobalPosition, _instance.Rotation);
        }
    }
}