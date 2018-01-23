// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderPipeline
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.Rendering
{
  public abstract class RenderPipeline : IRenderPipeline, IDisposable
  {
    public virtual void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
      if (this.disposed)
        throw new ObjectDisposedException(string.Format("{0} has been disposed. Do not call Render on disposed RenderLoops.", (object) this));
    }

    public bool disposed { get; private set; }

    public virtual void Dispose()
    {
      this.disposed = true;
    }
  }
}
