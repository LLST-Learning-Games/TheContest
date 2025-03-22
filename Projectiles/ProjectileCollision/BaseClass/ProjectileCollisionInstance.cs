using Godot;

namespace TheContest.Projectiles.ProjectileCollision.BaseClass;

public partial class ProjectileCollisionInstance : RigidBody2D
{
    [Export] public PackedScene _collisionPrefab;
    [Export] private AnimatedSprite2D _sprite;
    private BaseProjectileCollision _collision;
    private bool _hasCollided;

    public void TriggerProjectileCollision()
    {
        _hasCollided = false;
        InitializeCollision();
        HandleCollisionVisuals();
    }
    
    private void InitializeCollision()
    {
        _collision = _collisionPrefab.Instantiate<BaseProjectileCollision>();
        AddChild(_collision);
    }
    
    private void HandleCollisionVisuals()
    {
        _sprite.Scale = _collision.GetScale();
        _sprite.SetSpriteFrames(_collision.GetSpriteFrames());
        _sprite.Play("collide");
        _sprite.AnimationFinished += QueueFree;
    }
    
    private void OnBodyEntered(Node body)
    {
        if(!_hasCollided)
        {
            _collision.OnCollide(body, this);
            HandleCollisionVisuals();
        }
        _hasCollided = true;
    }
    
}