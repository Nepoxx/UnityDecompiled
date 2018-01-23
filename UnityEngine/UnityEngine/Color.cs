// Decompiled with JetBrains decompiler
// Type: UnityEngine.Color
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct Color
  {
    public float r;
    public float g;
    public float b;
    public float a;

    public Color(float r, float g, float b, float a)
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }

    public Color(float r, float g, float b)
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = 1f;
    }

    public override string ToString()
    {
      return UnityString.Format("RGBA({0:F3}, {1:F3}, {2:F3}, {3:F3})", (object) this.r, (object) this.g, (object) this.b, (object) this.a);
    }

    public string ToString(string format)
    {
      return UnityString.Format("RGBA({0}, {1}, {2}, {3})", (object) this.r.ToString(format), (object) this.g.ToString(format), (object) this.b.ToString(format), (object) this.a.ToString(format));
    }

    public override int GetHashCode()
    {
      return (Vector4) this.GetHashCode();
    }

    public override bool Equals(object other)
    {
      if (!(other is Color))
        return false;
      Color color = (Color) other;
      return this.r.Equals(color.r) && this.g.Equals(color.g) && this.b.Equals(color.b) && this.a.Equals(color.a);
    }

    public static Color operator +(Color a, Color b)
    {
      return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
    }

    public static Color operator -(Color a, Color b)
    {
      return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
    }

    public static Color operator *(Color a, Color b)
    {
      return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
    }

    public static Color operator *(Color a, float b)
    {
      return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
    }

    public static Color operator *(float b, Color a)
    {
      return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
    }

    public static Color operator /(Color a, float b)
    {
      return new Color(a.r / b, a.g / b, a.b / b, a.a / b);
    }

    public static bool operator ==(Color lhs, Color rhs)
    {
      return (Vector4) lhs == (Vector4) rhs;
    }

    public static bool operator !=(Color lhs, Color rhs)
    {
      return !(lhs == rhs);
    }

    public static Color Lerp(Color a, Color b, float t)
    {
      t = Mathf.Clamp01(t);
      return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
    }

    public static Color LerpUnclamped(Color a, Color b, float t)
    {
      return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
    }

    internal Color RGBMultiplied(float multiplier)
    {
      return new Color(this.r * multiplier, this.g * multiplier, this.b * multiplier, this.a);
    }

    internal Color AlphaMultiplied(float multiplier)
    {
      return new Color(this.r, this.g, this.b, this.a * multiplier);
    }

    internal Color RGBMultiplied(Color multiplier)
    {
      return new Color(this.r * multiplier.r, this.g * multiplier.g, this.b * multiplier.b, this.a);
    }

    public static Color red
    {
      get
      {
        return new Color(1f, 0.0f, 0.0f, 1f);
      }
    }

    public static Color green
    {
      get
      {
        return new Color(0.0f, 1f, 0.0f, 1f);
      }
    }

    public static Color blue
    {
      get
      {
        return new Color(0.0f, 0.0f, 1f, 1f);
      }
    }

    public static Color white
    {
      get
      {
        return new Color(1f, 1f, 1f, 1f);
      }
    }

    public static Color black
    {
      get
      {
        return new Color(0.0f, 0.0f, 0.0f, 1f);
      }
    }

    public static Color yellow
    {
      get
      {
        return new Color(1f, 0.9215686f, 0.01568628f, 1f);
      }
    }

    public static Color cyan
    {
      get
      {
        return new Color(0.0f, 1f, 1f, 1f);
      }
    }

    public static Color magenta
    {
      get
      {
        return new Color(1f, 0.0f, 1f, 1f);
      }
    }

    public static Color gray
    {
      get
      {
        return new Color(0.5f, 0.5f, 0.5f, 1f);
      }
    }

    public static Color grey
    {
      get
      {
        return new Color(0.5f, 0.5f, 0.5f, 1f);
      }
    }

    public static Color clear
    {
      get
      {
        return new Color(0.0f, 0.0f, 0.0f, 0.0f);
      }
    }

    public float grayscale
    {
      get
      {
        return (float) (0.29899999499321 * (double) this.r + 0.587000012397766 * (double) this.g + 57.0 / 500.0 * (double) this.b);
      }
    }

    public Color linear
    {
      get
      {
        return new Color(Mathf.GammaToLinearSpace(this.r), Mathf.GammaToLinearSpace(this.g), Mathf.GammaToLinearSpace(this.b), this.a);
      }
    }

    public Color gamma
    {
      get
      {
        return new Color(Mathf.LinearToGammaSpace(this.r), Mathf.LinearToGammaSpace(this.g), Mathf.LinearToGammaSpace(this.b), this.a);
      }
    }

    public float maxColorComponent
    {
      get
      {
        return Mathf.Max(Mathf.Max(this.r, this.g), this.b);
      }
    }

    public static implicit operator Vector4(Color c)
    {
      return new Vector4(c.r, c.g, c.b, c.a);
    }

    public static implicit operator Color(Vector4 v)
    {
      return new Color(v.x, v.y, v.z, v.w);
    }

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.r;
          case 1:
            return this.g;
          case 2:
            return this.b;
          case 3:
            return this.a;
          default:
            throw new IndexOutOfRangeException("Invalid Vector3 index!");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.r = value;
            break;
          case 1:
            this.g = value;
            break;
          case 2:
            this.b = value;
            break;
          case 3:
            this.a = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid Vector3 index!");
        }
      }
    }

    public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
    {
      if ((double) rgbColor.b > (double) rgbColor.g && (double) rgbColor.b > (double) rgbColor.r)
        Color.RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
      else if ((double) rgbColor.g > (double) rgbColor.r)
        Color.RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
      else
        Color.RGBToHSVHelper(0.0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
    }

    private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
    {
      V = dominantcolor;
      if ((double) V != 0.0)
      {
        float num1 = (double) colorone <= (double) colortwo ? colorone : colortwo;
        float num2 = V - num1;
        if ((double) num2 != 0.0)
        {
          S = num2 / V;
          H = offset + (colorone - colortwo) / num2;
        }
        else
        {
          S = 0.0f;
          H = offset + (colorone - colortwo);
        }
        H /= 6f;
        if ((double) H >= 0.0)
          return;
        ++H;
      }
      else
      {
        S = 0.0f;
        H = 0.0f;
      }
    }

    public static Color HSVToRGB(float H, float S, float V)
    {
      return Color.HSVToRGB(H, S, V, true);
    }

    public static Color HSVToRGB(float H, float S, float V, bool hdr)
    {
      Color white = Color.white;
      if ((double) S == 0.0)
      {
        white.r = V;
        white.g = V;
        white.b = V;
      }
      else if ((double) V == 0.0)
      {
        white.r = 0.0f;
        white.g = 0.0f;
        white.b = 0.0f;
      }
      else
      {
        white.r = 0.0f;
        white.g = 0.0f;
        white.b = 0.0f;
        float num1 = S;
        float num2 = V;
        float f = H * 6f;
        int num3 = (int) Mathf.Floor(f);
        float num4 = f - (float) num3;
        float num5 = num2 * (1f - num1);
        float num6 = num2 * (float) (1.0 - (double) num1 * (double) num4);
        float num7 = num2 * (float) (1.0 - (double) num1 * (1.0 - (double) num4));
        switch (num3 + 1)
        {
          case 0:
            white.r = num2;
            white.g = num5;
            white.b = num6;
            break;
          case 1:
            white.r = num2;
            white.g = num7;
            white.b = num5;
            break;
          case 2:
            white.r = num6;
            white.g = num2;
            white.b = num5;
            break;
          case 3:
            white.r = num5;
            white.g = num2;
            white.b = num7;
            break;
          case 4:
            white.r = num5;
            white.g = num6;
            white.b = num2;
            break;
          case 5:
            white.r = num7;
            white.g = num5;
            white.b = num2;
            break;
          case 6:
            white.r = num2;
            white.g = num5;
            white.b = num6;
            break;
          case 7:
            white.r = num2;
            white.g = num7;
            white.b = num5;
            break;
        }
        if (!hdr)
        {
          white.r = Mathf.Clamp(white.r, 0.0f, 1f);
          white.g = Mathf.Clamp(white.g, 0.0f, 1f);
          white.b = Mathf.Clamp(white.b, 0.0f, 1f);
        }
      }
      return white;
    }
  }
}
