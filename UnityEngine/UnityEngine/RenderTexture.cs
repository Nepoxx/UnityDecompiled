// Decompiled with JetBrains decompiler
// Type: UnityEngine.RenderTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Render textures are textures that can be rendered to.</para>
  /// </summary>
  [UsedByNativeCode]
  public class RenderTexture : Texture
  {
    /// <summary>
    ///   <para>Creates a new RenderTexture object.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Texture color format.</param>
    /// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
    /// <param name="desc">Create the RenderTexture with the settings in the RenderTextureDescriptor.</param>
    /// <param name="textureToCopy">Copy the settings from another RenderTexture.</param>
    public RenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite)
    {
      RenderTexture.Internal_CreateRenderTexture(this);
      this.width = width;
      this.height = height;
      this.depth = depth;
      this.format = format;
      bool sRGB = readWrite == RenderTextureReadWrite.sRGB;
      if (readWrite == RenderTextureReadWrite.Default)
        sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
      RenderTexture.Internal_SetSRGBReadWrite(this, sRGB);
    }

    /// <summary>
    ///   <para>Creates a new RenderTexture object.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Texture color format.</param>
    /// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
    /// <param name="desc">Create the RenderTexture with the settings in the RenderTextureDescriptor.</param>
    /// <param name="textureToCopy">Copy the settings from another RenderTexture.</param>
    public RenderTexture(int width, int height, int depth, RenderTextureFormat format)
    {
      RenderTexture.Internal_CreateRenderTexture(this);
      this.width = width;
      this.height = height;
      this.depth = depth;
      this.format = format;
      RenderTexture.Internal_SetSRGBReadWrite(this, QualitySettings.activeColorSpace == ColorSpace.Linear);
    }

    /// <summary>
    ///   <para>Creates a new RenderTexture object.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Texture color format.</param>
    /// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
    /// <param name="desc">Create the RenderTexture with the settings in the RenderTextureDescriptor.</param>
    /// <param name="textureToCopy">Copy the settings from another RenderTexture.</param>
    public RenderTexture(int width, int height, int depth)
    {
      RenderTexture.Internal_CreateRenderTexture(this);
      this.width = width;
      this.height = height;
      this.depth = depth;
      this.format = RenderTextureFormat.Default;
      RenderTexture.Internal_SetSRGBReadWrite(this, QualitySettings.activeColorSpace == ColorSpace.Linear);
    }

    protected internal RenderTexture()
    {
    }

    /// <summary>
    ///   <para>Creates a new RenderTexture object.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Texture color format.</param>
    /// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
    /// <param name="desc">Create the RenderTexture with the settings in the RenderTextureDescriptor.</param>
    /// <param name="textureToCopy">Copy the settings from another RenderTexture.</param>
    public RenderTexture(RenderTextureDescriptor desc)
    {
      RenderTexture.ValidateRenderTextureDesc(desc);
      RenderTexture.Internal_CreateRenderTexture(this);
      this.SetRenderTextureDescriptor(desc);
    }

    /// <summary>
    ///   <para>Creates a new RenderTexture object.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Texture color format.</param>
    /// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
    /// <param name="desc">Create the RenderTexture with the settings in the RenderTextureDescriptor.</param>
    /// <param name="textureToCopy">Copy the settings from another RenderTexture.</param>
    public RenderTexture(RenderTexture textureToCopy)
    {
      if ((Object) textureToCopy == (Object) null)
        throw new ArgumentNullException(nameof (textureToCopy));
      RenderTexture.ValidateRenderTextureDesc(textureToCopy.descriptor);
      RenderTexture.Internal_CreateRenderTexture(this);
      this.SetRenderTextureDescriptor(textureToCopy.descriptor);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateRenderTexture([Writable] RenderTexture rt);

    private void SetRenderTextureDescriptor(RenderTextureDescriptor desc)
    {
      RenderTexture.INTERNAL_CALL_SetRenderTextureDescriptor(this, ref desc);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetRenderTextureDescriptor(RenderTexture self, ref RenderTextureDescriptor desc);

    private RenderTextureDescriptor GetDescriptor()
    {
      RenderTextureDescriptor textureDescriptor;
      RenderTexture.INTERNAL_CALL_GetDescriptor(this, out textureDescriptor);
      return textureDescriptor;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetDescriptor(RenderTexture self, out RenderTextureDescriptor value);

    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode, VRTextureUsage vrUsage)
    {
      bool useDynamicScale = false;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode)
    {
      bool useDynamicScale = false;
      VRTextureUsage vrUsage = VRTextureUsage.None;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    /// <summary>
    ///   <para>Allocate a temporary render texture.</para>
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="depthBuffer">Depth buffer bits (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Render texture format.</param>
    /// <param name="readWrite">Color space conversion mode.</param>
    /// <param name="msaaSamples">Number of antialiasing samples to store in the texture. Valid values are 1, 2, 4, and 8. Throws an exception if any other value is passed.</param>
    /// <param name="memorylessMode">Render texture memoryless mode.</param>
    /// <param name="desc">Use this RenderTextureDesc for the settings when creating the temporary RenderTexture.</param>
    /// <param name="antiAliasing"></param>
    /// <param name="vrUsage"></param>
    /// <param name="useDynamicScale"></param>
    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
    {
      bool useDynamicScale = false;
      VRTextureUsage vrUsage = VRTextureUsage.None;
      RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
    {
      bool useDynamicScale = false;
      VRTextureUsage vrUsage = VRTextureUsage.None;
      RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None;
      int antiAliasing = 1;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
    {
      bool useDynamicScale = false;
      VRTextureUsage vrUsage = VRTextureUsage.None;
      RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None;
      int antiAliasing = 1;
      RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
    {
      bool useDynamicScale = false;
      VRTextureUsage vrUsage = VRTextureUsage.None;
      RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None;
      int antiAliasing = 1;
      RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
      RenderTextureFormat format = RenderTextureFormat.Default;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    [ExcludeFromDocs]
    public static RenderTexture GetTemporary(int width, int height)
    {
      bool useDynamicScale = false;
      VRTextureUsage vrUsage = VRTextureUsage.None;
      RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None;
      int antiAliasing = 1;
      RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
      RenderTextureFormat format = RenderTextureFormat.Default;
      int depthBuffer = 0;
      return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
    }

    public static RenderTexture GetTemporary(int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing, [DefaultValue("RenderTextureMemoryless.None")] RenderTextureMemoryless memorylessMode, [DefaultValue("VRTextureUsage.None")] VRTextureUsage vrUsage, [DefaultValue("false")] bool useDynamicScale)
    {
      return RenderTexture.GetTemporary(new RenderTextureDescriptor(width, height) { depthBufferBits = depthBuffer, vrUsage = vrUsage, colorFormat = format, sRGB = readWrite != RenderTextureReadWrite.Linear, msaaSamples = antiAliasing, memoryless = memorylessMode, useDynamicScale = useDynamicScale });
    }

    private static RenderTexture GetTemporary_Internal(RenderTextureDescriptor desc)
    {
      return RenderTexture.INTERNAL_CALL_GetTemporary_Internal(ref desc);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern RenderTexture INTERNAL_CALL_GetTemporary_Internal(ref RenderTextureDescriptor desc);

    /// <summary>
    ///   <para>Release a temporary texture allocated with GetTemporary.</para>
    /// </summary>
    /// <param name="temp"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ReleaseTemporary(RenderTexture temp);

    /// <summary>
    ///   <para>Force an antialiased render texture to be resolved.</para>
    /// </summary>
    /// <param name="target">The render texture to resolve into.  If set, the target render texture must have the same dimensions and format as the source.</param>
    public void ResolveAntiAliasedSurface()
    {
      this.Internal_ResolveAntiAliasedSurface((RenderTexture) null);
    }

    /// <summary>
    ///   <para>Force an antialiased render texture to be resolved.</para>
    /// </summary>
    /// <param name="target">The render texture to resolve into.  If set, the target render texture must have the same dimensions and format as the source.</param>
    public void ResolveAntiAliasedSurface(RenderTexture target)
    {
      this.Internal_ResolveAntiAliasedSurface(target);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_ResolveAntiAliasedSurface(RenderTexture target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetWidth(RenderTexture mono);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetWidth(RenderTexture mono, int width);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetHeight(RenderTexture mono);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetHeight(RenderTexture mono, int width);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern VRTextureUsage Internal_GetVRUsage(RenderTexture mono);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetVRUsage(RenderTexture mono, VRTextureUsage vrUsage);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetSRGBReadWrite(RenderTexture mono, bool sRGB);

    /// <summary>
    ///   <para>The width of the render texture in pixels.</para>
    /// </summary>
    public override int width
    {
      get
      {
        return RenderTexture.Internal_GetWidth(this);
      }
      set
      {
        RenderTexture.Internal_SetWidth(this, value);
      }
    }

    /// <summary>
    ///   <para>The height of the render texture in pixels.</para>
    /// </summary>
    public override int height
    {
      get
      {
        return RenderTexture.Internal_GetHeight(this);
      }
      set
      {
        RenderTexture.Internal_SetHeight(this, value);
      }
    }

    /// <summary>
    ///   <para>If this RenderTexture is a VR eye texture used in stereoscopic rendering, this property decides what special rendering occurs, if any.</para>
    /// </summary>
    public VRTextureUsage vrUsage
    {
      get
      {
        return RenderTexture.Internal_GetVRUsage(this);
      }
      set
      {
        RenderTexture.Internal_SetVRUsage(this, value);
      }
    }

    /// <summary>
    ///   <para>The precision of the render texture's depth buffer in bits (0, 16, 24/32 are supported).</para>
    /// </summary>
    public extern int depth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool isPowerOfTwo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does this render texture use sRGB read/write conversions (Read Only).</para>
    /// </summary>
    public extern bool sRGB { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The color format of the render texture.</para>
    /// </summary>
    public extern RenderTextureFormat format { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Render texture has mipmaps when this flag is set.</para>
    /// </summary>
    public extern bool useMipMap { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mipmap levels are generated automatically when this flag is set.</para>
    /// </summary>
    public extern bool autoGenerateMips { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern TextureDimension Internal_GetDimension(RenderTexture rt);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetDimension(RenderTexture rt, TextureDimension dim);

    /// <summary>
    ///   <para>Dimensionality (type) of the render texture.</para>
    /// </summary>
    public override TextureDimension dimension
    {
      get
      {
        return RenderTexture.Internal_GetDimension(this);
      }
      set
      {
        RenderTexture.Internal_SetDimension(this, value);
      }
    }

    [Obsolete("Use RenderTexture.dimension instead.")]
    public extern bool isCubemap { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, this Render Texture will be used as a Texture3D.</para>
    /// </summary>
    [Obsolete("Use RenderTexture.dimension instead.")]
    public extern bool isVolume { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Volume extent of a 3D render texture or number of slices of array texture.</para>
    /// </summary>
    public extern int volumeDepth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The render texture memoryless mode property.</para>
    /// </summary>
    public extern RenderTextureMemoryless memorylessMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The antialiasing level for the RenderTexture.</para>
    /// </summary>
    public extern int antiAliasing { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If true and antiAliasing is greater than 1, the render texture will not be resolved by default.  Use this if the render texture needs to be bound as a multisampled texture in a shader.</para>
    /// </summary>
    public extern bool bindTextureMS { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable random access write into this render texture on Shader Model 5.0 level shaders.</para>
    /// </summary>
    public extern bool enableRandomWrite { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the render texture marked to be scaled by the Dynamic Resolution system.</para>
    /// </summary>
    public extern bool useDynamicScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Actually creates the RenderTexture.</para>
    /// </summary>
    public bool Create()
    {
      return RenderTexture.INTERNAL_CALL_Create(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Create(RenderTexture self);

    /// <summary>
    ///   <para>Releases the RenderTexture.</para>
    /// </summary>
    public void Release()
    {
      RenderTexture.INTERNAL_CALL_Release(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Release(RenderTexture self);

    /// <summary>
    ///   <para>Is the render texture actually created?</para>
    /// </summary>
    public bool IsCreated()
    {
      return RenderTexture.INTERNAL_CALL_IsCreated(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_IsCreated(RenderTexture self);

    /// <summary>
    ///   <para>Hint the GPU driver that the contents of the RenderTexture will not be used.</para>
    /// </summary>
    /// <param name="discardColor">Should the colour buffer be discarded?</param>
    /// <param name="discardDepth">Should the depth buffer be discarded?</param>
    public void DiscardContents()
    {
      RenderTexture.INTERNAL_CALL_DiscardContents(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_DiscardContents(RenderTexture self);

    /// <summary>
    ///   <para>Hint the GPU driver that the contents of the RenderTexture will not be used.</para>
    /// </summary>
    /// <param name="discardColor">Should the colour buffer be discarded?</param>
    /// <param name="discardDepth">Should the depth buffer be discarded?</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DiscardContents(bool discardColor, bool discardDepth);

    /// <summary>
    ///   <para>Indicate that there's a RenderTexture restore operation expected.</para>
    /// </summary>
    public void MarkRestoreExpected()
    {
      RenderTexture.INTERNAL_CALL_MarkRestoreExpected(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MarkRestoreExpected(RenderTexture self);

    /// <summary>
    ///   <para>Generate mipmap levels of a render texture.</para>
    /// </summary>
    public void GenerateMips()
    {
      RenderTexture.INTERNAL_CALL_GenerateMips(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GenerateMips(RenderTexture self);

    /// <summary>
    ///   <para>Color buffer of the render texture (Read Only).</para>
    /// </summary>
    public RenderBuffer colorBuffer
    {
      get
      {
        RenderBuffer res;
        this.GetColorBuffer(out res);
        return res;
      }
    }

    /// <summary>
    ///   <para>Depth/stencil buffer of the render texture (Read Only).</para>
    /// </summary>
    public RenderBuffer depthBuffer
    {
      get
      {
        RenderBuffer res;
        this.GetDepthBuffer(out res);
        return res;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetColorBuffer(out RenderBuffer res);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetDepthBuffer(out RenderBuffer res);

    /// <summary>
    ///   <para>Retrieve a native (underlying graphics API) pointer to the depth buffer resource.</para>
    /// </summary>
    /// <returns>
    ///   <para>Pointer to an underlying graphics API depth buffer resource.</para>
    /// </returns>
    public IntPtr GetNativeDepthBufferPtr()
    {
      IntPtr num;
      RenderTexture.INTERNAL_CALL_GetNativeDepthBufferPtr(this, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetNativeDepthBufferPtr(RenderTexture self, out IntPtr value);

    /// <summary>
    ///   <para>Assigns this RenderTexture as a global shader property named propertyName.</para>
    /// </summary>
    /// <param name="propertyName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetGlobalShaderProperty(string propertyName);

    /// <summary>
    ///   <para>Currently active render texture.</para>
    /// </summary>
    public static extern RenderTexture active { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("RenderTexture.enabled is always now, no need to use it")]
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("GetTexelOffset always returns zero now, no point in using it.")]
    public Vector2 GetTexelOffset()
    {
      return Vector2.zero;
    }

    /// <summary>
    ///   <para>Does a RenderTexture have stencil buffer?</para>
    /// </summary>
    /// <param name="rt">Render texture, or null for main screen.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SupportsStencil(RenderTexture rt);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern VRTextureUsage GetActiveVRUsage();

    [Obsolete("SetBorderColor is no longer supported.", true)]
    public void SetBorderColor(Color color)
    {
    }

    /// <summary>
    ///   <para>Allocate a temporary render texture.</para>
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="depthBuffer">Depth buffer bits (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
    /// <param name="format">Render texture format.</param>
    /// <param name="readWrite">Color space conversion mode.</param>
    /// <param name="msaaSamples">Number of antialiasing samples to store in the texture. Valid values are 1, 2, 4, and 8. Throws an exception if any other value is passed.</param>
    /// <param name="memorylessMode">Render texture memoryless mode.</param>
    /// <param name="desc">Use this RenderTextureDesc for the settings when creating the temporary RenderTexture.</param>
    /// <param name="antiAliasing"></param>
    /// <param name="vrUsage"></param>
    /// <param name="useDynamicScale"></param>
    public static RenderTexture GetTemporary(RenderTextureDescriptor desc)
    {
      RenderTexture.ValidateRenderTextureDesc(desc);
      desc.createdFromScript = true;
      return RenderTexture.GetTemporary_Internal(desc);
    }

    /// <summary>
    ///   <para>This struct contains all the information required to create a RenderTexture. It can be copied, cached, and reused to easily create RenderTextures that all share the same properties.</para>
    /// </summary>
    public RenderTextureDescriptor descriptor
    {
      get
      {
        return this.GetDescriptor();
      }
      set
      {
        RenderTexture.ValidateRenderTextureDesc(value);
        this.SetRenderTextureDescriptor(value);
      }
    }

    private static void ValidateRenderTextureDesc(RenderTextureDescriptor desc)
    {
      if (desc.width <= 0)
        throw new ArgumentException("RenderTextureDesc width must be greater than zero.", "desc.width");
      if (desc.height <= 0)
        throw new ArgumentException("RenderTextureDesc height must be greater than zero.", "desc.height");
      if (desc.volumeDepth <= 0)
        throw new ArgumentException("RenderTextureDesc volumeDepth must be greater than zero.", "desc.volumeDepth");
      if (desc.msaaSamples != 1 && desc.msaaSamples != 2 && (desc.msaaSamples != 4 && desc.msaaSamples != 8))
        throw new ArgumentException("RenderTextureDesc msaaSamples must be 1, 2, 4, or 8.", "desc.msaaSamples");
      if (desc.depthBufferBits != 0 && desc.depthBufferBits != 16 && desc.depthBufferBits != 24)
        throw new ArgumentException("RenderTextureDesc depthBufferBits must be 0, 16, or 24.", "desc.depthBufferBits");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use RenderTexture.autoGenerateMips instead (UnityUpgradable) -> autoGenerateMips", false)]
    public bool generateMips
    {
      get
      {
        return this.autoGenerateMips;
      }
      set
      {
        this.autoGenerateMips = value;
      }
    }
  }
}
