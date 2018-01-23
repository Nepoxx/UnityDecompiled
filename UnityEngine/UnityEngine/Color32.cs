// Decompiled with JetBrains decompiler
// Type: UnityEngine.Color32
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Explicit)]
  public struct Color32
  {
    [FieldOffset(0)]
    private int rgba;
    [FieldOffset(0)]
    public byte r;
    [FieldOffset(1)]
    public byte g;
    [FieldOffset(2)]
    public byte b;
    [FieldOffset(3)]
    public byte a;

    public Color32(byte r, byte g, byte b, byte a)
    {
      this.rgba = 0;
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }

    public static implicit operator Color32(Color c)
    {
      return new Color32((byte) ((double) Mathf.Clamp01(c.r) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(c.g) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(c.b) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(c.a) * (double) byte.MaxValue));
    }

    public static implicit operator Color(Color32 c)
    {
      return new Color((float) c.r / (float) byte.MaxValue, (float) c.g / (float) byte.MaxValue, (float) c.b / (float) byte.MaxValue, (float) c.a / (float) byte.MaxValue);
    }

    public static Color32 Lerp(Color32 a, Color32 b, float t)
    {
      t = Mathf.Clamp01(t);
      return new Color32((byte) ((double) a.r + (double) ((int) b.r - (int) a.r) * (double) t), (byte) ((double) a.g + (double) ((int) b.g - (int) a.g) * (double) t), (byte) ((double) a.b + (double) ((int) b.b - (int) a.b) * (double) t), (byte) ((double) a.a + (double) ((int) b.a - (int) a.a) * (double) t));
    }

    public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
    {
      return new Color32((byte) ((double) a.r + (double) ((int) b.r - (int) a.r) * (double) t), (byte) ((double) a.g + (double) ((int) b.g - (int) a.g) * (double) t), (byte) ((double) a.b + (double) ((int) b.b - (int) a.b) * (double) t), (byte) ((double) a.a + (double) ((int) b.a - (int) a.a) * (double) t));
    }

    public override string ToString()
    {
      return UnityString.Format("RGBA({0}, {1}, {2}, {3})", (object) this.r, (object) this.g, (object) this.b, (object) this.a);
    }

    public string ToString(string format)
    {
      return UnityString.Format("RGBA({0}, {1}, {2}, {3})", (object) this.r.ToString(format), (object) this.g.ToString(format), (object) this.b.ToString(format), (object) this.a.ToString(format));
    }
  }
}
