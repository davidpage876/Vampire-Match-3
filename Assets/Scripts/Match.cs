using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a match made.
/// 
/// A match is made when consecutive matching tiles (i.e. of the same color)
/// appear together in a row, column or intersecting shape.
/// </summary>
public struct Match
{
    private Tile[] m_tiles;
    private int m_rowLength;
    private int m_columnLength;
    private bool m_hasIntesectPoint;
    private Vector2Int m_intersectPoint;

    /// <summary>
    /// List of tiles in the match.
    /// </summary>
    public Tile[] tiles { get { return m_tiles; } }

    /// <summary>
    /// Horizontal length of match.
    /// Is zero if the match is only vertical.
    /// </summary>
    public int rowLength { get { return m_rowLength; } }

    /// <summary>
    /// Vertical length of the match.
    /// Is zero if the match is only horizontal.
    /// </summary>
    public int columnLength { get { return m_columnLength; } }

    /// <summary>
    /// True if the match has a point where the row and column intersect.
    /// </summary>
    public bool hasIntesectPoint { get { return m_hasIntesectPoint; } }

    /// <summary>
    /// Returns the location where the row and column intersect, if there is one.
    /// Otherwise returns a zero vector.
    /// </summary>
    public Vector2Int intersectPoint { get { return m_intersectPoint; } }

    /// <summary>
    /// Construct a match with a list of tiles and row,column lengths.
    /// </summary>
    public Match(Tile[] tiles, int rowLength, int columnLength)
    {
        m_tiles = tiles;
        m_rowLength = rowLength;
        m_columnLength = columnLength;
        m_hasIntesectPoint = false;
        m_intersectPoint = Vector2Int.zero;
    }

    /// <summary>
    /// Construct a match with a list of tiles and row,column lengths, as well as the
    /// point of intersection between the rows and columns.
    /// </summary>
    public Match(Tile[] tiles, int rowLength, int columnLength, Vector2Int intersectPoint)
        : this(tiles, rowLength, columnLength)
    {
        m_hasIntesectPoint = true;
        m_intersectPoint = intersectPoint;
    }

    public override string ToString()
    {
        string t = "";
        bool first = true;
        foreach (Tile tile in m_tiles)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                t += ", ";
            }
            t += tile;

        }
        string s = "Match: " + m_rowLength + " x " + m_columnLength + ", tiles(" + m_tiles.Length + ") { " + t + " }";
        if (hasIntesectPoint)
        {
            return s + ", intersects at " + m_intersectPoint;
        }
        else
        {
            return s;
        }
    }
}
