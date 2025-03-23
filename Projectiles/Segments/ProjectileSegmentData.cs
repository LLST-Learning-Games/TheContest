using Godot;

namespace TheContest.Projectiles;

public abstract partial class ProjectileSegmentData : Resource
{
    [Export] public string Id;
    [Export] public SpriteFrames SpriteFrames;
    [Export] public Color Colour;
    [Export] public Vector2 Scale;
    [Export] public int AllowedChildCount;
    [Export] public PackedScene InstancePrefab;

    public abstract void OnInitialize(RigidBody2D instanceBody);
    public abstract void OnPhysicsProcess(double delta, RigidBody2D instanceBody);
    public abstract void OnCollide(Node otherBody, RigidBody2D instanceBody);
}