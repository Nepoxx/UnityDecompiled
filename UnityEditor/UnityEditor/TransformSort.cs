// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformSort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Is the default sorting method used by the hierarchy.</para>
  /// </summary>
  [Obsolete("BaseHierarchySort is no longer supported because of performance reasons")]
  public class TransformSort : BaseHierarchySort
  {
    private readonly GUIContent m_Content = new GUIContent((Texture) EditorGUIUtility.FindTexture("DefaultSorting"), "Transform Child Order");

    /// <summary>
    ///   <para>Content to visualize the transform sorting method.</para>
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
