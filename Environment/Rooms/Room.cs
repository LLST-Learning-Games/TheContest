using Godot;
using System;

public partial class Room : Node2D
{
    // ==================
    // Maybe we don't want this to live here?
    // This data has been migrated to RoomMap since it must be constant.
    // But later this script will handle enemy spawning etc, so I'll keep it around.
    // ==================
    //
    // [Export] private Vector2I _roomSizeInTiles = new Vector2I(24, 13);
    // [Export] private TileMapLayer _tileMapLayer;
    // public Vector2I GetRoomSizeInTiles()
    // {
    //     return _roomSizeInTiles;
    // }
    //
    // public Vector2 GetRoomSizeInPixels()
    // {
    //     var tileSize = _tileMapLayer.TileSet.TileSize;
    //     return _roomSizeInTiles * tileSize;
    // }
}
