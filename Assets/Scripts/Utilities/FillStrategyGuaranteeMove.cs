using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Replaces dropped tiles with random tiles, guaranteeing that at
/// least one move can be made afterwards.
/// </summary>
public class FillStrategyGuaranteeMove : FillStrategy
{
    public override void Fill(ref Grid grid, in Vector2Int[] spaces, out DroppedTile[] newTiles)
    {
        throw new System.NotImplementedException();
    }
}
