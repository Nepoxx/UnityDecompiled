// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ColorBlock
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Structure to store the state of a color transition on a Selectable.</para>
  /// </summary>
  [Serializable]
  public struct ColorBlock : IEquatable<ColorBlock>
  {
    [FormerlySerializedAs("normalColor")]
    [SerializeField]
    private Color m_NormalColor;
    [FormerlySerializedAs("highlightedColor")]
    [FormerlySerializedAs("m_SelectedColor")]
    [SerializeField]
    private Color m_HighlightedColor;
    [FormerlySerializedAs("pressedColor")]
    [SerializeField]
    private Color m_PressedColor;
    [FormerlySerializedAs("disabledColor")]
    [SerializeField]
    private Color m_DisabledColor;
    [Range(1f, 5f)]
    [SerializeField]
    private float m_ColorMultiplier;
    [FormerlySerializedAs("fadeDuration")]
    [SerializeField]
    private float m_FadeDuration;

    /// <summary>
    ///   <para>Normal Color.</para>
    /// </summary>
    public Color normalColor
    {
      get
      {
        return this.m_NormalColor;
      }
      set
      {
        this.m_NormalColor = value;
      }
    }

    /// <summary>
    ///   <para>Highlighted Color.</para>
    /// </summary>
    public Color highlightedColor
    {
      get
      {
        return this.m_HighlightedColor;
      }
      set
      {
        this.m_HighlightedColor = value;
      }
    }

    /// <summary>
    ///   <para>Pressed Color.</para>
    /// </summary>
    public Color pressedColor
    {
      get
      {
        return this.m_PressedColor;
      }
      set
      {
        this.m_PressedColor = value;
      }
    }

    /// <summary>
    ///   <para>Disabled Color.</para>
    /// </summary>
    public Color disabledColor
    {
      get
      {
        return this.m_DisabledColor;
      }
      set
      {
        this.m_DisabledColor = value;
      }
    }

    /// <summary>
    ///   <para>Multiplier applied to colors (allows brightening greater then base color).</para>
    /// </summary>
    public float colorMultiplier
    {
      get
      {
        return this.m_ColorMultiplier;
      }
      set
      {
        this.m_ColorMultiplier = value;
      }
    }

    /// <summary>
    ///   <para>How long a color transition should take.</para>
    /// </summary>
    public float fadeDuration
    {
      get
      {
        return this.m_FadeDuration;
      }
      set
      {
        this.m_FadeDuration = value;
      }
    }

    /// <summary>
    ///   <para>Simple getter for the default ColorBlock.</para>
    /// </summary>
    public static ColorBlock defaultColorBlock
    {
      get
      {
        return new ColorBlock() { m_NormalColor = (Color) new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), m_HighlightedColor = (Color) new Color32((byte) 245, (byte) 245, (byte) 245, byte.MaxValue), m_PressedColor = (Color) new Color32((byte) 200, (byte) 200, (byte) 200, byte.MaxValue), m_DisabledColor = (Color) new Color32((byte) 200, (byte) 200, (byte) 200, (byte) 128), colorMultiplier = 1f, fadeDuration = 0.1f };
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is ColorBlock))
        return false;
      return this.Equals((ColorBlock) obj);
    }

    public bool Equals(ColorBlock other)
    {
      return this.normalColor == other.normalColor && this.highlightedColor == other.highlightedColor && (this.pressedColor == other.pressedColor && this.disabledColor == other.disabledColor) && (double) this.colorMultiplier == (double) other.colorMultiplier && (double) this.fadeDuration == (double) other.fadeDuration;
    }

    public static bool operator ==(ColorBlock point1, ColorBlock point2)
    {
      return point1.Equals(point2);
    }

    public static bool operator !=(ColorBlock point1, ColorBlock point2)
    {
      return !point1.Equals(point2);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
