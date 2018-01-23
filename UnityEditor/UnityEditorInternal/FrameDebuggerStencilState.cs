// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameDebuggerStencilState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Rendering;

namespace UnityEditorInternal
{
  internal struct FrameDebuggerStencilState
  {
    public bool stencilEnable;
    public byte readMask;
    public byte writeMask;
    public byte padding;
    public CompareFunction stencilFuncFront;
    public StencilOp stencilPassOpFront;
    public StencilOp stencilFailOpFront;
    public StencilOp stencilZFailOpFront;
    public CompareFunction stencilFuncBack;
    public StencilOp stencilPassOpBack;
    public StencilOp stencilFailOpBack;
    public StencilOp stencilZFailOpBack;
  }
}
