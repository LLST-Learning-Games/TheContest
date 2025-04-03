using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentInstance : RigidBody2D
{
    [Export] private AnimatedSprite2D _sprite;
    [Export] private CollisionObject2D _collisionObject2D;
    private Array<ProjectileSegmentDefinition> _children;
    
    private ProjectileSegmentData _segmentData;
    private bool _hasCollided = false;

    public void Initialize(ProjectileSegmentData data, Array<ProjectileSegmentDefinition> children)
    {
        _segmentData = data;
        _sprite.SetSpriteFrames(data.SpriteFrames);
        _sprite.Play();
        _sprite.Modulate = data.Colour;
        _children = children;
        Scale = data.Scale;
        BodyEntered += OnCollide;
        ContactMonitor = true;
        MaxContactsReported = 1;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _segmentData.OnPhysicsProcess(delta, this);
    }

    public void OnCollide(Node body)
    {
        _segmentData.OnCollide(body, this);
        if(!_hasCollided)
        {
            SpawnChildren(body);
            HandleCollisionVisuals();
            _hasCollided = true;
        }
    }
    
    private void SpawnChildren(Node inheritedCollision)
    {
        if (_children == null || _children.Count == 0)
        {
            return;
        }

        foreach (var child in _children)
        {
            if (!IsInstanceValid(child))
            {
                return;
            }
            
            child.Fire(GlobalPosition, Rotation, inheritedCollision);
        }
    }
    public void HandleCollisionVisuals()
    {
        if (!_sprite.SpriteFrames.HasAnimation("collide"))
        {
            QueueFree();
            return;
        }
        _sprite.Play("collide");
        _sprite.AnimationFinished += QueueFree;
    }
    
    // todo - this should be improved with a lookup system 
    public void SetCollisionLayers(bool isEnemy)
    {
        if(isEnemy)
        {
            _collisionObject2D.SetCollisionLayer(0b1000);
            _collisionObject2D.SetCollisionMask(0b10001);	// player and environment
        }
    }

}