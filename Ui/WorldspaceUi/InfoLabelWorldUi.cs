using Godot;
using System;

public partial class InfoLabelWorldUi : Node2D
{
    [Export] private Label _label;
    [Export] private float _speed;
    [Export] private double _lifetime;

    private double timeAlive = 0;
    
    public void SetText(string text) => _label.Text = text;
    public override void _PhysicsProcess(double delta)
    {
        Position = new Vector2(
            Position.X,
            Position.Y - _speed * (float)delta);
        
        timeAlive += delta;
        
        float t = (float)Mathf.Clamp(timeAlive / _lifetime, 0.0, 1.0);

        // Lerp the alpha value
        Color color = Modulate;
        color.A = Mathf.Lerp(1f, 0f, t);
        Modulate = color;
        
        if (timeAlive >= _lifetime)
        {
            QueueFree();
        }
    }
}
