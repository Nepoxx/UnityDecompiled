// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderStateMask
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.Rendering
{
  [Flags]
  public enum RenderStateMask
  {
    Nothing = 0,
    Blend = 1,
    Raster = 2,
    Depth = 4,
    Stencil = 8,
    Everything = Stencil | Depth | Raster | Blend, // 0x0000000F
  }
}
