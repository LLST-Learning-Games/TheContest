using Godot;
using System;

public partial class ExitZone : Area2D
{
    [Export] private double _escapeTime = 0.5f;
    [Export] private EscapeUi _escapeUi;
    
    private bool _isPlayerPresent = false;
    private bool _hasEscaped = false;
    private double _currentTime = 0;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        _isPlayerPresent = false;
        _hasEscaped = false;
    }

    public override void _Process(double delta)
    {
        if (_isPlayerPresent)
        {
            _currentTime += delta;
            _escapeUi.SetFade((float) (_currentTime / _escapeTime));
        }
        
        if (_currentTime >= _escapeTime)
        {
            if (_hasEscaped)
            {
                return;
            }
            
            _escapeUi.OnEscape();
            _hasEscaped = true;
            return;
        }
        
        if (!_isPlayerPresent && _currentTime != 0)
        {
            _currentTime = 0;
            _escapeUi.SetFade(0);
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Character player)
        {
            return;
        }

        _currentTime = 0;
        _isPlayerPresent = true;
    }
    
    private void OnBodyExited(Node2D body)
    {
        if (body is not Character player)
        {
            return;
        }
        
        _isPlayerPresent = false;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
    }
}
