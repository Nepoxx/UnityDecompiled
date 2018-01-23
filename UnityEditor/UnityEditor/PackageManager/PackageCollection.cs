// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.PackageCollection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.PackageManager
{
  /// <summary>
  ///   <para>A collection of PackageInfo objects.</para>
  /// </summary>
  [Serializable]
  public class PackageCollection : IEnumerable<PackageInfo>, IEnumerable
  {
    [SerializeField]
    private PackageInfo[] m_PackageList;

    private PackageCollection()
    {
    }

    internal PackageCollection(IEnumerable<PackageInfo> packages)
    {
      this.m_PackageList = ((IEnumerable<PackageInfo>) ((object) packages ?? (object) new PackageInfo[0])).ToArray<PackageInfo>();
    }

    IEnumerator<PackageInfo> IEnumerable<PackageInfo>.GetEnumerator()
    {
      return ((IEnumerable<PackageInfo>) this.m_PackageList).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.m_PackageList.GetEnumerator();
    }
  }
}
