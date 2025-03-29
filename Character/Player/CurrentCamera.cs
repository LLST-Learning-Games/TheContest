using Godot;
using System;

public partial class CurrentCamera : Camera2D
{
    public override void _Ready()
    {
        MakeCurrent();
        base._Ready();
    }
}
