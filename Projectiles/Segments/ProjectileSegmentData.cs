using Godot;

namespace TheContest.Projectiles;

public abstract partial class ProjectileSegmentData : Resource
{
    [Export] public string Id;
    [Export] public SpriteFrames SpriteFrames;
    [Export] public Color Colour = Colors.White;
    [Export] public Vector2 Scale = Vector2.One;
    [Export] public int AllowedChildCount = 1;
    [Export] public float Delay = 0.5f;
    [Export] public float Speed;
    [Export] public PackedScene InstancePrefab;
    [Export] public bool ShouldInheritCollisions = false;

    public abstract void OnInitialize(RigidBody2D instanceBody, SceneTree tree);
    public abstract void OnPhysicsProcess(double delta, RigidBody2D instanceBody);
    public abstract void OnCollide(Node otherBody, RigidBody2D instanceBody);
}