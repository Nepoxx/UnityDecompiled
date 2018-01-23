// Decompiled with JetBrains decompiler
// Type: UnityEngine.CSSLayout.MeasureOutput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.CSSLayout
{
  internal class MeasureOutput
  {
    public static long Make(double width, double height)
    {
      return MeasureOutput.Make((int) width, (int) height);
    }

    public static long Make(int width, int height)
    {
      return (long) width << 32 | (long) (uint) height;
    }

    public static int GetWidth(long measureOutput)
    {
      return (int) ((long) uint.MaxValue & measureOutput >> 32);
    }

    public static int GetHeight(long measureOutput)
    {
      return (int) ((long) uint.MaxValue & measureOutput);
    }
  }
}
