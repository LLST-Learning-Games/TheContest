using Godot;
using System;

public partial class HealthComponent : Node
{
	[Export] private int _maxHealth = 100;
	private int _currentHealth;
	
	public int MaxHealth => _maxHealth;
	
	public Action OnDeath;
	public Action<int> OnHealthChanged;

	public override void _Ready()
	{
		_currentHealth = _maxHealth;
	}

	public void TriggerDeath()
	{
		UpdateHealth(-_currentHealth);
	}

	public void UpdateHealth(int delta)
	{
		var newHealth = _currentHealth + delta;
		newHealth = Mathf.Clamp(newHealth, 0, _maxHealth);
		if (newHealth != _currentHealth)
		{
			_currentHealth = newHealth;
			OnHealthChanged?.Invoke(_currentHealth);
		}

		if (_currentHealth == 0)
		{
			OnDeath?.Invoke();
		}
	}
}
