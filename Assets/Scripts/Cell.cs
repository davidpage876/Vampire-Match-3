using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A fixed space on the grid that can contain zero or one (and only one) tile.
/// </summary>
public class Cell
{
    private Tile m_tile;
    private Vector2Int m_location;

    /// <summary>
    /// Get/set the tile that occupies this cell.
    /// If null the cell is an empty space.
    /// </summary>
    public Tile tile
    {
        get { return m_tile; }
        set 
        { 
            m_tile = value;
            m_tile?.OnAttachedToCell(this);
        }
    }

    /// <summary>
    /// Cell location on the grid by column and row. (0,0) is top-left.
    /// </summary>
    public Vector2Int location
    {
        get { return m_location; }
    }

    /// <summary>
    /// Construct a cell at the given grid location.
    /// </summary>
    public Cell(Vector2Int location)
    {
        m_location = location;
    }

    public override string ToString()
    {
        string t;
        if (m_tile != null)
        {
            t = (m_tile.tileType != null) ? m_tile.formattedIdColored : "?";
        }
        else
        {
            t = "-";
        }
        return string.Format("[ {0} ]{1}", t, m_location);
    }
}
