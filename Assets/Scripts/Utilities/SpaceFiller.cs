using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Utilities for dropping tiles above to fill empty spaces, and then replacing the
/// spaces left behind with new tiles.
/// </summary>
public static class SpaceFiller
{
    /// <summary>
    /// Finds any empty spaces in the tile grid, drops tiles above to fill the space,
    /// then replaces the remaining empty spaces with new tiles dropped from outside of the grid, 
    /// chosen based on the fill strategy.
    /// 
    /// The results are applied to the given grid.
    /// </summary>
    /// <remarks>There may still be empty spaces present after this operation if a tile does not support being dropped
    /// or if the fill strategy decides not to replace a space with a new tile.</remarks>
    /// <param name="grid">Tile grid to attempt to fill empty spaces on.</param>
    /// <param name="strategy">Strategy for choosing which new tiles to fill spaces with.</param>
    /// <param name="droppedTiles">List of tiles that were dropped to fill spaces, sorted left-to-right, then from bottom to top.
    /// This also includes any new tiles created from outside of the grid.
    /// Is an empty list if no tiles dropped (i.e. there was no change).</param>
    public static void Fill(ref Grid grid, FillStrategy strategy, out DroppedTile[] droppedTiles)
    {
        Debug.Assert(grid != null);
        Debug.Assert(strategy != null);

        // Allow tiles to settle into place.
        DroppedTile[] existingTiles;
        ApplyGravity(ref grid, out existingTiles);

        // Find spaces to fill.
        Vector2Int[] spaces = LocateSpaces(grid);

        // Fill spaces.
        DroppedTile[] newTiles;
        strategy.Fill(ref grid, spaces, out newTiles);

        // Output results.
        droppedTiles = existingTiles.Concat(newTiles).ToArray();
    }

    /// <summary>
    /// Drops tiles on a grid to fill any space under them, until tiles have settled into place.
    /// 
    /// The results are applied to the given grid.
    /// </summary>
    /// <remarks>Some tiles may not fall to fill available space if they do not support being dropped.</remarks>
    public static void ApplyGravity(ref Grid grid, out DroppedTile[] droppedTiles)
    {
        // Drop tiles to fill spaces in each column.
        List<DroppedTile> d = new List<DroppedTile>();
        Vector2Int a = new Vector2Int();
        Vector2Int b = new Vector2Int();
        Cell ca, cb;
        bool atRest, isEmptySpace;
        for (int x = 0; x < grid.size.x; x++)
        {
            // Drop tiles in the column from bottom to top until they come to a rest.
            atRest = false;
            while (!atRest)
            {
                atRest = true;
                for (int y = grid.size.y - 1; y >= 0; y--)
                {
                    a.x = b.x = x;
                    a.y = b.y = y;
                    isEmptySpace = (grid.GetCellAt(a).tile == null);

                    // If a space is found drop all tiles above one space down to fill it.
                    if (isEmptySpace)
                    {
                        for (int i = y; i >= 0; i--)
                        {
                            a.y = i;
                            b.y = i - 1;

                            ca = grid.GetCellAt(a);
                            cb = grid.GetCellAt(b);

                            // Drop tile above one space if it supports it.
                            if (cb != null && cb.tile != null)
                            {
                                if (cb.tile.tileType.droppable)
                                {
                                    // Record tile above being dropped.
                                    RecordDrop(cb.tile, ca.location, ref d);

                                    // Drop tile.
                                    ca.tile = cb.tile;
                                    cb.tile = null;

                                    // A tile was dropped, so we are not at rest.
                                    atRest = false;
                                }
                                else
                                {
                                    // Stop dropping tiles as the tile above does not support being dropped.
                                    break;
                                }
                            }
                            else
                            {
                                ca.tile = null;
                            }
                        }
                    }
                }
            }
        }

        // Return results.
        droppedTiles = d.ToArray();
    }

    /// <summary>
    /// Records a dropped tile to a list of dropped tiles.
    /// 
    /// We update an existing entry if there was one, and add a new one otherwise.
    /// </summary>
    /// <param name="tile">The tile that dropped.</param>
    /// <param name="location">The location the tile dropped to.</param>
    /// <param name="droppedTiles">The list of dropped tiles to record to.</param>
    private static void RecordDrop(Tile tile, Vector2Int location, ref List<DroppedTile> droppedTiles)
    {
        bool found = false;
        DroppedTile dt;

        // Update existing entry if available. Otherwise, create a new entry.
        for (int i = 0; i < droppedTiles.Count; i++)
        {
            dt = droppedTiles[i];
            if (dt.tile == tile)
            {
                droppedTiles[i] = new DroppedTile(dt.tile, dt.from, location);

                found = true;
                break;
            }
        }
        if (!found)
        {
            droppedTiles.Add(new DroppedTile(tile, tile.cell.location, location));
        }
    }

    /// <summary>
    /// Find the locations of grid cells which do not contain a tile, i.e. spaces.
    /// </summary>
    /// <param name="grid">Grid to search for spaces in.</param>
    /// <returns>List of space locations in the grid, sorted from bottom to top.</returns>
    private static Vector2Int[] LocateSpaces(in Grid grid)
    {
        List<Vector2Int> spaces = new List<Vector2Int>();
        Vector2Int v;
        for (int y = grid.size.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.size.x; x++)
            {
                v = new Vector2Int(x, y);
                if (grid.GetCellAt(v).tile == null) 
                {
                    spaces.Add(v);
                }
            }
        }
        return spaces.ToArray();
    }
}
