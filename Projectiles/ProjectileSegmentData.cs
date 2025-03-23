using Godot;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentData : Resource
{
    public string Id;
    public SpriteFrames SpriteFrames;
    public Color Colour;
    public Vector2 Scale;
    public int AllowedChildCount;
    public PackedScene InstancePrefab;
}