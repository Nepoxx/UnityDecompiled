// Decompiled with JetBrains decompiler
// Type: UnityEngine.Texture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for texture handling. Contains functionality that is common to both Texture2D and RenderTexture classes.</para>
  /// </summary>
  [UsedByNativeCode]
  public class Texture : Object
  {
    public static extern int masterTextureLimit { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern AnisotropicFiltering anisotropicFiltering { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets Anisotropic limits.</para>
    /// </summary>
    /// <param name="forcedMin"></param>
    /// <param name="globalMax"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetWidth(Texture t);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetHeight(Texture t);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern TextureDimension Internal_GetDimension(Texture t);

    /// <summary>
    ///   <para>Width of the texture in pixels. (Read Only)</para>
    /// </summary>
    public virtual int width
    {
      get
      {
        return Texture.Internal_GetWidth(this);
      }
      set
      {
        throw new Exception("not implemented");
      }
    }

    /// <summary>
    ///   <para>Height of the texture in pixels. (Read Only)</para>
    /// </summary>
    public virtual int height
    {
      get
      {
        return Texture.Internal_GetHeight(this);
      }
      set
      {
        throw new Exception("not implemented");
      }
    }

    /// <summary>
    ///   <para>Dimensionality (type) of the texture (Read Only).</para>
    /// </summary>
    public virtual TextureDimension dimension
    {
      get
      {
        return Texture.Internal_GetDimension(this);
      }
      set
      {
        throw new Exception("not implemented");
      }
    }

    /// <summary>
    ///   <para>Filtering mode of the texture.</para>
    /// </summary>
    public extern FilterMode filterMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Anisotropic filtering level of the texture.</para>
    /// </summary>
    public extern int anisoLevel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture coordinate wrapping mode.</para>
    /// </summary>
    public extern TextureWrapMode wrapMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture U coordinate wrapping mode.</para>
    /// </summary>
    public extern TextureWrapMode wrapModeU { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture V coordinate wrapping mode.</para>
    /// </summary>
    public extern TextureWrapMode wrapModeV { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture W coordinate wrapping mode for Texture3D.</para>
    /// </summary>
    public extern TextureWrapMode wrapModeW { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip map bias of the texture.</para>
    /// </summary>
    public extern float mipMapBias { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector2 texelSize
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_texelSize(out vector2);
        return vector2;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_texelSize(out Vector2 value);

    /// <summary>
    ///   <para>Retrieve a native (underlying graphics API) pointer to the texture resource.</para>
    /// </summary>
    /// <returns>
    ///   <para>Pointer to an underlying graphics API texture resource.</para>
    /// </returns>
    public IntPtr GetNativeTexturePtr()
    {
      IntPtr num;
      Texture.INTERNAL_CALL_GetNativeTexturePtr(this, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetNativeTexturePtr(Texture self, out IntPtr value);

    [Obsolete("Use GetNativeTexturePtr instead.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetNativeTextureID();

    /// <summary>
    ///   <para>The hash value of the Texture.</para>
    /// </summary>
    public Hash128 imageContentsHash
    {
      get
      {
        Hash128 hash128;
        this.INTERNAL_get_imageContentsHash(out hash128);
        return hash128;
      }
      set
      {
        this.INTERNAL_set_imageContentsHash(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_imageContentsHash(out Hash128 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_imageContentsHash(ref Hash128 value);
  }
}
