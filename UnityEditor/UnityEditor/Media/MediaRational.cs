// Decompiled with JetBrains decompiler
// Type: UnityEditor.Media.MediaRational
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Media
{
  /// <summary>
  ///   <para>Rational number useful for expressing fractions precisely.</para>
  /// </summary>
  public struct MediaRational
  {
    /// <summary>
    ///   <para>Fraction numerator.</para>
    /// </summary>
    public int numerator;
    /// <summary>
    ///   <para>Fraction denominator.</para>
    /// </summary>
    public int denominator;

    /// <summary>
    ///   <para>Constructs a rational number whose denominator is 1.</para>
    /// </summary>
    /// <param name="num">Numerator. Will also become the rational value as the denominator is set to 1.</param>
    public MediaRational(int num)
    {
      this.numerator = num;
      this.denominator = 1;
    }
  }
}
