// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedGCHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>A description of a GC handle used by the virtual machine.</para>
  /// </summary>
  [Serializable]
  public struct PackedGCHandle
  {
    [SerializeField]
    internal ulong m_Target;

    /// <summary>
    ///   <para>The address of the managed object that the GC handle is referencing.</para>
    /// </summary>
    public ulong target
    {
      get
      {
        return this.m_Target;
      }
    }
  }
}
