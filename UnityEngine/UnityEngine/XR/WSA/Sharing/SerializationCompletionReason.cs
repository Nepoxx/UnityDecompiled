// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Sharing.SerializationCompletionReason
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Sharing
{
  /// <summary>
  ///   <para>This enum represents the result of a WorldAnchorTransferBatch operation.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Sharing")]
  public enum SerializationCompletionReason
  {
    Succeeded,
    NotSupported,
    AccessDenied,
    UnknownError,
  }
}
