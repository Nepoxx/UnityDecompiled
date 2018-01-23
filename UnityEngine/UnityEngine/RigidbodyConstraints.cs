// Decompiled with JetBrains decompiler
// Type: UnityEngine.RigidbodyConstraints
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Use these flags to constrain motion of Rigidbodies.</para>
  /// </summary>
  public enum RigidbodyConstraints
  {
    None = 0,
    FreezePositionX = 2,
    FreezePositionY = 4,
    FreezePositionZ = 8,
    FreezePosition = 14, // 0x0000000E
    FreezeRotationX = 16, // 0x00000010
    FreezeRotationY = 32, // 0x00000020
    FreezeRotationZ = 64, // 0x00000040
    FreezeRotation = 112, // 0x00000070
    FreezeAll = 126, // 0x0000007E
  }
}
