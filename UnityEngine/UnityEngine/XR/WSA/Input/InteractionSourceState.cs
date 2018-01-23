// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourceState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Represents a snapshot of the state of a spatial interaction source (hand, voice or controller) at a given time.</para>
  /// </summary>
  [RequiredByNativeCode]
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public struct InteractionSourceState
  {
    internal InteractionSourceProperties m_Properties;
    internal InteractionSource m_Source;
    internal Pose m_HeadPose;
    internal Vector2 m_ThumbstickPosition;
    internal Vector2 m_TouchpadPosition;
    internal float m_SelectPressedAmount;
    internal InteractionSourceStateFlags m_Flags;

    /// <summary>
    ///   <para>True if the source is in the pressed state.</para>
    /// </summary>
    public bool anyPressed
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.AnyPressed) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>Head pose of the user at the time of the interaction.</para>
    /// </summary>
    public Pose headPose
    {
      get
      {
        return this.m_HeadPose;
      }
    }

    /// <summary>
    ///   <para>Additional properties to explore the state of the interaction source.</para>
    /// </summary>
    public InteractionSourceProperties properties
    {
      get
      {
        return this.m_Properties;
      }
    }

    /// <summary>
    ///   <para>The interaction source that this state describes.</para>
    /// </summary>
    public InteractionSource source
    {
      get
      {
        return this.m_Source;
      }
    }

    /// <summary>
    ///   <para>Pose data of the interaction source at the time of the interaction.</para>
    /// </summary>
    public InteractionSourcePose sourcePose
    {
      get
      {
        return this.m_Properties.m_SourcePose;
      }
    }

    /// <summary>
    ///   <para>Normalized amount ([0, 1]) representing how much select is pressed.</para>
    /// </summary>
    public float selectPressedAmount
    {
      get
      {
        return this.m_SelectPressedAmount;
      }
    }

    /// <summary>
    ///   <para>Depending on the InteractionSourceType of the interaction source, this returning true could represent a number of equivalent things: main button on a clicker, air-tap on a hand, and the trigger on a motion controller. For hands, a select-press represents the user's index finger in the down position. For motion controllers, a select-press represents the controller's index-finger trigger (or primary face button, if no trigger) being fully pressed. Note that a voice command of "Select" causes an instant press and release, so you cannot poll for a voice press using this property - instead, you must use GestureRecognizer and subscribe to the Tapped event, or subscribe to the InteractionSourcePressed event from InteractionManager.</para>
    /// </summary>
    public bool selectPressed
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.SelectPressed) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>Whether or not the menu button is pressed.</para>
    /// </summary>
    public bool menuPressed
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.MenuPressed) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>Whether the controller is grasped.</para>
    /// </summary>
    public bool grasped
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.Grasped) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>Whether or not the touchpad is touched.</para>
    /// </summary>
    public bool touchpadTouched
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.TouchpadTouched) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>Whether or not the touchpad is pressed, as if a button.</para>
    /// </summary>
    public bool touchpadPressed
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.TouchpadPressed) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>Normalized coordinates for the position of a touchpad interaction.</para>
    /// </summary>
    public Vector2 touchpadPosition
    {
      get
      {
        return this.m_TouchpadPosition;
      }
    }

    /// <summary>
    ///   <para>Normalized coordinates for the position of a thumbstick.</para>
    /// </summary>
    public Vector2 thumbstickPosition
    {
      get
      {
        return this.m_ThumbstickPosition;
      }
    }

    /// <summary>
    ///   <para>Whether or not the thumbstick is pressed.</para>
    /// </summary>
    public bool thumbstickPressed
    {
      get
      {
        return (this.m_Flags & InteractionSourceStateFlags.ThumbstickPressed) != InteractionSourceStateFlags.None;
      }
    }

    /// <summary>
    ///   <para>True if the source is in the pressed state, false otherwise.</para>
    /// </summary>
    [Obsolete("InteractionSourceState.pressed is deprecated, and will be removed in a future release. Use InteractionSourceState.selectPressed instead. (UnityUpgradable) -> selectPressed", false)]
    public bool pressed
    {
      get
      {
        return this.selectPressed;
      }
    }

    /// <summary>
    ///   <para>The Ray at the time represented by this InteractionSourceState.</para>
    /// </summary>
    [Obsolete("InteractionSourceState.headRay is obsolete - update your scripts to use InteractionSourceLocation.headPose instead.", false)]
    public Ray headRay
    {
      get
      {
        return new Ray(this.m_HeadPose.position, this.m_HeadPose.rotation * Vector3.forward);
      }
    }
  }
}
