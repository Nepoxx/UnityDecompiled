// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.FilterRenderersSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.Rendering
{
  public struct FilterRenderersSettings
  {
    private RenderQueueRange m_RenderQueueRange;
    private int m_LayerMask;

    public FilterRenderersSettings(bool initializeValues = false)
    {
      this = new FilterRenderersSettings();
      if (!initializeValues)
        return;
      this.m_RenderQueueRange = RenderQueueRange.all;
      this.m_LayerMask = -1;
    }

    public RenderQueueRange renderQueueRange
    {
      get
      {
        return this.m_RenderQueueRange;
      }
      set
      {
        this.m_RenderQueueRange = value;
      }
    }

    public int layerMask
    {
      get
      {
        return this.m_LayerMask;
      }
      set
      {
        this.m_LayerMask = value;
      }
    }
  }
}
