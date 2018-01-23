// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Renders particles on to the screen (Shuriken).</para>
  /// </summary>
  [RequireComponent(typeof (Transform))]
  public sealed class ParticleSystemRenderer : Renderer
  {
    /// <summary>
    ///   <para>How particles are drawn.</para>
    /// </summary>
    public extern ParticleSystemRenderMode renderMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched in their direction of motion.</para>
    /// </summary>
    public extern float lengthScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched depending on "how fast they move".</para>
    /// </summary>
    public extern float velocityScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched depending on the Camera's speed.</para>
    /// </summary>
    public extern float cameraVelocityScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are billboard particle normals oriented towards the camera.</para>
    /// </summary>
    public extern float normalDirection { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Control the direction that particles face.</para>
    /// </summary>
    public extern ParticleSystemRenderSpace alignment { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Modify the pivot point used for rotating particles.</para>
    /// </summary>
    public Vector3 pivot
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_pivot(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_pivot(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_pivot(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_pivot(ref Vector3 value);

    /// <summary>
    ///   <para>Sort particles within a system.</para>
    /// </summary>
    public extern ParticleSystemSortMode sortMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Biases particle system sorting amongst other transparencies.</para>
    /// </summary>
    public extern float sortingFudge { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clamp the minimum particle size.</para>
    /// </summary>
    public extern float minParticleSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clamp the maximum particle size.</para>
    /// </summary>
    public extern float maxParticleSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mesh used as particle instead of billboarded texture.</para>
    /// </summary>
    public extern Mesh mesh { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of meshes being used for particle rendering.</para>
    /// </summary>
    public int meshCount
    {
      get
      {
        return this.Internal_GetMeshCount();
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int Internal_GetMeshCount();

    /// <summary>
    ///   <para>Get the array of meshes to be used as particles.</para>
    /// </summary>
    /// <param name="meshes">This array will be populated with the list of meshes being used for particle rendering.</param>
    /// <returns>
    ///   <para>The number of meshes actually written to the destination array.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetMeshes(Mesh[] meshes);

    /// <summary>
    ///   <para>Set an array of meshes to be used as particles when the ParticleSystemRenderer.renderMode is set to ParticleSystemRenderMode.Mesh.</para>
    /// </summary>
    /// <param name="meshes">Array of meshes to be used.</param>
    /// <param name="size">Number of elements from the mesh array to be applied.</param>
    public void SetMeshes(Mesh[] meshes)
    {
      this.SetMeshes(meshes, meshes.Length);
    }

    /// <summary>
    ///   <para>Set an array of meshes to be used as particles when the ParticleSystemRenderer.renderMode is set to ParticleSystemRenderMode.Mesh.</para>
    /// </summary>
    /// <param name="meshes">Array of meshes to be used.</param>
    /// <param name="size">Number of elements from the mesh array to be applied.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetMeshes(Mesh[] meshes, int size);

    /// <summary>
    ///   <para>Set the material used by the Trail module for attaching trails to particles.</para>
    /// </summary>
    public extern Material trailMaterial { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of currently active custom vertex streams.</para>
    /// </summary>
    public extern int activeVertexStreamsCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public void SetActiveVertexStreams(List<ParticleSystemVertexStream> streams)
    {
      if (streams == null)
        throw new ArgumentNullException(nameof (streams));
      this.SetActiveVertexStreamsInternal((object) streams);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetActiveVertexStreamsInternal(object streams);

    public void GetActiveVertexStreams(List<ParticleSystemVertexStream> streams)
    {
      if (streams == null)
        throw new ArgumentNullException(nameof (streams));
      this.GetActiveVertexStreamsInternal((object) streams);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetActiveVertexStreamsInternal(object streams);

    /// <summary>
    ///   <para>Enable a set of vertex shader streams on the particle system renderer.</para>
    /// </summary>
    /// <param name="streams">Streams to enable.</param>
    [Obsolete("EnableVertexStreams is deprecated. Use SetActiveVertexStreams instead.")]
    public void EnableVertexStreams(ParticleSystemVertexStreams streams)
    {
      this.Internal_SetVertexStreams(streams, true);
    }

    /// <summary>
    ///         <para>Disable a set of vertex shader streams on the particle system renderer.
    /// The position stream is always enabled, and any attempts to remove it will be ignored.</para>
    ///       </summary>
    /// <param name="streams">Streams to disable.</param>
    [Obsolete("DisableVertexStreams is deprecated. Use SetActiveVertexStreams instead.")]
    public void DisableVertexStreams(ParticleSystemVertexStreams streams)
    {
      this.Internal_SetVertexStreams(streams, false);
    }

    /// <summary>
    ///   <para>Query whether the particle system renderer uses a particular set of vertex streams.</para>
    /// </summary>
    /// <param name="streams">Streams to query.</param>
    /// <returns>
    ///   <para>Whether all the queried streams are enabled or not.</para>
    /// </returns>
    [Obsolete("AreVertexStreamsEnabled is deprecated. Use GetActiveVertexStreams instead.")]
    public bool AreVertexStreamsEnabled(ParticleSystemVertexStreams streams)
    {
      return this.Internal_GetEnabledVertexStreams(streams) == streams;
    }

    /// <summary>
    ///   <para>Query whether the particle system renderer uses a particular set of vertex streams.</para>
    /// </summary>
    /// <param name="streams">Streams to query.</param>
    /// <returns>
    ///   <para>Returns the subset of the queried streams that are actually enabled.</para>
    /// </returns>
    [Obsolete("GetEnabledVertexStreams is deprecated. Use GetActiveVertexStreams instead.")]
    public ParticleSystemVertexStreams GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
    {
      return this.Internal_GetEnabledVertexStreams(streams);
    }

    [Obsolete("Internal_SetVertexStreams is deprecated. Use SetActiveVertexStreams instead.")]
    internal void Internal_SetVertexStreams(ParticleSystemVertexStreams streams, bool enabled)
    {
      List<ParticleSystemVertexStream> streams1 = new List<ParticleSystemVertexStream>(this.activeVertexStreamsCount);
      this.GetActiveVertexStreams(streams1);
      if (enabled)
      {
        if ((streams & ParticleSystemVertexStreams.Position) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Position))
          streams1.Add(ParticleSystemVertexStream.Position);
        if ((streams & ParticleSystemVertexStreams.Normal) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Normal))
          streams1.Add(ParticleSystemVertexStream.Normal);
        if ((streams & ParticleSystemVertexStreams.Tangent) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Tangent))
          streams1.Add(ParticleSystemVertexStream.Tangent);
        if ((streams & ParticleSystemVertexStreams.Color) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Color))
          streams1.Add(ParticleSystemVertexStream.Color);
        if ((streams & ParticleSystemVertexStreams.UV) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.UV))
          streams1.Add(ParticleSystemVertexStream.UV);
        if ((streams & ParticleSystemVertexStreams.UV2BlendAndFrame) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.UV2))
        {
          streams1.Add(ParticleSystemVertexStream.UV2);
          streams1.Add(ParticleSystemVertexStream.AnimBlend);
          streams1.Add(ParticleSystemVertexStream.AnimFrame);
        }
        if ((streams & ParticleSystemVertexStreams.CenterAndVertexID) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Center))
        {
          streams1.Add(ParticleSystemVertexStream.Center);
          streams1.Add(ParticleSystemVertexStream.VertexID);
        }
        if ((streams & ParticleSystemVertexStreams.Size) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.SizeXYZ))
          streams1.Add(ParticleSystemVertexStream.SizeXYZ);
        if ((streams & ParticleSystemVertexStreams.Rotation) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Rotation3D))
          streams1.Add(ParticleSystemVertexStream.Rotation3D);
        if ((streams & ParticleSystemVertexStreams.Velocity) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Velocity))
          streams1.Add(ParticleSystemVertexStream.Velocity);
        if ((streams & ParticleSystemVertexStreams.Lifetime) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.AgePercent))
        {
          streams1.Add(ParticleSystemVertexStream.AgePercent);
          streams1.Add(ParticleSystemVertexStream.InvStartLifetime);
        }
        if ((streams & ParticleSystemVertexStreams.Custom1) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Custom1XYZW))
          streams1.Add(ParticleSystemVertexStream.Custom1XYZW);
        if ((streams & ParticleSystemVertexStreams.Custom2) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.Custom2XYZW))
          streams1.Add(ParticleSystemVertexStream.Custom2XYZW);
        if ((streams & ParticleSystemVertexStreams.Random) != ParticleSystemVertexStreams.None && !streams1.Contains(ParticleSystemVertexStream.StableRandomXYZ))
        {
          streams1.Add(ParticleSystemVertexStream.StableRandomXYZ);
          streams1.Add(ParticleSystemVertexStream.VaryingRandomX);
        }
      }
      else
      {
        if ((streams & ParticleSystemVertexStreams.Position) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Position);
        if ((streams & ParticleSystemVertexStreams.Normal) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Normal);
        if ((streams & ParticleSystemVertexStreams.Tangent) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Tangent);
        if ((streams & ParticleSystemVertexStreams.Color) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Color);
        if ((streams & ParticleSystemVertexStreams.UV) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.UV);
        if ((streams & ParticleSystemVertexStreams.UV2BlendAndFrame) != ParticleSystemVertexStreams.None)
        {
          streams1.Remove(ParticleSystemVertexStream.UV2);
          streams1.Remove(ParticleSystemVertexStream.AnimBlend);
          streams1.Remove(ParticleSystemVertexStream.AnimFrame);
        }
        if ((streams & ParticleSystemVertexStreams.CenterAndVertexID) != ParticleSystemVertexStreams.None)
        {
          streams1.Remove(ParticleSystemVertexStream.Center);
          streams1.Remove(ParticleSystemVertexStream.VertexID);
        }
        if ((streams & ParticleSystemVertexStreams.Size) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.SizeXYZ);
        if ((streams & ParticleSystemVertexStreams.Rotation) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Rotation3D);
        if ((streams & ParticleSystemVertexStreams.Velocity) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Velocity);
        if ((streams & ParticleSystemVertexStreams.Lifetime) != ParticleSystemVertexStreams.None)
        {
          streams1.Remove(ParticleSystemVertexStream.AgePercent);
          streams1.Remove(ParticleSystemVertexStream.InvStartLifetime);
        }
        if ((streams & ParticleSystemVertexStreams.Custom1) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Custom1XYZW);
        if ((streams & ParticleSystemVertexStreams.Custom2) != ParticleSystemVertexStreams.None)
          streams1.Remove(ParticleSystemVertexStream.Custom2XYZW);
        if ((streams & ParticleSystemVertexStreams.Random) != ParticleSystemVertexStreams.None)
        {
          streams1.Remove(ParticleSystemVertexStream.StableRandomXYZW);
          streams1.Remove(ParticleSystemVertexStream.VaryingRandomX);
        }
      }
      this.SetActiveVertexStreams(streams1);
    }

    [Obsolete("Internal_GetVertexStreams is deprecated. Use GetActiveVertexStreams instead.")]
    internal ParticleSystemVertexStreams Internal_GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
    {
      List<ParticleSystemVertexStream> streams1 = new List<ParticleSystemVertexStream>(this.activeVertexStreamsCount);
      this.GetActiveVertexStreams(streams1);
      ParticleSystemVertexStreams systemVertexStreams = ParticleSystemVertexStreams.None;
      if (streams1.Contains(ParticleSystemVertexStream.Position))
        systemVertexStreams |= ParticleSystemVertexStreams.Position;
      if (streams1.Contains(ParticleSystemVertexStream.Normal))
        systemVertexStreams |= ParticleSystemVertexStreams.Normal;
      if (streams1.Contains(ParticleSystemVertexStream.Tangent))
        systemVertexStreams |= ParticleSystemVertexStreams.Tangent;
      if (streams1.Contains(ParticleSystemVertexStream.Color))
        systemVertexStreams |= ParticleSystemVertexStreams.Color;
      if (streams1.Contains(ParticleSystemVertexStream.UV))
        systemVertexStreams |= ParticleSystemVertexStreams.UV;
      if (streams1.Contains(ParticleSystemVertexStream.UV2))
        systemVertexStreams |= ParticleSystemVertexStreams.UV2BlendAndFrame;
      if (streams1.Contains(ParticleSystemVertexStream.Center))
        systemVertexStreams |= ParticleSystemVertexStreams.CenterAndVertexID;
      if (streams1.Contains(ParticleSystemVertexStream.SizeXYZ))
        systemVertexStreams |= ParticleSystemVertexStreams.Size;
      if (streams1.Contains(ParticleSystemVertexStream.Rotation3D))
        systemVertexStreams |= ParticleSystemVertexStreams.Rotation;
      if (streams1.Contains(ParticleSystemVertexStream.Velocity))
        systemVertexStreams |= ParticleSystemVertexStreams.Velocity;
      if (streams1.Contains(ParticleSystemVertexStream.AgePercent))
        systemVertexStreams |= ParticleSystemVertexStreams.Lifetime;
      if (streams1.Contains(ParticleSystemVertexStream.Custom1XYZW))
        systemVertexStreams |= ParticleSystemVertexStreams.Custom1;
      if (streams1.Contains(ParticleSystemVertexStream.Custom2XYZW))
        systemVertexStreams |= ParticleSystemVertexStreams.Custom2;
      if (streams1.Contains(ParticleSystemVertexStream.StableRandomXYZ))
        systemVertexStreams |= ParticleSystemVertexStreams.Random;
      return systemVertexStreams & streams;
    }

    internal extern bool editorEnabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Specifies how the Particle System Renderer interacts with SpriteMask.</para>
    /// </summary>
    public extern SpriteMaskInteraction maskInteraction { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
