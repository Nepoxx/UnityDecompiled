// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridBrushEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class for Grid Brush Editor.</para>
  /// </summary>
  [CustomEditor(typeof (GridBrushBase))]
  public class GridBrushEditorBase : Editor
  {
    public virtual void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
    {
      GridBrushEditorBase.OnPaintSceneGUIInternal(gridLayout, brushTarget, position, tool, executing);
    }

    /// <summary>
    ///   <para>Callback for painting the inspector GUI for the GridBrush in the tilemap palette.</para>
    /// </summary>
    public virtual void OnPaintInspectorGUI()
    {
      this.OnInspectorGUI();
    }

    /// <summary>
    ///   <para>Callback for drawing the Inspector GUI when there is an active GridSelection made in a GridLayout.</para>
    /// </summary>
    public virtual void OnSelectionInspectorGUI()
    {
    }

    /// <summary>
    ///   <para>Callback when the mouse cursor leaves a paintable region.</para>
    /// </summary>
    public virtual void OnMouseLeave()
    {
    }

    /// <summary>
    ///   <para>Callback when the mouse cursor enters a paintable region.</para>
    /// </summary>
    public virtual void OnMouseEnter()
    {
    }

    public virtual void OnToolActivated(GridBrushBase.Tool tool)
    {
    }

    public virtual void OnToolDeactivated(GridBrushBase.Tool tool)
    {
    }

    public virtual void RegisterUndo(GameObject brushTarget, GridBrushBase.Tool tool)
    {
    }

    /// <summary>
    ///   <para>Returns all valid targets that the brush can edit.</para>
    /// </summary>
    public virtual GameObject[] validTargets
    {
      get
      {
        return (GameObject[]) null;
      }
    }

    internal static void OnPaintSceneGUIInternal(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = Color.white;
      if (tool == GridBrushBase.Tool.Pick && executing)
        color = Color.cyan;
      if (tool == GridBrushBase.Tool.Paint && executing)
        color = Color.yellow;
      if (tool == GridBrushBase.Tool.Select || tool == GridBrushBase.Tool.Move)
      {
        if (executing)
          color = GridBrushEditorBase.Styles.executingColor;
        else if (GridSelection.active)
          color = GridBrushEditorBase.Styles.activeColor;
      }
      GridEditorUtility.DrawGridMarquee(gridLayout, position, color);
    }

    private static class Styles
    {
      public static readonly Color activeColor = new Color(1f, 0.5f, 0.0f);
      public static readonly Color executingColor = new Color(1f, 0.75f, 0.25f);
    }
  }
}
