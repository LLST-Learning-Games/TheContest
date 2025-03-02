using Godot;
using System;

public partial class Projectile : RigidBody2D
{
	[Export] private int _damage = 25;
	[Export] private float _speed = 5f;
	[Export] private float _delay;
	[Export] private AnimatedSprite2D _sprite;
	private Vector2 _direction = Vector2.Zero;

	public override void _Ready()
	{
		ContactMonitor = true;
		MaxContactsReported = 1;
		BodyEntered += OnBodyEntered;
	}

	public int GetDamage() => _damage;

	public void SetDirection(Vector2 dir)
	{
		_direction = dir;
	}

	public float GetDelay() => _delay;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_direction == Vector2.Zero)
		{
			return;
		}
		
		ApplyForce(_direction * _speed);
		_direction = Vector2.Zero;
	}
	
	private void OnBodyEntered(Node body)
	{
		_sprite.Play("pop");
		_sprite.AnimationFinished += QueueFree;
	}
}
