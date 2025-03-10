using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] private EnemyProjectileSpawnComponent _projectileSpawnComponent;
	
	private Character _target;	// eventually this should not automatically be the player.

	public override void _Ready()
	{
		ContactMonitor = true;
		MaxContactsReported = 1;
		_healthComponent.OnDeath += OnDeath;
		_target = GetNode<Character>("/root/Scene/Character");
		_projectileSpawnComponent.SetTarget(_target);
	}
	
	private void OnDeath()
	{
		QueueFree();
	}
}
