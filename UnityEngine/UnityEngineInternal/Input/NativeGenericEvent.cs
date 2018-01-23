// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativeGenericEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
  [StructLayout(LayoutKind.Explicit, Size = 36)]
  public struct NativeGenericEvent
  {
    [FieldOffset(0)]
    public NativeInputEvent baseEvent;
    [FieldOffset(20)]
    public int controlIndex;
    [FieldOffset(24)]
    public int rawValue;
    [FieldOffset(28)]
    public double scaledValue;

    public static NativeGenericEvent Value(int deviceId, double time, int controlIndex, int rawValue, double scaledValue)
    {
      NativeGenericEvent nativeGenericEvent;
      nativeGenericEvent.baseEvent = new NativeInputEvent(NativeInputEventType.Generic, 36, deviceId, time);
      nativeGenericEvent.controlIndex = controlIndex;
      nativeGenericEvent.rawValue = rawValue;
      nativeGenericEvent.scaledValue = scaledValue;
      return nativeGenericEvent;
    }
  }
}
