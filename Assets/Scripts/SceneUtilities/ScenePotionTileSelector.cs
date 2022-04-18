using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility component for selecting a potion tile type.
/// When used with ScenePotionTile supports changing tile types in the editor.
/// </summary>
#if UNITY_EDITOR
[ExecuteAlways]
#endif
[RequireComponent(typeof(ScenePotionTile))]
public class ScenePotionTileSelector : MonoBehaviour
{
    private ScenePotionTile m_potionTile;
    private MeshFilter m_potionBottleFilter;
    private MeshFilter m_potionLiquidFilter;

#if UNITY_EDITOR
    private bool updatePending = false;
#endif

    [Header("References")]
    [SerializeField]
    private MeshRenderer m_potionBottle;
    [SerializeField]
    private MeshRenderer m_potionLiquid;

    /// <summary>
    /// Request the potion display to be updated next frame.
    /// </summary>
    public void RequestUpdate()
    {
        // We update in the next frame to avoid Editor errors.
        updatePending = true;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (Application.IsPlaying(this))
#endif
        {
            // Set initial display state.
            UpdateDisplay();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.IsPlaying(this))
        {
            // Update next frame.
            updatePending = true;
        }
    }

    private void Update()
    {
        if (!Application.IsPlaying(this))
        {
            if (updatePending)
            {
                // Update in editor if changes pending.
                UpdateDisplay();
                updatePending = false;
            }
        }
    }
#endif

    private void UpdateDisplay()
    {
        Debug.Assert(m_potionBottle != null);
        Debug.Assert(m_potionLiquid != null);

        // Get mesh filter references.
        if (m_potionBottleFilter == null)
            m_potionBottleFilter = m_potionBottle.GetComponent<MeshFilter>();
        if (m_potionLiquidFilter == null)
            m_potionLiquidFilter = m_potionLiquid.GetComponent<MeshFilter>();

        // Get reference to potion tile.
        if (m_potionTile == null)
            m_potionTile = GetComponent<ScenePotionTile>();

        // Apply potion type visual properties.
        ScenePotionType potionType = m_potionTile.potionType;
        if (potionType)
        {
            m_potionBottleFilter.mesh = potionType.bottleMesh;
            m_potionLiquidFilter.mesh = potionType.liquidMesh;
            m_potionLiquid.material = potionType.liquidMaterial;
        }
        else
        {
            m_potionBottleFilter.mesh = null;
            m_potionLiquidFilter.mesh = null;
            m_potionLiquid.material = null;
        }
    }
}
