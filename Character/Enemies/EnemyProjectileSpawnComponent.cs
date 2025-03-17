using Godot;
using System;

public partial class EnemyProjectileSpawnComponent : Node2D
{
	[Export] private string _currentTrajectoryId = "TrajectoryStraight";
	[Export] private string _currentCollisionId = "CollisionSimpleDamage";
	[Export] private PackedScene _projectilePrefab;
	[Export] private float _spawnOffset;
	[Export] private Timer _delayTimer;

	private ProjectileLibrary _library;
	private Vector2 _mouseDirection = Vector2.Zero;
	
	private Node2D _target;
	
	public override void _Ready()
	{
		_library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");
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
		Projectile projectileInstance = _projectilePrefab.Instantiate<Projectile>();
		projectileInstance.Initialize(_library, _currentTrajectoryId, _currentCollisionId);
		GetTree().CurrentScene.AddChild(projectileInstance);
		projectileInstance.Position = GlobalPosition + (direction * _spawnOffset);
		projectileInstance.Fire(direction);
		projectileInstance.SetAsEnemyProjectile();
		_delayTimer.SetWaitTime(projectileInstance.GetDelay());
		_delayTimer.Start();
		_mouseDirection = Vector2.Zero;
	}
}
