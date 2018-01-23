// Decompiled with JetBrains decompiler
// Type: UnityEditor.TagManagerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (TagManager))]
  internal class TagManagerInspector : ProjectSettingsBaseEditor
  {
    private static TagManagerInspector.InitialExpansionState s_InitialExpansionState = TagManagerInspector.InitialExpansionState.None;
    protected bool m_IsEditable = false;
    private bool m_HaveRemovedTag = false;
    protected SerializedProperty m_Tags;
    protected SerializedProperty m_SortingLayers;
    protected SerializedProperty m_Layers;
    private ReorderableList m_TagsList;
    private ReorderableList m_SortLayersList;
    private ReorderableList m_LayersList;

    public TagManager tagManager
    {
      get
      {
        return this.target as TagManager;
      }
    }

    public virtual void OnEnable()
    {
      this.m_Tags = this.serializedObject.FindProperty("tags");
      this.CheckForRemovedTags();
      if (this.m_TagsList == null)
      {
        this.m_TagsList = new ReorderableList(this.serializedObject, this.m_Tags, false, false, true, true);
        this.m_TagsList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.NewElement);
        this.m_TagsList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveFromTagsList);
        this.m_TagsList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawTagListElement);
        this.m_TagsList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
        this.m_TagsList.headerHeight = 3f;
      }
      this.m_SortingLayers = this.serializedObject.FindProperty("m_SortingLayers");
      if (this.m_SortLayersList == null)
      {
        this.m_SortLayersList = new ReorderableList(this.serializedObject, this.m_SortingLayers, true, false, true, true);
        this.m_SortLayersList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.ReorderSortLayerList);
        this.m_SortLayersList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddToSortLayerList);
        this.m_SortLayersList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveFromSortLayerList);
        this.m_SortLayersList.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveSortLayerEntry);
        this.m_SortLayersList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawSortLayerListElement);
        this.m_SortLayersList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
        this.m_SortLayersList.headerHeight = 3f;
      }
      this.m_Layers = this.serializedObject.FindProperty("layers");
      if (this.m_LayersList == null)
      {
        this.m_LayersList = new ReorderableList(this.serializedObject, this.m_Layers, false, false, false, false);
        this.m_LayersList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawLayerListElement);
        this.m_LayersList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
        this.m_LayersList.headerHeight = 3f;
      }
      if (TagManagerInspector.s_InitialExpansionState == TagManagerInspector.InitialExpansionState.None)
        return;
      this.m_Tags.isExpanded = false;
      this.m_SortingLayers.isExpanded = false;
      this.m_Layers.isExpanded = false;
      switch (TagManagerInspector.s_InitialExpansionState)
      {
        case TagManagerInspector.InitialExpansionState.Tags:
          this.m_Tags.isExpanded = true;
          break;
        case TagManagerInspector.InitialExpansionState.Layers:
          this.m_Layers.isExpanded = true;
          break;
        case TagManagerInspector.InitialExpansionState.SortingLayers:
          this.m_SortingLayers.isExpanded = true;
          break;
      }
      TagManagerInspector.s_InitialExpansionState = TagManagerInspector.InitialExpansionState.None;
    }

    internal static void ShowWithInitialExpansion(TagManagerInspector.InitialExpansionState initialExpansionState)
    {
      TagManagerInspector.s_InitialExpansionState = initialExpansionState;
      Selection.activeObject = EditorApplication.tagManager;
    }

    private void CheckForRemovedTags()
    {
      for (int index = 0; index < this.m_Tags.arraySize; ++index)
      {
        if (string.IsNullOrEmpty(this.m_Tags.GetArrayElementAtIndex(index).stringValue))
          this.m_HaveRemovedTag = true;
      }
    }

    private void NewElement(Rect buttonRect, ReorderableList list)
    {
      buttonRect.x -= 400f;
      buttonRect.y -= 13f;
      PopupWindow.Show(buttonRect, (PopupWindowContent) new TagManagerInspector.EnterNamePopup(this.m_Tags, (TagManagerInspector.EnterNamePopup.EnterDelegate) (s => InternalEditorUtility.AddTag(s))), (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
    }

    private void RemoveFromTagsList(ReorderableList list)
    {
      SerializedProperty arrayElementAtIndex = this.m_Tags.GetArrayElementAtIndex(list.index);
      if (arrayElementAtIndex.stringValue == "")
        return;
      GameObject withTag = GameObject.FindWithTag(arrayElementAtIndex.stringValue);
      if ((Object) withTag != (Object) null)
      {
        EditorUtility.DisplayDialog("Error", "Can't remove this tag because it is being used by " + withTag.name, "OK");
      }
      else
      {
        InternalEditorUtility.RemoveTag(arrayElementAtIndex.stringValue);
        this.m_HaveRemovedTag = true;
      }
    }

    private void DrawTagListElement(Rect rect, int index, bool selected, bool focused)
    {
      rect.height -= 2f;
      rect.xMin -= 20f;
      string label2 = this.m_Tags.GetArrayElementAtIndex(index).stringValue;
      if (string.IsNullOrEmpty(label2))
        label2 = "(Removed)";
      EditorGUI.LabelField(rect, " Tag " + (object) index, label2);
    }

    private void AddToSortLayerList(ReorderableList list)
    {
      this.serializedObject.ApplyModifiedProperties();
      InternalEditorUtility.AddSortingLayer();
      this.serializedObject.Update();
      list.index = list.serializedProperty.arraySize - 1;
    }

    public void ReorderSortLayerList(ReorderableList list)
    {
      InternalEditorUtility.UpdateSortingLayersOrder();
    }

    private void RemoveFromSortLayerList(ReorderableList list)
    {
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
      this.serializedObject.ApplyModifiedProperties();
      this.serializedObject.Update();
      InternalEditorUtility.UpdateSortingLayersOrder();
    }

    private bool CanEditSortLayerEntry(int index)
    {
      if (index < 0 || index >= InternalEditorUtility.GetSortingLayerCount())
        return false;
      return !InternalEditorUtility.IsSortingLayerDefault(index);
    }

    private bool CanRemoveSortLayerEntry(ReorderableList list)
    {
      return this.CanEditSortLayerEntry(list.index);
    }

    private void DrawSortLayerListElement(Rect rect, int index, bool selected, bool focused)
    {
      rect.height -= 2f;
      rect.xMin -= 20f;
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable && this.CanEditSortLayerEntry(index);
      string sortingLayerName = InternalEditorUtility.GetSortingLayerName(index);
      string name = EditorGUI.TextField(rect, " Layer ", sortingLayerName);
      if (name != sortingLayerName)
      {
        this.serializedObject.ApplyModifiedProperties();
        InternalEditorUtility.SetSortingLayerName(index, name);
        this.serializedObject.Update();
      }
      GUI.enabled = enabled;
    }

    private void DrawLayerListElement(Rect rect, int index, bool selected, bool focused)
    {
      rect.height -= 2f;
      rect.xMin -= 20f;
      bool flag = index >= 8;
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable && flag;
      string stringValue = this.m_Layers.GetArrayElementAtIndex(index).stringValue;
      string str = !flag ? EditorGUI.TextField(rect, " Builtin Layer " + (object) index, stringValue) : EditorGUI.TextField(rect, " User Layer " + (object) index, stringValue);
      if (str != stringValue)
        this.m_Layers.GetArrayElementAtIndex(index).stringValue = str;
      GUI.enabled = enabled;
    }

    internal override string targetTitle
    {
      get
      {
        return "Tags & Layers";
      }
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.m_IsEditable = AssetDatabase.IsOpenForEdit("ProjectSettings/TagManager.asset", StatusQueryOptions.UseCachedIfPossible);
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable;
      this.m_Tags.isExpanded = EditorGUILayout.Foldout(this.m_Tags.isExpanded, "Tags", true);
      if (this.m_Tags.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.m_TagsList.DoLayoutList();
        --EditorGUI.indentLevel;
        if (this.m_HaveRemovedTag)
          EditorGUILayout.HelpBox("There are removed tags. They will be removed from this list the next time the project is loaded.", MessageType.Info, true);
      }
      this.m_SortingLayers.isExpanded = EditorGUILayout.Foldout(this.m_SortingLayers.isExpanded, "Sorting Layers", true);
      if (this.m_SortingLayers.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.m_SortLayersList.DoLayoutList();
        --EditorGUI.indentLevel;
      }
      this.m_Layers.isExpanded = EditorGUILayout.Foldout(this.m_Layers.isExpanded, "Layers", true);
      if (this.m_Layers.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.m_LayersList.DoLayoutList();
        --EditorGUI.indentLevel;
      }
      GUI.enabled = enabled;
      this.serializedObject.ApplyModifiedProperties();
    }

    internal enum InitialExpansionState
    {
      None,
      Tags,
      Layers,
      SortingLayers,
    }

    private class EnterNamePopup : PopupWindowContent
    {
      private string m_NewTagName = "New tag";
      private bool m_NeedsFocus = true;
      private readonly TagManagerInspector.EnterNamePopup.EnterDelegate EnterCB;

      public EnterNamePopup(SerializedProperty tags, TagManagerInspector.EnterNamePopup.EnterDelegate cb)
      {
        this.EnterCB = cb;
        List<string> stringList = new List<string>();
        for (int index = 0; index < tags.arraySize; ++index)
        {
          string stringValue = tags.GetArrayElementAtIndex(index).stringValue;
          if (!string.IsNullOrEmpty(stringValue))
            stringList.Add(stringValue);
        }
        this.m_NewTagName = ObjectNames.GetUniqueName(stringList.ToArray(), this.m_NewTagName);
      }

      public override Vector2 GetWindowSize()
      {
        return new Vector2(400f, 48f);
      }

      public override void OnGUI(Rect windowRect)
      {
        GUILayout.Space(5f);
        Event current = Event.current;
        bool flag = current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter);
        GUI.SetNextControlName("TagName");
        this.m_NewTagName = EditorGUILayout.TextField("New Tag Name", this.m_NewTagName, new GUILayoutOption[0]);
        if (this.m_NeedsFocus)
        {
          this.m_NeedsFocus = false;
          EditorGUI.FocusTextInControl("TagName");
        }
        GUI.enabled = this.m_NewTagName.Length != 0;
        if (!GUILayout.Button("Save") && !flag)
          return;
        this.EnterCB(this.m_NewTagName);
        this.editorWindow.Close();
      }

      public delegate void EnterDelegate(string str);
    }
  }
}
