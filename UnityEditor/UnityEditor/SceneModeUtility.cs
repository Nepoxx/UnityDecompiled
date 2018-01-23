// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneModeUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  public static class SceneModeUtility
  {
    private static System.Type s_FocusType = (System.Type) null;
    private static SceneHierarchyWindow s_HierarchyWindow = (SceneHierarchyWindow) null;
    private static GUIContent s_NoneButtonContent = (GUIContent) null;
    private static SceneModeUtility.Styles s_Styles;

    private static SceneModeUtility.Styles styles
    {
      get
      {
        if (SceneModeUtility.s_Styles == null)
          SceneModeUtility.s_Styles = new SceneModeUtility.Styles();
        return SceneModeUtility.s_Styles;
      }
    }

    public static T[] GetSelectedObjectsOfType<T>(out GameObject[] gameObjects, params System.Type[] types) where T : UnityEngine.Object
    {
      if (types.Length == 0)
        types = new System.Type[1]{ typeof (T) };
      List<GameObject> gameObjectList = new List<GameObject>();
      List<T> objList = new List<T>();
      foreach (Transform transform in Selection.GetTransforms(SelectionMode.ExcludePrefab | SelectionMode.Editable))
      {
        foreach (System.Type type in types)
        {
          UnityEngine.Object component = (UnityEngine.Object) transform.gameObject.GetComponent(type);
          if (component != (UnityEngine.Object) null)
          {
            gameObjectList.Add(transform.gameObject);
            objList.Add((T) component);
            break;
          }
        }
      }
      gameObjects = gameObjectList.ToArray();
      return objList.ToArray();
    }

    public static void SearchForType(System.Type type)
    {
      UnityEngine.Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (SceneHierarchyWindow));
      SceneHierarchyWindow sceneHierarchyWindow = objectsOfTypeAll.Length <= 0 ? (SceneHierarchyWindow) null : objectsOfTypeAll[0] as SceneHierarchyWindow;
      if ((bool) ((UnityEngine.Object) sceneHierarchyWindow))
      {
        SceneModeUtility.s_HierarchyWindow = sceneHierarchyWindow;
        if (type == null || type == typeof (GameObject))
        {
          SceneModeUtility.s_FocusType = (System.Type) null;
          sceneHierarchyWindow.ClearSearchFilter();
        }
        else
        {
          SceneModeUtility.s_FocusType = type;
          if (sceneHierarchyWindow.searchMode == SearchableEditorWindow.SearchMode.Name)
            sceneHierarchyWindow.searchMode = SearchableEditorWindow.SearchMode.All;
          sceneHierarchyWindow.SetSearchFilter("t:" + type.Name, sceneHierarchyWindow.searchMode, false);
          sceneHierarchyWindow.hasSearchFilterFocus = true;
        }
      }
      else
        SceneModeUtility.s_FocusType = (System.Type) null;
    }

    public static System.Type SearchBar(params System.Type[] types)
    {
      if (SceneModeUtility.s_NoneButtonContent == null)
      {
        SceneModeUtility.s_NoneButtonContent = EditorGUIUtility.IconContent("sv_icon_none");
        SceneModeUtility.s_NoneButtonContent.text = "None";
      }
      if (SceneModeUtility.s_FocusType != null && ((UnityEngine.Object) SceneModeUtility.s_HierarchyWindow == (UnityEngine.Object) null || SceneModeUtility.s_HierarchyWindow.m_SearchFilter != "t:" + SceneModeUtility.s_FocusType.Name))
        SceneModeUtility.s_FocusType = (System.Type) null;
      GUILayout.Label("Scene Filter:");
      EditorGUILayout.BeginHorizontal();
      if (SceneModeUtility.TypeButton(EditorGUIUtility.TempContent("All", (Texture) AssetPreview.GetMiniTypeThumbnail(typeof (GameObject))), SceneModeUtility.s_FocusType == null, SceneModeUtility.styles.typeButton))
        SceneModeUtility.SearchForType((System.Type) null);
      for (int index = 0; index < types.Length; ++index)
      {
        System.Type type = types[index];
        Texture2D texture2D = type != typeof (Renderer) ? (type != typeof (Terrain) ? AssetPreview.GetMiniTypeThumbnail(type) : EditorGUIUtility.IconContent("Terrain Icon").image as Texture2D) : EditorGUIUtility.IconContent("MeshRenderer Icon").image as Texture2D;
        if (SceneModeUtility.TypeButton(EditorGUIUtility.TempContent(ObjectNames.NicifyVariableName(type.Name) + "s", (Texture) texture2D), type == SceneModeUtility.s_FocusType, SceneModeUtility.styles.typeButton))
          SceneModeUtility.SearchForType(type);
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      return SceneModeUtility.s_FocusType;
    }

    private static bool TypeButton(GUIContent label, bool selected, GUIStyle style)
    {
      EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
      bool flag = GUILayout.Toggle(selected, label, style, new GUILayoutOption[0]);
      EditorGUIUtility.SetIconSize(Vector2.zero);
      return flag && flag != selected;
    }

    public static bool StaticFlagField(string label, SerializedProperty property, int flag)
    {
      bool flag1 = (property.intValue & flag) != 0;
      bool flag2 = (property.hasMultipleDifferentValuesBitwise & flag) != 0;
      EditorGUI.showMixedValue = flag2;
      EditorGUI.BeginChangeCheck();
      bool flagValue = EditorGUILayout.Toggle(label, flag1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (!SceneModeUtility.SetStaticFlags(property.serializedObject.targetObjects, flag, flagValue))
          return flag1 && !flag2;
        return flagValue;
      }
      EditorGUI.showMixedValue = false;
      return flagValue && !flag2;
    }

    public static bool SetStaticFlags(UnityEngine.Object[] targetObjects, int changedFlags, bool flagValue)
    {
      bool flag = changedFlags == -1;
      StaticEditorFlags staticEditorFlags1 = !flag ? (StaticEditorFlags) Enum.Parse(typeof (StaticEditorFlags), changedFlags.ToString()) : (StaticEditorFlags) 0;
      IEnumerable<GameObject> gameObjects = targetObjects.OfType<GameObject>();
      string title = "Change Static Flags";
      string message;
      if (flag)
        message = "Do you want to " + (!flagValue ? "disable" : "enable") + " the static flags for all the child objects as well?";
      else
        message = "Do you want to " + (!flagValue ? "disable" : "enable") + " the " + ObjectNames.NicifyVariableName(staticEditorFlags1.ToString()) + " flag for all the child objects as well?";
      GameObjectUtility.ShouldIncludeChildren shouldIncludeChildren = GameObjectUtility.DisplayUpdateChildrenDialogIfNeeded(gameObjects, title, message);
      if (shouldIncludeChildren == GameObjectUtility.ShouldIncludeChildren.Cancel)
      {
        GUIUtility.ExitGUI();
        return false;
      }
      GameObject[] objects = SceneModeUtility.GetObjects(targetObjects, shouldIncludeChildren == GameObjectUtility.ShouldIncludeChildren.IncludeChildren);
      Undo.RecordObjects((UnityEngine.Object[]) objects, "Change Static Flags");
      foreach (GameObject go in objects)
      {
        int staticEditorFlags2 = (int) GameObjectUtility.GetStaticEditorFlags(go);
        int num = !flagValue ? staticEditorFlags2 & ~changedFlags : staticEditorFlags2 | changedFlags;
        GameObjectUtility.SetStaticEditorFlags(go, (StaticEditorFlags) num);
      }
      return true;
    }

    private static void GetObjectsRecurse(Transform root, List<GameObject> arr)
    {
      arr.Add(root.gameObject);
      IEnumerator enumerator = root.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          SceneModeUtility.GetObjectsRecurse((Transform) enumerator.Current, arr);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    public static GameObject[] GetObjects(UnityEngine.Object[] gameObjects, bool includeChildren)
    {
      List<GameObject> arr = new List<GameObject>();
      if (!includeChildren)
      {
        foreach (GameObject gameObject in gameObjects)
          arr.Add(gameObject);
      }
      else
      {
        foreach (GameObject gameObject in gameObjects)
          SceneModeUtility.GetObjectsRecurse(gameObject.transform, arr);
      }
      return arr.ToArray();
    }

    private class Styles
    {
      public GUIStyle typeButton = (GUIStyle) "SearchModeFilter";
    }
  }
}
