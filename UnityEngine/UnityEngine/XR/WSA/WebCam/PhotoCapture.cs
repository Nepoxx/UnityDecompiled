// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WebCam.PhotoCapture
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
  ///   <para>Captures a photo from the web camera and stores it in memory or on disk.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.WebCam")]
  public sealed class PhotoCapture : IDisposable
  {
    private static readonly long HR_SUCCESS = 0;
    private static Resolution[] s_SupportedResolutions;
    private IntPtr m_NativePtr;

    private PhotoCapture(IntPtr nativeCaptureObject)
    {
      this.m_NativePtr = nativeCaptureObject;
    }

    private static PhotoCapture.PhotoCaptureResult MakeCaptureResult(PhotoCapture.CaptureResultType resultType, long hResult)
    {
      return new PhotoCapture.PhotoCaptureResult() { resultType = resultType, hResult = hResult };
    }

    private static PhotoCapture.PhotoCaptureResult MakeCaptureResult(long hResult)
    {
      return new PhotoCapture.PhotoCaptureResult() { resultType = hResult != PhotoCapture.HR_SUCCESS ? PhotoCapture.CaptureResultType.UnknownError : PhotoCapture.CaptureResultType.Success, hResult = hResult };
    }

    /// <summary>
    ///   <para>A list of all the supported device resolutions for taking pictures.</para>
    /// </summary>
    public static IEnumerable<Resolution> SupportedResolutions
    {
      get
      {
        if (PhotoCapture.s_SupportedResolutions == null)
          PhotoCapture.s_SupportedResolutions = PhotoCapture.GetSupportedResolutions_Internal();
        return (IEnumerable<Resolution>) PhotoCapture.s_SupportedResolutions;
      }
    }

    public static void CreateAsync(bool showHolograms, PhotoCapture.OnCaptureResourceCreatedCallback onCreatedCallback)
    {
      if (onCreatedCallback == null)
        throw new ArgumentNullException(nameof (onCreatedCallback));
      PhotoCapture.Instantiate_Internal(showHolograms, onCreatedCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnCreatedResourceDelegate(PhotoCapture.OnCaptureResourceCreatedCallback callback, IntPtr nativePtr)
    {
      if (nativePtr == IntPtr.Zero)
        callback((PhotoCapture) null);
      else
        callback(new PhotoCapture(nativePtr));
    }

    public void StartPhotoModeAsync(CameraParameters setupParams, PhotoCapture.OnPhotoModeStartedCallback onPhotoModeStartedCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Photo Capture Object before starting its photo mode.");
      if (onPhotoModeStartedCallback == null)
        throw new ArgumentException(nameof (onPhotoModeStartedCallback));
      if (setupParams.cameraResolutionWidth == 0 || setupParams.cameraResolutionHeight == 0)
        throw new ArgumentOutOfRangeException(nameof (setupParams), "The camera resolution must be set to a supported resolution.");
      this.StartPhotoMode_Internal(this.m_NativePtr, onPhotoModeStartedCallback, setupParams.hologramOpacity, setupParams.frameRate, setupParams.cameraResolutionWidth, setupParams.cameraResolutionHeight, (int) setupParams.pixelFormat);
    }

    [RequiredByNativeCode]
    private static void InvokeOnPhotoModeStartedDelegate(PhotoCapture.OnPhotoModeStartedCallback callback, long hResult)
    {
      callback(PhotoCapture.MakeCaptureResult(hResult));
    }

    public void StopPhotoModeAsync(PhotoCapture.OnPhotoModeStoppedCallback onPhotoModeStoppedCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Photo Capture Object before stopping its photo mode.");
      if (onPhotoModeStoppedCallback == null)
        throw new ArgumentException(nameof (onPhotoModeStoppedCallback));
      this.StopPhotoMode_Internal(this.m_NativePtr, onPhotoModeStoppedCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnPhotoModeStoppedDelegate(PhotoCapture.OnPhotoModeStoppedCallback callback, long hResult)
    {
      callback(PhotoCapture.MakeCaptureResult(hResult));
    }

    public void TakePhotoAsync(string filename, PhotoCaptureFileOutputFormat fileOutputFormat, PhotoCapture.OnCapturedToDiskCallback onCapturedPhotoToDiskCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Photo Capture Object before taking a photo.");
      if (onCapturedPhotoToDiskCallback == null)
        throw new ArgumentNullException(nameof (onCapturedPhotoToDiskCallback));
      if (string.IsNullOrEmpty(filename))
        throw new ArgumentNullException(nameof (filename));
      filename = filename.Replace("/", "\\");
      string directoryName = Path.GetDirectoryName(filename);
      if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
        throw new ArgumentException("The specified directory does not exist.", nameof (filename));
      FileInfo fileInfo = new FileInfo(filename);
      if (fileInfo.Exists && fileInfo.IsReadOnly)
        throw new ArgumentException("Cannot write to the file because it is read-only.", nameof (filename));
      this.CapturePhotoToDisk_Internal(this.m_NativePtr, filename, (int) fileOutputFormat, onCapturedPhotoToDiskCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnCapturedPhotoToDiskDelegate(PhotoCapture.OnCapturedToDiskCallback callback, long hResult)
    {
      callback(PhotoCapture.MakeCaptureResult(hResult));
    }

    public void TakePhotoAsync(PhotoCapture.OnCapturedToMemoryCallback onCapturedPhotoToMemoryCallback)
    {
      if (this.m_NativePtr == IntPtr.Zero)
        throw new InvalidOperationException("You must create a Photo Capture Object before taking a photo.");
      if (onCapturedPhotoToMemoryCallback == null)
        throw new ArgumentNullException(nameof (onCapturedPhotoToMemoryCallback));
      this.CapturePhotoToMemory_Internal(this.m_NativePtr, onCapturedPhotoToMemoryCallback);
    }

    [RequiredByNativeCode]
    private static void InvokeOnCapturedPhotoToMemoryDelegate(PhotoCapture.OnCapturedToMemoryCallback callback, long hResult, IntPtr photoCaptureFramePtr)
    {
      PhotoCaptureFrame photoCaptureFrame = (PhotoCaptureFrame) null;
      if (photoCaptureFramePtr != IntPtr.Zero)
        photoCaptureFrame = new PhotoCaptureFrame(photoCaptureFramePtr);
      callback(PhotoCapture.MakeCaptureResult(hResult), photoCaptureFrame);
    }

    /// <summary>
    ///   <para>Provides a COM pointer to the native IVideoDeviceController.</para>
    /// </summary>
    /// <returns>
    ///   <para>A native COM pointer to the IVideoDeviceController.</para>
    /// </returns>
    public IntPtr GetUnsafePointerToVideoDeviceController()
    {
      return PhotoCapture.GetUnsafePointerToVideoDeviceController_Internal(this.m_NativePtr);
    }

    /// <summary>
    ///   <para>Dispose must be called to shutdown the PhotoCapture instance.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_NativePtr != IntPtr.Zero)
      {
        PhotoCapture.Dispose_Internal(this.m_NativePtr);
        this.m_NativePtr = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    ~PhotoCapture()
    {
      if (!(this.m_NativePtr != IntPtr.Zero))
        return;
      PhotoCapture.DisposeThreaded_Internal(this.m_NativePtr);
      this.m_NativePtr = IntPtr.Zero;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Resolution[] GetSupportedResolutions_Internal();

    private static IntPtr Instantiate_Internal(bool showHolograms, PhotoCapture.OnCaptureResourceCreatedCallback onCreatedCallback)
    {
      IntPtr num;
      PhotoCapture.INTERNAL_CALL_Instantiate_Internal(showHolograms, onCreatedCallback, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Instantiate_Internal(bool showHolograms, PhotoCapture.OnCaptureResourceCreatedCallback onCreatedCallback, out IntPtr value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void StartPhotoMode_Internal(IntPtr photoCaptureObj, PhotoCapture.OnPhotoModeStartedCallback onPhotoModeStartedCallback, float hologramOpacity, float frameRate, int cameraResolutionWidth, int cameraResolutionHeight, int pixelFormat);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void StopPhotoMode_Internal(IntPtr photoCaptureObj, PhotoCapture.OnPhotoModeStoppedCallback onPhotoModeStoppedCallback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void CapturePhotoToDisk_Internal(IntPtr photoCaptureObj, string filename, int fileOutputFormat, PhotoCapture.OnCapturedToDiskCallback onCapturedPhotoToDiskCallback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void CapturePhotoToMemory_Internal(IntPtr photoCaptureObj, PhotoCapture.OnCapturedToMemoryCallback onCapturedPhotoToMemoryCallback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Dispose_Internal(IntPtr photoCaptureObj);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DisposeThreaded_Internal(IntPtr photoCaptureObj);

    [ThreadAndSerializationSafe]
    private static IntPtr GetUnsafePointerToVideoDeviceController_Internal(IntPtr photoCaptureObj)
    {
      IntPtr num;
      PhotoCapture.INTERNAL_CALL_GetUnsafePointerToVideoDeviceController_Internal(photoCaptureObj, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetUnsafePointerToVideoDeviceController_Internal(IntPtr photoCaptureObj, out IntPtr value);

    /// <summary>
    ///   <para>Contains the result of the capture request.</para>
    /// </summary>
    public enum CaptureResultType
    {
      Success,
      UnknownError,
    }

    /// <summary>
    ///   <para>A data container that contains the result information of a photo capture operation.</para>
    /// </summary>
    public struct PhotoCaptureResult
    {
      /// <summary>
      ///   <para>A generic result that indicates whether or not the PhotoCapture operation succeeded.</para>
      /// </summary>
      public PhotoCapture.CaptureResultType resultType;
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
          return this.resultType == PhotoCapture.CaptureResultType.Success;
        }
      }
    }

    /// <summary>
    ///   <para>Called when a PhotoCapture resource has been created.</para>
    /// </summary>
    /// <param name="captureObject">The PhotoCapture instance.</param>
    public delegate void OnCaptureResourceCreatedCallback(PhotoCapture captureObject);

    /// <summary>
    ///   <para>Called when photo mode has been started.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not photo mode was successfully activated.</param>
    public delegate void OnPhotoModeStartedCallback(PhotoCapture.PhotoCaptureResult result);

    /// <summary>
    ///   <para>Called when photo mode has been stopped.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not photo mode was successfully deactivated.</param>
    public delegate void OnPhotoModeStoppedCallback(PhotoCapture.PhotoCaptureResult result);

    /// <summary>
    ///   <para>Called when a photo has been saved to the file system.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not the photo was successfully saved to the file system.</param>
    public delegate void OnCapturedToDiskCallback(PhotoCapture.PhotoCaptureResult result);

    /// <summary>
    ///   <para>Called when a photo has been captured to memory.</para>
    /// </summary>
    /// <param name="result">Indicates whether or not the photo was successfully captured to memory.</param>
    /// <param name="photoCaptureFrame">Contains the target texture.  If available, the spatial information will be accessible through this structure as well.</param>
    public delegate void OnCapturedToMemoryCallback(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame);
  }
}
