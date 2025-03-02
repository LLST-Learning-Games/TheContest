using Godot;
using System;

public partial class Projectile : RigidBody2D
{
	[Export] private string _trajectoryId = "TrajectoryStraight";
	[Export] private int _damage = 25;
	[Export] private AnimatedSprite2D _sprite;
	private BaseProjectileTrajectory _trajectory;

	public override void _Ready()
	{
		var library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");
		var trajectoryData = library.GetTrajectoryScene(_trajectoryId);
		_trajectory = trajectoryData.Instantiate<BaseProjectileTrajectory>();
		AddChild(_trajectory);
		ContactMonitor = true;
		MaxContactsReported = 1;
		BodyEntered += OnBodyEntered;
	}

	public int GetDamage() => _damage;

	public void SetDirection(Vector2 dir)
	{
		_trajectory.SetTarget(dir);
	}

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
