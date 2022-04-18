using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply replaces dropped tiles with nothing.
/// </summary>
public class FillStrategyEmpty : FillStrategy
{
    public override void Fill(ref Grid grid, in Vector2Int[] spaces, out DroppedTile[] newTiles)
    {
        newTiles = new DroppedTile[0];
    }
}
