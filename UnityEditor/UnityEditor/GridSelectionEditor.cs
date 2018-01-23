// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridSelectionEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (GridSelection))]
  internal class GridSelectionEditor : Editor
  {
    private const float iconSize = 32f;

    public override void OnInspectorGUI()
    {
      if (!(bool) ((Object) GridPaintingState.activeBrushEditor) || !GridSelection.active)
        return;
      GridPaintingState.activeBrushEditor.OnSelectionInspectorGUI();
    }

    protected override void OnHeaderGUI()
    {
      EditorGUILayout.BeginHorizontal(GridSelectionEditor.Styles.header, new GUILayoutOption[0]);
      GUILayout.Label((Texture) AssetPreview.GetMiniTypeThumbnail(typeof (Grid)), new GUILayoutOption[2]
      {
        GUILayout.Width(32f),
        GUILayout.Height(32f)
      });
      EditorGUILayout.BeginVertical();
      GUILayout.Label(GridSelectionEditor.Styles.gridSelectionLabel);
      GridSelection.position = EditorGUILayout.BoundsIntField(GUIContent.none, GridSelection.position, new GUILayoutOption[0]);
      EditorGUILayout.EndVertical();
      EditorGUILayout.EndHorizontal();
      this.DrawHeaderHelpAndSettingsGUI(GUILayoutUtility.GetLastRect());
    }

    public bool HasFrameBounds()
    {
      return GridSelection.active;
    }

    public Bounds OnGetFrameBounds()
    {
      Bounds bounds = new Bounds();
      if (GridSelection.active)
      {
        Vector3Int min = GridSelection.position.min;
        Vector3Int max = GridSelection.position.max;
        Vector3 world1 = GridSelection.grid.CellToWorld(min);
        Vector3 world2 = GridSelection.grid.CellToWorld(max);
        bounds = new Bounds((world2 + world1) * 0.5f, world2 - world1);
      }
      return bounds;
    }

    private static class Styles
    {
      public static readonly GUIStyle header = new GUIStyle((GUIStyle) "IN GameObjectHeader");
      public static readonly GUIContent gridSelectionLabel = EditorGUIUtility.TextContent("Grid Selection");
    }
  }
}
