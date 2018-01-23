// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorApplication
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Main Application class.</para>
  /// </summary>
  public sealed class EditorApplication
  {
    private static float s_DelayedCallbackTime = 0.0f;
    internal static UnityAction projectWasLoaded;
    internal static UnityAction editorApplicationQuit;
    /// <summary>
    ///   <para>Delegate for OnGUI events for every visible list item in the ProjectWindow.</para>
    /// </summary>
    public static EditorApplication.ProjectWindowItemCallback projectWindowItemOnGUI;
    /// <summary>
    ///   <para>Delegate for OnGUI events for every visible list item in the HierarchyWindow.</para>
    /// </summary>
    public static EditorApplication.HierarchyWindowItemCallback hierarchyWindowItemOnGUI;
    /// <summary>
    ///   <para>Delegate for generic updates.</para>
    /// </summary>
    public static EditorApplication.CallbackFunction update;
    /// <summary>
    ///   <para>Delegate which is called once after all inspectors update.</para>
    /// </summary>
    public static EditorApplication.CallbackFunction delayCall;
    /// <summary>
    ///         <para>A callback to be raised when an object in the hierarchy changes.
    /// 
    /// Each time an object is (or a group of objects are) created, renamed, parented, unparented or destroyed this callback is raised.
    /// </para>
    ///       </summary>
    public static EditorApplication.CallbackFunction hierarchyWindowChanged;
    /// <summary>
    ///   <para>Callback raised whenever the state of the Project window changes.</para>
    /// </summary>
    public static EditorApplication.CallbackFunction projectWindowChanged;
    /// <summary>
    ///   <para>Callback raised whenever the contents of a window's search box are changed.</para>
    /// </summary>
    public static EditorApplication.CallbackFunction searchChanged;
    internal static EditorApplication.CallbackFunction assetLabelsChanged;
    internal static EditorApplication.CallbackFunction assetBundleNameChanged;
    /// <summary>
    ///   <para>Delegate for changed keyboard modifier keys.</para>
    /// </summary>
    public static EditorApplication.CallbackFunction modifierKeysChanged;
    [Obsolete("Use EditorApplication.playModeStateChanged and/or EditorApplication.pauseStateChanged")]
    public static EditorApplication.CallbackFunction playmodeStateChanged;
    internal static EditorApplication.CallbackFunction globalEventHandler;
    internal static EditorApplication.CallbackFunction windowsReordered;
    /// <summary>
    ///   <para>Callback raised whenever the user contex-clicks on a property in an Inspector.</para>
    /// </summary>
    public static EditorApplication.SerializedPropertyCallbackFunction contextualPropertyMenu;
    private static EditorApplication.CallbackFunction delayedCallback;

    /// <summary>
    ///   <para>Load the given level in play mode.</para>
    /// </summary>
    /// <param name="path"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LoadLevelInPlayMode(string path);

    /// <summary>
    ///   <para>Load the given level additively in play mode.</para>
    /// </summary>
    /// <param name="path"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LoadLevelAdditiveInPlayMode(string path);

    /// <summary>
    ///   <para>Load the given level in play mode asynchronously.</para>
    /// </summary>
    /// <param name="path"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AsyncOperation LoadLevelAsyncInPlayMode(string path);

    /// <summary>
    ///   <para>Load the given level additively in play mode asynchronously</para>
    /// </summary>
    /// <param name="path"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AsyncOperation LoadLevelAdditiveAsyncInPlayMode(string path);

    /// <summary>
    ///   <para>Open another project.</para>
    /// </summary>
    /// <param name="projectPath">The path of a project to open.</param>
    /// <param name="args">Arguments to pass to command line.</param>
    public static void OpenProject(string projectPath, params string[] args)
    {
      EditorApplication.OpenProjectInternal(projectPath, args);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void OpenProjectInternal(string projectPath, string[] args);

    /// <summary>
    ///   <para>Saves all serializable assets that have not yet been written to disk (eg. Materials).</para>
    /// </summary>
    [Obsolete("Use AssetDatabase.SaveAssets instead (UnityUpgradable) -> AssetDatabase.SaveAssets()", true)]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SaveAssets();

    /// <summary>
    ///   <para>Is editor currently in play mode?</para>
    /// </summary>
    public static extern bool isPlaying { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is editor either currently in play mode, or about to switch to it? (Read Only)</para>
    /// </summary>
    public static extern bool isPlayingOrWillChangePlaymode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Perform a single frame step.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Step();

    /// <summary>
    ///   <para>Is editor currently paused?</para>
    /// </summary>
    public static extern bool isPaused { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is editor currently compiling scripts? (Read Only)</para>
    /// </summary>
    public static extern bool isCompiling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>True if the Editor is currently refreshing the AssetDatabase.</para>
    /// </summary>
    public static extern bool isUpdating { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is editor currently connected to Unity Remote 4 client app.</para>
    /// </summary>
    public static extern bool isRemoteConnected { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the scripting runtime version currently used by the Editor.</para>
    /// </summary>
    public static extern ScriptingRuntimeVersion scriptingRuntimeVersion { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern bool useLibmonoBackendForIl2cpp { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Prevents loading of assemblies when it is inconvenient.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LockReloadAssemblies();

    /// <summary>
    ///   <para>Invokes the menu item in the specified path.</para>
    /// </summary>
    /// <param name="menuItemPath"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ExecuteMenuItem(string menuItemPath);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool ExecuteMenuItemOnGameObjects(string menuItemPath, GameObject[] objects);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool ExecuteMenuItemWithTemporaryContext(string menuItemPath, UnityEngine.Object[] objects);

    /// <summary>
    ///   <para>Must be called after LockReloadAssemblies, to reenable loading of assemblies.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UnlockReloadAssemblies();

    /// <summary>
    ///   <para>Path to the Unity editor contents folder. (Read Only)</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern string applicationContentsPath { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the path to the Unity editor application. (Read Only)</para>
    /// </summary>
    public static extern string applicationPath { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern string userJavascriptPackagesPath { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the current project was created as a temporary project.</para>
    /// </summary>
    public static extern bool isTemporaryProject { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Sets the path that Unity should store the current temporary project at, when the project is closed.</para>
    /// </summary>
    /// <param name="path">The path that the current temporary project should be relocated to when closing it.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetTemporaryProjectKeepPath(string path);

    /// <summary>
    ///   <para>Exit the Unity editor application.</para>
    /// </summary>
    /// <param name="returnValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Exit(int returnValue);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSceneRepaintDirty();

    /// <summary>
    ///   <para>Normally, a player loop update will occur in the editor when the scene has been modified. This method allows you to queue a player loop update regardless of whether the scene has been modified.</para>
    /// </summary>
    public static void QueuePlayerLoopUpdate()
    {
      EditorApplication.SetSceneRepaintDirty();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateSceneIfNeeded();

    /// <summary>
    ///   <para>Plays system beep sound.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Beep();

    internal static extern UnityEngine.Object tagManager { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern UnityEngine.Object renderSettings { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The time since the editor was started. (Read Only)</para>
    /// </summary>
    public static extern double timeSinceStartup { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private static void Internal_ProjectWasLoaded()
    {
      if (EditorApplication.projectWasLoaded == null)
        return;
      EditorApplication.projectWasLoaded();
    }

    private static void Internal_EditorApplicationQuit()
    {
      if (EditorApplication.editorApplicationQuit == null)
        return;
      EditorApplication.editorApplicationQuit();
    }

    internal static bool supportsHiDPI
    {
      get
      {
        return Application.platform == RuntimePlatform.OSXEditor;
      }
    }

    /// <summary>
    ///   <para>Can be used to ensure repaint of the ProjectWindow.</para>
    /// </summary>
    public static void RepaintProjectWindow()
    {
      foreach (EditorWindow allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
        allProjectBrowser.Repaint();
    }

    public static void RepaintAnimationWindow()
    {
      foreach (AnimEditor allAnimationWindow in AnimEditor.GetAllAnimationWindows())
        allAnimationWindow.Repaint();
    }

    /// <summary>
    ///   <para>Can be used to ensure repaint of the HierarchyWindow.</para>
    /// </summary>
    public static void RepaintHierarchyWindow()
    {
      foreach (EditorWindow editorWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (SceneHierarchyWindow)))
        editorWindow.Repaint();
    }

    /// <summary>
    ///   <para>Set the hierarchy sorting method as dirty.</para>
    /// </summary>
    public static void DirtyHierarchyWindowSorting()
    {
      foreach (SceneHierarchyWindow sceneHierarchyWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (SceneHierarchyWindow)))
        sceneHierarchyWindow.DirtySortingMethods();
    }

    public static event Action<PauseState> pauseStateChanged;

    public static event Action<PlayModeStateChange> playModeStateChanged;

    private static void Internal_CallUpdateFunctions()
    {
      if (EditorApplication.update == null)
        return;
      EditorApplication.update();
    }

    private static void Internal_CallDelayFunctions()
    {
      EditorApplication.CallbackFunction delayCall = EditorApplication.delayCall;
      EditorApplication.delayCall = (EditorApplication.CallbackFunction) null;
      if (delayCall == null)
        return;
      delayCall();
    }

    private static void Internal_SwitchSkin()
    {
      EditorGUIUtility.Internal_SwitchSkin();
    }

    internal static void RequestRepaintAllViews()
    {
      foreach (GUIView guiView in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (GUIView)))
        guiView.Repaint();
    }

    private static void Internal_CallHierarchyWindowHasChanged()
    {
      if (EditorApplication.hierarchyWindowChanged == null)
        return;
      EditorApplication.hierarchyWindowChanged();
    }

    private static void Internal_CallProjectWindowHasChanged()
    {
      if (EditorApplication.projectWindowChanged == null)
        return;
      EditorApplication.projectWindowChanged();
    }

    internal static void Internal_CallSearchHasChanged()
    {
      if (EditorApplication.searchChanged == null)
        return;
      EditorApplication.searchChanged();
    }

    internal static void Internal_CallAssetLabelsHaveChanged()
    {
      if (EditorApplication.assetLabelsChanged == null)
        return;
      EditorApplication.assetLabelsChanged();
    }

    internal static void Internal_CallAssetBundleNameChanged()
    {
      if (EditorApplication.assetBundleNameChanged == null)
        return;
      EditorApplication.assetBundleNameChanged();
    }

    internal static void CallDelayed(EditorApplication.CallbackFunction function, float timeFromNow)
    {
      EditorApplication.delayedCallback = function;
      EditorApplication.s_DelayedCallbackTime = Time.realtimeSinceStartup + timeFromNow;
      EditorApplication.CallbackFunction update = EditorApplication.update;
      // ISSUE: reference to a compiler-generated field
      if (EditorApplication.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditorApplication.\u003C\u003Ef__mg\u0024cache0 = new EditorApplication.CallbackFunction(EditorApplication.CheckCallDelayed);
      }
      // ISSUE: reference to a compiler-generated field
      EditorApplication.CallbackFunction fMgCache0 = EditorApplication.\u003C\u003Ef__mg\u0024cache0;
      EditorApplication.update = update + fMgCache0;
    }

    private static void CheckCallDelayed()
    {
      if ((double) Time.realtimeSinceStartup <= (double) EditorApplication.s_DelayedCallbackTime)
        return;
      EditorApplication.CallbackFunction update = EditorApplication.update;
      // ISSUE: reference to a compiler-generated field
      if (EditorApplication.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditorApplication.\u003C\u003Ef__mg\u0024cache1 = new EditorApplication.CallbackFunction(EditorApplication.CheckCallDelayed);
      }
      // ISSUE: reference to a compiler-generated field
      EditorApplication.CallbackFunction fMgCache1 = EditorApplication.\u003C\u003Ef__mg\u0024cache1;
      EditorApplication.update = update - fMgCache1;
      EditorApplication.delayedCallback();
    }

    private static void Internal_PauseStateChanged(PauseState state)
    {
      if (EditorApplication.playmodeStateChanged != null)
        EditorApplication.playmodeStateChanged();
      // ISSUE: reference to a compiler-generated field
      if (EditorApplication.pauseStateChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorApplication.pauseStateChanged(state);
    }

    private static void Internal_PlayModeStateChanged(PlayModeStateChange state)
    {
      if (EditorApplication.playmodeStateChanged != null)
        EditorApplication.playmodeStateChanged();
      // ISSUE: reference to a compiler-generated field
      if (EditorApplication.playModeStateChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorApplication.playModeStateChanged(state);
    }

    private static void Internal_CallKeyboardModifiersChanged()
    {
      if (EditorApplication.modifierKeysChanged == null)
        return;
      EditorApplication.modifierKeysChanged();
    }

    private static void Internal_CallWindowsReordered()
    {
      if (EditorApplication.windowsReordered == null)
        return;
      EditorApplication.windowsReordered();
    }

    [RequiredByNativeCode]
    private static void Internal_CallGlobalEventHandler()
    {
      if (EditorApplication.globalEventHandler != null)
        EditorApplication.globalEventHandler();
      WindowLayout.MaximizeKeyHandler();
      Event.current = (Event) null;
    }

    /// <summary>
    ///   <para>Create a new scene.</para>
    /// </summary>
    [Obsolete("Use EditorSceneManager.NewScene (NewSceneSetup.DefaultGameObjects)")]
    public static void NewScene()
    {
      EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
    }

    /// <summary>
    ///   <para>Create a new absolutely empty scene.</para>
    /// </summary>
    [Obsolete("Use EditorSceneManager.NewScene (NewSceneSetup.EmptyScene)")]
    public static void NewEmptyScene()
    {
      EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
    }

    /// <summary>
    ///   <para>Opens the scene at path.</para>
    /// </summary>
    /// <param name="path"></param>
    [Obsolete("Use EditorSceneManager.OpenScene")]
    public static bool OpenScene(string path)
    {
      if (!EditorApplication.isPlaying)
        return EditorSceneManager.OpenScene(path).IsValid();
      throw new InvalidOperationException("EditorApplication.OpenScene() cannot be called when in the Unity Editor is in play mode.");
    }

    /// <summary>
    ///   <para>Opens the scene at path additively.</para>
    /// </summary>
    /// <param name="path"></param>
    [Obsolete("Use EditorSceneManager.OpenScene")]
    public static void OpenSceneAdditive(string path)
    {
      if (Application.isPlaying)
        Debug.LogWarning((object) "Exiting playmode.\nOpenSceneAdditive was called at a point where there was no active scene.\nThis usually means it was called in a PostprocessScene function during scene loading or it was called during playmode.\nThis is no longer allowed. Use SceneManager.LoadScene to load scenes at runtime or in playmode.");
      SceneManager.MergeScenes(EditorSceneManager.OpenScene(path, OpenSceneMode.Additive), SceneManager.GetActiveScene());
    }

    /// <summary>
    ///   <para>Save the open scene.</para>
    /// </summary>
    /// <param name="path">The file path to save at. If empty, the current open scene will be overwritten, or if never saved before, a save dialog is shown.</param>
    /// <param name="saveAsCopy">If set to true, the scene will be saved without changing the currentScene and without clearing the unsaved changes marker.</param>
    /// <returns>
    ///   <para>True if the save succeeded, otherwise false.</para>
    /// </returns>
    [Obsolete("Use EditorSceneManager.SaveScene")]
    public static bool SaveScene()
    {
      return EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
    }

    /// <summary>
    ///   <para>Save the open scene.</para>
    /// </summary>
    /// <param name="path">The file path to save at. If empty, the current open scene will be overwritten, or if never saved before, a save dialog is shown.</param>
    /// <param name="saveAsCopy">If set to true, the scene will be saved without changing the currentScene and without clearing the unsaved changes marker.</param>
    /// <returns>
    ///   <para>True if the save succeeded, otherwise false.</para>
    /// </returns>
    [Obsolete("Use EditorSceneManager.SaveScene")]
    public static bool SaveScene(string path)
    {
      return EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path, false);
    }

    /// <summary>
    ///   <para>Save the open scene.</para>
    /// </summary>
    /// <param name="path">The file path to save at. If empty, the current open scene will be overwritten, or if never saved before, a save dialog is shown.</param>
    /// <param name="saveAsCopy">If set to true, the scene will be saved without changing the currentScene and without clearing the unsaved changes marker.</param>
    /// <returns>
    ///   <para>True if the save succeeded, otherwise false.</para>
    /// </returns>
    [Obsolete("Use EditorSceneManager.SaveScene")]
    public static bool SaveScene(string path, bool saveAsCopy)
    {
      return EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path, saveAsCopy);
    }

    /// <summary>
    ///   <para>Ask the user if they want to save the open scene.</para>
    /// </summary>
    [Obsolete("Use EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo")]
    public static bool SaveCurrentSceneIfUserWantsTo()
    {
      return EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }

    [Obsolete("This function is internal and no longer supported")]
    internal static bool SaveCurrentSceneIfUserWantsToForce()
    {
      return false;
    }

    /// <summary>
    ///   <para>Explicitly mark the current opened scene as modified.</para>
    /// </summary>
    [Obsolete("Use EditorSceneManager.MarkSceneDirty or EditorSceneManager.MarkAllScenesDirty")]
    public static void MarkSceneDirty()
    {
      EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    /// <summary>
    ///   <para>Is true if the currently open scene in the editor contains unsaved modifications.</para>
    /// </summary>
    [Obsolete("Use Scene.isDirty instead. Use EditorSceneManager.GetScene API to get each open scene")]
    public static bool isSceneDirty
    {
      get
      {
        return SceneManager.GetActiveScene().isDirty;
      }
    }

    /// <summary>
    ///   <para>The path of the scene that the user has currently open (Will be an empty string if no scene is currently open). (Read Only)</para>
    /// </summary>
    [Obsolete("Use EditorSceneManager to see which scenes are currently loaded")]
    public static string currentScene
    {
      get
      {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.IsValid())
          return activeScene.path;
        return "";
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Delegate to be called for every visible list item in the ProjectWindow on every OnGUI event.</para>
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="selectionRect"></param>
    public delegate void ProjectWindowItemCallback(string guid, Rect selectionRect);

    /// <summary>
    ///   <para>Delegate to be called for every visible list item in the HierarchyWindow on every OnGUI event.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="selectionRect"></param>
    public delegate void HierarchyWindowItemCallback(int instanceID, Rect selectionRect);

    /// <summary>
    ///   <para>Delegate to be called from EditorApplication callbacks.</para>
    /// </summary>
    public delegate void CallbackFunction();

    /// <summary>
    ///   <para>Delegate to be called from EditorApplication contextual inspector callbacks.</para>
    /// </summary>
    /// <param name="menu">The contextual menu which is about to be shown to the user.</param>
    /// <param name="property">The property for which the contextual menu is shown.</param>
    public delegate void SerializedPropertyCallbackFunction(GenericMenu menu, SerializedProperty property);
  }
}
