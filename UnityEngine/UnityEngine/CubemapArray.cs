// Decompiled with JetBrains decompiler
// Type: UnityEngine.CubemapArray
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for handling Cubemap arrays.</para>
  /// </summary>
  public sealed class CubemapArray : Texture
  {
    /// <summary>
    ///   <para>Create a new cubemap array.</para>
    /// </summary>
    /// <param name="faceSize">Cubemap face size in pixels.</param>
    /// <param name="cubemapCount">Number of elements in the cubemap array.</param>
    /// <param name="format">Format of the pixel data.</param>
    /// <param name="mipmap">Should mipmaps be created?</param>
    /// <param name="linear">Does the texture contain non-color data (i.e. don't do any color space conversions when sampling)? Default is false.</param>
    public CubemapArray(int faceSize, int cubemapCount, TextureFormat format, bool mipmap)
    {
      CubemapArray.Internal_Create(this, faceSize, cubemapCount, format, mipmap, false);
    }

    /// <summary>
    ///   <para>Create a new cubemap array.</para>
    /// </summary>
    /// <param name="faceSize">Cubemap face size in pixels.</param>
    /// <param name="cubemapCount">Number of elements in the cubemap array.</param>
    /// <param name="format">Format of the pixel data.</param>
    /// <param name="mipmap">Should mipmaps be created?</param>
    /// <param name="linear">Does the texture contain non-color data (i.e. don't do any color space conversions when sampling)? Default is false.</param>
    public CubemapArray(int faceSize, int cubemapCount, TextureFormat format, bool mipmap, bool linear)
    {
      CubemapArray.Internal_Create(this, faceSize, cubemapCount, format, mipmap, linear);
    }

    /// <summary>
    ///   <para>Number of cubemaps in the array (Read Only).</para>
    /// </summary>
    public extern int cubemapCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Texture format (Read Only).</para>
    /// </summary>
    public extern TextureFormat format { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Actually apply all previous SetPixels changes.</para>
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

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] CubemapArray mono, int faceSize, int cubemapCount, TextureFormat format, bool mipmap, bool linear);

    /// <summary>
    ///   <para>Set pixel colors for a single array slice/face.</para>
    /// </summary>
    /// <param name="colors">An array of pixel colors.</param>
    /// <param name="face">Cubemap face to set pixels for.</param>
    /// <param name="arrayElement">Array element index to set pixels for.</param>
    /// <param name="miplevel">Mipmap level to set pixels for.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels(Color[] colors, CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels(Color[] colors, CubemapFace face, int arrayElement)
    {
      int miplevel = 0;
      this.SetPixels(colors, face, arrayElement, miplevel);
    }

    /// <summary>
    ///   <para>Set pixel colors for a single array slice/face.</para>
    /// </summary>
    /// <param name="colors">An array of pixel colors in low precision (8 bits/channel) format.</param>
    /// <param name="face">Cubemap face to set pixels for.</param>
    /// <param name="arrayElement">Array element index to set pixels for.</param>
    /// <param name="miplevel">Mipmap level to set pixels for.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement)
    {
      int miplevel = 0;
      this.SetPixels32(colors, face, arrayElement, miplevel);
    }

    /// <summary>
    ///   <para>Returns pixel colors of a single array slice/face.</para>
    /// </summary>
    /// <param name="face">Cubemap face to read pixels from.</param>
    /// <param name="arrayElement">Array slice to read pixels from.</param>
    /// <param name="miplevel">Mipmap level to read pixels from.</param>
    /// <returns>
    ///   <para>Array of pixel colors.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color[] GetPixels(CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color[] GetPixels(CubemapFace face, int arrayElement)
    {
      int miplevel = 0;
      return this.GetPixels(face, arrayElement, miplevel);
    }

    /// <summary>
    ///   <para>Returns pixel colors of a single array slice/face.</para>
    /// </summary>
    /// <param name="face">Cubemap face to read pixels from.</param>
    /// <param name="arrayElement">Array slice to read pixels from.</param>
    /// <param name="miplevel">Mipmap level to read pixels from.</param>
    /// <returns>
    ///   <para>Array of pixel colors in low precision (8 bits/channel) format.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color32[] GetPixels32(CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color32[] GetPixels32(CubemapFace face, int arrayElement)
    {
      int miplevel = 0;
      return this.GetPixels32(face, arrayElement, miplevel);
    }
  }
}
