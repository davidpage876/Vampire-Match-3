using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages overall state for a single "match 3" game/level.
/// </summary>
public class Game
{
    private event GameModeChanged m_gameModeChanged;
    private GameMode.ModeType m_modeType = GameMode.ModeType.Undefined;
    private GameMode m_mode;
    private GameMode m_defaultMode = new GameMode(GameMode.ModeType.Default);
    private GameMode m_jumpMode = new GameMode(GameMode.ModeType.Jump);    
    private Grid m_grid;
    private bool m_started = false;

    /// <summary>
    /// Notification that the game mode has changed type.
    /// Provides a new game mode instance.
    /// </summary>
    public delegate void GameModeChanged(
        GameMode.ModeType newMode, GameMode newInstance, 
        GameMode.ModeType oldMode, GameMode oldInstance);

    /// <summary>
    /// Current game mode type.
    /// </summary>
    public GameMode.ModeType gameMode
    {
        get 
        {
            Debug.Assert(m_started);
            return m_modeType; 
        }
        set 
        {
            var oldType = m_modeType;
            var oldInstance = m_mode;
            m_modeType = value;
            m_mode = GetModeInstance(m_modeType);

            // Notify listeners.
            m_gameModeChanged?.Invoke(m_modeType, m_mode, oldType, oldInstance);
        }
    }

    /// <summary>
    /// Current game mode instance.
    /// </summary>
    public GameMode gameModeInstance 
    { 
        get
        {
            Debug.Assert(m_started);
            return m_mode; 
        }
    }

    /// <summary>
    /// Register listener to be notified when the game mode changed.
    /// The event is also fired initially (see <see cref="Start"/>).
    /// </summary>
    public void RegisterOnGameModeChanged(GameModeChanged listener)
    {
        m_gameModeChanged += listener;
    }

    /// <summary>
    /// Stop listening to game mode changed notifications.
    /// </summary>
    public void UnregisterOnGameModeChanged(GameModeChanged listener)
    {
        m_gameModeChanged -= listener;
    }

    /// <summary>
    /// Gets the grid upon which the game is played.
    /// </summary>
    public Grid grid { get { return m_grid; } }

    /// <summary>
    /// Start the game. Call once all game elements have been set up.
    /// </summary>
    public void Start(GameMode.ModeType initialGameMode = GameMode.ModeType.Default)
    {
        // Set up initial game mode.
        gameMode = initialGameMode;

        m_started = true;
    }

    /// <summary>
    /// Construct game with specified grid size.
    /// </summary>
    public Game(Vector2Int gridSize)
    {
        // Create grid.
        m_grid = new Grid(this, gridSize);
    }

    private GameMode GetModeInstance(GameMode.ModeType modeType)
    {
        switch (modeType)
        {
            case GameMode.ModeType.Default:
                return m_defaultMode;
            case GameMode.ModeType.Jump:
                return m_jumpMode;
            default:
                return null;
        }
    }
}
