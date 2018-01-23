// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.ProjectWindowCallback;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  public class ProjectWindowUtil
  {
    internal static int k_FavoritesStartInstanceID = 1000000000;
    internal static string k_DraggingFavoriteGenericData = "DraggingFavorite";
    internal static string k_IsFolderGenericData = "IsFolder";

    [MenuItem("Assets/Create/GUI Skin", false, 601)]
    public static void CreateNewGUISkin()
    {
      GUISkin instance = ScriptableObject.CreateInstance<GUISkin>();
      GUISkin builtinResource = UnityEngine.Resources.GetBuiltinResource(typeof (GUISkin), "GameSkin/GameSkin.guiskin") as GUISkin;
      if ((bool) ((UnityEngine.Object) builtinResource))
        EditorUtility.CopySerialized((UnityEngine.Object) builtinResource, (UnityEngine.Object) instance);
      else
        UnityEngine.Debug.LogError((object) "Internal error: unable to load builtin GUIskin");
      ProjectWindowUtil.CreateAsset((UnityEngine.Object) instance, "New GUISkin.guiskin");
    }

    internal static string GetActiveFolderPath()
    {
      ProjectBrowser projectBrowserIfExists = ProjectWindowUtil.GetProjectBrowserIfExists();
      if ((UnityEngine.Object) projectBrowserIfExists == (UnityEngine.Object) null)
        return "Assets";
      return projectBrowserIfExists.GetActiveFolderPath();
    }

    internal static void EndNameEditAction(EndNameEditAction action, int instanceId, string pathName, string resourceFile)
    {
      pathName = AssetDatabase.GenerateUniqueAssetPath(pathName);
      if (!((UnityEngine.Object) action != (UnityEngine.Object) null))
        return;
      action.Action(instanceId, pathName, resourceFile);
      action.CleanUp();
    }

    public static void CreateAsset(UnityEngine.Object asset, string pathName)
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(asset.GetInstanceID(), (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateNewAsset>(), pathName, AssetPreview.GetMiniThumbnail(asset), (string) null);
    }

    public static void CreateFolder()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateFolder>(), "New Folder", EditorGUIUtility.IconContent(EditorResourcesUtility.emptyFolderIconName).image as Texture2D, (string) null);
    }

    public static void CreateScene()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateScene>(), "New Scene.unity", EditorGUIUtility.FindTexture("SceneAsset Icon"), (string) null);
    }

    public static void CreatePrefab()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreatePrefab>(), "New Prefab.prefab", EditorGUIUtility.IconContent("Prefab Icon").image as Texture2D, (string) null);
    }

    internal static void CreateAssetWithContent(string filename, string content)
    {
      DoCreateAssetWithContent instance = ScriptableObject.CreateInstance<DoCreateAssetWithContent>();
      instance.filecontent = content;
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) instance, filename, (Texture2D) null, (string) null);
    }

    private static void CreateScriptAsset(string templatePath, string destName)
    {
      string fileName = Path.GetFileName(templatePath);
      if (fileName.ToLower().Contains("editortest") || fileName.ToLower().Contains("editmode"))
      {
        string str1 = AssetDatabase.GetUniquePathNameAtSelectedPath(destName);
        if (!str1.ToLower().Contains("/editor/"))
        {
          string str2 = str1.Substring(0, str1.Length - destName.Length - 1);
          string str3 = Path.Combine(str2, "Editor");
          if (!Directory.Exists(str3))
            AssetDatabase.CreateFolder(str2, "Editor");
          str1 = Path.Combine(str3, destName).Replace("\\", "/");
        }
        destName = str1;
      }
      Texture2D image;
      switch (Path.GetExtension(destName))
      {
        case ".js":
          image = EditorGUIUtility.IconContent("js Script Icon").image as Texture2D;
          break;
        case ".cs":
          image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
          break;
        case ".boo":
          image = EditorGUIUtility.IconContent("boo Script Icon").image as Texture2D;
          break;
        case ".shader":
          image = EditorGUIUtility.IconContent("Shader Icon").image as Texture2D;
          break;
        case ".asmdef":
          image = EditorGUIUtility.IconContent("AssemblyDefinitionAsset Icon").image as Texture2D;
          break;
        default:
          image = EditorGUIUtility.IconContent("TextAsset Icon").image as Texture2D;
          break;
      }
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateScriptAsset>(), destName, image, templatePath);
    }

    public static void ShowCreatedAsset(UnityEngine.Object o)
    {
      Selection.activeObject = o;
      if (!(bool) o)
        return;
      ProjectWindowUtil.FrameObjectInProjectWindow(o.GetInstanceID());
    }

    private static void CreateAnimatorController()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateAnimatorController>(), "New Animator Controller.controller", EditorGUIUtility.IconContent("AnimatorController Icon").image as Texture2D, (string) null);
    }

    private static void CreateAudioMixer()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateAudioMixer>(), "NewAudioMixer.mixer", EditorGUIUtility.IconContent("AudioMixerController Icon").image as Texture2D, (string) null);
    }

    private static void CreateSpritePolygon(int sides)
    {
      string str;
      switch (sides)
      {
        case 0:
          str = "Square";
          break;
        case 3:
          str = "Triangle";
          break;
        case 4:
          str = "Diamond";
          break;
        case 6:
          str = "Hexagon";
          break;
        default:
          str = sides == 42 ? "Everythingon" : (sides == 128 ? "Circle" : "Polygon");
          break;
      }
      Texture2D image = EditorGUIUtility.IconContent("Sprite Icon").image as Texture2D;
      DoCreateSpritePolygon instance = ScriptableObject.CreateInstance<DoCreateSpritePolygon>();
      instance.sides = sides;
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) instance, str + ".png", image, (string) null);
    }

    internal static string SetLineEndings(string content, LineEndingsMode lineEndingsMode)
    {
      string replacement;
      switch (lineEndingsMode)
      {
        case LineEndingsMode.OSNative:
          replacement = Application.platform != RuntimePlatform.WindowsEditor ? "\n" : "\r\n";
          break;
        case LineEndingsMode.Unix:
          replacement = "\n";
          break;
        case LineEndingsMode.Windows:
          replacement = "\r\n";
          break;
        default:
          replacement = "\n";
          break;
      }
      content = Regex.Replace(content, "\\r\\n?|\\n", replacement);
      return content;
    }

    internal static UnityEngine.Object CreateScriptAssetWithContent(string pathName, string templateContent)
    {
      templateContent = ProjectWindowUtil.SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);
      string fullPath = Path.GetFullPath(pathName);
      UTF8Encoding utF8Encoding = new UTF8Encoding(true);
      File.WriteAllText(fullPath, templateContent, (Encoding) utF8Encoding);
      AssetDatabase.ImportAsset(pathName);
      return AssetDatabase.LoadAssetAtPath(pathName, typeof (UnityEngine.Object));
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
      string str1 = File.ReadAllText(resourceFile).Replace("#NOTRIM#", "");
      string withoutExtension = Path.GetFileNameWithoutExtension(pathName);
      string str2 = str1.Replace("#NAME#", withoutExtension);
      string str3 = withoutExtension.Replace(" ", "");
      string str4 = str2.Replace("#SCRIPTNAME#", str3);
      string templateContent;
      if (char.IsUpper(str3, 0))
      {
        string newValue = ((int) char.ToLower(str3[0])).ToString() + str3.Substring(1);
        templateContent = str4.Replace("#SCRIPTNAME_LOWER#", newValue);
      }
      else
      {
        string newValue = "my" + (object) char.ToUpper(str3[0]) + str3.Substring(1);
        templateContent = str4.Replace("#SCRIPTNAME_LOWER#", newValue);
      }
      return ProjectWindowUtil.CreateScriptAssetWithContent(pathName, templateContent);
    }

    public static void StartNameEditingIfProjectWindowExists(int instanceID, EndNameEditAction endAction, string pathName, Texture2D icon, string resourceFile)
    {
      ProjectBrowser projectBrowserIfExists = ProjectWindowUtil.GetProjectBrowserIfExists();
      if ((bool) ((UnityEngine.Object) projectBrowserIfExists))
      {
        projectBrowserIfExists.Focus();
        projectBrowserIfExists.BeginPreimportedNameEditing(instanceID, endAction, pathName, icon, resourceFile);
        projectBrowserIfExists.Repaint();
      }
      else
      {
        if (!pathName.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
          pathName = "Assets/" + pathName;
        ProjectWindowUtil.EndNameEditAction(endAction, instanceID, pathName, resourceFile);
        Selection.activeObject = EditorUtility.InstanceIDToObject(instanceID);
      }
    }

    private static ProjectBrowser GetProjectBrowserIfExists()
    {
      return ProjectBrowser.s_LastInteractedProjectBrowser;
    }

    internal static void FrameObjectInProjectWindow(int instanceID)
    {
      ProjectBrowser projectBrowserIfExists = ProjectWindowUtil.GetProjectBrowserIfExists();
      if (!(bool) ((UnityEngine.Object) projectBrowserIfExists))
        return;
      projectBrowserIfExists.FrameObject(instanceID, false);
    }

    internal static bool IsFavoritesItem(int instanceID)
    {
      return instanceID >= ProjectWindowUtil.k_FavoritesStartInstanceID;
    }

    internal static void StartDrag(int draggedInstanceID, List<int> selectedInstanceIDs)
    {
      DragAndDrop.PrepareStartDrag();
      string title = "";
      if (ProjectWindowUtil.IsFavoritesItem(draggedInstanceID))
      {
        DragAndDrop.SetGenericData(ProjectWindowUtil.k_DraggingFavoriteGenericData, (object) draggedInstanceID);
      }
      else
      {
        bool flag = ProjectWindowUtil.IsFolder(draggedInstanceID);
        DragAndDrop.objectReferences = ProjectWindowUtil.GetDragAndDropObjects(draggedInstanceID, selectedInstanceIDs);
        DragAndDrop.SetGenericData(ProjectWindowUtil.k_IsFolderGenericData, !flag ? (object) "" : (object) "isFolder");
        string[] dragAndDropPaths = ProjectWindowUtil.GetDragAndDropPaths(draggedInstanceID, selectedInstanceIDs);
        if (dragAndDropPaths.Length > 0)
          DragAndDrop.paths = dragAndDropPaths;
        title = DragAndDrop.objectReferences.Length <= 1 ? ObjectNames.GetDragAndDropTitle(InternalEditorUtility.GetObjectFromInstanceID(draggedInstanceID)) : "<Multiple>";
      }
      DragAndDrop.StartDrag(title);
    }

    internal static UnityEngine.Object[] GetDragAndDropObjects(int draggedInstanceID, List<int> selectedInstanceIDs)
    {
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>(selectedInstanceIDs.Count);
      if (selectedInstanceIDs.Contains(draggedInstanceID))
      {
        for (int index = 0; index < selectedInstanceIDs.Count; ++index)
        {
          UnityEngine.Object objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(selectedInstanceIDs[index]);
          if (objectFromInstanceId != (UnityEngine.Object) null)
            objectList.Add(objectFromInstanceId);
        }
      }
      else
      {
        UnityEngine.Object objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(draggedInstanceID);
        if (objectFromInstanceId != (UnityEngine.Object) null)
          objectList.Add(objectFromInstanceId);
      }
      return objectList.ToArray();
    }

    internal static string[] GetDragAndDropPaths(int draggedInstanceID, List<int> selectedInstanceIDs)
    {
      List<string> stringList = new List<string>();
      foreach (int selectedInstanceId in selectedInstanceIDs)
      {
        if (AssetDatabase.IsMainAsset(selectedInstanceId))
        {
          string assetPath = AssetDatabase.GetAssetPath(selectedInstanceId);
          stringList.Add(assetPath);
        }
      }
      string assetPath1 = AssetDatabase.GetAssetPath(draggedInstanceID);
      if (string.IsNullOrEmpty(assetPath1))
        return new string[0];
      if (stringList.Contains(assetPath1))
        return stringList.ToArray();
      return new string[1]{ assetPath1 };
    }

    public static int[] GetAncestors(int instanceID)
    {
      List<int> intList = new List<int>();
      int mainAssetInstanceId1 = AssetDatabase.GetMainAssetInstanceID(AssetDatabase.GetAssetPath(instanceID));
      if (mainAssetInstanceId1 != instanceID)
        intList.Add(mainAssetInstanceId1);
      int mainAssetInstanceId2;
      for (string containingFolder = ProjectWindowUtil.GetContainingFolder(AssetDatabase.GetAssetPath(mainAssetInstanceId1)); !string.IsNullOrEmpty(containingFolder); containingFolder = ProjectWindowUtil.GetContainingFolder(AssetDatabase.GetAssetPath(mainAssetInstanceId2)))
      {
        mainAssetInstanceId2 = AssetDatabase.GetMainAssetInstanceID(containingFolder);
        intList.Add(mainAssetInstanceId2);
      }
      return intList.ToArray();
    }

    public static bool IsFolder(int instanceID)
    {
      return AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(instanceID));
    }

    public static string GetContainingFolder(string path)
    {
      if (string.IsNullOrEmpty(path))
        return (string) null;
      path = path.Trim('/');
      int length = path.LastIndexOf("/", StringComparison.Ordinal);
      if (length != -1)
        return path.Substring(0, length);
      return (string) null;
    }

    public static string[] GetBaseFolders(string[] folders)
    {
      if (folders.Length <= 1)
        return folders;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>((IEnumerable<string>) folders);
      for (int index = 0; index < stringList2.Count; ++index)
        stringList2[index] = stringList2[index].Trim('/');
      stringList2.Sort();
      for (int index = 0; index < stringList2.Count; ++index)
      {
        if (!stringList2[index].EndsWith("/"))
          stringList2[index] += "/";
      }
      string str = stringList2[0];
      stringList1.Add(str);
      for (int index = 1; index < stringList2.Count; ++index)
      {
        if (stringList2[index].IndexOf(str, StringComparison.Ordinal) != 0)
        {
          stringList1.Add(stringList2[index]);
          str = stringList2[index];
        }
      }
      for (int index = 0; index < stringList1.Count; ++index)
        stringList1[index] = stringList1[index].Trim('/');
      return stringList1.ToArray();
    }

    internal static void DuplicateSelectedAssets()
    {
      AssetDatabase.Refresh();
      List<UnityEngine.Object> list = ((IEnumerable<UnityEngine.Object>) Selection.objects).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (o => o is AnimationClip && AssetDatabase.Contains(o))).ToList<UnityEngine.Object>();
      Selection.objects = (list.Count <= 0 ? ProjectWindowUtil.DuplicateAssets(((IEnumerable<UnityEngine.Object>) Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets)).Except<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) list).Select<UnityEngine.Object, int>((Func<UnityEngine.Object, int>) (o => o.GetInstanceID()))) : ProjectWindowUtil.DuplicateAnimationClips(list.Cast<AnimationClip>()).Cast<UnityEngine.Object>()).ToArray<UnityEngine.Object>();
    }

    internal static bool DeleteAssets(List<int> instanceIDs, bool askIfSure)
    {
      if (instanceIDs.Count == 0)
        return true;
      if (instanceIDs.IndexOf(ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID()) >= 0)
      {
        EditorUtility.DisplayDialog("Cannot Delete", "Deleting the 'Assets' folder is not allowed", "Ok");
        return false;
      }
      List<string> list = ProjectWindowUtil.GetMainPathsOfAssets((IEnumerable<int>) instanceIDs).ToList<string>();
      if (list.Count == 0)
        return false;
      if (askIfSure)
      {
        string str1 = "Delete selected asset";
        if (list.Count > 1)
          str1 += "s";
        string title = str1 + "?";
        int num = 3;
        string str2 = "";
        for (int index = 0; index < list.Count && index < num; ++index)
          str2 = str2 + "   " + list[index] + "\n";
        if (list.Count > num)
          str2 += "   ...\n";
        string message = str2 + "\nYou cannot undo this action.";
        if (!EditorUtility.DisplayDialog(title, message, "Delete", "Cancel"))
          return false;
      }
      bool flag = true;
      AssetDatabase.StartAssetEditing();
      foreach (string path in list)
      {
        if (!AssetDatabase.MoveAssetToTrash(path))
          flag = false;
      }
      AssetDatabase.StopAssetEditing();
      return flag;
    }

    internal static IEnumerable<AnimationClip> DuplicateAnimationClips(IEnumerable<AnimationClip> clips)
    {
      AssetDatabase.Refresh();
      List<string> source = new List<string>();
      foreach (AnimationClip clip in clips)
      {
        if ((UnityEngine.Object) clip != (UnityEngine.Object) null)
        {
          string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath((UnityEngine.Object) clip)), clip.name) + ".anim");
          AnimationClip animationClip = new AnimationClip();
          EditorUtility.CopySerialized((UnityEngine.Object) clip, (UnityEngine.Object) animationClip);
          AssetDatabase.CreateAsset((UnityEngine.Object) animationClip, uniqueAssetPath);
          source.Add(uniqueAssetPath);
        }
      }
      AssetDatabase.Refresh();
      return source.Select<string, AnimationClip>((Func<string, AnimationClip>) (s => AssetDatabase.LoadMainAssetAtPath(s) as AnimationClip));
    }

    internal static IEnumerable<UnityEngine.Object> DuplicateAssets(IEnumerable<string> assets)
    {
      AssetDatabase.Refresh();
      List<string> stringList = new List<string>();
      foreach (string asset in assets)
      {
        string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(asset);
        if (uniqueAssetPath.Length != 0 && AssetDatabase.CopyAsset(asset, uniqueAssetPath))
          stringList.Add(uniqueAssetPath);
      }
      AssetDatabase.Refresh();
      List<string> source = stringList;
      // ISSUE: reference to a compiler-generated field
      if (ProjectWindowUtil.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ProjectWindowUtil.\u003C\u003Ef__mg\u0024cache0 = new Func<string, UnityEngine.Object>(AssetDatabase.LoadMainAssetAtPath);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, UnityEngine.Object> fMgCache0 = ProjectWindowUtil.\u003C\u003Ef__mg\u0024cache0;
      return source.Select<string, UnityEngine.Object>(fMgCache0);
    }

    internal static IEnumerable<UnityEngine.Object> DuplicateAssets(IEnumerable<int> instanceIDs)
    {
      return ProjectWindowUtil.DuplicateAssets(ProjectWindowUtil.GetMainPathsOfAssets(instanceIDs));
    }

    [DebuggerHidden]
    internal static IEnumerable<string> GetMainPathsOfAssets(IEnumerable<int> instanceIDs)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProjectWindowUtil.\u003CGetMainPathsOfAssets\u003Ec__Iterator0 assetsCIterator0 = new ProjectWindowUtil.\u003CGetMainPathsOfAssets\u003Ec__Iterator0() { instanceIDs = instanceIDs };
      // ISSUE: reference to a compiler-generated field
      assetsCIterator0.\u0024PC = -2;
      return (IEnumerable<string>) assetsCIterator0;
    }
  }
}
