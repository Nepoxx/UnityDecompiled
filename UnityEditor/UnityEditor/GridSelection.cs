// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores the selection made on a GridLayout.</para>
  /// </summary>
  public class GridSelection : ScriptableObject
  {
    private BoundsInt m_Position;
    private GameObject m_Target;
    [SerializeField]
    private UnityEngine.Object m_PreviousSelection;

    public static event Action gridSelectionChanged;

    /// <summary>
    ///   <para>Whether there is an active GridSelection made on a GridLayout.</para>
    /// </summary>
    public static bool active
    {
      get
      {
        return Selection.activeObject is GridSelection && (UnityEngine.Object) GridSelection.selection.m_Target != (UnityEngine.Object) null;
      }
    }

    private static GridSelection selection
    {
      get
      {
        return Selection.activeObject as GridSelection;
      }
    }

    /// <summary>
    ///   <para>The cell coordinates of the active GridSelection made on the GridLayout.</para>
    /// </summary>
    public static BoundsInt position
    {
      get
      {
        return !((UnityEngine.Object) GridSelection.selection != (UnityEngine.Object) null) ? new BoundsInt() : GridSelection.selection.m_Position;
      }
      set
      {
        if (!((UnityEngine.Object) GridSelection.selection != (UnityEngine.Object) null) || !(GridSelection.selection.m_Position != value))
          return;
        GridSelection.selection.m_Position = value;
        if (GridSelection.gridSelectionChanged != null)
          GridSelection.gridSelectionChanged();
      }
    }

    /// <summary>
    ///   <para>The GameObject of the GridLayout where the active GridSelection was made.</para>
    /// </summary>
    public static GameObject target
    {
      get
      {
        return !((UnityEngine.Object) GridSelection.selection != (UnityEngine.Object) null) ? (GameObject) null : GridSelection.selection.m_Target;
      }
    }

    /// <summary>
    ///   <para>The Grid of the target of the active GridSelection.</para>
    /// </summary>
    public static Grid grid
    {
      get
      {
        return !((UnityEngine.Object) GridSelection.selection != (UnityEngine.Object) null) || !((UnityEngine.Object) GridSelection.selection.m_Target != (UnityEngine.Object) null) ? (Grid) null : GridSelection.selection.m_Target.GetComponentInParent<Grid>();
      }
    }

    /// <summary>
    ///   <para>Creates a new GridSelection and sets it as the active GridSelection.</para>
    /// </summary>
    /// <param name="target">The target GameObject for the GridSelection.</param>
    /// <param name="bounds">The cell coordinates of selection made.</param>
    public static void Select(UnityEngine.Object target, BoundsInt bounds)
    {
      GridSelection instance = ScriptableObject.CreateInstance<GridSelection>();
      instance.m_PreviousSelection = Selection.activeObject;
      instance.m_Target = target as GameObject;
      instance.m_Position = bounds;
      Selection.activeObject = (UnityEngine.Object) instance;
      // ISSUE: reference to a compiler-generated field
      if (GridSelection.gridSelectionChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      GridSelection.gridSelectionChanged();
    }

    /// <summary>
    ///   <para>Clears the active GridSelection.</para>
    /// </summary>
    public static void Clear()
    {
      if (!GridSelection.active)
        return;
      GridSelection.selection.m_Position = new BoundsInt();
      Selection.activeObject = GridSelection.selection.m_PreviousSelection;
      // ISSUE: reference to a compiler-generated field
      if (GridSelection.gridSelectionChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        GridSelection.gridSelectionChanged();
      }
    }
  }
}
