// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRNodeState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  [Obsolete("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead (UnityUpgradable) -> UnityEngine.XR.XRNodeState", true)]
  public struct VRNodeState
  {
    public ulong uniqueID
    {
      get
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public VRNode nodeType
    {
      get
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public bool tracked
    {
      get
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public Vector3 position
    {
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public Quaternion rotation
    {
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public Vector3 velocity
    {
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public Vector3 angularVelocity
    {
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public Vector3 acceleration
    {
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public Vector3 angularAcceleration
    {
      set
      {
        throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
      }
    }

    public bool TryGetPosition(out Vector3 position)
    {
      position = new Vector3();
      throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
    }

    public bool TryGetRotation(out Quaternion rotation)
    {
      rotation = new Quaternion();
      throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
    }

    public bool TryGetVelocity(out Vector3 velocity)
    {
      velocity = new Vector3();
      throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
    }

    public bool TryGetAngularVelocity(out Vector3 angularVelocity)
    {
      angularVelocity = new Vector3();
      throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
    }

    public bool TryGetAcceleration(out Vector3 acceleration)
    {
      acceleration = new Vector3();
      throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
    }

    public bool TryGetAngularAcceleration(out Vector3 angularAcceleration)
    {
      angularAcceleration = new Vector3();
      throw new NotSupportedException("VRNodeState has been moved and renamed.  Use UnityEngine.XR.XRNodeState instead.");
    }
  }
}
