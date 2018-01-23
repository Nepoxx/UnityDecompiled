// Decompiled with JetBrains decompiler
// Type: UnityEngine.Accessibility.VisionUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace UnityEngine.Accessibility
{
  /// <summary>
  ///   <para>A class containing methods to assist with accessibility for users with different vision capabilities.</para>
  /// </summary>
  [UsedByNativeCode]
  public static class VisionUtility
  {
    private static readonly Color[] s_ColorBlindSafePalette = new Color[11]{ (Color) new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue), (Color) new Color32((byte) 73, (byte) 0, (byte) 146, byte.MaxValue), (Color) new Color32((byte) 7, (byte) 71, (byte) 81, byte.MaxValue), (Color) new Color32((byte) 0, (byte) 146, (byte) 146, byte.MaxValue), (Color) new Color32((byte) 182, (byte) 109, byte.MaxValue, byte.MaxValue), (Color) new Color32(byte.MaxValue, (byte) 109, (byte) 182, byte.MaxValue), (Color) new Color32((byte) 109, (byte) 182, byte.MaxValue, byte.MaxValue), (Color) new Color32((byte) 36, byte.MaxValue, (byte) 36, byte.MaxValue), (Color) new Color32(byte.MaxValue, (byte) 182, (byte) 219, byte.MaxValue), (Color) new Color32((byte) 182, (byte) 219, byte.MaxValue, byte.MaxValue), (Color) new Color32(byte.MaxValue, byte.MaxValue, (byte) 109, byte.MaxValue) };
    private static readonly float[] s_ColorBlindSafePaletteLuminanceValues = ((IEnumerable<Color>) VisionUtility.s_ColorBlindSafePalette).Select<Color, float>((Func<Color, float>) (c => VisionUtility.ComputePerceivedLuminance(c))).ToArray<float>();

    private static float ComputePerceivedLuminance(Color color)
    {
      color = color.linear;
      return Mathf.LinearToGammaSpace((float) (0.212599992752075 * (double) color.r + 0.715200006961823 * (double) color.g + 0.0722000002861023 * (double) color.b));
    }

    /// <summary>
    ///   <para>Gets a palette of colors that should be distinguishable for normal vision, deuteranopia, protanopia, and tritanopia.</para>
    /// </summary>
    /// <param name="palette">An array of colors to populate with a palette.</param>
    /// <param name="minimumLuminance">Minimum allowable perceived luminance from 0 to 1. A value of 0.2 or greater is recommended for dark backgrounds.</param>
    /// <param name="maximumLuminance">Maximum allowable perceived luminance from 0 to 1. A value of 0.8 or less is recommended for light backgrounds.</param>
    /// <returns>
    ///   <para>The number of unambiguous colors in the palette.</para>
    /// </returns>
    public static int GetColorBlindSafePalette(Color[] palette, float minimumLuminance, float maximumLuminance)
    {
      if (palette == null)
        throw new ArgumentNullException(nameof (palette));
      Color[] array = Enumerable.Range(0, VisionUtility.s_ColorBlindSafePalette.Length).Where<int>((Func<int, bool>) (i => (double) VisionUtility.s_ColorBlindSafePaletteLuminanceValues[i] >= (double) minimumLuminance && (double) VisionUtility.s_ColorBlindSafePaletteLuminanceValues[i] <= (double) maximumLuminance)).Select<int, Color>((Func<int, Color>) (i => VisionUtility.s_ColorBlindSafePalette[i])).ToArray<Color>();
      int num = Mathf.Min(palette.Length, array.Length);
      if (num > 0)
      {
        int index = 0;
        for (int length = palette.Length; index < length; ++index)
          palette[index] = array[index % num];
      }
      else
      {
        int index = 0;
        for (int length = palette.Length; index < length; ++index)
          palette[index] = new Color();
      }
      return num;
    }
  }
}
