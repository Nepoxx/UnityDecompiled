// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativeClickEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
  [StructLayout(LayoutKind.Explicit, Size = 32)]
  public struct NativeClickEvent
  {
    [FieldOffset(0)]
    public NativeInputEvent baseEvent;
    [FieldOffset(20)]
    public bool isPressed;
    [FieldOffset(24)]
    public int controlIndex;
    [FieldOffset(28)]
    public int clickCount;

    public static NativeClickEvent Press(int deviceId, double time, int controlIndex, int clickCount)
    {
      NativeClickEvent nativeClickEvent;
      nativeClickEvent.baseEvent = new NativeInputEvent(NativeInputEventType.Click, 32, deviceId, time);
      nativeClickEvent.isPressed = true;
      nativeClickEvent.controlIndex = controlIndex;
      nativeClickEvent.clickCount = clickCount;
      return nativeClickEvent;
    }

    public static NativeClickEvent Release(int deviceId, double time, int controlIndex, int clickCount)
    {
      NativeClickEvent nativeClickEvent;
      nativeClickEvent.baseEvent = new NativeInputEvent(NativeInputEventType.Click, 32, deviceId, time);
      nativeClickEvent.isPressed = false;
      nativeClickEvent.controlIndex = controlIndex;
      nativeClickEvent.clickCount = clickCount;
      return nativeClickEvent;
    }
  }
}
