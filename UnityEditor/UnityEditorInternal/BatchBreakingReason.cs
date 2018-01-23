// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.BatchBreakingReason
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  public enum BatchBreakingReason
  {
    NoBreaking = 0,
    NotCoplanarWithCanvas = 1,
    CanvasInjectionIndex = 2,
    DifferentMaterialInstance = 4,
    DifferentRectClipping = 8,
    DifferentTexture = 16, // 0x00000010
    DifferentA8TextureUsage = 32, // 0x00000020
    DifferentClipRect = 64, // 0x00000040
    Unknown = 128, // 0x00000080
  }
}
