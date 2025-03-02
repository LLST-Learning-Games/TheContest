using Godot;
using System;

public partial class PlayerProjectileSpawnComponent : Node2D
{
	[Export] private PackedScene _projectilePrefab;
	[Export] private float _spawnOffset;
	[Export] private Timer _delayTimer;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_projectilePrefab == null)
		{
			return;
		}

		if (_delayTimer.TimeLeft > 0)
		{
			return;
		}
		
		Vector2 direction = new Vector2();
		if(Input.IsActionPressed("fire_up"))
		{
			direction.Y -= 1;
		}
		if (Input.IsActionPressed("fire_down"))
		{
			direction.Y += 1;
		}
		if (Input.IsActionPressed("fire_left"))
		{
			direction.X -= 1;
		}
		if (Input.IsActionPressed("fire_right"))
		{
			direction.X += 1;
		}

		if (direction == Vector2.Zero)
		{
			return;
		}
		
		direction = direction.Normalized();
		Projectile projectileInstance = _projectilePrefab.Instantiate<Projectile>();
		GetTree().CurrentScene.AddChild(projectileInstance);
		projectileInstance.Position = GlobalPosition + (direction * _spawnOffset);
		projectileInstance.SetDirection(direction);
		_delayTimer.SetWaitTime(projectileInstance.GetDelay());
		_delayTimer.Start();
	}
}
