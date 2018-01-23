// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUITexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A texture image used in a 2D GUI.</para>
  /// </summary>
  [Obsolete("This component is part of the legacy UI system and will be removed in a future release.")]
  public sealed class GUITexture : GUIElement
  {
    /// <summary>
    ///   <para>The color of the GUI texture.</para>
    /// </summary>
    public Color color
    {
      get
      {
        Color color;
        this.INTERNAL_get_color(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_color(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_color(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_color(ref Color value);

    /// <summary>
    ///   <para>The texture used for drawing.</para>
    /// </summary>
    public extern Texture texture { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Pixel inset used for pixel adjustments for size and position.</para>
    /// </summary>
    public Rect pixelInset
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_pixelInset(out rect);
        return rect;
      }
      set
      {
        this.INTERNAL_set_pixelInset(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_pixelInset(out Rect value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_pixelInset(ref Rect value);

    /// <summary>
    ///   <para>The border defines the number of pixels from the edge that are not affected by scale.</para>
    /// </summary>
    public extern RectOffset border { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
