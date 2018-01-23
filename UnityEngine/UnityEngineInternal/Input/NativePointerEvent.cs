// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.Input.NativePointerEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngineInternal.Input
{
  [StructLayout(LayoutKind.Explicit, Size = 80)]
  public struct NativePointerEvent
  {
    [FieldOffset(0)]
    public NativeInputEvent baseEvent;
    [FieldOffset(20)]
    public int pointerId;
    [FieldOffset(24)]
    public Vector3 position;
    [FieldOffset(36)]
    public Vector3 delta;
    [FieldOffset(48)]
    public float pressure;
    [FieldOffset(52)]
    public float twist;
    [FieldOffset(56)]
    public Vector2 tilt;
    [FieldOffset(64)]
    public Vector3 radius;
    [FieldOffset(76)]
    public int displayIndex;

    public static NativePointerEvent Down(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default (Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default (Vector2), Vector3 radius = default (Vector3), int displayIndex = 0)
    {
      NativePointerEvent nativePointerEvent;
      nativePointerEvent.baseEvent = new NativeInputEvent(NativeInputEventType.PointerDown, 80, deviceId, time);
      nativePointerEvent.pointerId = pointerId;
      nativePointerEvent.position = position;
      nativePointerEvent.delta = delta;
      nativePointerEvent.pressure = pressure;
      nativePointerEvent.twist = twist;
      nativePointerEvent.tilt = tilt;
      nativePointerEvent.radius = radius;
      nativePointerEvent.displayIndex = displayIndex;
      return nativePointerEvent;
    }

    public static NativePointerEvent Move(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default (Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default (Vector2), Vector3 radius = default (Vector3), int displayIndex = 0)
    {
      NativePointerEvent nativePointerEvent;
      nativePointerEvent.baseEvent = new NativeInputEvent(NativeInputEventType.PointerMove, 80, deviceId, time);
      nativePointerEvent.pointerId = pointerId;
      nativePointerEvent.position = position;
      nativePointerEvent.delta = delta;
      nativePointerEvent.pressure = pressure;
      nativePointerEvent.twist = twist;
      nativePointerEvent.tilt = tilt;
      nativePointerEvent.radius = radius;
      nativePointerEvent.displayIndex = displayIndex;
      return nativePointerEvent;
    }

    public static NativePointerEvent Up(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default (Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default (Vector2), Vector3 radius = default (Vector3), int displayIndex = 0)
    {
      NativePointerEvent nativePointerEvent;
      nativePointerEvent.baseEvent = new NativeInputEvent(NativeInputEventType.PointerUp, 80, deviceId, time);
      nativePointerEvent.pointerId = pointerId;
      nativePointerEvent.position = position;
      nativePointerEvent.delta = delta;
      nativePointerEvent.pressure = pressure;
      nativePointerEvent.twist = twist;
      nativePointerEvent.tilt = tilt;
      nativePointerEvent.radius = radius;
      nativePointerEvent.displayIndex = displayIndex;
      return nativePointerEvent;
    }

    public static NativePointerEvent Cancelled(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default (Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default (Vector2), Vector3 radius = default (Vector3), int displayIndex = 0)
    {
      NativePointerEvent nativePointerEvent;
      nativePointerEvent.baseEvent = new NativeInputEvent(NativeInputEventType.PointerCancelled, 80, deviceId, time);
      nativePointerEvent.pointerId = pointerId;
      nativePointerEvent.position = position;
      nativePointerEvent.delta = delta;
      nativePointerEvent.pressure = pressure;
      nativePointerEvent.twist = twist;
      nativePointerEvent.tilt = tilt;
      nativePointerEvent.radius = radius;
      nativePointerEvent.displayIndex = displayIndex;
      return nativePointerEvent;
    }
  }
}
