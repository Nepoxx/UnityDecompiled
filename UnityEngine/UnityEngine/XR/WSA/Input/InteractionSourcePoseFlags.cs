// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourcePoseFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.XR.WSA.Input
{
  internal enum InteractionSourcePoseFlags
  {
    None = 0,
    HasGripPosition = 1,
    HasGripRotation = 2,
    HasPointerPosition = 4,
    HasPointerRotation = 8,
    HasVelocity = 16, // 0x00000010
    HasAngularVelocity = 32, // 0x00000020
  }
}
