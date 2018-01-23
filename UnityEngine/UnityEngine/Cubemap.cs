// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cubemap
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for handling cube maps, Use this to create or modify existing.</para>
  /// </summary>
  public sealed class Cubemap : Texture
  {
    /// <summary>
    ///   <para>Create a new empty cubemap texture.</para>
    /// </summary>
    /// <param name="size">Width/height of a cube face in pixels.</param>
    /// <param name="format">Pixel data format to be used for the Cubemap.</param>
    /// <param name="mipmap">Should mipmaps be created?</param>
    public Cubemap(int size, TextureFormat format, bool mipmap)
    {
      Cubemap.Internal_Create(this, size, format, mipmap, IntPtr.Zero);
    }

    internal Cubemap(int size, TextureFormat format, bool mipmap, IntPtr nativeTex)
    {
      Cubemap.Internal_Create(this, size, format, mipmap, nativeTex);
    }

    /// <summary>
    ///   <para>Sets pixel color at coordinates (face, x, y).</para>
    /// </summary>
    /// <param name="face"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public void SetPixel(CubemapFace face, int x, int y, Color color)
    {
      Cubemap.INTERNAL_CALL_SetPixel(this, face, x, y, ref color);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetPixel(Cubemap self, CubemapFace face, int x, int y, ref Color color);

    /// <summary>
    ///   <para>Returns pixel color at coordinates (face, x, y).</para>
    /// </summary>
    /// <param name="face"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Color GetPixel(CubemapFace face, int x, int y)
    {
      Color color;
      Cubemap.INTERNAL_CALL_GetPixel(this, face, x, y, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPixel(Cubemap self, CubemapFace face, int x, int y, out Color value);

    /// <summary>
    ///   <para>Returns pixel colors of a cubemap face.</para>
    /// </summary>
    /// <param name="face">The face from which pixel data is taken.</param>
    /// <param name="miplevel">Mipmap level for the chosen face.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color[] GetPixels(CubemapFace face, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color[] GetPixels(CubemapFace face)
    {
      int miplevel = 0;
      return this.GetPixels(face, miplevel);
    }

    /// <summary>
    ///   <para>Sets pixel colors of a cubemap face.</para>
    /// </summary>
    /// <param name="colors">Pixel data for the Cubemap face.</param>
    /// <param name="face">The face to which the new data should be applied.</param>
    /// <param name="miplevel">The mipmap level for the face.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels(Color[] colors, CubemapFace face, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels(Color[] colors, CubemapFace face)
    {
      int miplevel = 0;
      this.SetPixels(colors, face, miplevel);
    }

    /// <summary>
    ///   <para>How many mipmap levels are in this texture (Read Only).</para>
    /// </summary>
    public extern int mipmapCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Actually apply all previous SetPixel and SetPixels changes.</para>
    /// </summary>
    /// <param name="updateMipmaps">When set to true, mipmap levels are recalculated.</param>
    /// <param name="makeNoLongerReadable">When set to true, system memory copy of a texture is released.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable);

    [ExcludeFromDocs]
    public void Apply(bool updateMipmaps)
    {
      bool makeNoLongerReadable = false;
      this.Apply(updateMipmaps, makeNoLongerReadable);
    }

    [ExcludeFromDocs]
    public void Apply()
    {
      this.Apply(true, false);
    }

    /// <summary>
    ///   <para>The format of the pixel data in the texture (Read Only).</para>
    /// </summary>
    public extern TextureFormat format { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Creates a Unity cubemap out of externally created native cubemap object.</para>
    /// </summary>
    /// <param name="size">The width and height of each face of the cubemap should be the same.</param>
    /// <param name="format">Format of underlying cubemap object.</param>
    /// <param name="mipmap">Does the cubemap have mipmaps?</param>
    /// <param name="nativeTex">Native cubemap texture object.</param>
    public static Cubemap CreateExternalTexture(int size, TextureFormat format, bool mipmap, IntPtr nativeTex)
    {
      if (nativeTex == IntPtr.Zero)
        throw new ArgumentException("nativeTex can not be null");
      return new Cubemap(size, format, mipmap, nativeTex);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] Cubemap mono, int size, TextureFormat format, bool mipmap, IntPtr nativeTex);

    /// <summary>
    ///   <para>Performs smoothing of near edge regions.</para>
    /// </summary>
    /// <param name="smoothRegionWidthInPixels">Pixel distance at edges over which to apply smoothing.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SmoothEdges([DefaultValue("1")] int smoothRegionWidthInPixels);

    [ExcludeFromDocs]
    public void SmoothEdges()
    {
      this.SmoothEdges(1);
    }
  }
}
