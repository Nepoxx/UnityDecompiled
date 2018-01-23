// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourceStateFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.XR.WSA.Input
{
  internal enum InteractionSourceStateFlags
  {
    None = 0,
    Grasped = 1,
    AnyPressed = 2,
    TouchpadPressed = 4,
    ThumbstickPressed = 8,
    SelectPressed = 16, // 0x00000010
    MenuPressed = 32, // 0x00000020
    TouchpadTouched = 64, // 0x00000040
  }
}
