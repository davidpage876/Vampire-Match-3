using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a single move (i.e. "match 3" swap). 
/// </summary>
public struct Move
{
    private Vector2Int m_from;
    private Vector2Int m_to;

    /// <summary>
    /// Source cell location in the grid.
    /// </summary>
    public Vector2Int From { get { return m_from; } }

    /// <summary>
    /// Destination cell location in the grid.
    /// </summary>
    public Vector2Int To { get { return m_to; } }

    /// <summary>
    /// Construct a move from one cell location to another in the grid.
    /// </summary>
    public Move(Vector2Int from, Vector2Int to)
    {
        m_from = from;
        m_to = to;
    }
}
