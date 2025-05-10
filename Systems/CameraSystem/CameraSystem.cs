using Godot;
using Systems;

public partial class CameraSystem : BaseSystem
{
    [Export] private Camera2D _camera;

    private Node2D _followNode;
    
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
        
        _camera.GlobalPosition = _followNode.GlobalPosition;
    }

    public override void OnGameplayEnd()
    {
        _followNode = null;
        _camera.PositionSmoothingEnabled = false;
    }
}
