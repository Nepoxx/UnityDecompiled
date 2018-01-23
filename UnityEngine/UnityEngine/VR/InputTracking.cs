// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.InputTracking
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.VR
{
  [Obsolete("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead (UnityUpgradable) -> UnityEngine.XR.InputTracking", true)]
  public static class InputTracking
  {
    public static Vector3 GetLocalPosition(VRNode node)
    {
      throw new NotSupportedException("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead.");
    }

    public static void Recenter()
    {
      throw new NotSupportedException("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead.");
    }

    public static string GetNodeName(ulong uniqueID)
    {
      throw new NotSupportedException("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead.");
    }

    public static void GetNodeStates(List<VRNodeState> nodeStates)
    {
      throw new NotSupportedException("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead.");
    }

    public static bool disablePositionalTracking
    {
      get
      {
        throw new NotSupportedException("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead.");
      }
      set
      {
        throw new NotSupportedException("InputTracking has been moved.  Use UnityEngine.XR.InputTracking instead.");
      }
    }
  }
}
