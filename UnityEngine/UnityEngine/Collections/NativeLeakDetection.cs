// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collections.NativeLeakDetection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Collections
{
  public static class NativeLeakDetection
  {
    private static int s_NativeLeakDetectionMode;

    public static NativeLeakDetectionMode Mode
    {
      get
      {
        return (NativeLeakDetectionMode) NativeLeakDetection.s_NativeLeakDetectionMode;
      }
      set
      {
        NativeLeakDetection.s_NativeLeakDetectionMode = (int) value;
      }
    }
  }
}
