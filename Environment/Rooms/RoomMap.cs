using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class RoomMap : NavigationRegion2D
{
    private const float PASSAGE_CLOSE_ODDS = 0.2f;
    
    [Export] private Vector2I _roomMapSize = new Vector2I(3, 3);
    [Export] private Array<PackedScene> _roomPrefabs;
    [Export] private PackedScene _enterRoomPrefab;
    [Export] private PackedScene _exitRoomPrefab;
    
    [Export] private Vector2I _roomSizeInTiles = new Vector2I(24, 13);
    [Export] private int _tileSizeInPixels = 48;

    private Room[,] _rooms;
    private readonly RandomNumberGenerator _rng = new();
    
    public override void _Ready()
    {
        _rooms = new Room[_roomMapSize.X,_roomMapSize.Y];
        
        GenerateRooms();
        ClosePassages();
    }

    private void GenerateRooms()
    {
        Vector2I exitPosition = new Vector2I(
            _rng.RandiRange(1,_roomMapSize.X - 1), 
            _rng.RandiRange(1,_roomMapSize.Y - 1));
        
        GD.Print($"[{GetType().Name}] Selected {exitPosition} for exit room.");
        
        for (int x = 0; x < _roomMapSize.X; x++)
        {
            for (int y = 0; y < _roomMapSize.Y; y++)
            {
                Room newRoom;
                if (x == 0 && y == 0)
                {
                    newRoom = _enterRoomPrefab.Instantiate<Room>();
                }
                else if (x == exitPosition.X && y == exitPosition.Y)
                {
                    newRoom = _exitRoomPrefab.Instantiate<Room>();
                }
                else
                {
                    newRoom = SelectRandomRoom();
                }
                
                GD.Print($"[{GetType().Name}] Placing room of type {newRoom.Name} at position ({x}, {y})");
                
                AddChild(newRoom);
                _rooms[x,y] = newRoom;
                newRoom.Position = GetRoomPosition(x, y);
            }
        }
    }

    private void ClosePassages()
    {
        for (int x = 0; x < _roomMapSize.X; x++)
        {
            for (int y = 0; y < _roomMapSize.Y; y++)
            {
                var room = _rooms[x,y];
                CloseMapEdges(room,x,y);
                
                if (room.Name == "ExitRoom")
                {
                    continue;
                }
                
                CloseRandomPassages(room,x,y);
            }
        }
    }
    
    private void CloseMapEdges(Room newRoom, int x, int y)
    {
        if (x == 0)
        {
            newRoom.ClosePassage(PassageDirection.West);
        }
        else if (x == _roomMapSize.X - 1)
        {
            newRoom.ClosePassage(PassageDirection.East);
        }

        if (y == 0)
        {
            newRoom.ClosePassage(PassageDirection.North);
        }
        else if (y == _roomMapSize.Y - 1)
        {
            newRoom.ClosePassage(PassageDirection.South);
        }
    }

    
    private void CloseRandomPassages(Room newRoom, int x, int y)
    {
        bool shouldClose;
        foreach(var direction in Enum.GetValues(typeof(PassageDirection)).Cast<PassageDirection>())
        {
            shouldClose = _rng.Randf() < PASSAGE_CLOSE_ODDS;
            if (!shouldClose)
            {
                return;
            }
            
            GD.Print($"[{GetType().Name}] Closing passage {direction.ToString()} in room at ({x},{y})");
            newRoom.ClosePassage(direction);
            
            CloseAdjoiningRoom(direction, x, y);
        }
    }

    private void CloseAdjoiningRoom(PassageDirection direction, int x, int y)
    {
        switch (direction)
        {
            case PassageDirection.West:
            {
                if (x == 0)
                {
                    break;
                }
                var room = _rooms[x - 1,y];
                room.ClosePassage(PassageDirection.East);
                break;
            }
            case PassageDirection.East:
            {
                if (x == _roomMapSize.X - 1)
                {
                    break;
                }
                var room = _rooms[x + 1,y];
                room.ClosePassage(PassageDirection.West);
                break;
            }
            case PassageDirection.North:
            {
                if (y == 0)
                {
                    break;
                }
                var room = _rooms[x,y - 1];
                room.ClosePassage(PassageDirection.South);
                break;
            }
            case PassageDirection.South:
            {
                if (y == _roomMapSize.Y - 1)
                {
                    break;
                }
                var room = _rooms[x,y + 1];
                room.ClosePassage(PassageDirection.North);
                break;
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
