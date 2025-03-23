using Godot;

namespace TheContest.Projectiles;

public partial class Projectile : RigidBody2D
{
    [Export] private AnimatedSprite2D _sprite;
    [Export] private CollisionObject2D _collisionObject2D;
}