// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorOverrideController
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
  ///   <para>Interface to control Animator Override Controller.</para>
  /// </summary>
  public sealed class AnimatorOverrideController : RuntimeAnimatorController
  {
    internal AnimatorOverrideController.OnOverrideControllerDirtyCallback OnOverrideControllerDirty;

    /// <summary>
    ///   <para>Creates an empty Animator Override Controller.</para>
    /// </summary>
    public AnimatorOverrideController()
    {
      AnimatorOverrideController.Internal_CreateAnimatorOverrideController(this, (RuntimeAnimatorController) null);
    }

    /// <summary>
    ///   <para>Creates an Animator Override Controller that overrides controller.</para>
    /// </summary>
    /// <param name="controller">Runtime Animator Controller to override.</param>
    public AnimatorOverrideController(RuntimeAnimatorController controller)
    {
      AnimatorOverrideController.Internal_CreateAnimatorOverrideController(this, controller);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAnimatorOverrideController([Writable] AnimatorOverrideController self, RuntimeAnimatorController controller);

    /// <summary>
    ///   <para>The Runtime Animator Controller that the Animator Override Controller overrides.</para>
    /// </summary>
    public extern RuntimeAnimatorController runtimeAnimatorController { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AnimationClip this[string name]
    {
      get
      {
        return this.Internal_GetClipByName(name, true);
      }
      set
      {
        this.Internal_SetClipByName(name, value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern AnimationClip Internal_GetClipByName(string name, bool returnEffectiveClip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetClipByName(string name, AnimationClip clip);

    public AnimationClip this[AnimationClip clip]
    {
      get
      {
        return this.Internal_GetClip(clip, true);
      }
      set
      {
        this.Internal_SetClip(clip, value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern AnimationClip Internal_GetClip(AnimationClip originalClip, bool returnEffectiveClip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip, [DefaultValue("true")] bool notify);

    [ExcludeFromDocs]
    private void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip)
    {
      bool notify = true;
      this.Internal_SetClip(originalClip, overrideClip, notify);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SendNotification();

    [RequiredByNativeCode]
    internal static void OnInvalidateOverrideController(AnimatorOverrideController controller)
    {
      if (controller.OnOverrideControllerDirty == null)
        return;
      controller.OnOverrideControllerDirty();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern AnimationClip Internal_GetOriginalClip(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern AnimationClip Internal_GetOverrideClip(AnimationClip originalClip);

    /// <summary>
    ///   <para>Returns the count of overrides.</para>
    /// </summary>
    public extern int overridesCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public void GetOverrides(List<KeyValuePair<AnimationClip, AnimationClip>> overrides)
    {
      if (overrides == null)
        throw new ArgumentNullException(nameof (overrides));
      int overridesCount = this.overridesCount;
      if (overrides.Capacity < overridesCount)
        overrides.Capacity = overridesCount;
      overrides.Clear();
      for (int index = 0; index < overridesCount; ++index)
      {
        AnimationClip originalClip = this.Internal_GetOriginalClip(index);
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(originalClip, this.Internal_GetOverrideClip(originalClip)));
      }
    }

    public void ApplyOverrides(IList<KeyValuePair<AnimationClip, AnimationClip>> overrides)
    {
      if (overrides == null)
        throw new ArgumentNullException(nameof (overrides));
      for (int index = 0; index < overrides.Count; ++index)
        this.Internal_SetClip(overrides[index].Key, overrides[index].Value, false);
      this.SendNotification();
    }

    /// <summary>
    ///   <para>Returns the list of orignal Animation Clip from the controller and their override Animation Clip.</para>
    /// </summary>
    [Obsolete("clips property is deprecated. Use AnimatorOverrideController.GetOverrides and AnimatorOverrideController.ApplyOverrides instead.")]
    public AnimationClipPair[] clips
    {
      get
      {
        int overridesCount = this.overridesCount;
        AnimationClipPair[] animationClipPairArray = new AnimationClipPair[overridesCount];
        for (int index = 0; index < overridesCount; ++index)
        {
          animationClipPairArray[index] = new AnimationClipPair();
          animationClipPairArray[index].originalClip = this.Internal_GetOriginalClip(index);
          animationClipPairArray[index].overrideClip = this.Internal_GetOverrideClip(animationClipPairArray[index].originalClip);
        }
        return animationClipPairArray;
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
          this.Internal_SetClip(value[index].originalClip, value[index].overrideClip, false);
        this.SendNotification();
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void PerformOverrideClipListCleanup();

    internal delegate void OnOverrideControllerDirtyCallback();
  }
}
