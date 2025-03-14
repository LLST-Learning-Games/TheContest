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
		_player = GetNode<CharacterBody2D>("/root/Scene/Character");
		_timer.Timeout += OnTimeoutCheckDistance;
	}

	private void OnTimeoutCheckDistance()
	{
		var distance = _player.GlobalPosition.DistanceTo(GlobalPosition);
		if (distance > _destroyDistance)
		{
			_nodeToDestroy.QueueFree();
		}
	}
}
