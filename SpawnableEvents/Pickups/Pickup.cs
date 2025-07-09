using Godot;
using Systems;
using Systems.Currency;

namespace TheContest.Projectiles.SpawnableEvents;

public partial class Pickup : Node2D
{
    [Export] private Area2D _area2D;
    [Export] private double _pickupTime = 0.2;
    [Export] private ProgressBarUi _pickupProgressBar;
    [Export] private PickupBehaviour _pickupBehaviour;
    [Export] private PackedScene _onPickupUiPrefab;

    private bool _isInArea = false;
    private double _elapsedPickupTime = 0;
    
    public override void _Ready()
    {
        _area2D.BodyEntered += OnBodyEntered;
        _area2D.BodyExited += OnBodyExited;
        _pickupProgressBar.MaxValue = (float) _pickupTime;
        _pickupProgressBar.Visible = false;
        SetElapsedPickupTime(0);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Character)
        {
            _isInArea = true;
        }
    }

    public override void _Process(double delta)
    {
        _pickupProgressBar.Visible = _isInArea;
        
        if (!_isInArea)
        {
            return;
        }
        
        if (Input.IsActionPressed("Interact"))
        {
            SetElapsedPickupTime(_elapsedPickupTime + delta);
            GD.Print($"[{GetType().Name}] Picking up! Elapsed pickup Time: {_elapsedPickupTime}");
            if (_elapsedPickupTime >= _pickupTime)
            {
                PickItUp();
            }
        }
        else
        {
            SetElapsedPickupTime(0);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Character)
        {
            _pickupProgressBar.Visible = false;
            _isInArea = false;
            SetElapsedPickupTime(0);
            GD.Print($"[{GetType().Name}] Cancel pickup. Elapsed pickup Time: {_elapsedPickupTime}");
        }
        
    }

    private void SetElapsedPickupTime(double time)
    {
        _elapsedPickupTime = time;
        _pickupProgressBar.OnCurrentValueChanged((float)_elapsedPickupTime);
    }
    
    

    private void PickItUp()
    {
        _pickupBehaviour.PickItUp();
        if (_onPickupUiPrefab != null)
        {
            var pickupUi = _onPickupUiPrefab.Instantiate<InfoLabelWorldUi>();
            pickupUi.SetText(_pickupBehaviour.SetText());
            GetTree().Root.AddChild(pickupUi);
            pickupUi.GlobalPosition = GlobalPosition;
        }
        QueueFree();
    }

    public override void _ExitTree()
    {
        _area2D.BodyEntered -= OnBodyEntered;
    }
}