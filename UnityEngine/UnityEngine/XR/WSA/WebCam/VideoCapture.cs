// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WebCam.VideoCapture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.WebCam
{
  /// <summary>
  ///   <para>Records a video from the web camera directly to disk.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.WebCam")]
  public sealed class VideoCapture : IDisposable
  {
    private static readonly long HR_SUCCESS = 0;
    private static Resolution[] s_SupportedResolutions;
    private IntPtr m_NativePtr;

    private VideoCapture(IntPtr nativeCaptureObject)
    {
      this.m_NativePtr = nativeCaptureObject;
    }

    private static VideoCapture.VideoCaptureResult MakeCaptureResult(VideoCapture.CaptureResultType resultType, long hResult)
    {
      return new VideoCapture.VideoCaptureResult() { resultType = resultType, hResult = hResult };
    }

    private static VideoCapture.VideoCaptureResult MakeCaptureResult(long hResult)
    {
      return new VideoCapture.VideoCaptureResult() { resultType = hResult != VideoCapture.HR_SUCCESS ? VideoCapture.CaptureResultType.UnknownError : VideoCapture.CaptureResultType.Success, hResult = hResult };
    }

    /// <summary>
    ///   <para>A list of all the supported device resolutions for recording videos.</para>
    /// </summary>
    public static IEnumerable<Resolution> SupportedResolutions
    {
      get
      {
        if (VideoCapture.s_SupportedResolutions == null)
          VideoCapture.s_SupportedResolutions = VideoCapture.GetSupportedResolutions_Internal();
        return (IEnumerable<Resolution>) VideoCapture.s_SupportedResolutions;
      }
    }

    /// <summary>
    ///   <para>Returns the supported frame rates at which a video can be recorded given a resolution.</para>
    /// </summary>
    /// <param name="resolution">A recording resolution.</param>
    /// <returns>
    ///   <para>The frame rates at which the video can be recorded.</para>
    /// </returns>
    public static IEnumerable<float> GetSupportedFrameRatesForResolution(Resolution resolution)
    {
      return (IEnumerable<float>) VideoCapture.GetSupportedFrameRatesForResolution_Internal(resolution.width, resolution.height);
    }

    /// <summary>
    ///   <para>Indicates whether or not the VideoCapture instance is currently recording video.</para>
    /// </summary>
    public bool IsRecording
    {
      get
      {
        if (this.m_NativePtr == IntPtr.Zero)
          throw new InvalidOperationException("You must create a Video Capture Object before using it.");
        return this.IsRecording_Internal(this.m_NativePtr);
      }
    }

    public static void CreateAsync(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback)
    {
      if (onCreatedCallback == null)
        throw new ArgumentNullException(nameof (onCreatedCallback));
      VideoCapture.Instantiate_Internal(showHolograms, onCreatedCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnCreatedVideoCaptureResourceDelegate(VideoCapture.OnVideoCaptureResourceCreatedCallback callback, IntPtr nativePtr)
    {
      if (nativePtr == IntPtr.Zero)
        callback((VideoCapture) null);
      else
        callback(new VideoCapture(nativePtr));
    }

    public void StartVideoModeAsync(CameraParameters setupParams, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Video Capture Object before starting its video mode.");
      if (onVideoModeStartedCallback == null)
        throw new ArgumentNullException(nameof (onVideoModeStartedCallback));
      if (setupParams.cameraResolutionWidth == 0 || setupParams.cameraResolutionHeight == 0)
        throw new ArgumentOutOfRangeException(nameof (setupParams), "The camera resolution must be set to a supported resolution.");
      if ((double) setupParams.frameRate == 0.0)
        throw new ArgumentOutOfRangeException(nameof (setupParams), "The camera frame rate must be set to a supported recording frame rate.");
      this.StartVideoMode_Internal(this.m_NativePtr, (int) audioState, onVideoModeStartedCallback, setupParams.hologramOpacity, setupParams.frameRate, setupParams.cameraResolutionWidth, setupParams.cameraResolutionHeight, (int) setupParams.pixelFormat);
    }

    [RequiredByNativeCode]
    private static void InvokeOnVideoModeStartedDelegate(VideoCapture.OnVideoModeStartedCallback callback, long hResult)
    {
      callback(VideoCapture.MakeCaptureResult(hResult));
    }

    public void StopVideoModeAsync(VideoCapture.OnVideoModeStoppedCallback onVideoModeStoppedCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Video Capture Object before stopping its video mode.");
      if (onVideoModeStoppedCallback == null)
        throw new ArgumentNullException(nameof (onVideoModeStoppedCallback));
      this.StopVideoMode_Internal(this.m_NativePtr, onVideoModeStoppedCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnVideoModeStoppedDelegate(VideoCapture.OnVideoModeStoppedCallback callback, long hResult)
    {
      callback(VideoCapture.MakeCaptureResult(hResult));
    }

    public void StartRecordingAsync(string filename, VideoCapture.OnStartedRecordingVideoCallback onStartedRecordingVideoCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Video Capture Object before recording video.");
      if (onStartedRecordingVideoCallback == null)
        throw new ArgumentNullException(nameof (onStartedRecordingVideoCallback));
      if (string.IsNullOrEmpty(filename))
        throw new ArgumentNullException(nameof (filename));
      filename = filename.Replace("/", "\\");
      string directoryName = Path.GetDirectoryName(filename);
      if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
        throw new ArgumentException("The specified directory does not exist.", nameof (filename));
      FileInfo fileInfo = new FileInfo(filename);
      if (fileInfo.Exists && fileInfo.IsReadOnly)
        throw new ArgumentException("Cannot write to the file because it is read-only.", nameof (filename));
      this.StartRecordingVideoToDisk_Internal(this.m_NativePtr, filename, onStartedRecordingVideoCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnStartedRecordingVideoToDiskDelegate(VideoCapture.OnStartedRecordingVideoCallback callback, long hResult)
    {
      callback(VideoCapture.MakeCaptureResult(hResult));
    }

    public void StopRecordingAsync(VideoCapture.OnStoppedRecordingVideoCallback onStoppedRecordingVideoCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Video Capture Object before recording video.");
      if (onStoppedRecordingVideoCallback == null)
        throw new ArgumentNullException(nameof (onStoppedRecordingVideoCallback));
      this.StopRecordingVideoToDisk_Internal(this.m_NativePtr, onStoppedRecordingVideoCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnStoppedRecordingVideoToDiskDelegate(VideoCapture.OnStoppedRecordingVideoCallback callback, long hResult)
    {
      callback(VideoCapture.MakeCaptureResult(hResult));
    }

    /// <summary>
    ///   <para>Provides a COM pointer to the native IVideoDeviceController.</para>
    /// </summary>
    /// <returns>
    ///   <para>A native COM pointer to the IVideoDeviceController.</para>
    /// </returns>
    public IntPtr GetUnsafePointerToVideoDeviceController()
    {
      return VideoCapture.GetUnsafePointerToVideoDeviceController_Internal(this.m_NativePtr);
    }

    /// <summary>
    ///   <para>Dispose must be called to shutdown the PhotoCapture instance.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_NativePtr != IntPtr.Zero)
      {
        VideoCapture.Dispose_Internal(this.m_NativePtr);
        this.m_NativePtr = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    ~VideoCapture()
    {
      if (!(this.m_NativePtr != IntPtr.Zero))
        return;
      VideoCapture.DisposeThreaded_Internal(this.m_NativePtr);
      this.m_NativePtr = IntPtr.Zero;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Resolution[] GetSupportedResolutions_Internal();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float[] GetSupportedFrameRatesForResolution_Internal(int resolutionWidth, int resolutionHeight);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsRecording_Internal(IntPtr videoCaptureObj);

    private static IntPtr Instantiate_Internal(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback)
    {
      IntPtr num;
      VideoCapture.INTERNAL_CALL_Instantiate_Internal(showHolograms, onCreatedCallback, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Instantiate_Internal(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback, out IntPtr value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void StartVideoMode_Internal(IntPtr videoCaptureObj, int audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback, float hologramOpacity, float frameRate, int cameraResolutionWidth, int cameraResolutionHeight, int pixelFormat);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void StopVideoMode_Internal(IntPtr videoCaptureObj, VideoCapture.OnVideoModeStoppedCallback onVideoModeStoppedCallback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void StartRecordingVideoToDisk_Internal(IntPtr videoCaptureObj, string filename, VideoCapture.OnStartedRecordingVideoCallback onStartedRecordingVideoCallback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void StopRecordingVideoToDisk_Internal(IntPtr videoCaptureObj, VideoCapture.OnStoppedRecordingVideoCallback onStoppedRecordingVideoCallback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Dispose_Internal(IntPtr videoCaptureObj);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DisposeThreaded_Internal(IntPtr videoCaptureObj);

    [ThreadAndSerializationSafe]
    private static IntPtr GetUnsafePointerToVideoDeviceController_Internal(IntPtr videoCaptureObj)
    {
      IntPtr num;
      VideoCapture.INTERNAL_CALL_GetUnsafePointerToVideoDeviceController_Internal(videoCaptureObj, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetUnsafePointerToVideoDeviceController_Internal(IntPtr videoCaptureObj, out IntPtr value);

    /// <summary>
    ///   <para>Contains the result of the capture request.</para>
    /// </summary>
    public enum CaptureResultType
    {
      Success,
      UnknownError,
    }

    /// <summary>
    ///   <para>Specifies what audio sources should be recorded while recording the video.</para>
    /// </summary>
    public enum AudioState
    {
      MicAudio,
      ApplicationAudio,
      ApplicationAndMicAudio,
      None,
    }

    /// <summary>
    ///   <para>A data container that contains the result information of a video recording operation.</para>
    /// </summary>
    public struct VideoCaptureResult
    {
      /// <summary>
      ///   <para>A generic result that indicates whether or not the VideoCapture operation succeeded.</para>
      /// </summary>
      public VideoCapture.CaptureResultType resultType;
      /// <summary>
      ///   <para>The specific HResult value.</para>
      /// </summary>
      public long hResult;

      /// <summary>
      ///   <para>Indicates whether or not the operation was successful.</para>
      /// </summary>
      public bool success
      {
        get
        {
          return this.resultType == VideoCapture.CaptureResultType.Success;
        }
      }
    }

    /// <summary>
    ///   <para>Called when a VideoCapture resource has been created.</para>
    /// </summary>
    /// <param name="captureObject">The VideoCapture instance.</param>
    public delegate void OnVideoCaptureResourceCreatedCallback(VideoCapture captureObject);

    /// <summary>
    ///   <para>Called when video mode has been started.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not video mode was successfully activated.</param>
    public delegate void OnVideoModeStartedCallback(VideoCapture.VideoCaptureResult result);

    /// <summary>
    ///   <para>Called when video mode has been stopped.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not video mode was successfully deactivated.</param>
    public delegate void OnVideoModeStoppedCallback(VideoCapture.VideoCaptureResult result);

    /// <summary>
    ///   <para>Called when the web camera begins recording the video.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not video recording started successfully.</param>
    public delegate void OnStartedRecordingVideoCallback(VideoCapture.VideoCaptureResult result);

    /// <summary>
    ///   <para>Called when the video recording has been saved to the file system.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not video recording was saved successfully to the file system.</param>
    public delegate void OnStoppedRecordingVideoCallback(VideoCapture.VideoCaptureResult result);
  }
}
