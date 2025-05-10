using System;
using Godot;
using Systems;

public partial class CameraSystem : BaseSystem
{
    [Export] private Camera2D _camera;
    [Export] private double _delayBetweenShakes = 0.2;
    
    private Node2D _followNode;
    private RandomNumberGenerator _rng = new();
    
    private bool _isShaking = false;
    private float _shakeSize = 0f;
    private double _shakeDuration = 0f;
    private double _shakeDelayTime = 0f;
    
    public override void OnGameplayStart()
    {
        GetCameraTarget();
        _camera.GlobalPosition = _followNode.GlobalPosition;
        _camera.ForceUpdateTransform();
        _camera.PositionSmoothingEnabled = true;
    }

    private void GetCameraTarget()
    {
        var targetGroup = GetTree().GetNodesInGroup("Player");
        if (targetGroup.Count == 0)
        {
            return;
        }
        _followNode = targetGroup[0] as Node2D;
    }
    
    public override void Initialize()
    {
        //..
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_followNode is null)
        {
            return;
        }

        var position = _followNode.GlobalPosition;
        position = HandleShake(delta, position);
        
        _camera.GlobalPosition = position;
    }

    private Vector2 HandleShake(double delta, Vector2 position)
    {
        if (_isShaking)
        {
            _shakeDuration -= delta;
            if (_shakeDuration <= 0)
            {
                _isShaking = false;
                _shakeSize = 0;
            }

            _shakeDelayTime -= delta;
            if (_shakeDelayTime <= 0)
            {
                _shakeDelayTime = _delayBetweenShakes;
                position += GetShakeOffset();
            }
        }

        return position;
    }

    public override void OnGameplayEnd()
    {
        _followNode = null;
        _camera.PositionSmoothingEnabled = false;
    }

    public void TriggerCameraShake(float size, double duration)
    {
        _isShaking = true;
        _shakeDuration = duration;
        _shakeSize += size;
        _shakeDelayTime = 0;
    }

    public Vector2 GetShakeOffset()
    {
        float decayedSize = _shakeSize * (float)_shakeDuration;
        return new Vector2(_rng.RandfRange(-decayedSize, decayedSize), _rng.RandfRange(-decayedSize / 2, decayedSize / 2));
    }
}
