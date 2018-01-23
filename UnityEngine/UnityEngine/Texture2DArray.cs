// Decompiled with JetBrains decompiler
// Type: UnityEngine.Texture2DArray
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for handling 2D texture arrays.</para>
  /// </summary>
  public sealed class Texture2DArray : Texture
  {
    /// <summary>
    ///   <para>Create a new texture array.</para>
    /// </summary>
    /// <param name="width">Width of texture array in pixels.</param>
    /// <param name="height">Height of texture array in pixels.</param>
    /// <param name="depth">Number of elements in the texture array.</param>
    /// <param name="format">Format of the texture.</param>
    /// <param name="mipmap">Should mipmaps be created?</param>
    /// <param name="linear">Does the texture contain non-color data (i.e. don't do any color space conversions when sampling)? Default is false.</param>
    public Texture2DArray(int width, int height, int depth, TextureFormat format, bool mipmap)
    {
      Texture2DArray.Internal_Create(this, width, height, depth, format, mipmap, false);
    }

    /// <summary>
    ///   <para>Create a new texture array.</para>
    /// </summary>
    /// <param name="width">Width of texture array in pixels.</param>
    /// <param name="height">Height of texture array in pixels.</param>
    /// <param name="depth">Number of elements in the texture array.</param>
    /// <param name="format">Format of the texture.</param>
    /// <param name="mipmap">Should mipmaps be created?</param>
    /// <param name="linear">Does the texture contain non-color data (i.e. don't do any color space conversions when sampling)? Default is false.</param>
    public Texture2DArray(int width, int height, int depth, TextureFormat format, bool mipmap, bool linear)
    {
      Texture2DArray.Internal_Create(this, width, height, depth, format, mipmap, linear);
    }

    /// <summary>
    ///   <para>Number of elements in a texture array (Read Only).</para>
    /// </summary>
    public extern int depth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

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
    private static extern void Internal_Create([Writable] Texture2DArray mono, int width, int height, int depth, TextureFormat format, bool mipmap, bool linear);

    /// <summary>
    ///   <para>Set pixel colors for the whole mip level.</para>
    /// </summary>
    /// <param name="colors">An array of pixel colors.</param>
    /// <param name="arrayElement">The texture array element index.</param>
    /// <param name="miplevel">The mip level.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels(Color[] colors, int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels(Color[] colors, int arrayElement)
    {
      int miplevel = 0;
      this.SetPixels(colors, arrayElement, miplevel);
    }

    /// <summary>
    ///   <para>Set pixel colors for the whole mip level.</para>
    /// </summary>
    /// <param name="colors">An array of pixel colors.</param>
    /// <param name="arrayElement">The texture array element index.</param>
    /// <param name="miplevel">The mip level.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels32(Color32[] colors, int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels32(Color32[] colors, int arrayElement)
    {
      int miplevel = 0;
      this.SetPixels32(colors, arrayElement, miplevel);
    }

    /// <summary>
    ///   <para>Returns pixel colors of a single array slice.</para>
    /// </summary>
    /// <param name="arrayElement">Array slice to read pixels from.</param>
    /// <param name="miplevel">Mipmap level to read pixels from.</param>
    /// <returns>
    ///   <para>Array of pixel colors.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color[] GetPixels(int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color[] GetPixels(int arrayElement)
    {
      int miplevel = 0;
      return this.GetPixels(arrayElement, miplevel);
    }

    /// <summary>
    ///   <para>Returns pixel colors of a single array slice.</para>
    /// </summary>
    /// <param name="arrayElement">Array slice to read pixels from.</param>
    /// <param name="miplevel">Mipmap level to read pixels from.</param>
    /// <returns>
    ///   <para>Array of pixel colors in low precision (8 bits/channel) format.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color32[] GetPixels32(int arrayElement, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color32[] GetPixels32(int arrayElement)
    {
      int miplevel = 0;
      return this.GetPixels32(arrayElement, miplevel);
    }
  }
}
