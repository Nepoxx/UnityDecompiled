// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.TangoDevice
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.XR.Tango
{
  internal static class TangoDevice
  {
    private static ARBackgroundRenderer m_BackgroundRenderer = (ARBackgroundRenderer) null;
    private static string m_AreaDescriptionUUID = "";

    internal static extern CoordinateFrame baseCoordinateFrame { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Connect(string[] boolKeys, bool[] boolValues, string[] intKeys, int[] intValues, string[] longKeys, long[] longValues, string[] doubleKeys, double[] doubleValues, string[] stringKeys, string[] stringValues);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Disconnect();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool TryGetHorizontalFov(out float fovOut);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool TryGetVerticalFov(out float fovOut);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetRenderMode(ARRenderMode mode);

    internal static extern uint depthCameraRate { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool synchronizeFramerateWithColorCamera { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetBackgroundMaterial(Material material);

    internal static bool TryGetLatestPointCloud(ref PointCloudData pointCloudData)
    {
      if (pointCloudData.points == null)
        pointCloudData.points = new List<Vector4>();
      pointCloudData.points.Clear();
      return TangoDevice.TryGetLatestPointCloudInternal(pointCloudData.points, out pointCloudData.version, out pointCloudData.timestamp);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TryGetLatestPointCloudInternal(List<Vector4> pointCloudData, out uint version, out double timestamp);

    internal static bool TryGetLatestImageData(ref ImageData image)
    {
      if (image.planeData == null)
        image.planeData = new List<byte>();
      if (image.planeInfos == null)
        image.planeInfos = new List<ImageData.PlaneInfo>();
      image.planeData.Clear();
      return TangoDevice.TryGetLatestImageDataInternal(image.planeData, image.planeInfos, out image.width, out image.height, out image.format, out image.timestampNs, out image.metadata);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TryGetLatestImageDataInternal(List<byte> imageData, List<ImageData.PlaneInfo> planeInfos, out uint width, out uint height, out int format, out long timestampNs, out ImageData.CameraMetadata metadata);

    internal static extern bool isServiceConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern bool isServiceAvailable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static bool TryAcquireLatestPointCloud(ref NativePointCloud pointCloud)
    {
      return TangoDevice.Internal_TryAcquireLatestPointCloud(out pointCloud.version, out pointCloud.timestamp, out pointCloud.numPoints, out pointCloud.points, out pointCloud.nativePtr);
    }

    internal static void ReleasePointCloud(IntPtr pointCloudNativePtr)
    {
      TangoDevice.Internal_ReleasePointCloud(pointCloudNativePtr);
    }

    internal static bool TryAcquireLatestImageBuffer(ref NativeImage nativeImage)
    {
      if (nativeImage.planeInfos == null)
        nativeImage.planeInfos = new List<ImageData.PlaneInfo>();
      return TangoDevice.Internal_TryAcquireLatestImageBuffer(nativeImage.planeInfos, out nativeImage.width, out nativeImage.height, out nativeImage.format, out nativeImage.timestampNs, out nativeImage.planeData, out nativeImage.nativePtr, out nativeImage.metadata);
    }

    internal static void ReleaseImageBuffer(IntPtr imageBufferNativePtr)
    {
      TangoDevice.Internal_ReleaseImageBuffer(imageBufferNativePtr);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_TryAcquireLatestImageBuffer(List<ImageData.PlaneInfo> planeInfos, out uint width, out uint height, out int format, out long timestampNs, out IntPtr planeData, out IntPtr nativePtr, out ImageData.CameraMetadata metadata);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_TryAcquireLatestPointCloud(out uint version, out double timestamp, out uint numPoints, out IntPtr points, out IntPtr nativePtr);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_ReleasePointCloud(IntPtr pointCloudPtr);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_ReleaseImageBuffer(IntPtr imageBufferPtr);

    internal static string areaDescriptionUUID
    {
      get
      {
        return TangoDevice.m_AreaDescriptionUUID;
      }
      set
      {
        TangoDevice.m_AreaDescriptionUUID = value;
      }
    }

    internal static ARBackgroundRenderer backgroundRenderer
    {
      get
      {
        return TangoDevice.m_BackgroundRenderer;
      }
      set
      {
        if (value == null)
          return;
        if (TangoDevice.m_BackgroundRenderer != null)
        {
          ARBackgroundRenderer backgroundRenderer = TangoDevice.m_BackgroundRenderer;
          if (TangoDevice.\u003C\u003Ef__mg\u0024cache0 == null)
            TangoDevice.\u003C\u003Ef__mg\u0024cache0 = new Action(TangoDevice.OnBackgroundRendererChanged);
          Action fMgCache0 = TangoDevice.\u003C\u003Ef__mg\u0024cache0;
          backgroundRenderer.backgroundRendererChanged -= fMgCache0;
        }
        TangoDevice.m_BackgroundRenderer = value;
        ARBackgroundRenderer backgroundRenderer1 = TangoDevice.m_BackgroundRenderer;
        if (TangoDevice.\u003C\u003Ef__mg\u0024cache1 == null)
          TangoDevice.\u003C\u003Ef__mg\u0024cache1 = new Action(TangoDevice.OnBackgroundRendererChanged);
        Action fMgCache1 = TangoDevice.\u003C\u003Ef__mg\u0024cache1;
        backgroundRenderer1.backgroundRendererChanged += fMgCache1;
        TangoDevice.OnBackgroundRendererChanged();
      }
    }

    private static void OnBackgroundRendererChanged()
    {
      TangoDevice.SetBackgroundMaterial(TangoDevice.m_BackgroundRenderer.backgroundMaterial);
      TangoDevice.SetRenderMode(TangoDevice.m_BackgroundRenderer.mode);
    }

    internal static bool Connect(TangoConfig config)
    {
      string[] keys1;
      bool[] values1;
      TangoDevice.CopyDictionaryToArrays<bool>(config.m_boolParams, out keys1, out values1);
      string[] keys2;
      int[] values2;
      TangoDevice.CopyDictionaryToArrays<int>(config.m_intParams, out keys2, out values2);
      string[] keys3;
      long[] values3;
      TangoDevice.CopyDictionaryToArrays<long>(config.m_longParams, out keys3, out values3);
      string[] keys4;
      double[] values4;
      TangoDevice.CopyDictionaryToArrays<double>(config.m_doubleParams, out keys4, out values4);
      string[] keys5;
      string[] values5;
      TangoDevice.CopyDictionaryToArrays<string>(config.m_stringParams, out keys5, out values5);
      return TangoDevice.Connect(keys1, values1, keys2, values2, keys3, values3, keys4, values4, keys5, values5);
    }

    private static void CopyDictionaryToArrays<T>(Dictionary<string, T> dictionary, out string[] keys, out T[] values)
    {
      if (dictionary.Count == 0)
      {
        keys = (string[]) null;
        values = (T[]) null;
      }
      else
      {
        keys = new string[dictionary.Count];
        values = new T[dictionary.Count];
        int index = 0;
        foreach (KeyValuePair<string, T> keyValuePair in dictionary)
        {
          keys[index] = keyValuePair.Key;
          values[index++] = keyValuePair.Value;
        }
      }
    }
  }
}
