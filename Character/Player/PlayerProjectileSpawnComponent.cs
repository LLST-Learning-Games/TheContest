using Godot;
using System;
using Systems;
using TheContest.Projectiles;

public partial class PlayerProjectileSpawnComponent : Node2D
{
	[Export] private NeuroPulse _defaultWeapon;
	[Export] private float _spawnOffset;
	[Export] private Timer _delayTimer;

	private ProjectileLibrary _library => SystemLoader.GetSystem<ProjectileLibrary>();
	private NeuroPulse _currentPulse;
	private Vector2 _direction;

	private bool _isShooting = false;

	public override void _Ready()
	{
		_currentPulse = _library.PlayerPulse;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_currentPulse == null)
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
		var spawnPosition = GlobalPosition + _direction * _spawnOffset;
		_currentPulse.Fire(spawnPosition, _direction.Angle());
		
		_delayTimer.SetWaitTime(_currentPulse.GetDelay());
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
