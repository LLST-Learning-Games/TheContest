using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class Room : Node2D
{
    [Export] private Array<PassageData> _passages = new Array<PassageData>();
    [Export] private TileMapLayer _tileMap;

    public Array<PassageData> Passages => _passages;
    
    public void ClosePassage(PassageDirection directionToClose)
    {
        var passage = _passages.First(x => x.PassageDirection == directionToClose);

        foreach (var directionTile in passage.TileLocations)
        {
            _tileMap.SetCell(
                directionTile,
                passage.TileSourceId,
                passage.TileAtlasCoords);
        }
    }
    
    
}


