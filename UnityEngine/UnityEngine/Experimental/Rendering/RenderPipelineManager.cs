// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderPipelineManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
  public static class RenderPipelineManager
  {
    private static IRenderPipelineAsset s_CurrentPipelineAsset;

    public static IRenderPipeline currentPipeline { get; private set; }

    [RequiredByNativeCode]
    internal static void CleanupRenderPipeline()
    {
      if (RenderPipelineManager.s_CurrentPipelineAsset != null)
        RenderPipelineManager.s_CurrentPipelineAsset.DestroyCreatedInstances();
      RenderPipelineManager.s_CurrentPipelineAsset = (IRenderPipelineAsset) null;
      RenderPipelineManager.currentPipeline = (IRenderPipeline) null;
    }

    [RequiredByNativeCode]
    private static void DoRenderLoop_Internal(IRenderPipelineAsset pipe, Camera[] cameras, IntPtr loopPtr)
    {
      RenderPipelineManager.PrepareRenderPipeline(pipe);
      if (RenderPipelineManager.currentPipeline == null)
        return;
      RenderPipelineManager.currentPipeline.Render(new ScriptableRenderContext(loopPtr), cameras);
    }

    private static void PrepareRenderPipeline(IRenderPipelineAsset pipe)
    {
      if (RenderPipelineManager.s_CurrentPipelineAsset != pipe)
      {
        if (RenderPipelineManager.s_CurrentPipelineAsset != null)
          RenderPipelineManager.CleanupRenderPipeline();
        RenderPipelineManager.s_CurrentPipelineAsset = pipe;
      }
      if (RenderPipelineManager.s_CurrentPipelineAsset == null || RenderPipelineManager.currentPipeline != null && !RenderPipelineManager.currentPipeline.disposed)
        return;
      RenderPipelineManager.currentPipeline = RenderPipelineManager.s_CurrentPipelineAsset.CreatePipeline();
    }
  }
}
