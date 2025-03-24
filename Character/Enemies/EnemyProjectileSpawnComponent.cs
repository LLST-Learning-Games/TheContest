using Godot;
using System;

public partial class EnemyProjectileSpawnComponent : Node2D
{
	[Export] private string _currentTrajectoryId = "TrajectoryStraight";
	[Export] private string _currentCollisionId = "CollisionSimpleDamage";
	[Export] private PackedScene _projectilePrefab;
	[Export] private float _spawnOffset;
	[Export] private Timer _delayTimer;

	private ProjectileLibrary_Old _library;
	private Vector2 _mouseDirection = Vector2.Zero;
	
	private Node2D _target;
	
	public override void _Ready()
	{
		_library = GetNode<ProjectileLibrary_Old>("/root/Scene/ProjectileLibrary_Old");
	}
	
	public void SetTarget(Node2D target) => _target = target;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (_projectilePrefab == null || !IsInstanceValid(_target))
		{
			return;
		}
		
		if (_delayTimer.TimeLeft > 0)
		{
			return;
		}

		Vector2 direction = _target.Position - GlobalPosition;
		
		direction = direction.Normalized();
		OldProjectile oldProjectileInstance = _projectilePrefab.Instantiate<OldProjectile>();
		oldProjectileInstance.Initialize(_library, _currentTrajectoryId, _currentCollisionId);
		GetTree().CurrentScene.AddChild(oldProjectileInstance);
		oldProjectileInstance.Position = GlobalPosition + (direction * _spawnOffset);
		oldProjectileInstance.Fire(direction);
		oldProjectileInstance.SetAsEnemyProjectile();
		_delayTimer.SetWaitTime(oldProjectileInstance.GetDelay());
		_delayTimer.Start();
		_mouseDirection = Vector2.Zero;
	}
}
