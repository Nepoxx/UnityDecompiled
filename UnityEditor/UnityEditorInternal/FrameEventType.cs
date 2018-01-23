// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameEventType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  internal enum FrameEventType
  {
    ClearNone,
    ClearColor,
    ClearDepth,
    ClearColorDepth,
    ClearStencil,
    ClearColorStencil,
    ClearDepthStencil,
    ClearAll,
    SetRenderTarget,
    ResolveRT,
    ResolveDepth,
    GrabIntoRT,
    StaticBatch,
    DynamicBatch,
    Mesh,
    DynamicGeometry,
    GLDraw,
    SkinOnGPU,
    DrawProcedural,
    ComputeDispatch,
    PluginEvent,
    InstancedMesh,
  }
}
