// Decompiled with JetBrains decompiler
// Type: UnityEditor.AlphabeticalSort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Hierarchy sort method to allow for items and their children to be sorted alphabetically.</para>
  /// </summary>
  [Obsolete("BaseHierarchySort is no longer supported because of performance reasons")]
  public class AlphabeticalSort : BaseHierarchySort
  {
    private readonly GUIContent m_Content = new GUIContent((Texture) EditorGUIUtility.FindTexture("AlphabeticalSorting"), "Alphabetical Order");

    /// <summary>
    ///   <para>Content to visualize the alphabetical sorting method.</para>
    /// </summary>
    public override GUIContent content
    {
      get
      {
        return this.m_Content;
      }
    }
  }
}
