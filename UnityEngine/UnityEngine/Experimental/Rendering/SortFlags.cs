// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.SortFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.Rendering
{
  [Flags]
  public enum SortFlags
  {
    None = 0,
    SortingLayer = 1,
    RenderQueue = 2,
    BackToFront = 4,
    QuantizedFrontToBack = 8,
    OptimizeStateChanges = 16, // 0x00000010
    CanvasOrder = 32, // 0x00000020
    CommonOpaque = CanvasOrder | OptimizeStateChanges | QuantizedFrontToBack | RenderQueue | SortingLayer, // 0x0000003B
    CommonTransparent = OptimizeStateChanges | BackToFront | RenderQueue | SortingLayer, // 0x00000017
  }
}
