using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using Systems.Currency;
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
        if (SystemLoader.IsSystemLoadComplete)
        {
            LookupProjectileData();
        }
        else
        {
            SystemLoader.OnSystemLoadComplete += LookupProjectileData;
        }
    }
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && !eventKey.Echo &&eventKey.Keycode == Key.X)
            {
                DebugAwardCurrency();
            }
        }
    }

    private void DebugAwardCurrency()
    {
        CurrencySystem currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        currency.UpdateCurrencyByDelta(50);
    }
    
    private void LookupProjectileData()
    {
        var startingSegment = Library.PlayerPulse.StartingSegment;
        _rootPulse = RecursivelyInitializePulseUi(startingSegment);
    }
    
    private PulseGraphNode RecursivelyInitializePulseUi(ProjectileSegmentDefinition currentSegment, int depth = 1)
    {
        var node = CreateNewPulseGraphNode(currentSegment.GetData());

        for (int i = 0; i < currentSegment.Children.Count; i++)
        {
            var child = currentSegment.Children[i];
            var childNode = RecursivelyInitializePulseUi(child, depth + 1);
            childNode.PositionOffset = new Vector2(node.PositionOffset.X + node.Size.X * 2f * depth,
                node.PositionOffset.Y + node.Size.Y * 1.3f * i);
            ConnectNode(node.Name, 0, childNode.Name, 0);
        }

        return node;
    }

    private void OnConnectionRequest(StringName fromNode, long fromSlot, StringName toNode, long toSlot)
    {
        var node = GetPulseNode((string)fromNode);
        var children = GetChildConnections((string)fromNode);
        if(children.Count >= node.Data.AllowedChildCount)
        {
            GD.Print($"[{GetType().Name}] Node has max children already.");
            return;
        }
        
        // todo - check to ensure we don't have multiple parents!
        
        ConnectNode(fromNode, (int)fromSlot, toNode, (int)toSlot);
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var draggable = data.AsGodotObject() as Draggable;
        if (draggable != null)
        {
            UpdateCash(-draggable.Data.Cost);
            
            var instance = CreateNewPulseGraphNode(draggable.Data);
            instance.PositionOffset = new Vector2(atPosition.X - draggable.Size.X * 3f/2f,
                atPosition.Y - draggable.Size.Y * 3f/2f);

            if (!IsInstanceValid(_rootPulse))
            {
                _rootPulse = instance;
            }
        }
    }

    private PulseGraphNode CreateNewPulseGraphNode(ProjectileSegmentData data)
    {
        var instance = _pulseGraphNodePrefab.Instantiate<PulseGraphNode>();
        AddChild(instance);
        instance.Initialize(data, _descriptionLabel);
        return instance;
    }


    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        var draggable = data.AsGodotObject() as Draggable;
        if (draggable is null)
        {
            return false;
        }
        
        return CanAfford(draggable.Data.Cost);
    }
    
    public void OnConfirmSelection()
    {
        ProjectileSegmentDefinition trajectoryDefinition = Library.Factory.TryAddPulse(_rootPulse.Data.Id);
        RecursivelyGeneratePulseTree(trajectoryDefinition, _rootPulse);
        
        var newWeapon = Library.Factory.ExportNeuroPulse();
        Library.SetPlayerPulse(newWeapon);
        
        GetParent().GetParent().QueueFree();        // clear the selection UI
    }

    private void RecursivelyGeneratePulseTree(ProjectileSegmentDefinition definition, PulseGraphNode graphNode)
    {
        var children = GetChildConnections(graphNode.Name);
        if (!children.Any())
        {
            return;
        }
        
        foreach (var child in children)
        {
            var newDefinition = Library.Factory.TryAddPulse(child.Data.Id, definition);
            RecursivelyGeneratePulseTree(newDefinition, child);
        }
    }
    
    private List<PulseGraphNode> GetChildConnections(string nodeName)
    {
        List<PulseGraphNode> children = new List<PulseGraphNode>();
        
        var connections = GetConnectionList();
        foreach (var conn in connections)
        {
            var fromNode = (string)conn["from_node"];
            var toNode = (string)conn["to_node"];

            if (fromNode == nodeName)
            {
                GD.Print($"Connection: {fromNode}[{conn["from_port"]}] → {toNode}[{conn["to_port"]}]");
                children.Add(GetNode<PulseGraphNode>(toNode));
            }
        }

        return children;
    }
    
    private PulseGraphNode GetPulseNode(string nodeName)
    {
        var node = GetNodeOrNull<PulseGraphNode>(nodeName);
        return node;
    }
    
    private void UpdateCash(float amount)
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        currency.UpdateCurrencyByDelta(amount);
    }

    private bool CanAfford(float amount)
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        return currency.Balance >= amount;
    }
}
