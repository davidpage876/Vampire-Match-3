using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene representation of the game grid.
/// </summary>
public class SceneGrid : MonoBehaviour
{
    private SceneCell[] m_childCells = null;
    private List<ScenePotionTile> m_potionTiles = new List<ScenePotionTile>(); 
    private bool m_settled = true;

    [SerializeField]
    private Vector2Int m_size;

    /// <summary>
    /// Size of the grid in columns and rows.
    /// </summary>
    public Vector2Int size { get { return m_size; } }

    /// <summary>
    /// Returns a list of child SceneCells.
    /// </summary>
    public SceneCell[] childCells
    {
        get 
        { 
            if (m_childCells == null)
            {
                m_childCells = GetComponentsInChildren<SceneCell>();

                // Sort for fast lookup.
                System.Array.Sort(m_childCells);
            }
            return m_childCells; 
        }
    }

    /// <summary>
    /// True if all tiles have finished dropping and settled into place.
    /// 
    /// Starts as true.
    /// </summary>
    public bool settled { get { return m_settled; } }

    /// <summary>
    /// Gets the child SceneCell at the given grid location.
    /// Locations start at top-left (0,0).
    /// Returns null if the location is outside of the grid.
    /// </summary>
    public SceneCell GetChildCellAt(Vector2Int location)
    {
        if (location.x >= 0 && location.x < m_size.x &&
            location.y >= 0 && location.y < m_size.y)
        {
            return m_childCells[location.y * m_size.x + location.x];
        }
        else
        {
            return null;
        }
    }

    private void Awake()
    {
        // Pre-fill child component references.
        _ = childCells;
    }

    private void Start()
    {
        // Listen for events.
        SceneGame.instance.RegisterOnTileDropStarted(OnTileStartedDropping);
        SceneGame.instance.RegisterOnTileDropStopped(OnTileFinishedDropping);
        SceneGame.instance.RegisterOnTilesSettled(OnTilesSettled);
    }

    private void OnDestroy()
    {
        // Stop listening for events.
        if (SceneGame.instance)
        {
            SceneGame.instance.UnregisterOnTileDropStarted(OnTileStartedDropping);
            SceneGame.instance.UnregisterOnTileDropStopped(OnTileFinishedDropping);
            SceneGame.instance.UnregisterOnTilesSettled(OnTilesSettled);
        }
    }

    private void OnTileStartedDropping(ScenePotionTile tile, DroppedTile data)
    {
        Debug.Assert(!m_potionTiles.Contains(tile));

        m_settled = false;
        m_potionTiles.Add(tile);
    }

    private void OnTileFinishedDropping(ScenePotionTile tile, DroppedTile data)
    {
        // Move tile to dropped location.
        SceneCell fromCell = GetChildCellAt(data.from);
        SceneCell toCell = GetChildCellAt(data.to);
        fromCell.potionTile = null;
        toCell.potionTile = tile;

        // If there are no tiles in motion we are settled.
        m_potionTiles.Remove(tile);

        if (m_potionTiles.Count == 0)
        {
            m_settled = true;

            // Notify listeners of settled event.
            SceneGame.instance.NotifyTilesSettled(this);
        }
    }

    private void OnTilesSettled(SceneGrid grid)
    {
        Debug.Log("Settled");
    }
}
