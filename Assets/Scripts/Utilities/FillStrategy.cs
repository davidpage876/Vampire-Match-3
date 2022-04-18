using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strategy for replacing the empty space left by dropped tiles.
/// </summary>
public abstract class FillStrategy
{
    /// <summary>
    /// Requests that specified spaces in a given grid be filled.
    /// 
    /// The results are applied to the given grid.
    /// </summary>
    /// <remarks>
    /// Depending on the derived FillStrategy class, spaces are typically filled with newly spawned tiles.
    /// The type and placement of spawned tiles is determined by the derived FillStrategy class.
    /// </remarks>
    /// <param name="grid">Tile grid to fill spaces on. The results are applied to this grid.</param>
    /// <param name="spaces">List of grid locations with spaces that can be filled.</param>
    /// <param name="newTiles">Output list of newly spawned tiles, and where they dropped from. 
    /// Note that they always drop from outside of the grid. 
    /// The distance dropped is determined based on the position of spaces.
    /// TODO: Be more specific.</param>
    public abstract void Fill(ref Grid grid, in Vector2Int[] spaces, out DroppedTile[] newTiles);
}
