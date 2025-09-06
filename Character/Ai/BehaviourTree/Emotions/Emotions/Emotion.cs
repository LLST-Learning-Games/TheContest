using System;
using Godot;

namespace Behaviours.Emotions;

public partial class Emotion : Node
{
    [Export] public string Id;
    [Export] public int Intensity = 0;
    [Export] public int MaxIntensity = 100;
    [Export] public int CalmRatePerTick = 1;
    [Export] public double CalmTickInSeconds = 1d;
    [Export] public int Threshold = 50;
    [Export] private bool _isVerbose = false;

    public bool IsTriggered { get; private set; }
    public Action OnTriggered;
    public Action OnCalmed;

    private double _timeSinceLastCalm = 0;
    
    public override void _Process(double delta)
    {
        _timeSinceLastCalm += delta;
        if (Intensity != 0 && _timeSinceLastCalm >= CalmTickInSeconds)
        {
            UpdateIntensity(-CalmRatePerTick);
            _timeSinceLastCalm = 0;
        }
    }
    
    public void UpdateIntensity(int delta)
    {
        SetIntensity(Intensity + delta);
    }

    public void SetIntensity(int newIntensity)
    {
        Intensity = Mathf.Clamp(newIntensity, 0, MaxIntensity);
        if (_isVerbose)
        {
            GD.Print($"[{GetType().Name}] [{Id}] Updating intensity! New Intensity: {Intensity}");
        }
        if (Intensity >= Threshold && !IsTriggered)
        {
            IsTriggered = true;
            OnTriggered?.Invoke();
            if (_isVerbose)
            {
                GD.Print($"[{GetType().Name}] [{Id}] Triggered! ");
            }
        }
        else if (Intensity < Threshold && IsTriggered)
        {
            IsTriggered = false;
            OnCalmed?.Invoke();
            if (_isVerbose)
            {
                GD.Print($"[{GetType().Name}] [{Id}] Calmed! ");
            }
        }
    }
}