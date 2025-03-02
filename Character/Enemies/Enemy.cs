using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export] private HealthComponent _healthComponent;

	public override void _Ready()
	{
		ContactMonitor = true;
		MaxContactsReported = 1;
		BodyEntered += OnBodyEntered;
		_healthComponent.OnDeath += OnDeath;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Projectile projectile)
		{
			_healthComponent.UpdateHealth(-projectile.GetDamage());
		}
		
	}

	private void OnDeath()
	{
		QueueFree();
	}
}
