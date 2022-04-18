using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the behaviour of a tile.
/// </summary>
/// <remarks>Is expected to be stateless.</remarks>
public abstract class TileBehavior
{
    public abstract void Move(Tile tile, Vector2Int location);

    public abstract bool MatchesWith(Tile tile, IMatchable matchable);
}
