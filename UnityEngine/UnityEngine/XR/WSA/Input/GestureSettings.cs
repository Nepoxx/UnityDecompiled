// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.GestureSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>This enumeration represents the set of gestures that may be recognized by GestureRecognizer.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public enum GestureSettings
  {
    None = 0,
    Tap = 1,
    DoubleTap = 2,
    Hold = 4,
    ManipulationTranslate = 8,
    NavigationX = 16, // 0x00000010
    NavigationY = 32, // 0x00000020
    NavigationZ = 64, // 0x00000040
    NavigationRailsX = 128, // 0x00000080
    NavigationRailsY = 256, // 0x00000100
    NavigationRailsZ = 512, // 0x00000200
  }
}
