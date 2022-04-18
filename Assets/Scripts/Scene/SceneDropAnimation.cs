using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the animation of a tile being dropped.
/// </summary>
[RequireComponent(typeof(ScenePotionTile))]
public class SceneDropAnimation : MonoBehaviour
{
    private ScenePotionTile m_potionTile;
    private SceneGrid m_sceneGrid;
    private bool m_animating = false;
    private DroppedTile m_data;
    private SceneCell m_fromCell;
    private SceneCell m_toCell;
    private Vector3 m_fromWorld;
    private Vector3 m_toWorld;
    private int m_cellsDropped;
    private float m_timeStarted;

    /// <summary>
    /// Triggers the drop animation and notifies the tile when completed.
    /// </summary>
    /// <param name="data">Information about the drop.</param>
    public void TriggerDrop(DroppedTile data)
    {
        m_animating = true;
        m_data = data;
        m_fromCell = sceneGrid.GetChildCellAt(data.from);
        m_toCell = sceneGrid.GetChildCellAt(data.to);
        m_fromWorld = m_fromCell.worldPosition;
        m_toWorld = m_toCell.worldPosition;
        m_cellsDropped = data.distance;
        m_timeStarted = Time.time;

        SceneGame.instance.NotifyTileDropStarted(potionTile, m_data);
    }

    /// <summary>
    /// Resets the drop animation.
    /// </summary>
   /* public void ResetDrop()
    {
        transform.localPosition = Vector3.zero;
    }*/

    private ScenePotionTile potionTile
    {
        get
        {
            if (m_potionTile == null)
                m_potionTile = GetComponent<ScenePotionTile>();
            return m_potionTile;
        }
    }

    private SceneGrid sceneGrid
    {
        get 
        {
            if (m_sceneGrid == null)
                m_sceneGrid = GetComponentInParent<SceneGrid>();
            return m_sceneGrid; 
        }
    }

    private void Awake()
    {
        // Pre-fill component references.
        _ = potionTile;
        _ = sceneGrid;
    }

    private void Update()
    {
        // Update animation.
        if (m_animating)
        {
            // Linearly interpolate over time.
            float dt = Time.time - m_timeStarted;
            float t = Mathf.Clamp01(dt / m_cellsDropped * SceneGame.instance.tileDropRate);

            Vector3 p = transform.position;
            p.y = Mathf.Lerp(m_fromWorld.y, m_toWorld.y, t);
            transform.position = p;

            // Finish animation.
            if (t >= 1f)
            {
                m_animating = false;
                SceneGame.instance.NotifyTileDropStopped(potionTile, m_data);
            }
        }
    }
}
