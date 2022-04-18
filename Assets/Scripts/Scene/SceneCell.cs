using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene representation of a cell on the game grid.
/// </summary>
public class SceneCell : MonoBehaviour, System.IComparable
{
    private SceneGrid m_sceneGrid;

    [SerializeField]
    private Vector2Int m_location;

    [Header("Debug read-only")]
    [SerializeField]
    private ScenePotionTile m_potionTile;

    /// <summary>
    /// Returns the location of the cell in the scene grid.
    /// Location is column and row, with (0,0) at top-left.
    /// </summary>
    public Vector2Int location { get { return m_location; } }

    /// <summary>
    /// The potion tile that occupies the scene cell.
    /// Null if there is none.
    /// </summary>
    public ScenePotionTile potionTile
    {
        get
        {
            if (m_potionTile == null)
                m_potionTile = GetComponentInChildren<ScenePotionTile>();
            return m_potionTile;
        }
        set
        {
            m_potionTile = value;

            // Attach potion tile to us.
            if (m_potionTile != null)
            {
                m_potionTile.transform.localPosition = Vector3.zero;
                m_potionTile.transform.SetParent(transform, false);
            }
        }
    }

    /// <summary>
    /// Returns the scene grid this cell is contained in.
    /// </summary>
    public SceneGrid sceneGrid
    {
        get
        {
            if (m_sceneGrid == null)
                m_sceneGrid = GetComponentInParent<SceneGrid>();
            Debug.Assert(m_sceneGrid != null);

            return m_sceneGrid;
        }
    }

    /// <summary>
    /// Returns the position of the cell in world space.
    /// </summary>
    public Vector3 worldPosition
    {
        get
        {
            return transform.position;
        }
    }

    private void Awake()
    {
        // Pre-fill component references.
        _ = sceneGrid;
        _ = potionTile;
    }

    // Used to sort cells to have the same order as Cells in Grid.
    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;

        SceneCell other = obj as SceneCell;
        if (other != null)
        {
            int width = sceneGrid.size.x;
            int i = m_location.y * width + m_location.x;
            int j = other.m_location.y * width + other.m_location.x;

            return i.CompareTo(j);
        }
        else
            throw new System.ArgumentException("Object is not a SceneCell");
    }
}
