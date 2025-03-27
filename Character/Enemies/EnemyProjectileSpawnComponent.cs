using Godot;
using System;
using Godot.Collections;
using Systems;
using TheContest.Projectiles;

public partial class EnemyProjectileSpawnComponent : Node2D
{
	[Export] private string _currentTrajectoryId = "Straight";
	[Export] private string _currentCollisionId = "SimpleDamage";
	[Export] private float _spawnOffset = 20f;
	[Export] private Timer _delayTimer;

	private ProjectileLibrary _library => SystemLoader.GetSystem<ProjectileLibrary>();
	private Vector2 _mouseDirection = Vector2.Zero;
	
	private Node2D _target;
	private NeuroPulse _currentPulse;
	
	public override void _Ready()
	{
		if (SystemLoader.IsSystemLoadComplete)
		{
			GenerateNeuroPulse();
		}
		else
		{
			SystemLoader.OnSystemLoadComplete += GenerateNeuroPulse;
		}
	}

	private void GenerateNeuroPulse()
	{
		_currentPulse = new NeuroPulse();
		_currentPulse.SetDelay(1f);
		AddChild(_currentPulse);
		var trajectoryData = _library.GetTrajectoryResource(_currentTrajectoryId);
		ProjectileSegmentDefinition trajectoryDefinition = CreateDefinition(trajectoryData);
		var collisionData = _library.GetCollisionResource(_currentCollisionId);
		ProjectileSegmentDefinition collisionDefinition = CreateDefinition(collisionData);
		trajectoryDefinition.SetChildren(new Array<ProjectileSegmentDefinition>{collisionDefinition});
		_currentPulse.InjectStartingSegment(trajectoryDefinition, true);
	}
	
	private ProjectileSegmentDefinition CreateDefinition(ProjectileSegmentData data)
	{
		var definition = new ProjectileSegmentDefinition();
		definition.SetData(data);
		_currentPulse.AddChild(definition);
		return definition;
	}

	public void SetTarget(Node2D target) => _target = target;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!IsInstanceValid(_target))
		{
			return;
		}
		
		if (_delayTimer.TimeLeft > 0)
		{
			return;
		}

		Vector2 direction = _target.Position - GlobalPosition;
		
		direction = direction.Normalized();
		var spawnPosition = GlobalPosition + direction * _spawnOffset;
		
		_currentPulse.Fire(spawnPosition, direction.Angle());
		_delayTimer.SetWaitTime(_currentPulse.GetDelay());
		_delayTimer.Start();
		_mouseDirection = Vector2.Zero;
	}
}
