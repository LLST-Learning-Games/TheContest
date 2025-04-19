using Godot;
using System;
using TheContest.Projectiles;

public partial class PulseGraphNode : GraphNode
{
    [Export] private TextureRect _textureRect;
    [Export] private Label _descriptionLabel;

    private ProjectileSegmentData _data;

    public void Initialize(ProjectileSegmentData data, Label label)
    {
        _data = data;
        SetTexture(_data.Icon);
        SetTitle(_data.SegmentName);
        SetDescriptionLabel(label);
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }
    public void SetTexture(Texture2D texture) => _textureRect.Texture = texture;
    public void SetDescriptionLabel(Label label) => _descriptionLabel = label;
    
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent &&
            mouseButtonEvent.Pressed &&
            mouseButtonEvent.ButtonIndex == MouseButton.Right)
        {
            MouseEntered -= OnMouseEntered;
            MouseExited -= OnMouseExited;
            QueueFree();
        }
    }
    
    private void OnMouseEntered()
    {
        if (_data is null)
        {
            return;
        }
        
        _descriptionLabel.Text = _data.GetDescription();
    }

    private void OnMouseExited()
    {
        _descriptionLabel.Text = "";  
    }
    
}
