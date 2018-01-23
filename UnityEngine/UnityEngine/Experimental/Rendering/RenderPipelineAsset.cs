// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderPipelineAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering
{
  public abstract class RenderPipelineAsset : ScriptableObject, IRenderPipelineAsset
  {
    private readonly List<IRenderPipeline> m_CreatedPipelines = new List<IRenderPipeline>();

    public void DestroyCreatedInstances()
    {
      foreach (IDisposable createdPipeline in this.m_CreatedPipelines)
        createdPipeline.Dispose();
      this.m_CreatedPipelines.Clear();
    }

    public IRenderPipeline CreatePipeline()
    {
      IRenderPipeline pipeline = this.InternalCreatePipeline();
      if (pipeline != null)
        this.m_CreatedPipelines.Add(pipeline);
      return pipeline;
    }

    public virtual int GetTerrainBrushPassIndex()
    {
      return 2500;
    }

    public virtual Material GetDefaultMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefaultParticleMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefaultLineMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefaultTerrainMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefaultUIMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefaultUIOverdrawMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefaultUIETC1SupportedMaterial()
    {
      return (Material) null;
    }

    public virtual Material GetDefault2DMaterial()
    {
      return (Material) null;
    }

    public virtual Shader GetDefaultShader()
    {
      return (Shader) null;
    }

    protected abstract IRenderPipeline InternalCreatePipeline();

    protected IEnumerable<IRenderPipeline> CreatedInstances()
    {
      return (IEnumerable<IRenderPipeline>) this.m_CreatedPipelines;
    }

    private void OnValidate()
    {
      this.DestroyCreatedInstances();
    }

    private void OnDisable()
    {
      this.DestroyCreatedInstances();
    }
  }
}
