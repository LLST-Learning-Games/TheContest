using Godot;

namespace TheContest.Projectiles.CollisionSegment;

public partial class PrefabSpawnerCollisionSegment : ProjectileSegmentData
{
    [Export] private PackedScene _prefabToSpawn;
    public override void OnInitialize(RigidBody2D instanceBody, SceneTree tree)
    {
        var spawnedScene = _prefabToSpawn.Instantiate<Node2D>();
        spawnedScene.GlobalPosition = instanceBody.GlobalPosition;
        AddChildToTreeDeferred(spawnedScene, tree);
    }

    public override void OnPhysicsProcess(double delta, RigidBody2D instanceBody)
    {
        // ..
    }

    public override void OnTriggerEntered(Node otherBody, RigidBody2D instanceBody)
    {

    }
    
    public async void AddChildToTreeDeferred(Node2D instance, SceneTree tree)
    {
        await ToSignal(tree, "process_frame");
        tree.CurrentScene.AddChild(instance);
    }
    
    public override string GetDescription()
    {
        return base.GetDescription() + $"\n Damage: 50";
    }
}