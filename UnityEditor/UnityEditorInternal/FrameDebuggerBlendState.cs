// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameDebuggerBlendState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Rendering;

namespace UnityEditorInternal
{
  internal struct FrameDebuggerBlendState
  {
    public uint writeMask;
    public BlendMode srcBlend;
    public BlendMode dstBlend;
    public BlendMode srcBlendAlpha;
    public BlendMode dstBlendAlpha;
    public BlendOp blendOp;
    public BlendOp blendOpAlpha;
  }
}
