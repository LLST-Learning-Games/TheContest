using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export] private HealthComponent _healthComponent;

	public override void _Ready()
	{
		ContactMonitor = true;
		MaxContactsReported = 1;
		_healthComponent.OnDeath += OnDeath;
	}
	
	private void OnDeath()
	{
		QueueFree();
	}
}
