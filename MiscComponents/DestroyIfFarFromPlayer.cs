using Godot;
using System;

public partial class DestroyIfFarFromPlayer : Node2D
{
	[Export] private Node _nodeToDestroy;
	[Export] private Timer _timer;
	[Export] private float _destroyDistance;
	private CharacterBody2D _player;
		
	public override void _Ready()
	{
		var playerGroup = GetTree().GetNodesInGroup("Player");
		if (playerGroup.Count == 0)
		{
			return;
		}
		
		_player = playerGroup[0] as CharacterBody2D;
		_timer.Timeout += OnTimeoutCheckDistance;
	}

	private void OnTimeoutCheckDistance()
	{
		if (!IsInstanceValid(_player))
		{
			return;
		}
		var distance = _player.GlobalPosition.DistanceTo(GlobalPosition);
		if (distance > _destroyDistance)
		{
			_nodeToDestroy.QueueFree();
		}
	}
}
