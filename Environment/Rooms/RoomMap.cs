using Godot;
using System;
using System.Collections.Generic;

public partial class RoomMap : NavigationRegion2D
{
    [Export] private Vector2I _roomMapSize = new Vector2I(3, 3);
    [Export] private PackedScene _roomPrefab;
    
    [Export] private Vector2I _roomSizeInTiles = new Vector2I(24, 13);
    [Export] private int _tileSizeInPixels = 48;

    private List<Room> _rooms;
    
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
        return _roomPrefab.Instantiate<Room>();
    }
}
