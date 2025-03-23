using System;
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
        _instance.GlobalPosition = globalPosition;
        _instance.Rotation = facing;
        _instance.Initialize(_segmentData);
        _instance.BodyEntered += OnCollide;
    }

    private void OnCollide(Node body)
    {
        foreach (var child in _children)
        {
            child.Fire(_instance.GlobalPosition, _instance.Rotation);
        }
    }
}