using Godot;
using System;

public partial class Projectile : RigidBody2D
{
	[Export] private int _damage = 25;
	[Export] private AnimatedSprite2D _sprite;
	private string _trajectoryId = "TrajectoryStraight";
	private BaseProjectileTrajectory _trajectory;
	private ProjectileLibrary _library;

	public void Initialize(ProjectileLibrary library, string trajectoryId)
	{
		_library = library;
		_trajectoryId = trajectoryId;
	}

	public void Fire(Vector2 target)
	{
		if (_library is null)
		{
			GD.PrintErr($"[{GetType().Name}] No library found. Projectile is not initialized.");
		}
		
		var trajectoryData = _library.GetTrajectoryScene(_trajectoryId);
		_trajectory = trajectoryData.Instantiate<BaseProjectileTrajectory>();
		AddChild(_trajectory);
		ContactMonitor = true;
		MaxContactsReported = 1;
		BodyEntered += OnBodyEntered;
		_trajectory.SetTarget(target);
	}

	public int GetDamage() => _damage;

	public float GetDelay() => _trajectory.GetDelay();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_trajectory.UpdatePosition(this);
	}
	
	private void OnBodyEntered(Node body)
	{
		_sprite.Play("pop");
		_sprite.AnimationFinished += QueueFree;
	}
}
