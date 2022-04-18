using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene representation of a potion tile attached to a cell on the game grid.
/// 
/// Updates the potion type displayed when changed in the editor.
/// </summary>
[SelectionBase]
[RequireComponent(typeof(ScenePotionTileSelector))]
public class ScenePotionTile : MonoBehaviour
{
    private SceneCell m_sceneCell;
    private SceneDropAnimation m_dropAnimation;
    private ScenePotionTileSelector m_potionSelector;

    [Header("Initial State")]
    [SerializeField]
    private ScenePotionType m_potionType;

    /// <summary>
    /// Get the current potion tile type. 
    /// </summary>
    public ScenePotionType potionType
    {
        get { return m_potionType; }
    }

    /// <summary>
    /// Get tile type description.
    /// </summary>
    public Tile.TileType tileType
    {
        get
        {
            return m_potionType?.tileType;
        }
    }

    /// <summary>
    /// Scene cell that holds this tile.
    /// </summary>
    public SceneCell cell
    {
        get
        {
            if (m_sceneCell == null)
                m_sceneCell = GetComponentInParent<SceneCell>();
            return m_sceneCell;
        }
    }

    /// <summary>
    /// Get SceneDropAnimation component.
    /// </summary>
    private SceneDropAnimation dropAnimation
    {
        get
        {
            if (m_dropAnimation == null)
                m_dropAnimation = GetComponent<SceneDropAnimation>();
            return m_dropAnimation; 
        }
    }

    /// <summary>
    /// Get ScenePotionTileSelector component.
    /// </summary>
    private ScenePotionTileSelector potionSelector
    {
        get 
        {
            if (m_potionSelector == null)
                m_potionSelector = GetComponent<ScenePotionTileSelector>();
            return m_potionSelector; 
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Update appearance on editor change.
        if (!Application.IsPlaying(this))
        {
            potionSelector.RequestUpdate();
        }
    }
#endif

    private void Awake()
    {
        // Pre-fill component references.
        _ = dropAnimation;
        _ = cell;
    }

    private void Start()
    {
        // Listen for events.
        SceneGame.instance.RegisterOnSceneTileDropped(OnDrop);
    }

    private void OnDestroy()
    {
        // Stop listening for events.
        if (SceneGame.instance)
            SceneGame.instance.UnregisterOnSceneTileDropped(OnDrop);
    }

    private void OnDrop(ScenePotionTile t, DroppedTile dt)
    {
        if (t == this)
        {
            // Trigger drop animation.
            dropAnimation?.TriggerDrop(dt);
        }
    }

    // Todo: Receive notifications of a tile type change, and update display in response.
}
