using Godot;
using System;

public partial class HealthComponent : Node
{
	[Export] private int _maxHealth = 100;
	private int _currentHealth;
	
	public Action OnDeath;

	public override void _Ready()
	{
		_currentHealth = _maxHealth;
	}

	public void UpdateHealth(int delta)
	{
		_currentHealth += delta;
		_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
		GD.Print($"Current Health: {_currentHealth}");

		if (_currentHealth == 0)
		{
			OnDeath?.Invoke();
		}
	}
}
