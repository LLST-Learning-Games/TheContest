using System;
using Godot;

namespace Behaviours.Emotions;

public partial class Emotion : Node
{
    [Export] public string Id;
    [Export] public int Intensity = 0;
    [Export] public int MaxIntensity = 100;
    [Export] public int Threshold = 50;
    [Export] private bool _isVerbose = false;
    
    public bool IsActive => Intensity >= Threshold;
    public Action OnActivation;

    public void UpdateIntensity(int delta)
    {
        SetIntensity(Intensity + delta);
    }

    public void SetIntensity(int newIntensity)
    {
        Intensity = Mathf.Clamp(newIntensity, 0, MaxIntensity);
        if (_isVerbose)
        {
            GD.Print($"[{GetType().Name}] [{Id}] New Intensity: {newIntensity}");
        }
        if (Intensity > Threshold)
        {
            OnActivation?.Invoke();
            if (_isVerbose)
            {
                GD.Print($"[{GetType().Name}]  [{Id}] Triggered! ");
            }
        }
    }
}