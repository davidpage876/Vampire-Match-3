using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScenePotionType))]
public class ScenePotionTypeEditor : Editor
{
    // Show potion icon in editor.
    //
    // See: https://docs.unity3d.com/ScriptReference/Editor.RenderStaticPreview.html
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        ScenePotionType p = (ScenePotionType)target;

        if (p == null || p.m_editorIcon == null)
            return null;

        Texture2D t = new Texture2D(width, height);
        EditorUtility.CopySerialized(p.m_editorIcon, t);
        return t;
    }
}
