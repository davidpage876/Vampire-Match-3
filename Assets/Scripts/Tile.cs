using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile that can occupy a cell on the grid and
/// supports "match 3" features such as swapping and matching with other tiles.
/// </summary>
public class Tile : IMoveable, IMatchable, IClearable, ISpawnable, IDroppable
{
    private Game m_game;
    private TileBehavior m_behavior;
    private Cell m_cell;
    private TileType m_tileType;

    /// <summary>
    /// Describes a type of tile.
    /// </summary>
    [System.Serializable]
    public class TileType
    {
        [Tooltip("Unique identifier for the tile type.")]
        [SerializeField]
        private char m_id;

        [Tooltip("True if the tile can be moved/swapped by the player.")]
        [SerializeField]
        private bool m_moveable;

        [Tooltip("True if the tile can be matched with other tiles.")]
        [SerializeField]
        private bool m_matchable;

        [Tooltip("True if the tile can be removed from the board, such as when a match is made.")]
        [SerializeField]
        private bool m_clearable;

        [Tooltip("True if the tile can be created to fill empty spaces created by dropped tiles.")]
        [SerializeField]
        private bool m_spawnable;

        [Tooltip("True if the tile can drop when there is a space below it.")]
        [SerializeField]
        private bool m_droppable;
        
        [Tooltip(
            "Editor only: Text color for the tile to use in debug text in the editor." +
            "See https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html#supported-colors")]
        [SerializeField]
        private string m_editorTileColor;

        /// <summary>
        /// Unique identifier for the tile type. 
        /// </summary>
        public char id { get { return m_id; } }

        /// <summary>
        /// True if the tile can be moved/swapped by the player.
        /// </summary>
        public bool moveable { get { return m_moveable; } }

        /// <summary>
        /// True if the tile can be matched with other tiles.
        /// </summary>
        public bool matchable { get { return m_matchable; } }

        /// <summary>
        /// True if the tile can be removed from the board, such as when a match is made.
        /// </summary>
        public bool clearable { get { return m_clearable; } }

        /// <summary>
        /// True if the tile can be created to fill empty spaces created by dropped tiles.
        /// </summary>
        public bool spawnable { get { return m_spawnable; } }

        /// <summary>
        /// True if the tile can drop when there is a space below it.
        /// </summary>
        public bool droppable { get { return m_droppable; } }

        /// <summary>
        /// Editor only: Text color for the tile to use in debug text in the editor.
        /// See https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html#supported-colors.
        /// </summary>
        public string editorTileColor { get { return m_editorTileColor; } }

        /// <summary>
        /// Construct a tile type.
        /// </summary>
        /// <param name="id">Unique identifier for the tile type. </param>
        /// <param name="moveable">True if the tile can be moved/swapped by the player.</param>
        /// <param name="matchable">True if the tile can be matched with other tiles.</param>
        /// <param name="clearable">True if the tile can be removed from the board, such as when a match is made.</param>
        /// <param name="spawnable">True if the tile can be created to fill empty spaces created by dropped tiles.</param>
        /// <param name="droppable">True if the tile can drop when there is a space below it.</param>
        /// <param name="editorTileColor">Editor only: Text color for the tile to use in debug text in the editor.
        /// See https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html#supported-colors.</param>
        public TileType(char id, 
            bool moveable = true, bool matchable = true, bool clearable = true, 
            bool spawnable = true, bool droppable = true, string editorTileColor = "")
        {
            m_id = id;
            m_moveable = moveable;
            m_matchable = matchable;
            m_clearable = clearable;
            m_spawnable = spawnable;
            m_droppable = droppable;
            m_editorTileColor = editorTileColor;
        }

        public override string ToString()
        {
            return m_id.ToString();
        }
    }

    /// <summary>
    /// Returns the grid cell this tile is attached to.
    /// If the tile is not attached to a cell returns null.
    /// </summary>
    public Cell cell { get { return m_cell; } }

    /// <summary>
    /// Returns a description of the type of this tile.
    /// </summary>
    public TileType tileType { get { return m_tileType; } }

    /// <summary>
    /// Construct a tile in the game with a given type.
    /// </summary>
    /// <remarks>Game is used to get the current game mode.</remarks>
    public Tile(Game game, TileType type)
    {
        m_game = game;
        m_tileType = type;

        // Notify us when game mode changes.
        m_game.RegisterOnGameModeChanged(OnGameModeChanged);
    }

    /// <summary>
    /// Called by a cell when this tile is attached to it.
    /// </summary>
    public void OnAttachedToCell(Cell cell)
    {
        m_cell = cell;
    }

    ~Tile()
    {
        // Stop notifying us when game mode changes.
        m_game.UnregisterOnGameModeChanged(OnGameModeChanged);
    }

    void IMoveable.Move(Vector2Int location)
    {
        Debug.Assert(m_behavior != null, "Game must be Start()'d first");
        Debug.Assert(m_tileType.moveable);
        m_behavior.Move(this, location);
    }

    bool IMatchable.MatchesWith(IMatchable matchable)
    {
        Debug.Assert(m_behavior != null, "Game must be Start()'d first");
        if (m_tileType.matchable)
        {
            return m_behavior.MatchesWith(this, matchable);
        }
        else
        {
            return false;
        }
    }

    private void OnGameModeChanged(
        GameMode.ModeType newMode, GameMode newInstance,
        GameMode.ModeType oldMode, GameMode oldInstance)
    {
        // Choose our behavior based on the current game mode.
        m_behavior = newInstance?.tileBehaviour;
    }

    /// <summary>
    /// Returns the tile ID formatted for display in debug logs.
    /// </summary>
    public string formattedId
    {
        get
        {
            return (m_tileType != null) ? m_tileType.ToString() : "?";
        }
    }

    /// <summary>
    /// Returns the tile ID formatted for display in debug logs, colored with rich text.
    /// </summary>
    public string formattedIdColored
    {
        get
        {
            string id = formattedId;
            if (m_tileType != null && m_tileType.editorTileColor != "")
            {
                id = string.Format("<color={0}>{1}</color>", m_tileType.editorTileColor, id);
            }
            return id;
        }
    }

    /// <summary>
    /// Returns the tile cell location formatted for display in debug logs.
    /// </summary>
    public string formattedLocation
    {
        get
        {
            return (m_cell != null) ? m_cell.location.ToString() : "(floating)";
        }
    }

    public override string ToString()
    {
        return string.Format("[ {0} ]{1}", formattedIdColored, formattedLocation);
    }

    // Todo: Update Cell if Tile detaches?
}
