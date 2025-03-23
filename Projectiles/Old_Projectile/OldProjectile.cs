using Godot;

public partial class OldProjectile : RigidBody2D
{
	[Export] private AnimatedSprite2D _sprite;
	[Export] private CollisionObject2D _collisionObject2D;
	private string _trajectoryId = "TrajectoryStraight";
	private string _collisionId = "CollisionSimpleDamage";
	private BaseProjectileTrajectory _trajectory;
	private BaseProjectileCollision _collision;
	private ProjectileLibrary _library;
	private bool _hasCollided;

	public void Initialize(ProjectileLibrary library, string trajectoryId, string collisionId)
	{
		_library = library;
		_trajectoryId = trajectoryId;
		_collisionId = collisionId;
	}

	public void SetAsEnemyProjectile()
	{
		_collisionObject2D.SetCollisionLayer(0b1000);
		_collisionObject2D.SetCollisionMask(0b10001);	// player and environment
		_collision.SetAsEnemyCollision();
	}

	public void Fire(Vector2 target)
	{
		if (_library is null)
		{
			GD.PrintErr($"[{GetType().Name}] No library found. Projectile is not initialized.");
			return;
		}
		
		InitializeTrajectory();
		InitializeCollision();
		ContactMonitor = true;
		MaxContactsReported = 1;
		BodyEntered += OnBodyEntered;
		_trajectory.SetTarget(target);
	}

	private void InitializeCollision()
	{
		_hasCollided = false;
		var collisionData = _library.GetCollisionScene(_collisionId);
		_collision = collisionData.Instantiate<BaseProjectileCollision>();
		AddChild(_collision);
	}

	private void InitializeTrajectory()
	{
		var trajectoryData = _library.GetTrajectoryScene(_trajectoryId);
		_trajectory = trajectoryData.Instantiate<BaseProjectileTrajectory>();
		_sprite.SetSpriteFrames(_trajectory.GetSpriteFrames());
		_sprite.SetModulate(_trajectory.GetSpriteColor());
		AddChild(_trajectory);
	}

	public float GetDelay() => _trajectory.GetDelay();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_hasCollided)
		{
			return;
		}
		
		_trajectory.UpdatePosition(this, delta);
	}
	
	private void OnBodyEntered(Node body)
	{
		if(!_hasCollided)
		{
			_collision.OnCollide(body, this);
			HandleCollisionVisuals();
		}
		_hasCollided = true;
	}

	public void HandleCollisionVisuals()
	{
		_sprite.Scale = _collision.GetScale();
		_sprite.SetSpriteFrames(_collision.GetSpriteFrames());
		_sprite.Play("collide");
		_sprite.AnimationFinished += QueueFree;
	}
}
