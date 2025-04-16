using Godot;

namespace TheContest.Projectiles;

public abstract partial class ProjectileSegmentData : Resource
{
    [Export] public bool IncludeInLibrary = true;
    [Export] public string Id;
    [Export] public Texture2D Icon;
    [Export] public SpriteFrames SpriteFrames;
    [Export] public Color Colour = Colors.White;
    [Export] public Vector2 Scale = Vector2.One;
    [Export] public int Cost = 10;
    [Export] public int AllowedChildCount = 1;
    [Export] public float Delay = 0.5f;
    [Export] public PackedScene InstancePrefab;
    [Export] public bool ShouldInheritCollisions = false;
    [Export] public bool ShouldTriggerOnInit = false;
    [Export] public string SegmentName = "";
    [Export(PropertyHint.MultilineText)] public string Description;

    public abstract void OnInitialize(RigidBody2D instanceBody, SceneTree tree);
    public abstract void OnPhysicsProcess(double delta, RigidBody2D instanceBody);
    public abstract void OnTriggerEntered(Node otherBody, RigidBody2D instanceBody);
    public virtual string GetDescription()
    {
        string description = "";
        description += SegmentName.ToUpper();
        description += $"\n Description: {Description}";
        description += $"\n Cost: {Cost}";
        description += $"\n Delay: {Delay}";
        return description;
    }

    public virtual void OnDraw(RigidBody2D instanceBody)
    {
        // ..
    }

    public virtual float GetRotationOffset(int childIndex, int childCount) => 0f;
}