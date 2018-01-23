// Decompiled with JetBrains decompiler
// Type: UnityEngine.ColorUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A collection of common color functions.</para>
  /// </summary>
  public sealed class ColorUtility
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool DoTryParseHtmlColor(string htmlString, out Color32 color);

    public static bool TryParseHtmlString(string htmlString, out Color color)
    {
      Color32 color1;
      bool htmlColor = ColorUtility.DoTryParseHtmlColor(htmlString, out color1);
      color = (Color) color1;
      return htmlColor;
    }

    /// <summary>
    ///   <para>Returns the color as a hexadecimal string in the format "RRGGBB".</para>
    /// </summary>
    /// <param name="color">The color to be converted.</param>
    /// <returns>
    ///   <para>Hexadecimal string representing the color.</para>
    /// </returns>
    public static string ToHtmlStringRGB(Color color)
    {
      Color32 color32 = new Color32((byte) Mathf.Clamp(Mathf.RoundToInt(color.r * (float) byte.MaxValue), 0, (int) byte.MaxValue), (byte) Mathf.Clamp(Mathf.RoundToInt(color.g * (float) byte.MaxValue), 0, (int) byte.MaxValue), (byte) Mathf.Clamp(Mathf.RoundToInt(color.b * (float) byte.MaxValue), 0, (int) byte.MaxValue), (byte) 1);
      return string.Format("{0:X2}{1:X2}{2:X2}", (object) color32.r, (object) color32.g, (object) color32.b);
    }

    /// <summary>
    ///   <para>Returns the color as a hexadecimal string in the format "RRGGBBAA".</para>
    /// </summary>
    /// <param name="color">The color to be converted.</param>
    /// <returns>
    ///   <para>Hexadecimal string representing the color.</para>
    /// </returns>
    public static string ToHtmlStringRGBA(Color color)
    {
      Color32 color32 = new Color32((byte) Mathf.Clamp(Mathf.RoundToInt(color.r * (float) byte.MaxValue), 0, (int) byte.MaxValue), (byte) Mathf.Clamp(Mathf.RoundToInt(color.g * (float) byte.MaxValue), 0, (int) byte.MaxValue), (byte) Mathf.Clamp(Mathf.RoundToInt(color.b * (float) byte.MaxValue), 0, (int) byte.MaxValue), (byte) Mathf.Clamp(Mathf.RoundToInt(color.a * (float) byte.MaxValue), 0, (int) byte.MaxValue));
      return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", (object) color32.r, (object) color32.g, (object) color32.b, (object) color32.a);
    }
  }
}
