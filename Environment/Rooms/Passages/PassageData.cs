using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PassageData : Resource
{
    [Export] public PassageDirection PassageDirection;
    [Export] public Array<Vector2I> TileLocations;
    [Export] public int TileSourceId;
    [Export] public Vector2I TileAtlasCoords;
}

public enum PassageDirection
{
    North,
    East,
    South,
    West
}