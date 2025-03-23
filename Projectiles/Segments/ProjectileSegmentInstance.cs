using Godot;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentInstance : RigidBody2D
{
    [Export] private AnimatedSprite2D _sprite;
    [Export] private CollisionObject2D _collisionObject2D;
    
    private ProjectileSegmentData _data;

    public void Initialize(ProjectileSegmentData data)
    {
        _data = data;
        _sprite.SetSpriteFrames(data.SpriteFrames);
        _sprite.Modulate = data.Colour;
        Scale = data.Scale;
        ContactMonitor = true;
        MaxContactsReported = 1;
    }
}