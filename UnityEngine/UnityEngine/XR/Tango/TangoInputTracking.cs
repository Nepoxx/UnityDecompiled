// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.TangoInputTracking
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
  [UsedByNativeCode]
  internal static class TangoInputTracking
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_TryGetPoseAtTime(double time, ScreenOrientation screenOrientation, CoordinateFrame baseFrame, CoordinateFrame targetFrame, out PoseData pose);

    internal static bool TryGetPoseAtTime(out PoseData pose, CoordinateFrame baseFrame, CoordinateFrame targetFrame, double time, ScreenOrientation screenOrientation)
    {
      return TangoInputTracking.Internal_TryGetPoseAtTime(time, screenOrientation, baseFrame, targetFrame, out pose);
    }

    internal static bool TryGetPoseAtTime(out PoseData pose, CoordinateFrame baseFrame, CoordinateFrame targetFrame, double time = 0.0)
    {
      return TangoInputTracking.Internal_TryGetPoseAtTime(time, Screen.orientation, baseFrame, targetFrame, out pose);
    }

    internal static event Action<CoordinateFrame> trackingAcquired = null;

    internal static event Action<CoordinateFrame> trackingLost = null;

    [UsedByNativeCode]
    private static void InvokeTangoTrackingEvent(TangoInputTracking.TrackingStateEventType eventType, CoordinateFrame frame)
    {
      Action<CoordinateFrame> action;
      if (eventType != TangoInputTracking.TrackingStateEventType.TrackingAcquired)
      {
        if (eventType != TangoInputTracking.TrackingStateEventType.TrackingLost)
          throw new ArgumentException("TrackingEventHandler - Invalid EventType: " + (object) eventType);
        // ISSUE: reference to a compiler-generated field
        action = TangoInputTracking.trackingLost;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        action = TangoInputTracking.trackingAcquired;
      }
      if (action == null)
        return;
      action(frame);
    }

    private enum TrackingStateEventType
    {
      TrackingAcquired,
      TrackingLost,
    }
  }
}
