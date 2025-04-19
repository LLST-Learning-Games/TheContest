using Godot;
using System;
using Systems;
using TheContest.Projectiles;

public partial class PulseGraphEdit : GraphEdit
{
    [Export] private PackedScene _pulseGraphNodePrefab;
    [Export] private Label _descriptionLabel;
    
    private PulseGraphNode _rootPulse;
    
    private ProjectileLibrary Library => _library ??= SystemLoader.GetSystem<ProjectileLibrary>();
    private ProjectileLibrary _library;
    
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

            if (_rootPulse is null)
            {
                _rootPulse = instance;
            }
        }
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.AsGodotObject() is Draggable;
    }
    
    public void OnConfirmSelection()
    {
        ProjectileSegmentDefinition trajectoryDefinition = Library.Factory.TryAddPulse(_rootPulse.Data.Id);
        RecursivelyGeneratePulseTree(trajectoryDefinition, _rootPulse);
        
        var newWeapon = Library.Factory.ExportNeuroPulse();
        Library.SetPlayerPulse(newWeapon);
        
        GetParent().QueueFree();        // clear the selection UI
    }

    private void RecursivelyGeneratePulseTree(ProjectileSegmentDefinition definition, PulseGraphNode graphNode)
    {
        if (!graphNode.)
        {
            return;
        }
        
        foreach (var child in graphNode.Children)
        {
            var newDefinition = Library.Factory.TryAddPulse(child._projectileId, definition);
            RecursivelyGeneratePulseTree(newDefinition, child);
        }
    }
    
    private void GetChildConnections(string nodeName)
    {
        var connections = GetConnectionList();

        foreach (var conn in connections)
        {
            var fromNode = (string)conn["from_node"];
            var toNode = (string)conn["to_node"];

            if (fromNode == nodeName || toNode == nodeName)
            {
                GD.Print($"Connection: {fromNode}[{conn["from_port"]}] → {toNode}[{conn["to_port"]}]");
            }
        }
    }
}
