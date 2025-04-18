using Godot;

namespace TheContest.Projectiles;

public partial class SplitterSegment : ProjectileSegmentData
{
    [Export] private float _spread = 15f;
    
    public override void OnInitialize(RigidBody2D instanceBody, SceneTree tree)
    {

    }

    public override void OnPhysicsProcess(double delta, RigidBody2D instanceBody)
    {
        
    }

    public override void OnTriggerEntered(Node otherBody, RigidBody2D instanceBody)
    {
        
    }

    public override float GetRotationOffset(int childIndex, int childCount)
    {
        float totalSpread = (childCount - 1) * _spread;
        float startAngle = -totalSpread / 2f;
        
        float angle = startAngle + childIndex * _spread;
        return Mathf.DegToRad(angle);
    }

    public override string GetDescription()
    {
        string description = base.GetDescription();
        description += "\nFragments: " + AllowedChildCount;
        description += "\nSpread: " + _spread;
        return description;
    }
}