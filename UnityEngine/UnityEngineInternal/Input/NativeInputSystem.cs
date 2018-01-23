// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativeInputSystem
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Input
{
  public class NativeInputSystem
  {
    public static NativeUpdateCallback onUpdate;
    public static NativeEventCallback onEvents;
    private static NativeDeviceDiscoveredCallback s_OnDeviceDiscoveredCallback;

    static NativeInputSystem()
    {
      NativeInputSystem.hasDeviceDiscoveredCallback = false;
    }

    public static event NativeDeviceDiscoveredCallback onDeviceDiscovered
    {
      add
      {
        NativeInputSystem.s_OnDeviceDiscoveredCallback += value;
        NativeInputSystem.hasDeviceDiscoveredCallback = NativeInputSystem.s_OnDeviceDiscoveredCallback != null;
      }
      remove
      {
        NativeInputSystem.s_OnDeviceDiscoveredCallback -= value;
        NativeInputSystem.hasDeviceDiscoveredCallback = NativeInputSystem.s_OnDeviceDiscoveredCallback != null;
      }
    }

    [RequiredByNativeCode]
    internal static void NotifyUpdate(NativeInputUpdateType updateType)
    {
      NativeUpdateCallback onUpdate = NativeInputSystem.onUpdate;
      if (onUpdate == null)
        return;
      onUpdate(updateType);
    }

    [RequiredByNativeCode]
    internal static void NotifyEvents(int eventCount, IntPtr eventData)
    {
      NativeEventCallback onEvents = NativeInputSystem.onEvents;
      if (onEvents == null)
        return;
      onEvents(eventCount, eventData);
    }

    [RequiredByNativeCode]
    internal static void NotifyDeviceDiscovered(NativeInputDeviceInfo deviceInfo)
    {
      NativeDeviceDiscoveredCallback discoveredCallback = NativeInputSystem.s_OnDeviceDiscoveredCallback;
      if (discoveredCallback == null)
        return;
      discoveredCallback(deviceInfo);
    }

    public static extern double zeroEventTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool hasDeviceDiscoveredCallback { [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static void SendInput<TInputEvent>(TInputEvent inputEvent) where TInputEvent : struct
    {
      NativeInputSystem.SendInput(UnsafeUtility.AddressOf<TInputEvent>(ref inputEvent));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SendInput(IntPtr inputEvent);

    public static bool SendOutput<TOutputEvent>(int deviceId, int type, TOutputEvent outputEvent) where TOutputEvent : struct
    {
      return NativeInputSystem.SendOutput(deviceId, type, UnsafeUtility.SizeOf<TOutputEvent>(), UnsafeUtility.AddressOf<TOutputEvent>(ref outputEvent));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SendOutput(int deviceId, int type, int sizeInBytes, IntPtr data);

    public static string GetDeviceConfiguration(int deviceId)
    {
      return (string) null;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetControlConfiguration(int deviceId, int controlIndex);

    public static void SetPollingFrequency(float hertz)
    {
      if ((double) hertz < 1.0)
        throw new ArgumentException("Polling frequency cannot be less than 1Hz");
      NativeInputSystem.SetPollingFrequencyInternal(hertz);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetPollingFrequencyInternal(float hertz);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SendEvents();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Update(NativeInputUpdateType updateType);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int ReportNewInputDevice(string descriptor);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ReportInputDeviceDisconnect(int nativeDeviceId);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ReportInputDeviceReconnect(int nativeDeviceId);
  }
}
