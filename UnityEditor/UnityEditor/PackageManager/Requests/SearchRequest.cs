// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.SearchRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Represents an asynchronous request to find a package.</para>
  /// </summary>
  [Serializable]
  public sealed class SearchRequest : Request<UnityEditor.PackageManager.PackageInfo[]>
  {
    [SerializeField]
    private string m_PackageIdOrName;

    private SearchRequest()
    {
    }

    internal SearchRequest(long operationId, NativeClient.StatusCode initialStatus, string packageIdOrName)
      : base(operationId, initialStatus)
    {
      this.m_PackageIdOrName = packageIdOrName;
    }

    /// <summary>
    ///   <para>The package id or name used in this search operation.</para>
    /// </summary>
    public string PackageIdOrName
    {
      get
      {
        return this.m_PackageIdOrName;
      }
    }

    protected override UnityEditor.PackageManager.PackageInfo[] GetResult()
    {
      return ((IEnumerable<UpmPackageInfo>) NativeClient.GetSearchOperationData(this.Id)).Select<UpmPackageInfo, UnityEditor.PackageManager.PackageInfo>((Func<UpmPackageInfo, UnityEditor.PackageManager.PackageInfo>) (p => (UnityEditor.PackageManager.PackageInfo) p)).ToArray<UnityEditor.PackageManager.PackageInfo>();
    }
  }
}
