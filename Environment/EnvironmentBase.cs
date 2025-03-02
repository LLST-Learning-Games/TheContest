using Godot;
using System;

public partial class EnvironmentBase : StaticBody2D
{
	[Export] private Polygon2D _visualPolygon;
	[Export] private CollisionPolygon2D _collisionPolygon;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_visualPolygon.Polygon = _collisionPolygon.Polygon;
	}
}
