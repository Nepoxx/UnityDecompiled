// Decompiled with JetBrains decompiler
// Type: UnityEditor.GizmoType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Determines how a gizmo is drawn or picked in the Unity editor.</para>
  /// </summary>
  public enum GizmoType
  {
    [Obsolete("Use NotInSelectionHierarchy instead (UnityUpgradable) -> NotInSelectionHierarchy")] NotSelected = -127, // -0x0000007F
    [Obsolete("Use InSelectionHierarchy instead (UnityUpgradable) -> InSelectionHierarchy")] SelectedOrChild = -127, // -0x0000007F
    Pickable = 1,
    NotInSelectionHierarchy = 2,
    Selected = 4,
    Active = 8,
    InSelectionHierarchy = 16, // 0x00000010
    NonSelected = 32, // 0x00000020
  }
}
