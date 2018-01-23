// Decompiled with JetBrains decompiler
// Type: UnityEngine.Texture3D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for handling 3D Textures, Use this to create.</para>
  /// </summary>
  public sealed class Texture3D : Texture
  {
    /// <summary>
    ///   <para>Create a new empty 3D Texture.</para>
    /// </summary>
    /// <param name="width">Width of texture in pixels.</param>
    /// <param name="height">Height of texture in pixels.</param>
    /// <param name="depth">Depth of texture in pixels.</param>
    /// <param name="format">Texture data format.</param>
    /// <param name="mipmap">Should the texture have mipmaps?</param>
    public Texture3D(int width, int height, int depth, TextureFormat format, bool mipmap)
    {
      Texture3D.Internal_Create(this, width, height, depth, format, mipmap);
    }

    /// <summary>
    ///   <para>The depth of the texture (Read Only).</para>
    /// </summary>
    public extern int depth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns an array of pixel colors representing one mip level of the 3D texture.</para>
    /// </summary>
    /// <param name="miplevel"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color[] GetPixels([DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color[] GetPixels()
    {
      return this.GetPixels(0);
    }

    /// <summary>
    ///   <para>Returns an array of pixel colors representing one mip level of the 3D texture.</para>
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
    ///   <para>Sets pixel colors of a 3D texture.</para>
    /// </summary>
    /// <param name="colors">The colors to set the pixels to.</param>
    /// <param name="miplevel">The mipmap level to be affected by the new colors.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels(Color[] colors)
    {
      int miplevel = 0;
      this.SetPixels(colors, miplevel);
    }

    /// <summary>
    ///   <para>Sets pixel colors of a 3D texture.</para>
    /// </summary>
    /// <param name="colors">The colors to set the pixels to.</param>
    /// <param name="miplevel">The mipmap level to be affected by the new colors.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels32(Color32[] colors)
    {
      int miplevel = 0;
      this.SetPixels32(colors, miplevel);
    }

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

    /// <summary>
    ///   <para>The format of the pixel data in the texture (Read Only).</para>
    /// </summary>
    public extern TextureFormat format { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] Texture3D mono, int width, int height, int depth, TextureFormat format, bool mipmap);
  }
}
