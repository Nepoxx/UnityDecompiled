// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPaletteAddPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class GridPaletteAddPopup : EditorWindow
  {
    private string m_Name = "New Palette";
    private static long s_LastClosedTime;
    private static GridPaletteAddPopup s_Instance;
    private GridPaintPaletteWindow m_Owner;
    private GridLayout.CellLayout m_Layout;
    private GridPalette.CellSizing m_CellSizing;
    private Vector3 m_CellSize;

    private void Init(Rect buttonRect, GridPaintPaletteWindow owner)
    {
      this.m_Owner = owner;
      this.m_CellSize = new Vector3(1f, 1f, 0.0f);
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      this.ShowAsDropDown(buttonRect, new Vector2(312f, 140f), (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
    }

    internal void OnGUI()
    {
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, (GUIStyle) "grey_border");
      GUILayout.Space(3f);
      GUILayout.Label(GridPaletteAddPopup.Styles.header, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.Space(4f);
      GUILayout.BeginHorizontal();
      GUILayout.Label(GridPaletteAddPopup.Styles.nameLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(90f)
      });
      this.m_Name = EditorGUILayout.TextField(this.m_Name);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label(GridPaletteAddPopup.Styles.gridLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(90f)
      });
      this.m_Layout = (GridLayout.CellLayout) EditorGUILayout.EnumPopup((Enum) this.m_Layout);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label(GridPaletteAddPopup.Styles.sizeLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(90f)
      });
      this.m_CellSizing = (GridPalette.CellSizing) EditorGUILayout.EnumPopup((Enum) this.m_CellSizing);
      GUILayout.EndHorizontal();
      using (new EditorGUI.DisabledScope(this.m_CellSizing == GridPalette.CellSizing.Automatic))
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(GUIContent.none, new GUILayoutOption[1]
        {
          GUILayout.Width(90f)
        });
        this.m_CellSize = EditorGUILayout.Vector3Field(GUIContent.none, this.m_CellSize);
        GUILayout.EndHorizontal();
      }
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      if (GUILayout.Button(GridPaletteAddPopup.Styles.cancel))
        this.Close();
      using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(this.m_Name)))
      {
        if (GUILayout.Button(GridPaletteAddPopup.Styles.ok))
        {
          GameObject newPaletteNamed = GridPaletteUtility.CreateNewPaletteNamed(this.m_Name, this.m_Layout, this.m_CellSizing, this.m_CellSize);
          if ((UnityEngine.Object) newPaletteNamed != (UnityEngine.Object) null)
          {
            this.m_Owner.palette = newPaletteNamed;
            this.m_Owner.Repaint();
          }
          this.Close();
          GUIUtility.ExitGUI();
        }
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
    }

    internal static bool ShowAtPosition(Rect buttonRect, GridPaintPaletteWindow owner)
    {
      if (DateTime.Now.Ticks / 10000L < GridPaletteAddPopup.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) GridPaletteAddPopup.s_Instance == (UnityEngine.Object) null)
        GridPaletteAddPopup.s_Instance = ScriptableObject.CreateInstance<GridPaletteAddPopup>();
      GridPaletteAddPopup.s_Instance.Init(buttonRect, owner);
      return true;
    }

    private static class Styles
    {
      public static readonly GUIContent nameLabel = EditorGUIUtility.TextContent("Name");
      public static readonly GUIContent ok = EditorGUIUtility.TextContent("Create");
      public static readonly GUIContent cancel = EditorGUIUtility.TextContent("Cancel");
      public static readonly GUIContent header = EditorGUIUtility.TextContent("Create New Palette");
      public static readonly GUIContent gridLabel = EditorGUIUtility.TextContent("Grid");
      public static readonly GUIContent sizeLabel = EditorGUIUtility.TextContent("Cell Size");
    }
  }
}
