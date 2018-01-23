// Decompiled with JetBrains decompiler
// Type: UnityEngine.Apple.ReplayKit.ReplayKit
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Apple.ReplayKit
{
  public static class ReplayKit
  {
    public static extern bool APIAvailable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool broadcastingAPIAvailable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool recordingAvailable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isRecording { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isBroadcasting { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool cameraEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool microphoneEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string broadcastURL { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern string lastError { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool StartRecordingImpl(bool enableMicrophone, bool enableCamera);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void StartBroadcastingImpl(UnityEngine.Apple.ReplayKit.ReplayKit.BroadcastStatusCallback callback, bool enableMicrophone, bool enableCamera);

    public static bool StartRecording([DefaultValue("false")] bool enableMicrophone, [DefaultValue("false")] bool enableCamera)
    {
      return UnityEngine.Apple.ReplayKit.ReplayKit.StartRecordingImpl(enableMicrophone, enableCamera);
    }

    public static bool StartRecording(bool enableMicrophone)
    {
      return UnityEngine.Apple.ReplayKit.ReplayKit.StartRecording(enableMicrophone, false);
    }

    public static bool StartRecording()
    {
      return UnityEngine.Apple.ReplayKit.ReplayKit.StartRecording(false, false);
    }

    public static void StartBroadcasting(UnityEngine.Apple.ReplayKit.ReplayKit.BroadcastStatusCallback callback, [DefaultValue("false")] bool enableMicrophone, [DefaultValue("false")] bool enableCamera)
    {
      UnityEngine.Apple.ReplayKit.ReplayKit.StartBroadcastingImpl(callback, enableMicrophone, enableCamera);
    }

    public static void StartBroadcasting(UnityEngine.Apple.ReplayKit.ReplayKit.BroadcastStatusCallback callback, bool enableMicrophone)
    {
      UnityEngine.Apple.ReplayKit.ReplayKit.StartBroadcasting(callback, enableMicrophone, false);
    }

    public static void StartBroadcasting(UnityEngine.Apple.ReplayKit.ReplayKit.BroadcastStatusCallback callback)
    {
      UnityEngine.Apple.ReplayKit.ReplayKit.StartBroadcasting(callback, false, false);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool StopRecording();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopBroadcasting();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Preview();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Discard();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ShowCameraPreviewAt(float posX, float posY);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void HideCameraPreview();

    public delegate void BroadcastStatusCallback(bool hasStarted, string errorMessage);
  }
}
