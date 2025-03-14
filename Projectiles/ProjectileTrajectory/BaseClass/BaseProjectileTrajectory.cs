using Godot;

public abstract partial class BaseProjectileTrajectory : Node
{
    private string _id;
    [Export] protected float _delay;
    [Export] protected SpriteFrames _spriteFrames;
    [Export] protected Color _spriteColor = new Color(1f, 1f, 1f);

    public string GetId() => _id;
    public void SetId(string id) => _id = id;
    public float GetDelay() => _delay;
    public Color GetSpriteColor() => _spriteColor;
    public SpriteFrames GetSpriteFrames() => _spriteFrames;
    
    public abstract void SetTarget(Vector2 target);
    public abstract void UpdatePosition(RigidBody2D body, double delta);
}
