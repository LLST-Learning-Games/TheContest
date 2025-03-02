using Godot;


public partial class PlayerMovementComponent : Node2D
{
    [Export] private CharacterBody2D _characterBody2D;
    [Export] private float _speed;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = new Vector2();
        if(Input.IsActionPressed("up_button"))
        {
            direction.Y -= 1;
        }
        if (Input.IsActionPressed("down_button"))
        {
            direction.Y += 1;
        }
        if (Input.IsActionPressed("left_button"))
        {
            direction.X -= 1;
        }
        if (Input.IsActionPressed("right_button"))
        {
            direction.X += 1;
        }
        direction = direction.Normalized() * _speed;
		
        _characterBody2D.Velocity = direction;
        _characterBody2D.MoveAndSlide();
    }
}