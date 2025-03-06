using Godot;

public partial class Projectile : RigidBody2D
{
	[Export] private AnimatedSprite2D _sprite;
	private string _trajectoryId = "TrajectoryStraight";
	private string _collisionId = "CollisionSimpleDamage";
	private BaseProjectileTrajectory _trajectory;
	private BaseProjectileCollision _collision;
	private ProjectileLibrary _library;

	public void Initialize(ProjectileLibrary library, string trajectoryId, string collisionId)
	{
		_library = library;
		_trajectoryId = trajectoryId;
		_collisionId = collisionId;
	}

	public void Fire(Vector2 target)
	{
		if (_library is null)
		{
			GD.PrintErr($"[{GetType().Name}] No library found. Projectile is not initialized.");
			return;
		}
		
		var trajectoryData = _library.GetTrajectoryScene(_trajectoryId);
		_trajectory = trajectoryData.Instantiate<BaseProjectileTrajectory>();
		var collisionData = _library.GetCollisionScene(_collisionId);
		_collision = collisionData.Instantiate<BaseProjectileCollision>();
		
		AddChild(_trajectory);
		ContactMonitor = true;
		MaxContactsReported = 1;
		BodyEntered += OnBodyEntered;
		_trajectory.SetTarget(target);
	}

	public float GetDelay() => _trajectory.GetDelay();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_trajectory.UpdatePosition(this);
	}
	
	private void OnBodyEntered(Node body)
	{
		_collision.OnCollide(body);
		_sprite.Play("pop");
		_sprite.AnimationFinished += QueueFree;
	}
}
