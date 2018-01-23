// Decompiled with JetBrains decompiler
// Type: UnityEngine.SceneManagement.SceneUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.SceneManagement
{
  /// <summary>
  ///   <para>Scene and Build Settings related utilities.</para>
  /// </summary>
  public static class SceneUtility
  {
    /// <summary>
    ///   <para>Get the scene path from a build index.</para>
    /// </summary>
    /// <param name="buildIndex"></param>
    /// <returns>
    ///   <para>Scene path (e.g "AssetsScenesScene1.unity").</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetScenePathByBuildIndex(int buildIndex);

    /// <summary>
    ///   <para>Get the build index from a scene path.</para>
    /// </summary>
    /// <param name="scenePath">Scene path (e.g: "AssetsScenesScene1.unity").</param>
    /// <returns>
    ///   <para>Build index.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetBuildIndexByScenePath(string scenePath);
  }
}
