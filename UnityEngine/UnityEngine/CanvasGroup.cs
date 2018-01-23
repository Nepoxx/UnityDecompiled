// Decompiled with JetBrains decompiler
// Type: UnityEngine.CanvasGroup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A Canvas placable element that can be used to modify children Alpha, Raycasting, Enabled state.</para>
  /// </summary>
  public sealed class CanvasGroup : Component, ICanvasRaycastFilter
  {
    /// <summary>
    ///   <para>Set the alpha of the group.</para>
    /// </summary>
    public extern float alpha { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the group interactable (are the elements beneath the group enabled).</para>
    /// </summary>
    public extern bool interactable { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does this group block raycasting (allow collision).</para>
    /// </summary>
    public extern bool blocksRaycasts { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the group ignore parent groups?</para>
    /// </summary>
    public extern bool ignoreParentGroups { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true if the Group allows raycasts.</para>
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="eventCamera"></param>
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
      return this.blocksRaycasts;
    }
  }
}
