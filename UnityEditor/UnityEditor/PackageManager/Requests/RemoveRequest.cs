// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.RemoveRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Represents an asynchronous request to remove a package from the project.</para>
  /// </summary>
  [Serializable]
  public sealed class RemoveRequest : Request
  {
    [SerializeField]
    private string m_PackageIdOrName;

    private RemoveRequest()
    {
    }

    internal RemoveRequest(long operationId, NativeClient.StatusCode initialStatus, string packageName)
      : base(operationId, initialStatus)
    {
      this.m_PackageIdOrName = packageName;
    }

    /// <summary>
    ///   <para>The package being removed by this request.</para>
    /// </summary>
    public string PackageIdOrName
    {
      get
      {
        return this.m_PackageIdOrName;
      }
    }
  }
}
