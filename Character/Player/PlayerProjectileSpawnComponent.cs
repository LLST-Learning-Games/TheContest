using Godot;
using System;

public partial class PlayerProjectileSpawnComponent : Node2D
{
	[Export] private string _currentTrajectoryId = "TrajectoryStraight";
	[Export] private string _currentCollisionId = "CollisionSimpleDamage";
	[Export] private PackedScene _projectilePrefab;
	[Export] private float _spawnOffset;
	[Export] private Timer _delayTimer;

	private ProjectileLibrary _library;
	private Vector2 _mouseDirection = Vector2.Zero;
	
	public override void _Ready()
	{
		_library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");

	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (_projectilePrefab == null)
		{
			return;
		}

		if (_delayTimer.TimeLeft > 0)
		{
			return;
		}

		Vector2 direction = Vector2.Zero;
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			direction = GetGlobalMousePosition() - GlobalPosition;
		}
		
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
		projectileInstance.Initialize(_library, _currentTrajectoryId, _currentCollisionId);
		GetTree().CurrentScene.AddChild(projectileInstance);
		projectileInstance.Position = GlobalPosition + (direction * _spawnOffset);
		projectileInstance.Fire(direction);
		_delayTimer.SetWaitTime(projectileInstance.GetDelay());
		_delayTimer.Start();
		_mouseDirection = Vector2.Zero;
	}
}
