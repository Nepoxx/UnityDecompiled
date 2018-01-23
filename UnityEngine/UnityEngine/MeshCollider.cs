// Decompiled with JetBrains decompiler
// Type: UnityEngine.MeshCollider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A mesh collider allows you to do between meshes and primitives.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class MeshCollider : Collider
  {
    /// <summary>
    ///   <para>The mesh object used for collision detection.</para>
    /// </summary>
    public extern Mesh sharedMesh { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use a convex collider from the mesh.</para>
    /// </summary>
    public extern bool convex { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Options used to enable or disable certain features in mesh cooking.</para>
    /// </summary>
    public extern MeshColliderCookingOptions cookingOptions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allow the physics engine to increase the volume of the input mesh in attempt to generate a valid convex mesh.</para>
    /// </summary>
    public bool inflateMesh
    {
      get
      {
        return (this.cookingOptions & MeshColliderCookingOptions.InflateConvexMesh) != MeshColliderCookingOptions.None;
      }
      set
      {
        MeshColliderCookingOptions colliderCookingOptions = this.cookingOptions & ~MeshColliderCookingOptions.InflateConvexMesh;
        if (value)
          colliderCookingOptions |= MeshColliderCookingOptions.InflateConvexMesh;
        this.cookingOptions = colliderCookingOptions;
      }
    }

    /// <summary>
    ///   <para>Used when set to inflateMesh to determine how much inflation is acceptable.</para>
    /// </summary>
    public extern float skinWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Uses interpolated normals for sphere collisions instead of flat polygonal normals.</para>
    /// </summary>
    [Obsolete("Configuring smooth sphere collisions is no longer needed. PhysX3 has a better behaviour in place.")]
    public bool smoothSphereCollisions
    {
      get
      {
        return true;
      }
      set
      {
      }
    }
  }
}
