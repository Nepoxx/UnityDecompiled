// Decompiled with JetBrains decompiler
// Type: UnityEngine.Texture2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for texture handling.</para>
  /// </summary>
  public sealed class Texture2D : Texture
  {
    /// <summary>
    ///   <para>Create a new empty texture.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Texture2D(int width, int height)
    {
      Texture2D.Internal_Create(this, width, height, TextureFormat.RGBA32, true, false, IntPtr.Zero);
    }

    /// <summary>
    ///   <para>Create a new empty texture.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <param name="mipmap"></param>
    public Texture2D(int width, int height, TextureFormat format, bool mipmap)
    {
      Texture2D.Internal_Create(this, width, height, format, mipmap, false, IntPtr.Zero);
    }

    /// <summary>
    ///   <para>Create a new empty texture.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <param name="mipmap"></param>
    /// <param name="linear"></param>
    public Texture2D(int width, int height, TextureFormat format, bool mipmap, bool linear)
    {
      Texture2D.Internal_Create(this, width, height, format, mipmap, linear, IntPtr.Zero);
    }

    internal Texture2D(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
    {
      Texture2D.Internal_Create(this, width, height, format, mipmap, linear, nativeTex);
    }

    /// <summary>
    ///   <para>How many mipmap levels are in this texture (Read Only).</para>
    /// </summary>
    public extern int mipmapCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] Texture2D mono, int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex);

    /// <summary>
    ///   <para>Creates Unity Texture out of externally created native texture object.</para>
    /// </summary>
    /// <param name="nativeTex">Native 2D texture object.</param>
    /// <param name="width">Width of texture in pixels.</param>
    /// <param name="height">Height of texture in pixels.</param>
    /// <param name="format">Format of underlying texture object.</param>
    /// <param name="mipmap">Does the texture have mipmaps?</param>
    /// <param name="linear">Is texture using linear color space?</param>
    public static Texture2D CreateExternalTexture(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
    {
      if (nativeTex == IntPtr.Zero)
        throw new ArgumentException("nativeTex can not be null");
      return new Texture2D(width, height, format, mipmap, linear, nativeTex);
    }

    /// <summary>
    ///   <para>Updates Unity texture to use different native texture object.</para>
    /// </summary>
    /// <param name="nativeTex">Native 2D texture object.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void UpdateExternalTexture(IntPtr nativeTex);

    /// <summary>
    ///   <para>The format of the pixel data in the texture (Read Only).</para>
    /// </summary>
    public extern TextureFormat format { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get a small texture with all white pixels.</para>
    /// </summary>
    public static extern Texture2D whiteTexture { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get a small texture with all black pixels.</para>
    /// </summary>
    public static extern Texture2D blackTexture { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Sets pixel color at coordinates (x,y).</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public void SetPixel(int x, int y, Color color)
    {
      Texture2D.INTERNAL_CALL_SetPixel(this, x, y, ref color);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetPixel(Texture2D self, int x, int y, ref Color color);

    /// <summary>
    ///   <para>Returns pixel color at coordinates (x, y).</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Color GetPixel(int x, int y)
    {
      Color color;
      Texture2D.INTERNAL_CALL_GetPixel(this, x, y, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPixel(Texture2D self, int x, int y, out Color value);

    /// <summary>
    ///   <para>Returns filtered pixel color at normalized coordinates (u, v).</para>
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    public Color GetPixelBilinear(float u, float v)
    {
      Color color;
      Texture2D.INTERNAL_CALL_GetPixelBilinear(this, u, v, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPixelBilinear(Texture2D self, float u, float v, out Color value);

    [ExcludeFromDocs]
    public void SetPixels(Color[] colors)
    {
      int miplevel = 0;
      this.SetPixels(colors, miplevel);
    }

    /// <summary>
    ///   <para>Set a block of pixel colors.</para>
    /// </summary>
    /// <param name="colors">The array of pixel colours to assign (a 2D image flattened to a 1D array).</param>
    /// <param name="miplevel">The mip level of the texture to write to.</param>
    public void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel)
    {
      int blockWidth = this.width >> miplevel;
      if (blockWidth < 1)
        blockWidth = 1;
      int blockHeight = this.height >> miplevel;
      if (blockHeight < 1)
        blockHeight = 1;
      this.SetPixels(0, 0, blockWidth, blockHeight, colors, miplevel);
    }

    /// <summary>
    ///   <para>Set a block of pixel colors.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="blockWidth"></param>
    /// <param name="blockHeight"></param>
    /// <param name="colors"></param>
    /// <param name="miplevel"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors)
    {
      int miplevel = 0;
      this.SetPixels(x, y, blockWidth, blockHeight, colors, miplevel);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetAllPixels32(Color32[] colors, int miplevel);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetBlockOfPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, int miplevel);

    [ExcludeFromDocs]
    public void SetPixels32(Color32[] colors)
    {
      int miplevel = 0;
      this.SetPixels32(colors, miplevel);
    }

    /// <summary>
    ///   <para>Set a block of pixel colors.</para>
    /// </summary>
    /// <param name="colors"></param>
    /// <param name="miplevel"></param>
    public void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel)
    {
      this.SetAllPixels32(colors, miplevel);
    }

    [ExcludeFromDocs]
    public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors)
    {
      int miplevel = 0;
      this.SetPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
    }

    /// <summary>
    ///   <para>Set a block of pixel colors.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="blockWidth"></param>
    /// <param name="blockHeight"></param>
    /// <param name="colors"></param>
    /// <param name="miplevel"></param>
    public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, [DefaultValue("0")] int miplevel)
    {
      this.SetBlockOfPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void LoadRawTextureData_ImplArray(byte[] data);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void LoadRawTextureData_ImplPointer(IntPtr data, int size);

    /// <summary>
    ///   <para>Fills texture pixels with raw preformatted data.</para>
    /// </summary>
    /// <param name="data">Byte array to initialize texture pixels with.</param>
    /// <param name="size">Size of data in bytes.</param>
    public void LoadRawTextureData(byte[] data)
    {
      this.LoadRawTextureData_ImplArray(data);
    }

    /// <summary>
    ///   <para>Fills texture pixels with raw preformatted data.</para>
    /// </summary>
    /// <param name="data">Byte array to initialize texture pixels with.</param>
    /// <param name="size">Size of data in bytes.</param>
    public void LoadRawTextureData(IntPtr data, int size)
    {
      this.LoadRawTextureData_ImplPointer(data, size);
    }

    /// <summary>
    ///   <para>Get raw data from a texture.</para>
    /// </summary>
    /// <returns>
    ///   <para>Raw texture data as a byte array.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern byte[] GetRawTextureData();

    [ExcludeFromDocs]
    public Color[] GetPixels()
    {
      return this.GetPixels(0);
    }

    /// <summary>
    ///   <para>Get the pixel colors from the texture.</para>
    /// </summary>
    /// <param name="miplevel">The mipmap level to fetch the pixels from. Defaults to zero.</param>
    /// <returns>
    ///   <para>The array of all pixels in the mipmap level of the texture.</para>
    /// </returns>
    public Color[] GetPixels([DefaultValue("0")] int miplevel)
    {
      int blockWidth = this.width >> miplevel;
      if (blockWidth < 1)
        blockWidth = 1;
      int blockHeight = this.height >> miplevel;
      if (blockHeight < 1)
        blockHeight = 1;
      return this.GetPixels(0, 0, blockWidth, blockHeight, miplevel);
    }

    /// <summary>
    ///   <para>Get a block of pixel colors.</para>
    /// </summary>
    /// <param name="x">The x position of the pixel array to fetch.</param>
    /// <param name="y">The y position of the pixel array to fetch.</param>
    /// <param name="blockWidth">The width length of the pixel array to fetch.</param>
    /// <param name="blockHeight">The height length of the pixel array to fetch.</param>
    /// <param name="miplevel">The mipmap level to fetch the pixels. Defaults to zero, and is
    /// optional.</param>
    /// <returns>
    ///   <para>The array of pixels in the texture that have been selected.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
    {
      int miplevel = 0;
      return this.GetPixels(x, y, blockWidth, blockHeight, miplevel);
    }

    /// <summary>
    ///   <para>Get a block of pixel colors in Color32 format.</para>
    /// </summary>
    /// <param name="miplevel"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color32[] GetPixels32([DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color32[] GetPixels32()
    {
      return this.GetPixels32(0);
    }

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
    ///   <para>Resizes the texture.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <param name="hasMipMap"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool Resize(int width, int height, TextureFormat format, bool hasMipMap);

    /// <summary>
    ///   <para>Resizes the texture.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public bool Resize(int width, int height)
    {
      return this.Internal_ResizeWH(width, height);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_ResizeWH(int width, int height);

    /// <summary>
    ///   <para>Compress texture into DXT format.</para>
    /// </summary>
    /// <param name="highQuality"></param>
    public void Compress(bool highQuality)
    {
      Texture2D.INTERNAL_CALL_Compress(this, highQuality);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Compress(Texture2D self, bool highQuality);

    /// <summary>
    ///   <para>Packs multiple Textures into a texture atlas.</para>
    /// </summary>
    /// <param name="textures">Array of textures to pack into the atlas.</param>
    /// <param name="padding">Padding in pixels between the packed textures.</param>
    /// <param name="maximumAtlasSize">Maximum size of the resulting texture.</param>
    /// <param name="makeNoLongerReadable">Should the texture be marked as no longer readable?</param>
    /// <returns>
    ///   <para>An array of rectangles containing the UV coordinates in the atlas for each input texture, or null if packing fails.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Rect[] PackTextures(Texture2D[] textures, int padding, [DefaultValue("2048")] int maximumAtlasSize, [DefaultValue("false")] bool makeNoLongerReadable);

    [ExcludeFromDocs]
    public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize)
    {
      bool makeNoLongerReadable = false;
      return this.PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
    }

    [ExcludeFromDocs]
    public Rect[] PackTextures(Texture2D[] textures, int padding)
    {
      bool makeNoLongerReadable = false;
      int maximumAtlasSize = 2048;
      return this.PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
    }

    public static bool GenerateAtlas(Vector2[] sizes, int padding, int atlasSize, List<Rect> results)
    {
      if (sizes == null)
        throw new ArgumentException("sizes array can not be null");
      if (results == null)
        throw new ArgumentException("results list cannot be null");
      if (padding < 0)
        throw new ArgumentException("padding can not be negative");
      if (atlasSize <= 0)
        throw new ArgumentException("atlas size must be positive");
      results.Clear();
      if (sizes.Length == 0)
        return true;
      Texture2D.GenerateAtlasInternal(sizes, padding, atlasSize, (object) results);
      return results.Count != 0;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GenerateAtlasInternal(Vector2[] sizes, int padding, int atlasSize, object resultList);

    /// <summary>
    ///   <para>Read pixels from screen into the saved texture data.</para>
    /// </summary>
    /// <param name="source">Rectangular region of the view to read from. Pixels are read from current render target.</param>
    /// <param name="destX">Horizontal pixel position in the texture to place the pixels that are read.</param>
    /// <param name="destY">Vertical pixel position in the texture to place the pixels that are read.</param>
    /// <param name="recalculateMipMaps">Should the texture's mipmaps be recalculated after reading?</param>
    public void ReadPixels(Rect source, int destX, int destY, [DefaultValue("true")] bool recalculateMipMaps)
    {
      Texture2D.INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
    }

    [ExcludeFromDocs]
    public void ReadPixels(Rect source, int destX, int destY)
    {
      bool recalculateMipMaps = true;
      Texture2D.INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ReadPixels(Texture2D self, ref Rect source, int destX, int destY, bool recalculateMipMaps);

    public extern bool alphaIsTransparency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Flags used to control the encoding to an EXR file.</para>
    /// </summary>
    [Flags]
    public enum EXRFlags
    {
      None = 0,
      OutputAsFloat = 1,
      CompressZIP = 2,
      CompressRLE = 4,
      CompressPIZ = 8,
    }
  }
}
