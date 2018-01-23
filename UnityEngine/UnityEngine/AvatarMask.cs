// Decompiled with JetBrains decompiler
// Type: UnityEngine.AvatarMask
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AvatarMask are used to mask out humanoid body parts and transforms.</para>
  /// </summary>
  [MovedFrom("UnityEditor.Animations", true)]
  public sealed class AvatarMask : Object
  {
    /// <summary>
    ///   <para>Creates a new AvatarMask.</para>
    /// </summary>
    public AvatarMask()
    {
      AvatarMask.Internal_CreateAvatarMask(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAvatarMask([Writable] AvatarMask mono);

    [Obsolete("AvatarMask.humanoidBodyPartCount is deprecated. Use AvatarMaskBodyPart.LastBodyPart instead.")]
    private int humanoidBodyPartCount
    {
      get
      {
        return 13;
      }
    }

    /// <summary>
    ///   <para>Returns true if the humanoid body part at the given index is active.</para>
    /// </summary>
    /// <param name="index">The index of the humanoid body part.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetHumanoidBodyPartActive(AvatarMaskBodyPart index);

    /// <summary>
    ///   <para>Sets the humanoid body part at the given index to active or not.</para>
    /// </summary>
    /// <param name="index">The index of the humanoid body part.</param>
    /// <param name="value">Active or not.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetHumanoidBodyPartActive(AvatarMaskBodyPart index, bool value);

    /// <summary>
    ///   <para>Number of transforms.</para>
    /// </summary>
    public extern int transformCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [ExcludeFromDocs]
    public void AddTransformPath(Transform transform)
    {
      bool recursive = true;
      this.AddTransformPath(transform, recursive);
    }

    /// <summary>
    ///   <para>Adds a transform path into the AvatarMask.</para>
    /// </summary>
    /// <param name="transform">The transform to add into the AvatarMask.</param>
    /// <param name="recursive">Whether to also add all children of the specified transform.</param>
    public void AddTransformPath(Transform transform, [DefaultValue("true")] bool recursive)
    {
      if ((Object) transform == (Object) null)
        throw new ArgumentNullException(nameof (transform));
      this.Internal_AddTransformPath(transform, recursive);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_AddTransformPath(Transform transform, bool recursive);

    [ExcludeFromDocs]
    public void RemoveTransformPath(Transform transform)
    {
      bool recursive = true;
      this.RemoveTransformPath(transform, recursive);
    }

    /// <summary>
    ///   <para>Removes a transform path from the AvatarMask.</para>
    /// </summary>
    /// <param name="transform">The Transform that should be removed from the AvatarMask.</param>
    /// <param name="recursive">Whether to also remove all children of the specified transform.</param>
    public void RemoveTransformPath(Transform transform, [DefaultValue("true")] bool recursive)
    {
      if ((Object) transform == (Object) null)
        throw new ArgumentNullException(nameof (transform));
      this.Internal_RemoveTransformPath(transform, recursive);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_RemoveTransformPath(Transform transform, bool recursive);

    /// <summary>
    ///   <para>Returns the path of the transform at the given index.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetTransformPath(int index);

    /// <summary>
    ///   <para>Sets the path of the transform at the given index.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    /// <param name="path">The path of the transform.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetTransformPath(int index, string path);

    /// <summary>
    ///   <para>Returns true if the transform at the given index is active.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetTransformActive(int index);

    /// <summary>
    ///   <para>Sets the tranform at the given index to active or not.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    /// <param name="value">Active or not.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetTransformActive(int index, bool value);

    internal extern bool hasFeetIK { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal void Copy(AvatarMask other)
    {
      for (AvatarMaskBodyPart index = AvatarMaskBodyPart.Root; index < AvatarMaskBodyPart.LastBodyPart; ++index)
        this.SetHumanoidBodyPartActive(index, other.GetHumanoidBodyPartActive(index));
      this.transformCount = other.transformCount;
      for (int index = 0; index < other.transformCount; ++index)
      {
        this.SetTransformPath(index, other.GetTransformPath(index));
        this.SetTransformActive(index, other.GetTransformActive(index));
      }
    }
  }
}
