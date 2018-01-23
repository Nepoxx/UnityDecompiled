// Decompiled with JetBrains decompiler
// Type: UnityEngine.ScalableBufferManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public static class ScalableBufferManager
  {
    public static float widthScaleFactor
    {
      get
      {
        return ScalableBufferManager.GetWidthScaleFactor();
      }
    }

    public static float heightScaleFactor
    {
      get
      {
        return ScalableBufferManager.GetHeightScaleFactor();
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResizeBuffers(float widthScale, float heightScale);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float GetWidthScaleFactor();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float GetHeightScaleFactor();
  }
}
