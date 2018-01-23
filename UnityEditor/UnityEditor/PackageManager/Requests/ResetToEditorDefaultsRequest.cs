// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.ResetToEditorDefaultsRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Represents an asynchronous request to reset the project packages to the current Editor default configuration.</para>
  /// </summary>
  [Serializable]
  public sealed class ResetToEditorDefaultsRequest : Request
  {
    private ResetToEditorDefaultsRequest()
    {
    }

    internal ResetToEditorDefaultsRequest(long operationId, NativeClient.StatusCode initialStatus)
      : base(operationId, initialStatus)
    {
    }
  }
}
