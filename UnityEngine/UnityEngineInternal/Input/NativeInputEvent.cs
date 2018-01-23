// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativeInputEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
  [StructLayout(LayoutKind.Explicit, Size = 20)]
  public struct NativeInputEvent
  {
    [FieldOffset(0)]
    public NativeInputEventType type;
    [FieldOffset(4)]
    public int sizeInBytes;
    [FieldOffset(8)]
    public int deviceId;
    [FieldOffset(12)]
    public double time;

    public NativeInputEvent(NativeInputEventType type, int sizeInBytes, int deviceId, double time)
    {
      this.type = type;
      this.sizeInBytes = sizeInBytes;
      this.deviceId = deviceId;
      this.time = time;
    }
  }
}
