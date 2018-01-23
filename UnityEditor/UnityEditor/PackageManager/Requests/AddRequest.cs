// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.AddRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Represents an asynchronous request to add a package to the project.</para>
  /// </summary>
  [Serializable]
  public sealed class AddRequest : Request<UnityEditor.PackageManager.PackageInfo>
  {
    private AddRequest()
    {
    }

    internal AddRequest(long operationId, NativeClient.StatusCode initialStatus)
      : base(operationId, initialStatus)
    {
    }

    protected override UnityEditor.PackageManager.PackageInfo GetResult()
    {
      return (UnityEditor.PackageManager.PackageInfo) NativeClient.GetAddOperationData(this.Id);
    }
  }
}
