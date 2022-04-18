using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene representation of a "match 3" game/level.
/// </summary>
public class SceneGame : MonoBehaviour
{
    private static SceneGame m_instance;
    private Game m_game;
    private event TileDropped m_tileDropped;
    private event TileDropped m_tileDropStarted;
    private event TileDropped m_tileDropStopped;
    private event TilesSettled m_tilesSettled; // Todo

    /// <summary>
    /// Delegate for when a scene tile was dropped.
    /// </summary>
    /// <param name="tile">The scene tile that dropped.</param>
    /// <param name="data">Additional data about the tile drop.</param>
    public delegate void TileDropped(ScenePotionTile tile, DroppedTile data);

    /// <summary>
    /// Delegate for when tiles have settled into place after falling.
    /// </summary>
    /// <param name="grid">Grid on which tiles settled.</param>
    public delegate void TilesSettled(SceneGrid grid);

    [Header("References")]
    [SerializeField]
    private SceneGrid m_sceneGrid;

    [Header("Settings")]
    [SerializeField]
    private float m_tileDropRate;

    /// <summary>
    /// Returns a reference to the single scene game object.
    /// </summary>
    /// <remarks>Uses the singleton pattern.</remarks>
    public static SceneGame instance
    {
        get 
        {
            //Debug.Assert(m_instance != null, "Tried to access SceneGame instance before it is initialized");
            return m_instance; 
        }
    }

    /// <summary>
    /// Get reference to the game state.
    /// </summary>
    public Game game { get { return m_game; } }

    /// <summary>
    /// Returns the speed tiles fall in grid cell units per second.
    /// </summary>
    public float tileDropRate { get { return m_tileDropRate; } }

    /// <summary>
    /// Start listening for scene tile drop events.
    /// </summary>
    public void RegisterOnSceneTileDropped(TileDropped listener)
    {
        m_tileDropped += listener;
    }

    /// <summary>
    /// Stop listening for scene tile drop events.
    /// </summary>
    public void UnregisterOnSceneTileDropped(TileDropped listener)
    {
        m_tileDropped -= listener;
    }

    /// <summary>
    /// Start listening for scene tile drop animation started events.
    /// </summary>
    public void RegisterOnTileDropStarted(TileDropped listener)
    {
        m_tileDropStarted += listener;
    }

    /// <summary>
    /// Stop listening for scene tile drop animation started events.
    /// </summary>
    public void UnregisterOnTileDropStarted(TileDropped listener)
    {
        m_tileDropStarted -= listener;
    }

    /// <summary>
    /// Notifies listeners that the scene tile drop animation started.
    /// </summary>
    public void NotifyTileDropStarted(ScenePotionTile tile, DroppedTile data)
    {
        m_tileDropStarted?.Invoke(tile, data);
    }

    /// <summary>
    /// Start listening for scene tile drop animation stopped events.
    /// </summary>
    public void RegisterOnTileDropStopped(TileDropped listener)
    {
        m_tileDropStopped += listener;
    }

    /// <summary>
    /// Stop listening for scene tile drop animation stopped events.
    /// </summary>
    public void UnregisterOnTileDropStopped(TileDropped listener)
    {
        m_tileDropStopped -= listener;
    }

    /// <summary>
    /// Notifies listeners that the scene tile drop animation stopped.
    /// </summary>
    public void NotifyTileDropStopped(ScenePotionTile tile, DroppedTile data)
    {
        m_tileDropStopped?.Invoke(tile, data);
    }

    /// <summary>
    /// Start listening for tiles settled into place events.
    /// </summary>
    public void RegisterOnTilesSettled(TilesSettled listener)
    {
        m_tilesSettled += listener;
    }

    /// <summary>
    /// Stop listening for tiles settled into place events.
    /// </summary>
    public void UnregisterOnTilesSettled(TilesSettled listener)
    {
        m_tilesSettled -= listener;
    }

    /// <summary>
    /// Notifies listeners that the tiles settled into place.
    /// </summary>
    public void NotifyTilesSettled(SceneGrid grid)
    {
        m_tilesSettled?.Invoke(grid);
    }

    private void Awake()
    {
        // Set up singleton.
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("Multiple singleton instances found for SceneGame");
            return;
        }
        else
        {
            m_instance = this;
        }

        Debug.Assert(m_sceneGrid != null, "Game requires a SceneGrid");

        // Create game.
        m_game = new Game(m_sceneGrid.size);
    }

    private void Start()
    {
        // Get initial state from scene items.
        var grid = m_game.grid;
        if (m_sceneGrid != null)
        {
            foreach (var child in m_sceneGrid.childCells)
            {
                var cell = grid.GetCellAt(child.location);
                Debug.Assert(cell != null);

                if (child.potionTile != null)
                {
                    cell.tile = new Tile(m_game, child.potionTile.tileType);
                }
            }
        }

        // Start the game.
        m_game.Start();

        // Fill spaces.
        //FillSpaces();

        //RunTests();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            FillSpaces();
        }
    }

    private void FillSpaces()
    {
        var grid = m_game.grid;
        DroppedTile[] droppedTiles;
        SpaceFiller.Fill(ref grid, new FillStrategyEmpty(), out droppedTiles);

        // Notify listeners of each dropped tile.
        ScenePotionTile t;
        foreach (var dt in droppedTiles)
        {
            t = m_sceneGrid.GetChildCellAt(dt.from).potionTile;
            m_tileDropped?.Invoke(t, dt);
        }
    }

    private void RunTests()
    {
        var grid = m_game.grid;
        Debug.Log(grid);

        Debug.Log("----------------------------------------");
        Match[] results = MatchDetector.Find(grid);
        Debug.Log(results.Length + " match(s) found");
        foreach (Match r in results)
        {
            Debug.Log(r);
        }

        Debug.Log("----------------------------------------");
        DroppedTile[] droppedTiles;
        SpaceFiller.Fill(ref grid, new FillStrategyEmpty(), out droppedTiles);
        if (droppedTiles.Length > 0)
        {
            Debug.Log(droppedTiles.Length + " tiles dropped");
            foreach (DroppedTile dt in droppedTiles)
            {
                Debug.Log(dt);
            }
            Debug.Log(grid);
        }
        else
        {
            Debug.Log("No change after space fill");
            Debug.Log(grid);
        }
    }
}
