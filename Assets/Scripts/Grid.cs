using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fixed grid of cells.
/// </summary>
public class Grid
{
    private Game m_game;
    private Vector2Int m_size;
    private Cell[] m_cells;

    /// <summary>
    /// Size of the grid in columns and rows.
    /// </summary>
    public Vector2Int size { get { return m_size; } }

    /// <summary>
    /// Returns the list of all cells in the grid in a flat list,
    /// ordered from left to right, top to bottom.
    /// </summary>
    public Cell[] cells { get { return m_cells; } }

    /// <summary>
    /// Gets the cell at the given grid location.
    /// Locations start at top-left (0,0).
    /// Returns null if the location is outside of the grid.
    /// </summary>
    public Cell GetCellAt(Vector2Int location)
    {
        if (location.x >= 0 && location.x < m_size.x &&
            location.y >= 0 && location.y < m_size.y)
        {
            return cells[location.y * m_size.x + location.x];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Construct a grid with size in columns and rows.
    /// </summary>
    public Grid(Game game, Vector2Int size)
    {
        m_game = game;
        m_size = size;

        // Fill grid with empty cells.
        m_cells = new Cell[m_size.x * m_size.y];
        for (int y = 0; y < m_size.y; y++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                m_cells[y * m_size.x + x] = new Cell(new Vector2Int(x, y));
            }
        }
    }

    /// <summary>
    /// Construct a new grid by making a deep copy of an existing grid.
    /// </summary>
    /// <remarks>Game is shared between this grid and the original.</remarks>
    public Grid(Grid grid)
        : this(grid, grid.m_game)
    {}

    /// <summary>
    /// Construct a new grid by making a deep copy of an existing grid,
    /// specifying the game to use.
    /// </summary>
    public Grid(Grid grid, Game game)
        : this(game, grid.m_size)
    {
        // Copy cell contents.
        Cell a, b;
        for (int i = 0; i < m_cells.Length; i++)
        {
            a = m_cells[i];
            b = grid.m_cells[i];

            if (b.tile != null)
            {
                a.tile = new Tile(game, b.tile.tileType);
                // TODO: Consider an object pool for better efficiency.
            }
            else
            {
                a.tile = null;
            }
        }
    }

    public override string ToString()
    {
        string s = string.Format("Grid ({0}x{1}) {{{2}", m_size.x, m_size.y, System.Environment.NewLine);
        Cell cell;
        bool isLast;

        for (int y = 0; y < m_size.y; y++)
        {
            s += "   ";
            for (int x = 0; x < m_size.x; x++)
            {
                cell = m_cells[y * m_size.x + x];

                s += cell;
                isLast = (x == m_size.x - 1 && y == m_size.y - 1);
                if (!isLast)
                {
                    s += ", ";
                }
            }
            s += System.Environment.NewLine;
        }

        s += "}";
        return s;
    }
}
