using Godot;

public abstract partial class BaseProjectileCollision : Node
{
    [Export] protected SpriteFrames _spriteFrames;
    [Export] protected Color _spriteColor = new Color(1f, 1f, 1f);
    [Export] protected Vector2 _scale  = Vector2.One;
    
    private string _id;

    public string GetId() => _id;
    public void SetId(string id) => _id = id;
    public Color GetSpriteColor() => _spriteColor;
    public Vector2 GetScale() => _scale;
    public SpriteFrames GetSpriteFrames() => _spriteFrames;
    
    public abstract void OnCollide(Node body, RigidBody2D projectileBody);

    public virtual void SetAsEnemyCollision()
    {
        // ..
    }
}