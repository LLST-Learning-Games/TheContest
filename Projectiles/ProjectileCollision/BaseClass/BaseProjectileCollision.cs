using Godot;

public abstract partial class BaseProjectileCollision : Node
{
    private string _id;

    public string GetId() => _id;
    public void SetId(string id) => _id = id;
    
    public abstract void OnCollide(Node body, RigidBody2D projectileBody);
}