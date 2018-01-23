// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.MathfInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngineInternal
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct MathfInternal
  {
    public static volatile float FloatMinNormal = 1.175494E-38f;
    public static volatile float FloatMinDenormal = float.Epsilon;
    public static bool IsFlushToZeroEnabled = (double) MathfInternal.FloatMinDenormal == 0.0;
  }
}
