using Godot;
using System;

public partial class ProgressBarUi : Node2D
{
    [Export] private Sprite2D _progressBarSprite;
    public float MaxValue { get; set; }

    public override void _Ready()
    {
        _progressBarSprite.Scale = Vector2.One;
        _progressBarSprite.Position = Vector2.Zero;
    }
    public void OnCurrentValueChanged(float currentValue)
    {
        float percent = currentValue / MaxValue;
        _progressBarSprite.Scale = new Vector2(percent, 1f);
        _progressBarSprite.Position = new Vector2((percent - 1f)/ 2f, 0f);
    }
}
