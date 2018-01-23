// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderQueueRange
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.Rendering
{
  public struct RenderQueueRange
  {
    public int min;
    public int max;

    public static RenderQueueRange all
    {
      get
      {
        return new RenderQueueRange() { min = 0, max = 5000 };
      }
    }

    public static RenderQueueRange opaque
    {
      get
      {
        return new RenderQueueRange() { min = 0, max = 2500 };
      }
    }

    public static RenderQueueRange transparent
    {
      get
      {
        return new RenderQueueRange() { min = 2501, max = 5000 };
      }
    }
  }
}
