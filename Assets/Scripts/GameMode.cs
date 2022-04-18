using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gameplay mode used to determine tile behaviour.
/// The game can only be in one mode at a time.
/// </summary>
public class GameMode
{
    private ModeType m_mode = ModeType.Default;
    private TileBehavior m_tileBehavior;

    /// <summary>
    /// Mode type and associated behaviour.
    /// </summary>
    [System.Serializable]
    public enum ModeType
    {
        /// <summary>No behaviour assigned</summary>
        Undefined,
        /// <summary>Default "match 3" tile matching behaviour.</summary>
        Default,
        /// <summary>Tiles can "jump" over other tiles when swapping to make a match.</summary>
        Jump
    }

    /// <summary>
    /// Returns the mode type.
    /// </summary>
    public ModeType mode { get { return m_mode; } }

    /// <summary>
    /// Returns the tile behaviour instance to use for the current mode type.
    /// </summary>
    public TileBehavior tileBehaviour
    {
        get
        {
            if (m_tileBehavior == null)
            {
                switch (m_mode)
                {
                    case ModeType.Default:
                        m_tileBehavior = new DefaultTileBehavior(); break;
                    case ModeType.Jump:
                        m_tileBehavior = new JumpTileBehavior(); break;
                }
            }
            return m_tileBehavior;
        }
    }

    /// <summary>
    /// Construct a game mode with the given mode type.
    /// </summary>
    public GameMode(ModeType mode)
    {
        m_mode = mode;
        m_tileBehavior = tileBehaviour;
    }
}
