using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace TheContest.Projectiles;

public partial class ProjectileSegmentInstance : RigidBody2D
{
    [Export] private AnimatedSprite2D _sprite;
    [Export] private Area2D _triggerArea;
    private List<ProjectileSegmentDefinition> _children;

    private Array<Node2D> _bodiesPresentOnInitialization;
    private ProjectileSegmentData _segmentData;
    private bool _hasCollided = false;

    public void Initialize(ProjectileSegmentData data, Array<ProjectileSegmentDefinition> children)
    {
        _segmentData = data;
        _sprite.SetSpriteFrames(data.SpriteFrames);
        _sprite.Play();
        _sprite.Modulate = data.Colour;
        _children = new(children);
        Scale = data.Scale;
        _triggerArea.BodyEntered += OnTriggerEntered;
        ContactMonitor = true;
        MaxContactsReported = 1;
    }

    public override void _EnterTree()
    {
        CallDeferred(nameof(CheckForInitialCollisionsDeferred));
    }

    private async void CheckForInitialCollisionsDeferred()
    {
        // well, this is clearly not ideal... perhaps this whole structure needs a rethink.
        await ToSignal(GetTree(), "physics_frame");
        await ToSignal(GetTree(), "physics_frame");

        if(_bodiesPresentOnInitialization is null)
        {
            _bodiesPresentOnInitialization = _triggerArea.GetOverlappingBodies();
            // if (_bodiesPresentOnInitialization.Count == 0)
            // {
            //     GD.Print("No body!");
            // }
            // foreach (var body in _bodiesPresentOnInitialization)
            // {
            //     GD.Print("BODY: " + body.GetType().Name);
            // }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _segmentData.OnPhysicsProcess(delta, this);
    }

    public override void _Draw()
    {
        _segmentData.OnDraw(this);
        base._Draw();
    }

    public void OnTriggerEntered(Node body)
    {
        if (!IsInstanceValid(this))
        {
            return;
        }

        if (!_segmentData.ShouldInheritCollisions)
        {
            // what in sweet hot hell am I trying to do here? this "logic" is a mess. review the goal and refactor, plz!
            if (_bodiesPresentOnInitialization is null &&
                (!IsInstanceValid(body) || body is not TileMapLayer) &&     // todo - eventually we will want to bounce instead of blow up
                !_bodiesPresentOnInitialization.Contains(body as Node2D))
            {
                return;
            }
        }
        
        _segmentData.OnTriggerEntered(body, this);
        if(!_hasCollided)
        {
            SpawnChildren(body);
            HandleCollisionVisuals();
            _hasCollided = true;
        }
    }
    
    private void SpawnChildren(Node inheritedCollision = null)
    {
        if (_children == null || _children.Count == 0)
        {
            return;
        }

        //foreach (var child in _children)
        int childCount = _children.Count;
        for (int i = 0; i < childCount; i++)
        {
            float rotationOffset = _segmentData.GetRotationOffset(i, childCount);
            _children[i].Fire(GlobalPosition, Rotation + rotationOffset, inheritedCollision);
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
            _triggerArea.SetCollisionLayer(0b1000);
            _triggerArea.SetCollisionMask(0b10001);	// player and environment
        }
    }
    
    public override void _ExitTree()
    {
        _triggerArea.BodyEntered -= OnTriggerEntered;
        _children.Clear();
    }

}