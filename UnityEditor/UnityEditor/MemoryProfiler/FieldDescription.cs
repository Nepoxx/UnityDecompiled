// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.FieldDescription
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>Description of a field of a managed type.</para>
  /// </summary>
  [Serializable]
  public struct FieldDescription
  {
    [SerializeField]
    internal string m_Name;
    [SerializeField]
    internal int m_Offset;
    [SerializeField]
    internal int m_TypeIndex;
    [SerializeField]
    internal bool m_IsStatic;

    /// <summary>
    ///   <para>Name of this field.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>Offset of this field.</para>
    /// </summary>
    public int offset
    {
      get
      {
        return this.m_Offset;
      }
    }

    /// <summary>
    ///   <para>The typeindex into PackedMemorySnapshot.typeDescriptions of the type this field belongs to.</para>
    /// </summary>
    public int typeIndex
    {
      get
      {
        return this.m_TypeIndex;
      }
    }

    /// <summary>
    ///   <para>Is this field static?</para>
    /// </summary>
    public bool isStatic
    {
      get
      {
        return this.m_IsStatic;
      }
    }
  }
}
