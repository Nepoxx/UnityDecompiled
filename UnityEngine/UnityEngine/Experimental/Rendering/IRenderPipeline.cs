// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.IRenderPipeline
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.Rendering
{
  public interface IRenderPipeline : IDisposable
  {
    bool disposed { get; }

    void Render(ScriptableRenderContext renderContext, Camera[] cameras);
  }
}
