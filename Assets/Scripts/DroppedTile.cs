using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a tile that dropped to fill an empty space above.
/// </summary>
public class DroppedTile
{
    [SerializeField]
    private Tile m_tile;
    [SerializeField]
    private Vector2Int m_from;
    [SerializeField]
    private Vector2Int m_to;

    /// <summary>
    /// Tile that dropped.
    /// </summary>
    public Tile tile { get { return m_tile; } }

    /// <summary>
    /// Grid cell location the tile dropped from.
    /// For tiles that dropped from outside the board the location may be
    /// outside of the grid.
    /// </summary>
    public Vector2Int from { get { return m_from; } }

    /// <summary>
    /// Grid cell location the tile dropped to.
    /// </summary>
    public Vector2Int to { get { return m_to; } }

    /// <summary>
    /// Returns the distance the tile dropped vertically in grid units.
    /// </summary>
    public int distance
    {
        get
        {
            return m_to.y - m_from.y;
        }
    }

    /// <summary>
    /// Construct a tile that dropped from a cell location to another location.
    /// </summary>
    /// <remarks>
    /// "from" must be on the same column as "to" in the grid, and
    /// "from" must be vertically above "to" (i.e. from.y < to.y).</remarks>
    public DroppedTile(Tile tile, Vector2Int from, Vector2Int to)
    {
        Debug.Assert(tile != null);
        Debug.Assert(from != to);
        Debug.Assert(from.x == to.x);
        Debug.Assert(from.y < to.y);

        m_tile = tile;
        m_from = from;
        m_to = to;
    }

    public override string ToString()
    {
        return string.Format("[ {0} ] dropped from {1} to {2}", m_tile.formattedIdColored, m_from, m_to);
    }
}
