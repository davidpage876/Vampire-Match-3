using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A tile which supports being matched with another tile.
/// 
/// A row or column of (e.g. 3 or more) consecutive matching tiles forms a match.
/// </summary>
public interface IMatchable
{
    /// <summary>
    /// Returns true if we match with another matchable tile.
    /// </summary>
    /// <remarks>Matches are assumed to be commutative: 
    /// i.e. If A matches with B, then B also matches with A.</remarks>
    public bool MatchesWith(IMatchable matchable);
}
