// Decompiled with JetBrains decompiler
// Type: UnityEngine.CameraType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum CameraType
  {
    Game = 1,
    SceneView = 2,
    Preview = 4,
    VR = 8,
    Reflection = 16, // 0x00000010
  }
}
