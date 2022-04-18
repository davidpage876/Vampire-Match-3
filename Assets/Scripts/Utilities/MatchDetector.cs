using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Utilities for detecting matches made.
/// 
/// A match is made when consecutive matching tiles (i.e. of the same color)
/// appear together in a row, column or intersecting shape.
/// </summary>
public static class MatchDetector
{
    /// <summary>
    /// Checks a tile grid for matches, then returns a list of all matches found (if any).
    /// </summary>
    /// <remarks>
    /// The strategy for detecting matches is as follows:
    /// 1. We search each row for consecutive tiles that match with each other horizontally.
    /// 2. We search each column for consecutive tiles that match with each other vertically.
    /// 3. We merge row matches with column matches found that intersect and match with each other.
    /// 4. We return the matches found.
    /// </remarks>
    /// <param name="grid">The tile grid to search for matches on.</param>
    /// <param name="minMatchLength">Minimum number of matching consecutive tiles needed to make a match.
    /// Is typically 3 in a "match 3" game.</param>
    /// <returns>List of matches found. The list is empty if none were found.</returns>
    public static Match[] Find(Grid grid, int minMatchLength = 3)
    {
        List<Match> matchesFound = new List<Match>();
        Vector2Int gridSize = grid.size;
        int length = 0;
        List<Tile> tiles = new List<Tile>();
        Tile a, b;
        bool isLast = false;

        // Locate horizontal matches.
        for (int y = 0; y < gridSize.y; y++)
        {
            length = 0;
            tiles.Clear();

            // Search for matches from left to right.
            for (int x = 0; x < gridSize.x; x++)
            {
                isLast = (x == gridSize.x - 1);
                a = grid.GetCellAt(new Vector2Int(x, y)).tile;

                if (a != null && a.tileType.matchable)
                {
                    b = isLast ? null : grid.GetCellAt(new Vector2Int(x + 1, y)).tile;

                    if (b != null && b.tileType.matchable && ((IMatchable)a).MatchesWith(b))
                    {
                        // We found a matching tile.
                        tiles.Add(a);
                        length++;
                    } 
                    else if (length > 0)
                    {
                        // We found a non-matching tile.
                        if (length >= minMatchLength - 1)
                        {
                            tiles.Add(a);
                            length++;

                            // Make a match.
                            matchesFound.Add(new Match(tiles.ToArray(), length, 0));
                        }

                        length = 0;
                        tiles.Clear();
                    }
                }
            }

        }

        // Locate vertical matches.
        for (int x = 0; x < gridSize.x; x++)
        {
            length = 0;
            tiles.Clear();

            // Search for matches from top to bottom.
            for (int y = 0; y < gridSize.y; y++)
            {
                isLast = (y == gridSize.y - 1);
                a = grid.GetCellAt(new Vector2Int(x, y)).tile;

                if (a != null && a.tileType.matchable)
                {
                    b = isLast ? null : grid.GetCellAt(new Vector2Int(x, y + 1)).tile;

                    if (b != null && b.tileType.matchable && ((IMatchable)a).MatchesWith(b))
                    {
                        // We found a matching tile.
                        tiles.Add(a);
                        length++;
                    }
                    else if (length > 0)
                    {
                        // We found a non-matching tile.
                        if (length >= minMatchLength - 1)
                        {
                            tiles.Add(a);
                            length++;

                            // Make a match.
                            matchesFound.Add(new Match(tiles.ToArray(), 0, length));
                        }

                        length = 0;
                        tiles.Clear();
                    }
                }
            }

        }

        // Merge intersecting matches.
        matchesFound = MergeIntersectingMatches(matchesFound);

        // Return matches found.
        return matchesFound.ToArray();
    }

    /// <summary>
    /// Returns a new list of matches with intersecting matches merged together.
    /// </summary>
    private static List<Match> MergeIntersectingMatches(List<Match> matches)
    {
        List<Match> result = new List<Match>();

        // Create a list of match indices to check.
        List<int> toCheck = new List<int>();
        for (int i = 0; i < matches.Count; i++)
        {
            toCheck.Add(i);
        }

        // Check matches with each other match remaining.
        int next;
        Match m, n;
        Tile[] tiles;
        int xLength, yLength;
        Vector2Int intersectPoint;
        bool found;
        while (toCheck.Count > 0)
        {
            // Get next match index to check.
            next = toCheck[toCheck.Count - 1];
            toCheck.RemoveAt(toCheck.Count - 1);

            // Search for an intersection between match pairs.
            m = matches[next];
            found = false;
            for (int i = 0; i < toCheck.Count; i++)
            {
                n = matches[toCheck[i]];

                if (CheckIntersects(m, n, out intersectPoint))
                {
                    // Merge intersecting matches and add to result list.
                    tiles = Merge(m.tiles, n.tiles, intersectPoint);
                    xLength = Mathf.Max(m.rowLength, n.rowLength);
                    yLength = Mathf.Max(m.columnLength, n.columnLength);
                    result.Add(new Match(tiles, xLength, yLength, intersectPoint));

                    toCheck.RemoveAt(i);
                    found = true;
                    break;
                }
            }

            // No intersection found.
            if (!found)
            {
                // Add non-intersecting match to result list.
                result.Add(m);
            }
        }
        return result;
    }

    /// <summary>
    /// Returns true if the two matches are intersecting (i.e. share a common intersection point).
    /// 
    /// If the two matches intersect intersectPoint is filled with that location. Otherwise it is a zero vector.
    /// </summary>
    private static bool CheckIntersects(Match m, Match n, out Vector2Int intersectPoint)
    {
        foreach (var a in m.tiles)
        {
            foreach (var b in n.tiles)
            {
                if (a.cell.location == b.cell.location)
                {
                    intersectPoint = a.cell.location;
                    return true;
                }
            }
        }
        intersectPoint = Vector2Int.zero;
        return false;
    }

    /// <summary>
    /// Merges tiles from two matches into a list of tiles, 
    /// removing one of the overlapping tiles at the given intersect point.
    /// </summary>
    private static Tile[] Merge(Tile[] a, Tile[] b, Vector2Int intersectPoint)
    {
        List<Tile> merged = a.Concat(b).ToList();
        for (int i = 0; i < merged.Count; i++)
        {
            if (merged[i].cell.location == intersectPoint)
            {
                merged.RemoveAt(i);
                break;
            }
        }
        return merged.ToArray();
    }
}
