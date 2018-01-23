// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefabUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEditor.Utils;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Utility class for any prefab related operations.</para>
  /// </summary>
  public sealed class PrefabUtility
  {
    /// <summary>
    ///   <para>Called after prefab instances in the scene have been updated.</para>
    /// </summary>
    public static PrefabUtility.PrefabInstanceUpdated prefabInstanceUpdated;
    private const string kMaterialExtension = ".mat";

    /// <summary>
    ///   <para>Returns the parent asset object of source, or null if it can't be found.</para>
    /// </summary>
    /// <param name="source"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetPrefabParent(UnityEngine.Object source);

    /// <summary>
    ///   <para>Retrieves the enclosing prefab for any object contained within.</para>
    /// </summary>
    /// <param name="targetObject">An object contained within a prefab object.</param>
    /// <returns>
    ///   <para>The prefab the object is contained in.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetPrefabObject(UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Extract all modifications that are applied to the prefab instance compared to the parent prefab.</para>
    /// </summary>
    /// <param name="targetPrefab"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern PropertyModification[] GetPropertyModifications(UnityEngine.Object targetPrefab);

    /// <summary>
    ///   <para>Assigns all modifications that are applied to the prefab instance compared to the parent prefab.</para>
    /// </summary>
    /// <param name="targetPrefab"></param>
    /// <param name="modifications"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPropertyModifications(UnityEngine.Object targetPrefab, PropertyModification[] modifications);

    /// <summary>
    ///   <para>Instantiate an asset that is referenced by a prefab and use it on the prefab instance.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object InstantiateAttachedAsset(UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Causes modifications made to the Prefab instance to be recorded.</para>
    /// </summary>
    /// <param name="targetObject">Object to process.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RecordPrefabInstancePropertyModifications(UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Force re-merging all prefab instances of this prefab.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MergeAllPrefabInstances(UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Disconnects the prefab instance from its parent prefab.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DisconnectPrefabInstance(UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Instantiates the given prefab in a given scene.</para>
    /// </summary>
    /// <param name="target">Prefab asset to instantiate.</param>
    /// <param name="destinationScene">Scene to instantiate the prefab in.</param>
    /// <returns>
    ///   <para>The GameObject at the root of the prefab.</para>
    /// </returns>
    public static UnityEngine.Object InstantiatePrefab(UnityEngine.Object target)
    {
      return PrefabUtility.InternalInstantiatePrefab(target, EditorSceneManager.GetTargetSceneForNewGameObjects());
    }

    /// <summary>
    ///   <para>Instantiates the given prefab in a given scene.</para>
    /// </summary>
    /// <param name="target">Prefab asset to instantiate.</param>
    /// <param name="destinationScene">Scene to instantiate the prefab in.</param>
    /// <returns>
    ///   <para>The GameObject at the root of the prefab.</para>
    /// </returns>
    public static UnityEngine.Object InstantiatePrefab(UnityEngine.Object target, Scene destinationScene)
    {
      return PrefabUtility.InternalInstantiatePrefab(target, destinationScene);
    }

    private static UnityEngine.Object InternalInstantiatePrefab(UnityEngine.Object target, Scene destinationScene)
    {
      return PrefabUtility.INTERNAL_CALL_InternalInstantiatePrefab(target, ref destinationScene);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityEngine.Object INTERNAL_CALL_InternalInstantiatePrefab(UnityEngine.Object target, ref Scene destinationScene);

    /// <summary>
    ///   <para>Creates an empty prefab at given path.</para>
    /// </summary>
    /// <param name="path"></param>
    public static UnityEngine.Object CreateEmptyPrefab(string path)
    {
      if (!Paths.IsValidAssetPathWithErrorLogging(path, ".prefab"))
        return (UnityEngine.Object) null;
      return PrefabUtility.Internal_CreateEmptyPrefab(path);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityEngine.Object Internal_CreateEmptyPrefab(string path);

    /// <summary>
    ///   <para>Creates a prefab from a game object hierarchy.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public static GameObject CreatePrefab(string path, GameObject go)
    {
      ReplacePrefabOptions options = ReplacePrefabOptions.Default;
      return PrefabUtility.CreatePrefab(path, go, options);
    }

    /// <summary>
    ///   <para>Creates a prefab from a game object hierarchy.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    /// <param name="options"></param>
    public static GameObject CreatePrefab(string path, GameObject go, [DefaultValue("ReplacePrefabOptions.Default")] ReplacePrefabOptions options)
    {
      if (!Paths.IsValidAssetPathWithErrorLogging(path, ".prefab"))
        return (GameObject) null;
      return PrefabUtility.Internal_CreatePrefab(path, go, options);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern GameObject Internal_CreatePrefab(string path, GameObject go, [DefaultValue("ReplacePrefabOptions.Default")] ReplacePrefabOptions options);

    [ExcludeFromDocs]
    private static GameObject Internal_CreatePrefab(string path, GameObject go)
    {
      ReplacePrefabOptions options = ReplacePrefabOptions.Default;
      return PrefabUtility.Internal_CreatePrefab(path, go, options);
    }

    /// <summary>
    ///   <para>Replaces the targetPrefab with a copy of the game object hierarchy go.</para>
    /// </summary>
    /// <param name="go"></param>
    /// <param name="targetPrefab"></param>
    /// <param name="options"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject ReplacePrefab(GameObject go, UnityEngine.Object targetPrefab, [DefaultValue("ReplacePrefabOptions.Default")] ReplacePrefabOptions options);

    /// <summary>
    ///   <para>Replaces the targetPrefab with a copy of the game object hierarchy go.</para>
    /// </summary>
    /// <param name="go"></param>
    /// <param name="targetPrefab"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public static GameObject ReplacePrefab(GameObject go, UnityEngine.Object targetPrefab)
    {
      ReplacePrefabOptions options = ReplacePrefabOptions.Default;
      return PrefabUtility.ReplacePrefab(go, targetPrefab, options);
    }

    /// <summary>
    ///   <para>Connects the source prefab to the game object.</para>
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sourcePrefab"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject ConnectGameObjectToPrefab(GameObject go, GameObject sourcePrefab);

    /// <summary>
    ///   <para>Returns the topmost game object that has the same prefab parent as target.</para>
    /// </summary>
    /// <param name="destinationScene">Scene to instantiate the prefab in.</param>
    /// <param name="target"></param>
    /// <returns>
    ///   <para>The GameObject at the root of the prefab.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindRootGameObjectWithSameParentPrefab(GameObject target);

    /// <summary>
    ///   <para>Returns root game object of the prefab instance if that root prefab instance is a parent of the prefab.</para>
    /// </summary>
    /// <param name="target">GameObject to process.</param>
    /// <returns>
    ///   <para>Return the root game object of the prefab asset.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindValidUploadPrefabInstanceRoot(GameObject target);

    /// <summary>
    ///   <para>Connects the game object to the prefab that it was last connected to.</para>
    /// </summary>
    /// <param name="go"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ReconnectToLastPrefab(GameObject go);

    /// <summary>
    ///   <para>Resets the properties of the component or game object to the parent prefab state.</para>
    /// </summary>
    /// <param name="obj"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ResetToPrefabState(UnityEngine.Object obj);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsComponentAddedToPrefabInstance(UnityEngine.Object source);

    /// <summary>
    ///   <para>Resets the properties of all objects in the prefab, including child game objects and components that were added to the prefab instance.</para>
    /// </summary>
    /// <param name="go"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RevertPrefabInstance(GameObject go);

    /// <summary>
    ///   <para>Given an object, returns its prefab type (None, if it's not a prefab).</para>
    /// </summary>
    /// <param name="target"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern PrefabType GetPrefabType(UnityEngine.Object target);

    /// <summary>
    ///   <para>Helper function to find the prefab root of an object (used for picking niceness).</para>
    /// </summary>
    /// <param name="source">The object to check.</param>
    /// <returns>
    ///   <para>The prefab root.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindPrefabRoot(GameObject source);

    private static void Internal_CallPrefabInstanceUpdated(GameObject instance)
    {
      if (PrefabUtility.prefabInstanceUpdated == null)
        return;
      PrefabUtility.prefabInstanceUpdated(instance);
    }

    [RequiredByNativeCode]
    internal static void ExtractSelectedObjectsFromPrefab()
    {
      HashSet<string> stringSet = new HashSet<string>();
      string str1 = (string) null;
      foreach (UnityEngine.Object @object in Selection.objects)
      {
        string assetPath = AssetDatabase.GetAssetPath(@object);
        if (str1 == null)
        {
          string path = EditorUtility.SaveFolderPanel("Select Materials Folder", FileUtil.DeleteLastPathNameComponent(assetPath), "");
          if (string.IsNullOrEmpty(path))
            return;
          str1 = FileUtil.GetProjectRelativePath(path);
        }
        string str2 = !(@object is Material) ? string.Empty : ".mat";
        string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(FileUtil.CombinePaths(str1, @object.name) + str2);
        if (string.IsNullOrEmpty(AssetDatabase.ExtractAsset(@object, uniqueAssetPath)))
          stringSet.Add(assetPath);
      }
      foreach (string path in stringSet)
      {
        AssetDatabase.WriteImportSettingsIfDirty(path);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
      }
    }

    internal static void ExtractMaterialsFromAsset(UnityEngine.Object[] targets, string destinationPath)
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (UnityEngine.Object target in targets)
      {
        ModelImporter modelImporter = target as ModelImporter;
        foreach (UnityEngine.Object asset in ((IEnumerable<UnityEngine.Object>) AssetDatabase.LoadAllAssetsAtPath(modelImporter.assetPath)).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => x.GetType() == typeof (Material))))
        {
          string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(FileUtil.CombinePaths(destinationPath, asset.name) + ".mat");
          if (string.IsNullOrEmpty(AssetDatabase.ExtractAsset(asset, uniqueAssetPath)))
            stringSet.Add(modelImporter.assetPath);
        }
      }
      foreach (string path in stringSet)
      {
        AssetDatabase.WriteImportSettingsIfDirty(path);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
      }
    }

    private static void GetObjectListFromHierarchy(List<UnityEngine.Object> hierarchy, GameObject gameObject)
    {
      Transform transform = (Transform) null;
      List<Component> results = new List<Component>();
      hierarchy.Add((UnityEngine.Object) gameObject);
      gameObject.GetComponents<Component>(results);
      foreach (Component component in results)
      {
        if (component is Transform)
          transform = component as Transform;
        else
          hierarchy.Add((UnityEngine.Object) component);
      }
      if (!((UnityEngine.Object) transform != (UnityEngine.Object) null))
        return;
      int childCount = transform.childCount;
      for (int index = 0; index < childCount; ++index)
        PrefabUtility.GetObjectListFromHierarchy(hierarchy, transform.GetChild(index).gameObject);
    }

    private static void RegisterNewObjects(List<UnityEngine.Object> newHierarchy, List<UnityEngine.Object> hierarchy, string actionName)
    {
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      foreach (UnityEngine.Object object1 in newHierarchy)
      {
        bool flag = false;
        foreach (UnityEngine.Object object2 in hierarchy)
        {
          if (object2.GetInstanceID() == object1.GetInstanceID())
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          objectList.Add(object1);
      }
      HashSet<System.Type> typeSet = new HashSet<System.Type>() { typeof (Transform) };
      bool flag1 = false;
      while (objectList.Count > 0 && !flag1)
      {
        flag1 = true;
        for (int index = 0; index < objectList.Count; ++index)
        {
          UnityEngine.Object objectToUndo = objectList[index];
          object[] customAttributes = objectToUndo.GetType().GetCustomAttributes(typeof (RequireComponent), true);
          bool flag2 = true;
          foreach (RequireComponent requireComponent in customAttributes)
          {
            if (requireComponent.m_Type0 != null && !typeSet.Contains(requireComponent.m_Type0) || requireComponent.m_Type1 != null && !typeSet.Contains(requireComponent.m_Type1) || requireComponent.m_Type2 != null && !typeSet.Contains(requireComponent.m_Type2))
            {
              flag2 = false;
              break;
            }
          }
          if (flag2)
          {
            Undo.RegisterCreatedObjectUndo(objectToUndo, actionName);
            typeSet.Add(objectToUndo.GetType());
            objectList.RemoveAt(index);
            --index;
            flag1 = false;
          }
        }
      }
      foreach (UnityEngine.Object objectToUndo in objectList)
        Undo.RegisterCreatedObjectUndo(objectToUndo, actionName);
    }

    internal static void RevertPrefabInstanceWithUndo(GameObject target)
    {
      string str = "Revert Prefab Instance";
      PrefabType prefabType = PrefabUtility.GetPrefabType((UnityEngine.Object) target);
      bool flag = prefabType == PrefabType.DisconnectedModelPrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance;
      GameObject gameObject = !flag ? PrefabUtility.FindValidUploadPrefabInstanceRoot(target) : PrefabUtility.FindRootGameObjectWithSameParentPrefab(target);
      List<UnityEngine.Object> hierarchy = new List<UnityEngine.Object>();
      PrefabUtility.GetObjectListFromHierarchy(hierarchy, gameObject);
      Undo.RegisterFullObjectHierarchyUndo((UnityEngine.Object) gameObject, str);
      if (flag)
      {
        PrefabUtility.ReconnectToLastPrefab(gameObject);
        Undo.RegisterCreatedObjectUndo(PrefabUtility.GetPrefabObject((UnityEngine.Object) gameObject), str);
      }
      PrefabUtility.RevertPrefabInstance(gameObject);
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      PrefabUtility.GetObjectListFromHierarchy(objectList, PrefabUtility.FindPrefabRoot(gameObject));
      PrefabUtility.RegisterNewObjects(objectList, hierarchy, str);
    }

    internal static void ReplacePrefabWithUndo(GameObject target)
    {
      string str = "Apply instance to prefab";
      UnityEngine.Object prefabParent = PrefabUtility.GetPrefabParent((UnityEngine.Object) target);
      GameObject prefabInstanceRoot = PrefabUtility.FindValidUploadPrefabInstanceRoot(target);
      Undo.RegisterFullObjectHierarchyUndo(prefabParent, str);
      Undo.RegisterFullObjectHierarchyUndo((UnityEngine.Object) prefabInstanceRoot, str);
      Undo.RegisterCreatedObjectUndo((UnityEngine.Object) prefabInstanceRoot, str);
      List<UnityEngine.Object> hierarchy = new List<UnityEngine.Object>();
      PrefabUtility.GetObjectListFromHierarchy(hierarchy, prefabParent as GameObject);
      PrefabUtility.ReplacePrefab(prefabInstanceRoot, prefabParent, ReplacePrefabOptions.ConnectToPrefab);
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      PrefabUtility.GetObjectListFromHierarchy(objectList, prefabParent as GameObject);
      PrefabUtility.RegisterNewObjects(objectList, hierarchy, str);
    }

    /// <summary>
    ///   <para>Delegate for method that is called after prefab instances in the scene have been updated.</para>
    /// </summary>
    /// <param name="instance"></param>
    public delegate void PrefabInstanceUpdated(GameObject instance);
  }
}
