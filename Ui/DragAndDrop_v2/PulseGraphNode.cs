using Godot;
using System;
using TheContest.Projectiles;

public partial class PulseGraphNode : GraphNode
{
    [Export] private TextureRect _textureRect;

    private ProjectileSegmentData _data;

    public void Initialize(ProjectileSegmentData data)
    {
        _data = data;
        SetTexture(_data.Icon);
        SetTitle(_data.SegmentName);
    }
    public void SetTexture(Texture2D texture) => _textureRect.Texture = texture;
    
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent &&
            mouseButtonEvent.Pressed &&
            mouseButtonEvent.ButtonIndex == MouseButton.Right)
        {
            QueueFree();
        }
    }
    
    
}
