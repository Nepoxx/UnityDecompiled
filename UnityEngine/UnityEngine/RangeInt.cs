// Decompiled with JetBrains decompiler
// Type: UnityEngine.RangeInt
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct RangeInt
  {
    public int start;
    public int length;

    public RangeInt(int start, int length)
    {
      this.start = start;
      this.length = length;
    }

    public int end
    {
      get
      {
        return this.start + this.length;
      }
    }
  }
}
