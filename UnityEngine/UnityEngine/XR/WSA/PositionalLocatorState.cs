// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.PositionalLocatorState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA
{
  /// <summary>
  ///   <para>Indicates the lifecycle state of the device's spatial location system.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA")]
  public enum PositionalLocatorState
  {
    Unavailable,
    OrientationOnly,
    Activating,
    Active,
    Inhibited,
  }
}
