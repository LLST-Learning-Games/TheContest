using Godot;

namespace TheContest.Projectiles;

public abstract partial class ProjectileSegmentData : Resource
{
    public string Id;
    public SpriteFrames SpriteFrames;
    public Color Colour;
    public Vector2 Scale;
    public int AllowedChildCount;
    public PackedScene InstancePrefab;

    public abstract void OnPhysicsProcess(double delta, RigidBody2D body);
    public abstract void OnCollide(Node otherBody);
}