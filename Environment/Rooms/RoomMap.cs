using Godot;
using System.Collections.Generic;
using Godot.Collections;

public partial class RoomMap : NavigationRegion2D
{
    [Export] private Vector2I _roomMapSize = new Vector2I(3, 3);
    [Export] private Array<PackedScene> _roomPrefabs;
    
    [Export] private Vector2I _roomSizeInTiles = new Vector2I(24, 13);
    [Export] private int _tileSizeInPixels = 48;

    private List<Room> _rooms;
    private readonly RandomNumberGenerator _rng = new();
    
    public override void _Ready()
    {
        _rooms = new List<Room>(_roomMapSize.X * _roomMapSize.Y);
        for (int x = 0; x < _roomMapSize.X; x++)
        {
            for (int y = 0; y < _roomMapSize.Y; y++)
            {
                var newRoom = SelectRandomRoom();
                AddChild(newRoom);
                _rooms.Add(newRoom);
                newRoom.Position = GetRoomPosition(x, y);
            }
        }    
    }

    public Vector2I GetRoomPosition(int x, int y)
    {
        return new Vector2I(
            _roomSizeInTiles.X * _tileSizeInPixels * x, 
            _roomSizeInTiles.Y * _tileSizeInPixels * y
        );
    }

    private Room SelectRandomRoom()
    {
        int i = _rng.RandiRange(0,_roomPrefabs.Count - 1);
        return _roomPrefabs[i].Instantiate<Room>();
    }
}
