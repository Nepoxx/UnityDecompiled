// Decompiled with JetBrains decompiler
// Type: UnityEngine.SceneManagement.Scene
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.SceneManagement
{
  /// <summary>
  ///   <para>Run-time data structure for *.unity file.</para>
  /// </summary>
  public struct Scene
  {
    private int m_Handle;

    internal int handle
    {
      get
      {
        return this.m_Handle;
      }
    }

    internal Scene.LoadingState loadingState
    {
      get
      {
        return Scene.GetLoadingStateInternal(this.handle);
      }
    }

    /// <summary>
    ///         <para>Whether this is a valid scene.
    /// A scene may be invalid if, for example, you tried to open a scene that does not exist. In this case, the scene returned from EditorSceneManager.OpenScene would return False for IsValid.</para>
    ///       </summary>
    /// <returns>
    ///   <para>Whether this is a valid scene.</para>
    /// </returns>
    public bool IsValid()
    {
      return Scene.IsValidInternal(this.handle);
    }

    /// <summary>
    ///   <para>Returns the relative path of the scene. Like: "AssetsMyScenesMyScene.unity".</para>
    /// </summary>
    public string path
    {
      get
      {
        return Scene.GetPathInternal(this.handle);
      }
    }

    /// <summary>
    ///   <para>Returns the name of the scene.</para>
    /// </summary>
    public string name
    {
      get
      {
        return Scene.GetNameInternal(this.handle);
      }
      internal set
      {
        Scene.SetNameInternal(this.handle, value);
      }
    }

    internal string guid
    {
      get
      {
        return Scene.GetGUIDInternal(this.handle);
      }
    }

    /// <summary>
    ///   <para>Returns true if the scene is loaded.</para>
    /// </summary>
    public bool isLoaded
    {
      get
      {
        return Scene.GetIsLoadedInternal(this.handle);
      }
    }

    /// <summary>
    ///   <para>Returns the index of the scene in the Build Settings. Always returns -1 if the scene was loaded through an AssetBundle.</para>
    /// </summary>
    public int buildIndex
    {
      get
      {
        return Scene.GetBuildIndexInternal(this.handle);
      }
    }

    /// <summary>
    ///   <para>Returns true if the scene is modifed.</para>
    /// </summary>
    public bool isDirty
    {
      get
      {
        return Scene.GetIsDirtyInternal(this.handle);
      }
    }

    /// <summary>
    ///   <para>The number of root transforms of this scene.</para>
    /// </summary>
    public int rootCount
    {
      get
      {
        return Scene.GetRootCountInternal(this.handle);
      }
    }

    /// <summary>
    ///   <para>Returns all the root game objects in the scene.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of game objects.</para>
    /// </returns>
    public GameObject[] GetRootGameObjects()
    {
      List<GameObject> rootGameObjects = new List<GameObject>(this.rootCount);
      this.GetRootGameObjects(rootGameObjects);
      return rootGameObjects.ToArray();
    }

    public void GetRootGameObjects(List<GameObject> rootGameObjects)
    {
      if (rootGameObjects.Capacity < this.rootCount)
        rootGameObjects.Capacity = this.rootCount;
      rootGameObjects.Clear();
      if (!this.IsValid())
        throw new ArgumentException("The scene is invalid.");
      if (!Application.isPlaying && !this.isLoaded)
        throw new ArgumentException("The scene is not loaded.");
      if (this.rootCount == 0)
        return;
      Scene.GetRootGameObjectsInternal(this.handle, (object) rootGameObjects);
    }

    public static bool operator ==(Scene lhs, Scene rhs)
    {
      return lhs.handle == rhs.handle;
    }

    public static bool operator !=(Scene lhs, Scene rhs)
    {
      return lhs.handle != rhs.handle;
    }

    public override int GetHashCode()
    {
      return this.m_Handle;
    }

    public override bool Equals(object other)
    {
      if (!(other is Scene))
        return false;
      return this.handle == ((Scene) other).handle;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool IsValidInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetPathInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetNameInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetNameInternal(int sceneHandle, string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetGUIDInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetIsLoadedInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Scene.LoadingState GetLoadingStateInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetIsDirtyInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetBuildIndexInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetRootCountInternal(int sceneHandle);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetRootGameObjectsInternal(int sceneHandle, object resultRootList);

    internal enum LoadingState
    {
      NotLoaded,
      Loading,
      Loaded,
    }
  }
}
