using Godot;
using System;
using Systems;

public partial class HealthComponent : Node
{
	[Export] private int _maxHealth = 100;
	
	// todo - as noted below, let's bust CameraShake out into a subcomponent
	[Export] private bool _shouldShake = false;
	[Export] private float _shakeSize = 5f;
	[Export] private double _shakeDuration = 0.3;
	
	private int _currentHealth;
	
	public int MaxHealth => _maxHealth;
	
	public Action OnDeath;
	public Action<int> OnHealthChanged;
	
	// probably should make this some kind of decoupled component, I don't love this hard reference
	private CameraSystem CameraSystem => _cameraSystem ?? SystemLoader.GetSystem<CameraSystem>();
	private CameraSystem _cameraSystem;

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
			
			HandleShake(delta);
		}

		if (_currentHealth == 0)
		{
			OnDeath?.Invoke();
		}
	}

	private void HandleShake(int delta)
	{
		if(_shouldShake && delta < 0)
		{
			CameraSystem.TriggerCameraShake(_shakeSize * -delta, _shakeDuration);
		}
	}
}
