using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] private EnemyProjectileSpawnComponent _projectileSpawnComponent;
	[Export] private NavigationAgent2D _navAgent;
	
	public EnemyProjectileSpawnComponent EnemyProjectileSpawnComponent => _projectileSpawnComponent;
	public NavigationAgent2D NavAgent => _navAgent;
	public Action<Enemy> OnDeath;
	private Character _target;	// eventually this should not automatically be the player.

	public override void _Ready()
	{
		ContactMonitor = true;
		MaxContactsReported = 1;
		_healthComponent.OnDeath += OnDeathHandler;
	}
	
	private void OnDeathHandler()
	{
		OnDeath?.Invoke(this);
		QueueFree();
	}
}
