// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cloth
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Cloth class provides an interface to cloth simulation physics.</para>
  /// </summary>
  [RequireComponent(typeof (Transform), typeof (SkinnedMeshRenderer))]
  [NativeClass("Unity::Cloth")]
  public sealed class Cloth : Component
  {
    public void GetVirtualParticleIndices(List<uint> indices)
    {
      if (indices == null)
        throw new ArgumentNullException(nameof (indices));
      this.GetVirtualParticleIndicesMono((object) indices);
    }

    public void SetVirtualParticleIndices(List<uint> indices)
    {
      if (indices == null)
        throw new ArgumentNullException(nameof (indices));
      this.SetVirtualParticleIndicesMono((object) indices);
    }

    public void GetVirtualParticleWeights(List<Vector3> weights)
    {
      if (weights == null)
        throw new ArgumentNullException(nameof (weights));
      this.GetVirtualParticleWeightsMono((object) weights);
    }

    public void SetVirtualParticleWeights(List<Vector3> weights)
    {
      if (weights == null)
        throw new ArgumentNullException(nameof (weights));
      this.SetVirtualParticleWeightsMono((object) weights);
    }

    public void GetSelfAndInterCollisionIndices(List<uint> indices)
    {
      if (indices == null)
        throw new ArgumentNullException(nameof (indices));
      this.GetSelfAndInterCollisionIndicesMono((object) indices);
    }

    public void SetSelfAndInterCollisionIndices(List<uint> indices)
    {
      if (indices == null)
        throw new ArgumentNullException(nameof (indices));
      this.SetSelfAndInterCollisionIndicesMono((object) indices);
    }

    /// <summary>
    ///   <para>Cloth's sleep threshold.</para>
    /// </summary>
    public extern float sleepThreshold { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Bending stiffness of the cloth.</para>
    /// </summary>
    public extern float bendingStiffness { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Stretching stiffness of the cloth.</para>
    /// </summary>
    public extern float stretchingStiffness { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Damp cloth motion.</para>
    /// </summary>
    public extern float damping { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A constant, external acceleration applied to the cloth.</para>
    /// </summary>
    public Vector3 externalAcceleration
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_externalAcceleration(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_externalAcceleration(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_externalAcceleration(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_externalAcceleration(ref Vector3 value);

    /// <summary>
    ///   <para>A random, external acceleration applied to the cloth.</para>
    /// </summary>
    public Vector3 randomAcceleration
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_randomAcceleration(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_randomAcceleration(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_randomAcceleration(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_randomAcceleration(ref Vector3 value);

    /// <summary>
    ///   <para>Should gravity affect the cloth simulation?</para>
    /// </summary>
    public extern bool useGravity { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Deprecated. Cloth.selfCollisions is no longer supported since Unity 5.0.", true)]
    public extern bool selfCollision { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this cloth enabled?</para>
    /// </summary>
    public extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current vertex positions of the cloth object.</para>
    /// </summary>
    public extern Vector3[] vertices { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current normals of the cloth object.</para>
    /// </summary>
    public extern Vector3[] normals { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The friction of the cloth when colliding with the character.</para>
    /// </summary>
    public extern float friction { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much to increase mass of colliding particles.</para>
    /// </summary>
    public extern float collisionMassScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("useContinuousCollision is no longer supported, use enableContinuousCollision instead")]
    public extern float useContinuousCollision { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable continuous collision to improve collision stability.</para>
    /// </summary>
    public extern bool enableContinuousCollision { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Add one virtual particle per triangle to improve collision stability.</para>
    /// </summary>
    public extern float useVirtualParticles { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clear the pending transform changes from affecting the cloth simulation.</para>
    /// </summary>
    public void ClearTransformMotion()
    {
      Cloth.INTERNAL_CALL_ClearTransformMotion(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ClearTransformMotion(Cloth self);

    /// <summary>
    ///   <para>The cloth skinning coefficients used to set up how the cloth interacts with the skinned mesh.</para>
    /// </summary>
    public extern ClothSkinningCoefficient[] coefficients { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much world-space movement of the character will affect cloth vertices.</para>
    /// </summary>
    public extern float worldVelocityScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much world-space acceleration of the character will affect cloth vertices.</para>
    /// </summary>
    public extern float worldAccelerationScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Fade the cloth simulation in or out.</para>
    /// </summary>
    /// <param name="enabled">Fading enabled or not.</param>
    /// <param name="interpolationTime"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetEnabledFading(bool enabled, [DefaultValue("0.5f")] float interpolationTime);

    [ExcludeFromDocs]
    public void SetEnabledFading(bool enabled)
    {
      float interpolationTime = 0.5f;
      this.SetEnabledFading(enabled, interpolationTime);
    }

    [Obsolete("Parameter solverFrequency is obsolete and no longer supported. Please use clothSolverFrequency instead.")]
    public bool solverFrequency
    {
      get
      {
        return (double) this.clothSolverFrequency > 0.0;
      }
      set
      {
        this.clothSolverFrequency = !value ? 0.0f : 120f;
      }
    }

    /// <summary>
    ///   <para>Number of cloth solver iterations per second.</para>
    /// </summary>
    public extern float clothSolverFrequency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An array of CapsuleColliders which this Cloth instance should collide with.</para>
    /// </summary>
    public extern CapsuleCollider[] capsuleColliders { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An array of ClothSphereColliderPairs which this Cloth instance should collide with.</para>
    /// </summary>
    public extern ClothSphereColliderPair[] sphereColliders { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use Tether Anchors.</para>
    /// </summary>
    public extern bool useTethers { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the stiffness frequency parameter.</para>
    /// </summary>
    public extern float stiffnessFrequency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Minimum distance at which two cloth particles repel each other (default: 0.0).</para>
    /// </summary>
    public extern float selfCollisionDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Self-collision stiffness defines how strong the separating impulse should be for colliding particles.</para>
    /// </summary>
    public extern float selfCollisionStiffness { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetVirtualParticleIndicesMono(object indicesOutList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetVirtualParticleIndicesMono(object indicesInList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetVirtualParticleWeightsMono(object weightsOutList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetVirtualParticleWeightsMono(object weightsInList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetSelfAndInterCollisionIndicesMono(object indicesOutList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetSelfAndInterCollisionIndicesMono(object indicesInList);
  }
}
