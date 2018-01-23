// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativeTextEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
  [StructLayout(LayoutKind.Explicit, Size = 24)]
  public struct NativeTextEvent
  {
    [FieldOffset(0)]
    public NativeInputEvent baseEvent;
    [FieldOffset(20)]
    public int utf32Character;

    public static NativeTextEvent Character(int deviceId, double time, int utf32)
    {
      NativeTextEvent nativeTextEvent;
      nativeTextEvent.baseEvent = new NativeInputEvent(NativeInputEventType.Text, 24, deviceId, time);
      nativeTextEvent.utf32Character = utf32;
      return nativeTextEvent;
    }
  }
}
