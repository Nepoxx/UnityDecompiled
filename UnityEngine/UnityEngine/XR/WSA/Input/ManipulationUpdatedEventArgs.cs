// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.ManipulationUpdatedEventArgs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Contains fields that are relevant when a manipulation gesture gets updated.</para>
  /// </summary>
  [RequiredByNativeCode]
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public struct ManipulationUpdatedEventArgs
  {
    internal InteractionSource m_Source;
    internal InteractionSourcePose m_SourcePose;
    internal Pose m_HeadPose;
    internal Vector3 m_CumulativeDelta;

    /// <summary>
    ///   <para>The InteractionSource (hand, controller, or user's voice) being used for the manipulation gesture.</para>
    /// </summary>
    public InteractionSource source
    {
      get
      {
        return this.m_Source;
      }
    }

    /// <summary>
    ///   <para>Pose data of the interaction source at the time of the gesture.</para>
    /// </summary>
    public InteractionSourcePose sourcePose
    {
      get
      {
        return this.m_SourcePose;
      }
    }

    /// <summary>
    ///   <para>Head pose of the user at the time of the gesture.</para>
    /// </summary>
    public Pose headPose
    {
      get
      {
        return this.m_HeadPose;
      }
    }

    /// <summary>
    ///   <para>Total distance moved since the beginning of the manipulation gesture.</para>
    /// </summary>
    public Vector3 cumulativeDelta
    {
      get
      {
        return this.m_CumulativeDelta;
      }
    }
  }
}
