// Decompiled with JetBrains decompiler
// Type: UnityEngine.SpriteRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Renders a Sprite for 2D graphics.</para>
  /// </summary>
  [RequireComponent(typeof (Transform))]
  public sealed class SpriteRenderer : Renderer
  {
    /// <summary>
    ///   <para>The Sprite to render.</para>
    /// </summary>
    public Sprite sprite
    {
      get
      {
        return this.GetSprite_INTERNAL();
      }
      set
      {
        this.SetSprite_INTERNAL(value);
      }
    }

    /// <summary>
    ///   <para>The current draw mode of the Sprite Renderer.</para>
    /// </summary>
    public extern SpriteDrawMode drawMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal extern bool shouldSupportTiling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property to set/get the size to render when the SpriteRenderer.drawMode is set to SpriteDrawMode.NineSlice.</para>
    /// </summary>
    public Vector2 size
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_size(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_size(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_size(out Vector2 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_size(ref Vector2 value);

    /// <summary>
    ///   <para>The current threshold for Sprite Renderer tiling.</para>
    /// </summary>
    public extern float adaptiveModeThreshold { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current tile mode of the Sprite Renderer.</para>
    /// </summary>
    public extern SpriteTileMode tileMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Sprite GetSprite_INTERNAL();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetSprite_INTERNAL(Sprite sprite);

    /// <summary>
    ///   <para>Rendering color for the Sprite graphic.</para>
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
    ///   <para>Flips the sprite on the X axis.</para>
    /// </summary>
    public extern bool flipX { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Flips the sprite on the Y axis.</para>
    /// </summary>
    public extern bool flipY { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Specifies how the sprite interacts with the masks.</para>
    /// </summary>
    public extern SpriteMaskInteraction maskInteraction { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal Bounds GetSpriteBounds()
    {
      Bounds bounds;
      SpriteRenderer.INTERNAL_CALL_GetSpriteBounds(this, out bounds);
      return bounds;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSpriteBounds(SpriteRenderer self, out Bounds value);
  }
}
