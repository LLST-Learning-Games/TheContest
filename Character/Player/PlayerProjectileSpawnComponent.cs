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
	private Vector2 _direction;

	private bool _isShooting = false;
	
	public override void _Ready()
	{
		_library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");
	}
	
	public void SetCurrentTrajectoryId(string trajectoryId) => _currentTrajectoryId = trajectoryId;
	public void SetCurrentCollisionId(string collisionId) => _currentCollisionId = collisionId;
	
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

		if (_isShooting)
		{
			_direction = GetGlobalMousePosition() - GlobalPosition;
		}
		
		if(Input.IsActionPressed("fire_up"))
		{
			_direction.Y -= 1;
		}
		if (Input.IsActionPressed("fire_down"))
		{
			_direction.Y += 1;
		}
		if (Input.IsActionPressed("fire_left"))
		{
			_direction.X -= 1;
		}
		if (Input.IsActionPressed("fire_right"))
		{
			_direction.X += 1;
		}
		
		if (_direction == Vector2.Zero)
		{
			return;
		}
		
		_direction = _direction.Normalized();
		Projectile projectileInstance = _projectilePrefab.Instantiate<Projectile>();
		projectileInstance.Initialize(_library, _currentTrajectoryId, _currentCollisionId);
		GetTree().CurrentScene.AddChild(projectileInstance);
		projectileInstance.Position = GlobalPosition + (_direction * _spawnOffset);
		projectileInstance.Fire(_direction);
		_delayTimer.SetWaitTime(projectileInstance.GetDelay());
		_delayTimer.Start();
		_direction = Vector2.Zero;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left })
		{
			if (@event.IsPressed())
			{
				_isShooting = true;
			}
			else
			{
				_isShooting = false;
			}
		}
	}
}
