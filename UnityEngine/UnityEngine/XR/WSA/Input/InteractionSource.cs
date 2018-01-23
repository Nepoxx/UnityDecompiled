// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSource
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Represents one detected instance of an interaction source (hand, controller, or user's voice) that can cause interactions and gestures.</para>
  /// </summary>
  [RequiredByNativeCode]
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public struct InteractionSource
  {
    internal uint m_Id;
    internal InteractionSourceKind m_SourceKind;
    internal InteractionSourceHandedness m_Handedness;
    internal InteractionSourceFlags m_Flags;
    internal ushort m_VendorId;
    internal ushort m_ProductId;
    internal ushort m_ProductVersion;

    public override bool Equals(object obj)
    {
      InteractionSource? nullable = obj as InteractionSource?;
      if (!nullable.HasValue)
        return false;
      return (int) nullable.Value.m_Id == (int) this.m_Id;
    }

    public override int GetHashCode()
    {
      return (int) this.m_Id;
    }

    /// <summary>
    ///   <para>The identifier for the interaction source (hand, controller, or user's voice).</para>
    /// </summary>
    public uint id
    {
      get
      {
        return this.m_Id;
      }
    }

    /// <summary>
    ///   <para>Specifies the kind of an interaction source.</para>
    /// </summary>
    public InteractionSourceKind kind
    {
      get
      {
        return this.m_SourceKind;
      }
    }

    /// <summary>
    ///   <para>Denotes which hand was used as the input source.</para>
    /// </summary>
    public InteractionSourceHandedness handedness
    {
      get
      {
        return this.m_Handedness;
      }
    }

    /// <summary>
    ///   <para>This property returns true when the interaction source has at least one grasp button, and false if otherwise.</para>
    /// </summary>
    public bool supportsGrasp
    {
      get
      {
        return (this.m_Flags & InteractionSourceFlags.SupportsGrasp) != InteractionSourceFlags.None;
      }
    }

    /// <summary>
    ///   <para>This property returns true when the interaction source has a menu button, and false if otherwise.</para>
    /// </summary>
    public bool supportsMenu
    {
      get
      {
        return (this.m_Flags & InteractionSourceFlags.SupportsMenu) != InteractionSourceFlags.None;
      }
    }

    /// <summary>
    ///   <para>This property returns true if the interaction source has a separate pose for the pointer, and false if otherwise.</para>
    /// </summary>
    public bool supportsPointing
    {
      get
      {
        return (this.m_Flags & InteractionSourceFlags.SupportsPointing) != InteractionSourceFlags.None;
      }
    }

    /// <summary>
    ///   <para>Returns true if the interaction source has a thumbstick, and false if otherwise.</para>
    /// </summary>
    public bool supportsThumbstick
    {
      get
      {
        return (this.m_Flags & InteractionSourceFlags.SupportsThumbstick) != InteractionSourceFlags.None;
      }
    }

    /// <summary>
    ///   <para>Returns true if the interaction source has a touchpad, and false if otherwise.</para>
    /// </summary>
    public bool supportsTouchpad
    {
      get
      {
        return (this.m_Flags & InteractionSourceFlags.SupportsTouchpad) != InteractionSourceFlags.None;
      }
    }

    /// <summary>
    ///   <para>All interaction sources developed by the same company will have the same vendor ID.</para>
    /// </summary>
    public ushort vendorId
    {
      get
      {
        return this.m_VendorId;
      }
    }

    /// <summary>
    ///   <para>Following the make and model nomenclature of cars, this equates to the model number.</para>
    /// </summary>
    public ushort productId
    {
      get
      {
        return this.m_ProductId;
      }
    }

    /// <summary>
    ///   <para>Following the make and model nomenclature of cars, this would be a minor update to the model.</para>
    /// </summary>
    public ushort productVersion
    {
      get
      {
        return this.m_ProductVersion;
      }
    }
  }
}
