// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Various utilities for animator manipulation.</para>
  /// </summary>
  public sealed class AnimatorUtility
  {
    /// <summary>
    ///   <para>This function will remove all transform hierarchy under GameObject, the animator will write directly transform matrices into the skin mesh matrices saving alot of CPU cycles.</para>
    /// </summary>
    /// <param name="go">GameObject to Optimize.</param>
    /// <param name="exposedTransforms">List of transform name to expose.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OptimizeTransformHierarchy(GameObject go, string[] exposedTransforms);

    /// <summary>
    ///   <para>This function will recreate all transform hierarchy under GameObject.</para>
    /// </summary>
    /// <param name="go">GameObject to Deoptimize.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DeoptimizeTransformHierarchy(GameObject go);
  }
}
