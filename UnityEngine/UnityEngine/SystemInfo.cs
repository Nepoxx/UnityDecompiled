// Decompiled with JetBrains decompiler
// Type: UnityEngine.SystemInfo
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
  ///   <para>Access system and hardware information.</para>
  /// </summary>
  public sealed class SystemInfo
  {
    /// <summary>
    ///   <para>Value returned by SystemInfo string properties which are not supported on the current platform.</para>
    /// </summary>
    public const string unsupportedIdentifier = "n/a";

    /// <summary>
    ///   <para>The current battery level (Read Only).</para>
    /// </summary>
    public static extern float batteryLevel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the current status of the device's battery (Read Only).</para>
    /// </summary>
    public static extern BatteryStatus batteryStatus { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Operating system name with version (Read Only).</para>
    /// </summary>
    public static extern string operatingSystem { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the operating system family the game is running on (Read Only).</para>
    /// </summary>
    public static extern OperatingSystemFamily operatingSystemFamily { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Processor name (Read Only).</para>
    /// </summary>
    public static extern string processorType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Processor frequency in MHz (Read Only).</para>
    /// </summary>
    public static extern int processorFrequency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of processors present (Read Only).</para>
    /// </summary>
    public static extern int processorCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Amount of system memory present (Read Only).</para>
    /// </summary>
    public static extern int systemMemorySize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Amount of video memory present (Read Only).</para>
    /// </summary>
    public static extern int graphicsMemorySize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The name of the graphics device (Read Only).</para>
    /// </summary>
    public static extern string graphicsDeviceName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The vendor of the graphics device (Read Only).</para>
    /// </summary>
    public static extern string graphicsDeviceVendor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The identifier code of the graphics device (Read Only).</para>
    /// </summary>
    public static extern int graphicsDeviceID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The identifier code of the graphics device vendor (Read Only).</para>
    /// </summary>
    public static extern int graphicsDeviceVendorID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The graphics API type used by the graphics device (Read Only).</para>
    /// </summary>
    public static extern GraphicsDeviceType graphicsDeviceType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the texture UV coordinate convention for this platform has Y starting at the top of the image.</para>
    /// </summary>
    public static extern bool graphicsUVStartsAtTop { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The graphics API type and driver version used by the graphics device (Read Only).</para>
    /// </summary>
    public static extern string graphicsDeviceVersion { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Graphics device shader capability level (Read Only).</para>
    /// </summary>
    public static extern int graphicsShaderLevel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("graphicsPixelFillrate is no longer supported in Unity 5.0+.")]
    public static int graphicsPixelFillrate
    {
      get
      {
        return -1;
      }
    }

    [Obsolete("Vertex program support is required in Unity 5.0+")]
    public static bool supportsVertexPrograms
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    ///   <para>Is graphics device using multi-threaded rendering (Read Only)?</para>
    /// </summary>
    public static extern bool graphicsMultiThreaded { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are built-in shadows supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsShadows { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is sampling raw depth from shadowmaps supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsRawShadowDepthSampling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are render textures supported? (Read Only)</para>
    /// </summary>
    [Obsolete("supportsRenderTextures always returns true, no need to call it")]
    public static extern bool supportsRenderTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Whether motion vectors are supported on this platform.</para>
    /// </summary>
    public static extern bool supportsMotionVectors { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are cubemap render textures supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsRenderToCubemap { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are image effects supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsImageEffects { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are 3D (volume) textures supported? (Read Only)</para>
    /// </summary>
    public static extern bool supports3DTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are 2D Array textures supported? (Read Only)</para>
    /// </summary>
    public static extern bool supports2DArrayTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are 3D (volume) RenderTextures supported? (Read Only)</para>
    /// </summary>
    public static extern bool supports3DRenderTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are Cubemap Array textures supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsCubemapArrayTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Support for various Graphics.CopyTexture cases (Read Only).</para>
    /// </summary>
    public static extern CopyTextureSupport copyTextureSupport { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are compute shaders supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsComputeShaders { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is GPU draw call instancing supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsInstancing { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are sparse textures supported? (Read Only)</para>
    /// </summary>
    public static extern bool supportsSparseTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>How many simultaneous render targets (MRTs) are supported? (Read Only)</para>
    /// </summary>
    public static extern int supportedRenderTargetCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are multisampled textures supported? (Read Only)</para>
    /// </summary>
    public static extern int supportsMultisampledTextures { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the 'Mirror Once' texture wrap mode is supported. (Read Only)</para>
    /// </summary>
    public static extern int supportsTextureWrapMirrorOnce { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This property is true if the current platform uses a reversed depth buffer (where values range from 1 at the near plane and 0 at far plane), and false if the depth buffer is normal (0 is near, 1 is far). (Read Only)</para>
    /// </summary>
    public static extern bool usesReversedZBuffer { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the stencil buffer supported? (Read Only)</para>
    /// </summary>
    [Obsolete("supportsStencil always returns true, no need to call it")]
    public static extern int supportsStencil { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is render texture format supported?</para>
    /// </summary>
    /// <param name="format">The format to look up.</param>
    /// <returns>
    ///   <para>True if the format is supported.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SupportsRenderTextureFormat(RenderTextureFormat format);

    /// <summary>
    ///   <para>Is blending supported on render texture format?</para>
    /// </summary>
    /// <param name="format">The format to look up.</param>
    /// <returns>
    ///   <para>True if blending is supported on the given format.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SupportsBlendingOnRenderTextureFormat(RenderTextureFormat format);

    /// <summary>
    ///   <para>Is texture format supported on this device?</para>
    /// </summary>
    /// <param name="format">The TextureFormat format to look up.</param>
    /// <returns>
    ///   <para>True if the format is supported.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SupportsTextureFormat(TextureFormat format);

    /// <summary>
    ///   <para>What NPOT (non-power of two size) texture support does the GPU provide? (Read Only)</para>
    /// </summary>
    public static extern NPOTSupport npotSupport { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>A unique device identifier. It is guaranteed to be unique for every device (Read Only).</para>
    /// </summary>
    public static extern string deviceUniqueIdentifier { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The user defined name of the device (Read Only).</para>
    /// </summary>
    public static extern string deviceName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The model of the device (Read Only).</para>
    /// </summary>
    public static extern string deviceModel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is an accelerometer available on the device?</para>
    /// </summary>
    public static extern bool supportsAccelerometer { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is a gyroscope available on the device?</para>
    /// </summary>
    public static extern bool supportsGyroscope { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the device capable of reporting its location?</para>
    /// </summary>
    public static extern bool supportsLocationService { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the device capable of providing the user haptic feedback by vibration?</para>
    /// </summary>
    public static extern bool supportsVibration { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is there an Audio device available for playback?</para>
    /// </summary>
    public static extern bool supportsAudio { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the kind of device the application is running on (Read Only).</para>
    /// </summary>
    public static extern DeviceType deviceType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Maximum texture size (Read Only).</para>
    /// </summary>
    public static extern int maxTextureSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Maximum Cubemap texture size (Read Only).</para>
    /// </summary>
    public static extern int maxCubemapSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern int maxRenderTextureSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///         <para>Returns true when the platform supports asynchronous compute queues and false if otherwise.
    /// 
    /// Note that asynchronous compute queues are only supported on PS4.</para>
    ///       </summary>
    public static extern bool supportsAsyncCompute { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///         <para>Returns true when the platform supports GPUFences and false if otherwise.
    /// 
    /// Note that GPUFences are only supported on PS4.</para>
    ///       </summary>
    public static extern bool supportsGPUFence { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
