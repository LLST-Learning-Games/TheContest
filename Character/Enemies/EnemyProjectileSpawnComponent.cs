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
	
	private Character _target;
	
	public override void _Ready()
	{
		_library = GetNode<ProjectileLibrary>("/root/Scene/ProjectileLibrary");
	}
	
	public void SetTarget(Character target) => _target = target;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (_projectilePrefab == null || _target == null)
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
		projectileInstance.SetAsEnemyProjectile();
		GetTree().CurrentScene.AddChild(projectileInstance);
		projectileInstance.Position = GlobalPosition + (direction * _spawnOffset);
		projectileInstance.Fire(direction);
		_delayTimer.SetWaitTime(projectileInstance.GetDelay());
		_delayTimer.Start();
		_mouseDirection = Vector2.Zero;
	}
}
