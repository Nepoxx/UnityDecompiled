// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WebCam.PhotoCaptureFrame
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.WebCam
{
  /// <summary>
  ///   <para>Contains information captured from the web camera.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.WebCam")]
  public sealed class PhotoCaptureFrame : IDisposable
  {
    private IntPtr m_NativePtr;

    internal PhotoCaptureFrame(IntPtr nativePtr)
    {
      this.m_NativePtr = nativePtr;
      this.dataLength = PhotoCaptureFrame.GetDataLength(nativePtr);
      this.hasLocationData = PhotoCaptureFrame.GetHasLocationData(nativePtr);
      this.pixelFormat = PhotoCaptureFrame.GetCapturePixelFormat(nativePtr);
      GC.AddMemoryPressure((long) this.dataLength);
    }

    /// <summary>
    ///   <para>The length of the raw IMFMediaBuffer which contains the image captured.</para>
    /// </summary>
    public int dataLength { get; private set; }

    /// <summary>
    ///   <para>Specifies whether or not spatial data was captured.</para>
    /// </summary>
    public bool hasLocationData { get; private set; }

    /// <summary>
    ///   <para>The raw image data pixel format.</para>
    /// </summary>
    public CapturePixelFormat pixelFormat { get; private set; }

    public bool TryGetCameraToWorldMatrix(out Matrix4x4 cameraToWorldMatrix)
    {
      cameraToWorldMatrix = Matrix4x4.identity;
      if (!this.hasLocationData)
        return false;
      cameraToWorldMatrix = PhotoCaptureFrame.GetCameraToWorldMatrix(this.m_NativePtr);
      return true;
    }

    public bool TryGetProjectionMatrix(out Matrix4x4 projectionMatrix)
    {
      if (this.hasLocationData)
      {
        projectionMatrix = PhotoCaptureFrame.GetProjection(this.m_NativePtr);
        return true;
      }
      projectionMatrix = Matrix4x4.identity;
      return false;
    }

    public bool TryGetProjectionMatrix(float nearClipPlane, float farClipPlane, out Matrix4x4 projectionMatrix)
    {
      if (this.hasLocationData)
      {
        float num1 = 0.01f;
        if ((double) nearClipPlane < (double) num1)
          nearClipPlane = num1;
        if ((double) farClipPlane < (double) nearClipPlane + (double) num1)
          farClipPlane = nearClipPlane + num1;
        projectionMatrix = PhotoCaptureFrame.GetProjection(this.m_NativePtr);
        float num2 = (float) (1.0 / ((double) farClipPlane - (double) nearClipPlane));
        float num3 = (float) -((double) farClipPlane + (double) nearClipPlane) * num2;
        float num4 = (float) -(2.0 * (double) farClipPlane * (double) nearClipPlane) * num2;
        projectionMatrix.m22 = num3;
        projectionMatrix.m23 = num4;
        return true;
      }
      projectionMatrix = Matrix4x4.identity;
      return false;
    }

    /// <summary>
    ///   <para>This method will copy the captured image data into a user supplied texture for use in Unity.</para>
    /// </summary>
    /// <param name="targetTexture">The target texture that the captured image data will be copied to.</param>
    public void UploadImageDataToTexture(Texture2D targetTexture)
    {
      if ((UnityEngine.Object) targetTexture == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (targetTexture));
      if (this.pixelFormat != CapturePixelFormat.BGRA32)
        throw new ArgumentException("Uploading PhotoCaptureFrame to a texture is only supported with BGRA32 CameraFrameFormat!");
      PhotoCaptureFrame.UploadImageDataToTexture_Internal(this.m_NativePtr, targetTexture);
    }

    /// <summary>
    ///   <para>Provides a COM pointer to the native IMFMediaBuffer that contains the image data.</para>
    /// </summary>
    /// <returns>
    ///   <para>A native COM pointer to the IMFMediaBuffer which contains the image data.</para>
    /// </returns>
    public IntPtr GetUnsafePointerToBuffer()
    {
      return PhotoCaptureFrame.GetUnsafePointerToBuffer(this.m_NativePtr);
    }

    public void CopyRawImageDataIntoBuffer(List<byte> byteBuffer)
    {
      if (byteBuffer == null)
        throw new ArgumentNullException(nameof (byteBuffer));
      byte[] numArray = PhotoCaptureFrame.CopyRawImageDataIntoBuffer_Internal(this.m_NativePtr);
      int length = numArray.Length;
      if (byteBuffer.Capacity < length)
        byteBuffer.Capacity = length;
      byteBuffer.Clear();
      byteBuffer.AddRange((IEnumerable<byte>) numArray);
    }

    private void Cleanup()
    {
      if (!(this.m_NativePtr != IntPtr.Zero))
        return;
      GC.RemoveMemoryPressure((long) this.dataLength);
      PhotoCaptureFrame.Dispose_Internal(this.m_NativePtr);
      this.m_NativePtr = IntPtr.Zero;
    }

    /// <summary>
    ///   <para>Disposes the PhotoCaptureFrame and any resources it uses.</para>
    /// </summary>
    public void Dispose()
    {
      this.Cleanup();
      GC.SuppressFinalize((object) this);
    }

    ~PhotoCaptureFrame()
    {
      this.Cleanup();
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern CapturePixelFormat GetCapturePixelFormat(IntPtr photoCaptureFrame);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetHasLocationData(IntPtr photoCaptureFrame);

    [ThreadAndSerializationSafe]
    private static Matrix4x4 GetCameraToWorldMatrix(IntPtr photoCaptureFrame)
    {
      Matrix4x4 matrix4x4;
      PhotoCaptureFrame.INTERNAL_CALL_GetCameraToWorldMatrix(photoCaptureFrame, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetCameraToWorldMatrix(IntPtr photoCaptureFrame, out Matrix4x4 value);

    [ThreadAndSerializationSafe]
    private static Matrix4x4 GetProjection(IntPtr photoCaptureFrame)
    {
      Matrix4x4 matrix4x4;
      PhotoCaptureFrame.INTERNAL_CALL_GetProjection(photoCaptureFrame, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetProjection(IntPtr photoCaptureFrame, out Matrix4x4 value);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetDataLength(IntPtr photoCaptureFrame);

    [ThreadAndSerializationSafe]
    private static IntPtr GetUnsafePointerToBuffer(IntPtr photoCaptureFrame)
    {
      IntPtr num;
      PhotoCaptureFrame.INTERNAL_CALL_GetUnsafePointerToBuffer(photoCaptureFrame, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetUnsafePointerToBuffer(IntPtr photoCaptureFrame, out IntPtr value);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetData_Internal(IntPtr photoCaptureFrame, IntPtr targetBuffer);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern byte[] CopyRawImageDataIntoBuffer_Internal(IntPtr photoCaptureFrame);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UploadImageDataToTexture_Internal(IntPtr photoCaptureFrame, Texture2D targetTexture);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Dispose_Internal(IntPtr photoCaptureFrame);
  }
}
