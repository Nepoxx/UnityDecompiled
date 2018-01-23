// Decompiled with JetBrains decompiler
// Type: UnityEngine.WrapMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.</para>
  /// </summary>
  public enum WrapMode
  {
    Default = 0,
    Clamp = 1,
    Once = 1,
    Loop = 2,
    PingPong = 4,
    ClampForever = 8,
  }
}
