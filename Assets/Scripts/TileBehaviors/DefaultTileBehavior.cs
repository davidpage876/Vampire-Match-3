using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default behaviour for a tile when no special game modes are active. 
/// </summary>
public class DefaultTileBehavior : TileBehavior
{
    public override void Move(Tile tile, Vector2Int location)
    {
    }

    public override bool MatchesWith(Tile tile, IMatchable matchable)
    {
        // Match is true if we are both tiles and have the same tile type ID.
        Tile a = tile;
        Tile b = matchable as Tile;
        if (a != null && b != null && a.tileType.matchable && b.tileType.matchable)
        {
            int typeA = a.tileType.id;
            int typeB = b.tileType.id;
            if (typeA == typeB)
            {
                return true;
            }
        }
        return false;
    }
}
