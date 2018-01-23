// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativeKeyEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngineInternal.Input
{
  [StructLayout(LayoutKind.Explicit, Size = 24)]
  public struct NativeKeyEvent
  {
    [FieldOffset(0)]
    public NativeInputEvent baseEvent;
    [FieldOffset(20)]
    public KeyCode key;

    public static NativeKeyEvent Down(int deviceId, double time, KeyCode key)
    {
      NativeKeyEvent nativeKeyEvent;
      nativeKeyEvent.baseEvent = new NativeInputEvent(NativeInputEventType.KeyDown, 24, deviceId, time);
      nativeKeyEvent.key = key;
      return nativeKeyEvent;
    }

    public static NativeKeyEvent Up(int deviceId, double time, KeyCode key)
    {
      NativeKeyEvent nativeKeyEvent;
      nativeKeyEvent.baseEvent = new NativeInputEvent(NativeInputEventType.KeyUp, 24, deviceId, time);
      nativeKeyEvent.key = key;
      return nativeKeyEvent;
    }
  }
}
