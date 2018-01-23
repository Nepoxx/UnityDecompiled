// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.ListRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Represents an asynchronous request to list the packages in the project.</para>
  /// </summary>
  [Serializable]
  public sealed class ListRequest : Request<PackageCollection>
  {
    private ListRequest()
    {
    }

    internal ListRequest(long operationId, NativeClient.StatusCode initialStatus)
      : base(operationId, initialStatus)
    {
    }

    protected override PackageCollection GetResult()
    {
      return new PackageCollection(((IEnumerable<UpmPackageInfo>) NativeClient.GetListOperationData(this.Id).packageList).Select<UpmPackageInfo, UnityEditor.PackageManager.PackageInfo>((Func<UpmPackageInfo, UnityEditor.PackageManager.PackageInfo>) (p => (UnityEditor.PackageManager.PackageInfo) p)));
    }
  }
}
