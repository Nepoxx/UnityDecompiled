// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedNativeType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>A description of a C++ unity type.</para>
  /// </summary>
  [Serializable]
  public struct PackedNativeType
  {
    [SerializeField]
    internal string m_Name;
    [SerializeField]
    internal int m_NativeBaseTypeArrayIndex;

    /// <summary>
    ///   <para>Name of this C++ unity type.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    [Obsolete("PackedNativeType.baseClassId is obsolete. Use PackedNativeType.nativeBaseTypeArrayIndex instead (UnityUpgradable) -> nativeBaseTypeArrayIndex")]
    public int baseClassId
    {
      get
      {
        return this.m_NativeBaseTypeArrayIndex;
      }
    }

    /// <summary>
    ///   <para>The index used to obtain the native C++ base class description from the PackedMemorySnapshot.nativeTypes array.</para>
    /// </summary>
    public int nativeBaseTypeArrayIndex
    {
      get
      {
        return this.m_NativeBaseTypeArrayIndex;
      }
    }
  }
}
