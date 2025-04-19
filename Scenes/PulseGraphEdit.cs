using Godot;
using System;

public partial class PulseGraphEdit : GraphEdit
{
    [Export] private PackedScene _pulseGraphNodePrefab;
    [Export] private Label _descriptionLabel;

    public override void _Ready()
    {
        ConnectionRequest += OnConnectionRequest;
    }

    private void OnConnectionRequest(StringName fromNode, long fromSlot, StringName toNode, long toSlot)
    {
        ConnectNode(fromNode, (int)fromSlot, toNode, (int)toSlot);
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var draggable = data.AsGodotObject() as Draggable;
        if (draggable != null)
        {
            var instance = _pulseGraphNodePrefab.Instantiate<PulseGraphNode>();
            AddChild(instance);
            instance.Initialize(draggable.Data, _descriptionLabel);
            instance.PositionOffset = new Vector2(atPosition.X - draggable.Size.X * 3f/2f,
                atPosition.Y - draggable.Size.Y * 3f/2f);
        }
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.AsGodotObject() is Draggable;
    }
    
    
}
