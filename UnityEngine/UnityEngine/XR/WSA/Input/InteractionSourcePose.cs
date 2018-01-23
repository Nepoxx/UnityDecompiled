// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourcePose
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Pose data of the interaction source at the time of either the gesture or interaction.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct InteractionSourcePose
  {
    internal Quaternion m_GripRotation;
    internal Quaternion m_PointerRotation;
    internal Vector3 m_GripPosition;
    internal Vector3 m_PointerPosition;
    internal Vector3 m_Velocity;
    internal Vector3 m_AngularVelocity;
    internal InteractionSourcePositionAccuracy m_PositionAccuracy;
    internal InteractionSourcePoseFlags m_Flags;

    public bool TryGetPosition(out Vector3 position)
    {
      return this.TryGetPosition(out position, InteractionSourceNode.Grip);
    }

    public bool TryGetPosition(out Vector3 position, InteractionSourceNode node)
    {
      if (node == InteractionSourceNode.Grip)
      {
        position = this.m_GripPosition;
        return (this.m_Flags & InteractionSourcePoseFlags.HasGripPosition) != InteractionSourcePoseFlags.None;
      }
      position = this.m_PointerPosition;
      return (this.m_Flags & InteractionSourcePoseFlags.HasPointerPosition) != InteractionSourcePoseFlags.None;
    }

    public bool TryGetRotation(out Quaternion rotation, InteractionSourceNode node = InteractionSourceNode.Grip)
    {
      if (node == InteractionSourceNode.Grip)
      {
        rotation = this.m_GripRotation;
        return (this.m_Flags & InteractionSourcePoseFlags.HasGripRotation) != InteractionSourcePoseFlags.None;
      }
      rotation = this.m_PointerRotation;
      return (this.m_Flags & InteractionSourcePoseFlags.HasPointerRotation) != InteractionSourcePoseFlags.None;
    }

    public bool TryGetForward(out Vector3 forward, InteractionSourceNode node = InteractionSourceNode.Grip)
    {
      Quaternion rotation1;
      bool rotation2 = this.TryGetRotation(out rotation1, node);
      forward = rotation1 * Vector3.forward;
      return rotation2;
    }

    public bool TryGetRight(out Vector3 right, InteractionSourceNode node = InteractionSourceNode.Grip)
    {
      Quaternion rotation1;
      bool rotation2 = this.TryGetRotation(out rotation1, node);
      right = rotation1 * Vector3.right;
      return rotation2;
    }

    public bool TryGetUp(out Vector3 up, InteractionSourceNode node = InteractionSourceNode.Grip)
    {
      Quaternion rotation1;
      bool rotation2 = this.TryGetRotation(out rotation1, node);
      up = rotation1 * Vector3.up;
      return rotation2;
    }

    public bool TryGetVelocity(out Vector3 velocity)
    {
      velocity = this.m_Velocity;
      return (this.m_Flags & InteractionSourcePoseFlags.HasVelocity) != InteractionSourcePoseFlags.None;
    }

    public bool TryGetAngularVelocity(out Vector3 angularVelocity)
    {
      angularVelocity = this.m_AngularVelocity;
      return (this.m_Flags & InteractionSourcePoseFlags.HasAngularVelocity) != InteractionSourcePoseFlags.None;
    }

    /// <summary>
    ///   <para>The position-tracking accuracy of the interaction source.</para>
    /// </summary>
    public InteractionSourcePositionAccuracy positionAccuracy
    {
      get
      {
        return this.m_PositionAccuracy;
      }
    }
  }
}
