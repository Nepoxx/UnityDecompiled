// Decompiled with JetBrains decompiler
// Type: UnityEditor.BaseHierarchySort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The base class used to create new sorting.</para>
  /// </summary>
  [Obsolete("BaseHierarchySort is no longer supported because of performance reasons")]
  public abstract class BaseHierarchySort : IComparer<GameObject>
  {
    /// <summary>
    ///   <para>The content to display to quickly identify the hierarchy's mode.</para>
    /// </summary>
    public virtual GUIContent content
    {
      get
      {
        return (GUIContent) null;
      }
    }

    /// <summary>
    ///   <para>The sorting method used to determine the order of GameObjects.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    public virtual int Compare(GameObject lhs, GameObject rhs)
    {
      return 0;
    }
  }
}
