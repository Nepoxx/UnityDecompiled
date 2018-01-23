// Decompiled with JetBrains decompiler
// Type: UnityEngine.SceneManagement.SceneManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.SceneManagement
{
  /// <summary>
  ///   <para>Scene management at run-time.</para>
  /// </summary>
  [RequiredByNativeCode]
  public class SceneManager
  {
    /// <summary>
    ///   <para>The total number of currently loaded Scenes.</para>
    /// </summary>
    public static extern int sceneCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of Scenes in Build Settings.</para>
    /// </summary>
    public static extern int sceneCountInBuildSettings { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the currently active Scene.</para>
    /// </summary>
    /// <returns>
    ///   <para>The active Scene.</para>
    /// </returns>
    public static Scene GetActiveScene()
    {
      Scene scene;
      SceneManager.INTERNAL_CALL_GetActiveScene(out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetActiveScene(out Scene value);

    /// <summary>
    ///   <para>Set the Scene to be active.</para>
    /// </summary>
    /// <param name="scene">The Scene to be set.</param>
    /// <returns>
    ///   <para>Returns false if the Scene is not loaded yet.</para>
    /// </returns>
    public static bool SetActiveScene(Scene scene)
    {
      return SceneManager.INTERNAL_CALL_SetActiveScene(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SetActiveScene(ref Scene scene);

    /// <summary>
    ///   <para>Searches all Scenes loaded for a Scene that has the given asset path.</para>
    /// </summary>
    /// <param name="scenePath">Path of the Scene. Should be relative to the project folder. Like: "AssetsMyScenesMyScene.unity".</param>
    /// <returns>
    ///   <para>A reference to the Scene, if valid. If not, an invalid Scene is returned.</para>
    /// </returns>
    public static Scene GetSceneByPath(string scenePath)
    {
      Scene scene;
      SceneManager.INTERNAL_CALL_GetSceneByPath(scenePath, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSceneByPath(string scenePath, out Scene value);

    /// <summary>
    ///   <para>Searches through the Scenes loaded for a Scene with the given name.</para>
    /// </summary>
    /// <param name="name">Name of Scene to find.</param>
    /// <returns>
    ///   <para>A reference to the Scene, if valid. If not, an invalid Scene is returned.</para>
    /// </returns>
    public static Scene GetSceneByName(string name)
    {
      Scene scene;
      SceneManager.INTERNAL_CALL_GetSceneByName(name, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSceneByName(string name, out Scene value);

    /// <summary>
    ///   <para>Get a Scene struct from a build index.</para>
    /// </summary>
    /// <param name="buildIndex">Build index as shown in the Build Settings window.</param>
    /// <returns>
    ///   <para>A reference to the Scene, if valid. If not, an invalid Scene is returned.</para>
    /// </returns>
    public static Scene GetSceneByBuildIndex(int buildIndex)
    {
      Scene scene;
      SceneManager.INTERNAL_CALL_GetSceneByBuildIndex(buildIndex, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSceneByBuildIndex(int buildIndex, out Scene value);

    /// <summary>
    ///   <para>Get the Scene at index in the SceneManager's list of loaded Scenes.</para>
    /// </summary>
    /// <param name="index">Index of the Scene to get. Index must be greater than or equal to 0 and less than SceneManager.sceneCount.</param>
    /// <returns>
    ///   <para>A reference to the Scene at the index specified.</para>
    /// </returns>
    public static Scene GetSceneAt(int index)
    {
      Scene scene;
      SceneManager.INTERNAL_CALL_GetSceneAt(index, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSceneAt(int index, out Scene value);

    /// <summary>
    ///   <para>Returns an array of all the Scenes currently open in the hierarchy.</para>
    /// </summary>
    /// <returns>
    ///   <para>Array of Scenes in the Hierarchy.</para>
    /// </returns>
    [Obsolete("Use SceneManager.sceneCount and SceneManager.GetSceneAt(int index) to loop the all scenes instead.")]
    public static Scene[] GetAllScenes()
    {
      Scene[] sceneArray = new Scene[SceneManager.sceneCount];
      for (int index = 0; index < SceneManager.sceneCount; ++index)
        sceneArray[index] = SceneManager.GetSceneAt(index);
      return sceneArray;
    }

    [ExcludeFromDocs]
    public static void LoadScene(string sceneName)
    {
      LoadSceneMode mode = LoadSceneMode.Single;
      SceneManager.LoadScene(sceneName, mode);
    }

    /// <summary>
    ///   <para>Loads the Scene by its name or index in Build Settings.</para>
    /// </summary>
    /// <param name="sceneName">Name or path of the Scene to load.</param>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to load.</param>
    /// <param name="mode">Allows you to specify whether or not to load the Scene additively.
    /// See SceneManagement.LoadSceneMode for more information about the options.</param>
    public static void LoadScene(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
    {
      SceneManager.LoadSceneAsyncNameIndexInternal(sceneName, -1, mode == LoadSceneMode.Additive, true);
    }

    [ExcludeFromDocs]
    public static void LoadScene(int sceneBuildIndex)
    {
      LoadSceneMode mode = LoadSceneMode.Single;
      SceneManager.LoadScene(sceneBuildIndex, mode);
    }

    /// <summary>
    ///   <para>Loads the Scene by its name or index in Build Settings.</para>
    /// </summary>
    /// <param name="sceneName">Name or path of the Scene to load.</param>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to load.</param>
    /// <param name="mode">Allows you to specify whether or not to load the Scene additively.
    /// See SceneManagement.LoadSceneMode for more information about the options.</param>
    public static void LoadScene(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
    {
      SceneManager.LoadSceneAsyncNameIndexInternal((string) null, sceneBuildIndex, mode == LoadSceneMode.Additive, true);
    }

    [ExcludeFromDocs]
    public static AsyncOperation LoadSceneAsync(string sceneName)
    {
      LoadSceneMode mode = LoadSceneMode.Single;
      return SceneManager.LoadSceneAsync(sceneName, mode);
    }

    /// <summary>
    ///   <para>Loads the Scene asynchronously in the background.</para>
    /// </summary>
    /// <param name="sceneName">Name or path of the Scene to load.</param>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to load.</param>
    /// <param name="mode">If LoadSceneMode.Single then all current Scenes will be unloaded before loading.</param>
    /// <returns>
    ///   <para>Use the AsyncOperation to determine if the operation has completed.</para>
    /// </returns>
    public static AsyncOperation LoadSceneAsync(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
    {
      return SceneManager.LoadSceneAsyncNameIndexInternal(sceneName, -1, mode == LoadSceneMode.Additive, false);
    }

    [ExcludeFromDocs]
    public static AsyncOperation LoadSceneAsync(int sceneBuildIndex)
    {
      LoadSceneMode mode = LoadSceneMode.Single;
      return SceneManager.LoadSceneAsync(sceneBuildIndex, mode);
    }

    /// <summary>
    ///   <para>Loads the Scene asynchronously in the background.</para>
    /// </summary>
    /// <param name="sceneName">Name or path of the Scene to load.</param>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to load.</param>
    /// <param name="mode">If LoadSceneMode.Single then all current Scenes will be unloaded before loading.</param>
    /// <returns>
    ///   <para>Use the AsyncOperation to determine if the operation has completed.</para>
    /// </returns>
    public static AsyncOperation LoadSceneAsync(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
    {
      return SceneManager.LoadSceneAsyncNameIndexInternal((string) null, sceneBuildIndex, mode == LoadSceneMode.Additive, false);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AsyncOperation LoadSceneAsyncNameIndexInternal(string sceneName, int sceneBuildIndex, bool isAdditive, bool mustCompleteNextFrame);

    /// <summary>
    ///   <para>Create an empty new Scene at runtime with the given name.</para>
    /// </summary>
    /// <param name="sceneName">The name of the new Scene. It cannot be empty or null, or same as the name of the existing Scenes.</param>
    /// <returns>
    ///   <para>A reference to the new Scene that was created, or an invalid Scene if creation failed.</para>
    /// </returns>
    public static Scene CreateScene(string sceneName)
    {
      Scene scene;
      SceneManager.INTERNAL_CALL_CreateScene(sceneName, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CreateScene(string sceneName, out Scene value);

    /// <summary>
    ///   <para>Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.</para>
    /// </summary>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to unload.</param>
    /// <param name="sceneName">Name or path of the Scene to unload.</param>
    /// <param name="scene">Scene to unload.</param>
    /// <returns>
    ///   <para>Returns true if the Scene is unloaded.</para>
    /// </returns>
    [Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
    public static bool UnloadScene(Scene scene)
    {
      return SceneManager.UnloadSceneInternal(scene);
    }

    private static bool UnloadSceneInternal(Scene scene)
    {
      return SceneManager.INTERNAL_CALL_UnloadSceneInternal(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_UnloadSceneInternal(ref Scene scene);

    /// <summary>
    ///   <para>Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.</para>
    /// </summary>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to unload.</param>
    /// <param name="sceneName">Name or path of the Scene to unload.</param>
    /// <param name="scene">Scene to unload.</param>
    /// <returns>
    ///   <para>Returns true if the Scene is unloaded.</para>
    /// </returns>
    [Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
    public static bool UnloadScene(int sceneBuildIndex)
    {
      bool outSuccess;
      SceneManager.UnloadSceneNameIndexInternal("", sceneBuildIndex, true, out outSuccess);
      return outSuccess;
    }

    /// <summary>
    ///   <para>Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.</para>
    /// </summary>
    /// <param name="sceneBuildIndex">Index of the Scene in the Build Settings to unload.</param>
    /// <param name="sceneName">Name or path of the Scene to unload.</param>
    /// <param name="scene">Scene to unload.</param>
    /// <returns>
    ///   <para>Returns true if the Scene is unloaded.</para>
    /// </returns>
    [Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
    public static bool UnloadScene(string sceneName)
    {
      bool outSuccess;
      SceneManager.UnloadSceneNameIndexInternal(sceneName, -1, true, out outSuccess);
      return outSuccess;
    }

    /// <summary>
    ///   <para>Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.</para>
    /// </summary>
    /// <param name="sceneBuildIndex">Index of the Scene in BuildSettings.</param>
    /// <param name="sceneName">Name or path of the Scene to unload.</param>
    /// <param name="scene">Scene to unload.</param>
    /// <returns>
    ///   <para>Use the AsyncOperation to determine if the operation has completed.</para>
    /// </returns>
    public static AsyncOperation UnloadSceneAsync(int sceneBuildIndex)
    {
      bool outSuccess;
      return SceneManager.UnloadSceneNameIndexInternal("", sceneBuildIndex, false, out outSuccess);
    }

    /// <summary>
    ///   <para>Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.</para>
    /// </summary>
    /// <param name="sceneBuildIndex">Index of the Scene in BuildSettings.</param>
    /// <param name="sceneName">Name or path of the Scene to unload.</param>
    /// <param name="scene">Scene to unload.</param>
    /// <returns>
    ///   <para>Use the AsyncOperation to determine if the operation has completed.</para>
    /// </returns>
    public static AsyncOperation UnloadSceneAsync(string sceneName)
    {
      bool outSuccess;
      return SceneManager.UnloadSceneNameIndexInternal(sceneName, -1, false, out outSuccess);
    }

    /// <summary>
    ///   <para>Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.</para>
    /// </summary>
    /// <param name="sceneBuildIndex">Index of the Scene in BuildSettings.</param>
    /// <param name="sceneName">Name or path of the Scene to unload.</param>
    /// <param name="scene">Scene to unload.</param>
    /// <returns>
    ///   <para>Use the AsyncOperation to determine if the operation has completed.</para>
    /// </returns>
    public static AsyncOperation UnloadSceneAsync(Scene scene)
    {
      return SceneManager.UnloadSceneAsyncInternal(scene);
    }

    private static AsyncOperation UnloadSceneAsyncInternal(Scene scene)
    {
      return SceneManager.INTERNAL_CALL_UnloadSceneAsyncInternal(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AsyncOperation INTERNAL_CALL_UnloadSceneAsyncInternal(ref Scene scene);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AsyncOperation UnloadSceneNameIndexInternal(string sceneName, int sceneBuildIndex, bool immediately, out bool outSuccess);

    /// <summary>
    ///   <para>This will merge the source Scene into the destinationScene.</para>
    /// </summary>
    /// <param name="sourceScene">The Scene that will be merged into the destination Scene.</param>
    /// <param name="destinationScene">Existing Scene to merge the source Scene into.</param>
    public static void MergeScenes(Scene sourceScene, Scene destinationScene)
    {
      SceneManager.INTERNAL_CALL_MergeScenes(ref sourceScene, ref destinationScene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MergeScenes(ref Scene sourceScene, ref Scene destinationScene);

    /// <summary>
    ///   <para>Move a GameObject from its current Scene to a new Scene.</para>
    /// </summary>
    /// <param name="go">GameObject to move.</param>
    /// <param name="scene">Scene to move into.</param>
    public static void MoveGameObjectToScene(GameObject go, Scene scene)
    {
      SceneManager.INTERNAL_CALL_MoveGameObjectToScene(go, ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MoveGameObjectToScene(GameObject go, ref Scene scene);

    public static event UnityAction<Scene, LoadSceneMode> sceneLoaded;

    [RequiredByNativeCode]
    private static void Internal_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
      // ISSUE: reference to a compiler-generated field
      if (SceneManager.sceneLoaded == null)
        return;
      // ISSUE: reference to a compiler-generated field
      SceneManager.sceneLoaded(scene, mode);
    }

    public static event UnityAction<Scene> sceneUnloaded;

    [RequiredByNativeCode]
    private static void Internal_SceneUnloaded(Scene scene)
    {
      // ISSUE: reference to a compiler-generated field
      if (SceneManager.sceneUnloaded == null)
        return;
      // ISSUE: reference to a compiler-generated field
      SceneManager.sceneUnloaded(scene);
    }

    public static event UnityAction<Scene, Scene> activeSceneChanged;

    [RequiredByNativeCode]
    private static void Internal_ActiveSceneChanged(Scene previousActiveScene, Scene newActiveScene)
    {
      // ISSUE: reference to a compiler-generated field
      if (SceneManager.activeSceneChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      SceneManager.activeSceneChanged(previousActiveScene, newActiveScene);
    }
  }
}
