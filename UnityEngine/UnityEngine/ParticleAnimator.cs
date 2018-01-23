// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleAnimator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [Obsolete("This component is part of the legacy particle system, which is deprecated and will be removed in a future release. Use the ParticleSystem component instead.", false)]
  [RequireComponent(typeof (Transform))]
  public sealed class ParticleAnimator : Component
  {
    public extern bool doesAnimateColor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector3 worldRotationAxis
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_worldRotationAxis(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_worldRotationAxis(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_worldRotationAxis(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_worldRotationAxis(ref Vector3 value);

    public Vector3 localRotationAxis
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_localRotationAxis(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_localRotationAxis(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_localRotationAxis(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_localRotationAxis(ref Vector3 value);

    public extern float sizeGrow { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector3 rndForce
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_rndForce(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_rndForce(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_rndForce(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_rndForce(ref Vector3 value);

    public Vector3 force
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_force(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_force(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_force(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_force(ref Vector3 value);

    public extern float damping { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool autodestruct { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern Color[] colorAnimation { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
