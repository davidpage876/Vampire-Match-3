using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a type of scene potion tile.
/// </summary>
[CreateAssetMenu(fileName = "ScenePotionType", menuName = "ScriptableObjects/ScenePotionType", order = 1)]
public class ScenePotionType : ScriptableObject
{
    [SerializeField]
    private Tile.TileType m_tileType;

#if UNITY_EDITOR
    public Texture2D m_editorIcon;
#endif

    [SerializeField]
    private Mesh m_bottleMesh;

    [SerializeField]
    private Mesh m_liquidMesh;

    [SerializeField]
    private Material m_liquidMaterial;

    /// <summary>
    /// Tile type description.
    /// </summary>
    public Tile.TileType tileType { get { return m_tileType; } }

    /// <summary>
    /// Mesh to use for the potion bottle.
    /// </summary>
    public Mesh bottleMesh { get { return m_bottleMesh; } }

    /// <summary>
    /// Mesh to use for the potion liquid.
    /// </summary>
    public Mesh liquidMesh { get { return m_liquidMesh; } }

    /// <summary>
    /// Material to use for the potion liquid.
    /// </summary>
    public Material liquidMaterial { get { return m_liquidMaterial; } }

    private void Reset()
    {
        // Set default on reset in editor.
        m_tileType = new Tile.TileType('\0');
    }
}
