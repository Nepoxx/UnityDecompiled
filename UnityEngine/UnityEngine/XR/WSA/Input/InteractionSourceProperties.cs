// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourceProperties
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Represents the set of properties available to explore the current state of a hand or controller.</para>
  /// </summary>
  [RequiredByNativeCode]
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public struct InteractionSourceProperties
  {
    internal double m_SourceLossRisk;
    internal Vector3 m_SourceLossMitigationDirection;
    internal InteractionSourcePose m_SourcePose;

    /// <summary>
    ///   <para>Gets the risk that detection of the hand will be lost as a value from 0.0 to 1.0.</para>
    /// </summary>
    public double sourceLossRisk
    {
      get
      {
        return this.m_SourceLossRisk;
      }
    }

    /// <summary>
    ///   <para>The direction you should suggest that the user move their hand if it is nearing the edge of the detection area.</para>
    /// </summary>
    public Vector3 sourceLossMitigationDirection
    {
      get
      {
        return this.m_SourceLossMitigationDirection;
      }
    }

    /// <summary>
    ///   <para>The position and velocity of the hand, expressed in the specified coordinate system - this has been deprecated. Use InteractionSourcePose instead.</para>
    /// </summary>
    [Obsolete("InteractionSourceProperties.location is deprecated, and will be removed in a future release. Use InteractionSourceState.sourcePose instead.", true)]
    public InteractionSourceLocation location
    {
      get
      {
        return new InteractionSourceLocation();
      }
    }

    [Obsolete("InteractionSourceProperties.sourcePose is deprecated, and will be removed in a future release. Use InteractionSourceState.sourcePose instead.", false)]
    public InteractionSourcePose sourcePose
    {
      get
      {
        return this.m_SourcePose;
      }
    }
  }
}
