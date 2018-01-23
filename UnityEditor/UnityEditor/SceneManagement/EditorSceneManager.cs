// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneManagement.EditorSceneManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor.Utils;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor.SceneManagement
{
  /// <summary>
  ///   <para>Scene management in the Editor.</para>
  /// </summary>
  public sealed class EditorSceneManager : SceneManager
  {
    internal static UnityAction<Scene, NewSceneMode> sceneWasCreated;
    internal static UnityAction<Scene, OpenSceneMode> sceneWasOpened;

    /// <summary>
    ///   <para>The number of loaded Scenes.</para>
    /// </summary>
    public static extern int loadedSceneCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Open a Scene in the Editor.</para>
    /// </summary>
    /// <param name="scenePath">The path of the Scene. This should be relative to the Project folder; for example, "AssetsMyScenesMyScene.unity".</param>
    /// <param name="mode">Allows you to select how to open the specified Scene, and whether to keep existing Scenes in the Hierarchy. See SceneManagement.OpenSceneMode for more information about the options.</param>
    /// <returns>
    ///   <para>A reference to the opened Scene.</para>
    /// </returns>
    public static Scene OpenScene(string scenePath, [DefaultValue("OpenSceneMode.Single")] OpenSceneMode mode)
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_OpenScene(scenePath, mode, out scene);
      return scene;
    }

    [ExcludeFromDocs]
    public static Scene OpenScene(string scenePath)
    {
      OpenSceneMode mode = OpenSceneMode.Single;
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_OpenScene(scenePath, mode, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_OpenScene(string scenePath, OpenSceneMode mode, out Scene value);

    /// <summary>
    ///   <para>Create a new Scene.</para>
    /// </summary>
    /// <param name="setup">Whether the new Scene should use the default set of GameObjects.</param>
    /// <param name="mode">Whether to keep existing Scenes open.</param>
    /// <returns>
    ///   <para>A reference to the new Scene.</para>
    /// </returns>
    public static Scene NewScene(NewSceneSetup setup, [DefaultValue("NewSceneMode.Single")] NewSceneMode mode)
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_NewScene(setup, mode, out scene);
      return scene;
    }

    [ExcludeFromDocs]
    public static Scene NewScene(NewSceneSetup setup)
    {
      NewSceneMode mode = NewSceneMode.Single;
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_NewScene(setup, mode, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_NewScene(NewSceneSetup setup, NewSceneMode mode, out Scene value);

    /// <summary>
    ///   <para>Creates a new preview scene.  Any object added to a preview scene will only be rendered in that scene.</para>
    /// </summary>
    /// <returns>
    ///   <para>The new preview scene.</para>
    /// </returns>
    public static Scene NewPreviewScene()
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_NewPreviewScene(out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_NewPreviewScene(out Scene value);

    internal static bool CreateSceneAsset(string scenePath, bool createDefaultGameObjects)
    {
      if (!Paths.IsValidAssetPathWithErrorLogging(scenePath, ".unity"))
        return false;
      return EditorSceneManager.Internal_CreateSceneAsset(scenePath, createDefaultGameObjects);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Internal_CreateSceneAsset(string scenePath, bool createDefaultGameObjects);

    /// <summary>
    ///   <para>Close the Scene. If removeScene flag is true, the closed Scene will also be removed from EditorSceneManager.</para>
    /// </summary>
    /// <param name="scene">The Scene to be closed/removed.</param>
    /// <param name="removeScene">Bool flag to indicate if the Scene should be removed after closing.</param>
    /// <returns>
    ///   <para>Returns true if the Scene is closed/removed.</para>
    /// </returns>
    public static bool CloseScene(Scene scene, bool removeScene)
    {
      return EditorSceneManager.INTERNAL_CALL_CloseScene(ref scene, removeScene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_CloseScene(ref Scene scene, bool removeScene);

    /// <summary>
    ///   <para>Closes a preview scene created by NewPreviewScene.</para>
    /// </summary>
    /// <param name="scene">The preview scene to close.</param>
    /// <returns>
    ///   <para>True if the scene was successfully closed.</para>
    /// </returns>
    public static bool ClosePreviewScene(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_ClosePreviewScene(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_ClosePreviewScene(ref Scene scene);

    internal static bool ReloadScene(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_ReloadScene(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_ReloadScene(ref Scene scene);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetTargetSceneForNewGameObjects(int sceneHandle);

    internal static Scene GetTargetSceneForNewGameObjects()
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_GetTargetSceneForNewGameObjects(out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetTargetSceneForNewGameObjects(out Scene value);

    internal static Scene GetSceneByHandle(int handle)
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_GetSceneByHandle(handle, out scene);
      return scene;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSceneByHandle(int handle, out Scene value);

    /// <summary>
    ///   <para>Allows you to reorder the Scenes currently open in the Hierarchy window. Moves the source Scene so it comes before the destination Scene.</para>
    /// </summary>
    /// <param name="src">The Scene to move.</param>
    /// <param name="dst">The Scene which should come directly after the source Scene in the hierarchy.</param>
    public static void MoveSceneBefore(Scene src, Scene dst)
    {
      EditorSceneManager.INTERNAL_CALL_MoveSceneBefore(ref src, ref dst);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MoveSceneBefore(ref Scene src, ref Scene dst);

    /// <summary>
    ///   <para>Allows you to reorder the Scenes currently open in the Hierarchy window. Moves the source Scene so it comes after the destination Scene.</para>
    /// </summary>
    /// <param name="src">The Scene to move.</param>
    /// <param name="dst">The Scene which should come directly before the source Scene in the hierarchy.</param>
    public static void MoveSceneAfter(Scene src, Scene dst)
    {
      EditorSceneManager.INTERNAL_CALL_MoveSceneAfter(ref src, ref dst);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MoveSceneAfter(ref Scene src, ref Scene dst);

    internal static bool SaveSceneAs(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_SaveSceneAs(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SaveSceneAs(ref Scene scene);

    [ExcludeFromDocs]
    public static bool SaveScene(Scene scene, string dstScenePath)
    {
      bool saveAsCopy = false;
      return EditorSceneManager.SaveScene(scene, dstScenePath, saveAsCopy);
    }

    [ExcludeFromDocs]
    public static bool SaveScene(Scene scene)
    {
      bool saveAsCopy = false;
      string dstScenePath = "";
      return EditorSceneManager.SaveScene(scene, dstScenePath, saveAsCopy);
    }

    /// <summary>
    ///   <para>Save a Scene.</para>
    /// </summary>
    /// <param name="scene">The Scene to be saved.</param>
    /// <param name="dstScenePath">The file path to save the Scene to. If the path is empty, the current open Scene is overwritten. If it has not yet been saved at all, a save dialog is shown.</param>
    /// <param name="saveAsCopy">If set to true, the Scene is saved without changing the current Scene, and without clearing the unsaved changes marker.</param>
    /// <returns>
    ///   <para>True if the save succeeded, otherwise false.</para>
    /// </returns>
    public static bool SaveScene(Scene scene, [DefaultValue("\"\"")] string dstScenePath, [DefaultValue("false")] bool saveAsCopy)
    {
      if (!string.IsNullOrEmpty(dstScenePath) && !Paths.IsValidAssetPathWithErrorLogging(dstScenePath, ".unity"))
        return false;
      return EditorSceneManager.Internal_SaveScene(scene, dstScenePath, saveAsCopy);
    }

    private static bool Internal_SaveScene(Scene scene, string dstScenePath, bool saveAsCopy)
    {
      return EditorSceneManager.INTERNAL_CALL_Internal_SaveScene(ref scene, dstScenePath, saveAsCopy);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Internal_SaveScene(ref Scene scene, string dstScenePath, bool saveAsCopy);

    /// <summary>
    ///   <para>Save all open Scenes.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if all open Scenes are successfully saved.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveOpenScenes();

    /// <summary>
    ///   <para>Save a list of Scenes.</para>
    /// </summary>
    /// <param name="scenes">List of Scenes that should be saved.</param>
    /// <returns>
    ///   <para>True if the save succeeded. Otherwise false.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveScenes(Scene[] scenes);

    /// <summary>
    ///   <para>Asks you if you want to save the modified Scene or Scenes.</para>
    /// </summary>
    /// <returns>
    ///   <para>This returns true if you chose to save the Scene or Scenes, and returns false if you pressed Cancel.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveCurrentModifiedScenesIfUserWantsTo();

    /// <summary>
    ///   <para>Asks whether the modfied input Scenes should be saved.</para>
    /// </summary>
    /// <param name="scenes">Scenes that should be saved if they are modified.</param>
    /// <returns>
    ///   <para>Your choice of whether to save or not save the Scenes.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveModifiedScenesIfUserWantsTo(Scene[] scenes);

    /// <summary>
    ///   <para>Shows a save dialog if an Untitled scene exists in the current scene manager setup.</para>
    /// </summary>
    /// <param name="dialogContent">Text shown in the save dialog.</param>
    /// <returns>
    ///   <para>True if the scene is saved or if there is no Untitled scene.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EnsureUntitledSceneHasBeenSaved(string dialogContent);

    /// <summary>
    ///   <para>Mark the specified Scene as modified.</para>
    /// </summary>
    /// <param name="scene">The Scene to be marked as modified.</param>
    /// <returns>
    ///   <para>Whether the Scene was successfully marked as dirty.</para>
    /// </returns>
    public static bool MarkSceneDirty(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_MarkSceneDirty(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_MarkSceneDirty(ref Scene scene);

    /// <summary>
    ///   <para>Mark all the loaded Scenes as modified.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MarkAllScenesDirty();

    /// <summary>
    ///   <para>Returns the current setup of the SceneManager.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of SceneSetup classes - one item for each Scene.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern SceneSetup[] GetSceneManagerSetup();

    /// <summary>
    ///   <para>Restore the setup of the SceneManager.</para>
    /// </summary>
    /// <param name="value">In this array, at least one Scene should be loaded, and there must be one active Scene.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RestoreSceneManagerSetup(SceneSetup[] value);

    /// <summary>
    ///   <para>Controls whether cross-Scene references are allowed in the Editor.</para>
    /// </summary>
    public static extern bool preventCrossSceneReferences { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Detects cross-scene references in a Scene.</para>
    /// </summary>
    /// <param name="scene">Scene to check for cross-scene references.</param>
    /// <returns>
    ///   <para>Was any cross-scene references found.</para>
    /// </returns>
    public static bool DetectCrossSceneReferences(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_DetectCrossSceneReferences(ref scene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_DetectCrossSceneReferences(ref Scene scene);

    /// <summary>
    ///   <para>Loads this SceneAsset when you start Play Mode.</para>
    /// </summary>
    public static extern SceneAsset playModeStartScene { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    private static void Internal_NewSceneWasCreated(Scene scene, NewSceneMode mode)
    {
      if (EditorSceneManager.sceneWasCreated == null)
        return;
      EditorSceneManager.sceneWasCreated(scene, mode);
    }

    private static void Internal_SceneWasOpened(Scene scene, OpenSceneMode mode)
    {
      if (EditorSceneManager.sceneWasOpened == null)
        return;
      EditorSceneManager.sceneWasOpened(scene, mode);
    }

    public static event EditorSceneManager.NewSceneCreatedCallback newSceneCreated;

    public static event EditorSceneManager.SceneOpeningCallback sceneOpening;

    public static event EditorSceneManager.SceneOpenedCallback sceneOpened;

    public static event EditorSceneManager.SceneClosingCallback sceneClosing;

    public static event EditorSceneManager.SceneClosedCallback sceneClosed;

    public static event EditorSceneManager.SceneSavingCallback sceneSaving;

    public static event EditorSceneManager.SceneSavedCallback sceneSaved;

    [RequiredByNativeCode]
    private static void Internal_NewSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.newSceneCreated == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.newSceneCreated(scene, setup, mode);
    }

    [RequiredByNativeCode]
    private static void Internal_SceneOpening(string path, OpenSceneMode mode)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.sceneOpening == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneOpening(path, mode);
    }

    [RequiredByNativeCode]
    private static void Internal_SceneOpened(Scene scene, OpenSceneMode mode)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.sceneOpened == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneOpened(scene, mode);
    }

    [RequiredByNativeCode]
    private static void Internal_SceneClosing(Scene scene, bool removingScene)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.sceneClosing == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneClosing(scene, removingScene);
    }

    [RequiredByNativeCode]
    private static void Internal_SceneClosed(Scene scene)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.sceneClosed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneClosed(scene);
    }

    [RequiredByNativeCode]
    private static void Internal_SceneSaving(Scene scene, string path)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.sceneSaving == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneSaving(scene, path);
    }

    [RequiredByNativeCode]
    private static void Internal_SceneSaved(Scene scene)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorSceneManager.sceneSaved == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneSaved(scene);
    }

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the newSceneCreated event are called after a new Scene has been created.</para>
    /// </summary>
    /// <param name="scene">The Scene that was created.</param>
    /// <param name="setup">The setup mode used when creating the Scene.</param>
    /// <param name="mode">The mode used for creating the Scene.</param>
    public delegate void NewSceneCreatedCallback(Scene scene, NewSceneSetup setup, NewSceneMode mode);

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the sceneOpening event are called just before opening a Scene.</para>
    /// </summary>
    /// <param name="path">Path of the Scene to be opened. This is relative to the Project path.</param>
    /// <param name="mode">Mode that is used when opening the Scene.</param>
    public delegate void SceneOpeningCallback(string path, OpenSceneMode mode);

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the sceneOpened event are called after a Scene has been opened.</para>
    /// </summary>
    /// <param name="scene">The Scene that was opened.</param>
    /// <param name="mode">The mode used to open the Scene.</param>
    public delegate void SceneOpenedCallback(Scene scene, OpenSceneMode mode);

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the sceneClosing event are called just before a Scene is closed.</para>
    /// </summary>
    /// <param name="scene">The Scene that is going to be closed.</param>
    /// <param name="removingScene">Whether or not the Scene is also going to be removed from the Scene Manager after closing. If true the Scene is removed after closing.</param>
    public delegate void SceneClosingCallback(Scene scene, bool removingScene);

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the sceneClosed event are called immediately after the Scene has been closed.</para>
    /// </summary>
    /// <param name="scene">The Scene that was closed.</param>
    public delegate void SceneClosedCallback(Scene scene);

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the sceneSaving event are called just before the Scene is saved.</para>
    /// </summary>
    /// <param name="scene">The Scene to be saved.</param>
    /// <param name="path">The path to which the Scene is saved.</param>
    public delegate void SceneSavingCallback(Scene scene, string path);

    /// <summary>
    ///   <para>Callbacks of this type which have been added to the sceneSaved event are called after a Scene has been saved.</para>
    /// </summary>
    /// <param name="scene">The Scene that was saved.</param>
    public delegate void SceneSavedCallback(Scene scene);
  }
}
