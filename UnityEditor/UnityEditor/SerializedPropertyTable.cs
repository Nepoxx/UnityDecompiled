// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedPropertyTable
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityEditor
{
  internal class SerializedPropertyTable
  {
    private static readonly string s_TableHeight = "_TableHeight";
    private float m_TableHeight = 200f;
    private bool m_DragHandleEnabled = false;
    private readonly float m_FilterHeight = 20f;
    private readonly float m_DragHeight = 20f;
    private readonly float m_DragWidth = 32f;
    private SerializedPropertyDataStore.GatherDelegate m_GatherDelegate;
    private SerializedPropertyTable.HeaderDelegate m_HeaderDelegate;
    private bool m_Initialized;
    private TreeViewState m_TreeViewState;
    private MultiColumnHeaderState m_MultiColumnHeaderState;
    private SerializedPropertyTreeView m_TreeView;
    private SerializedPropertyDataStore m_DataStore;
    private float m_ColumnHeaderHeight;
    private string m_SerializationUID;

    public SerializedPropertyTable(string serializationUID, SerializedPropertyDataStore.GatherDelegate gatherDelegate, SerializedPropertyTable.HeaderDelegate headerDelegate)
    {
      this.m_SerializationUID = serializationUID;
      this.m_GatherDelegate = gatherDelegate;
      this.m_HeaderDelegate = headerDelegate;
      this.OnEnable();
    }

    public bool dragHandleEnabled
    {
      get
      {
        return this.m_DragHandleEnabled;
      }
      set
      {
        this.m_DragHandleEnabled = value;
      }
    }

    private void InitIfNeeded()
    {
      if (this.m_Initialized)
        return;
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      if (this.m_MultiColumnHeaderState == null)
      {
        string[] propNames;
        this.m_MultiColumnHeaderState = new MultiColumnHeaderState((MultiColumnHeaderState.Column[]) this.m_HeaderDelegate(out propNames));
        this.m_DataStore = new SerializedPropertyDataStore(propNames, this.m_GatherDelegate);
      }
      MultiColumnHeader multicolumnHeader = new MultiColumnHeader(this.m_MultiColumnHeaderState);
      this.m_ColumnHeaderHeight = multicolumnHeader.height;
      this.m_TreeView = new SerializedPropertyTreeView(this.m_TreeViewState, multicolumnHeader, this.m_DataStore);
      this.m_TreeView.DeserializeState(this.m_SerializationUID);
      this.m_TreeView.Reload();
      this.m_Initialized = true;
    }

    private float GetMinHeight()
    {
      float singleLineHeight = EditorGUIUtility.singleLineHeight;
      return this.m_FilterHeight + this.m_ColumnHeaderHeight + singleLineHeight + this.m_DragHeight + singleLineHeight * 3f;
    }

    public void OnInspectorUpdate()
    {
      if (this.m_DataStore != null && this.m_DataStore.Repopulate() && this.m_TreeView != null)
      {
        this.m_TreeView.FullReload();
      }
      else
      {
        if (this.m_TreeView == null || !this.m_TreeView.Update())
          return;
        this.m_TreeView.Repaint();
      }
    }

    public void OnHierarchyChange()
    {
      if (this.m_DataStore == null || !this.m_DataStore.Repopulate() || this.m_TreeView == null)
        return;
      this.m_TreeView.FullReload();
    }

    public void OnSelectionChange()
    {
      this.OnSelectionChange(Selection.instanceIDs);
    }

    public void OnSelectionChange(int[] instanceIDs)
    {
      if (this.m_TreeView == null)
        return;
      this.m_TreeView.SetSelection((IList<int>) instanceIDs);
    }

    public void OnGUI()
    {
      Profiler.BeginSample("SerializedPropertyTable.OnGUI");
      this.InitIfNeeded();
      Rect position1 = !this.dragHandleEnabled ? GUILayoutUtility.GetRect(0.0f, float.MaxValue, 0.0f, float.MaxValue) : GUILayoutUtility.GetRect(0.0f, 10000f, this.m_TableHeight, this.m_TableHeight);
      if (Event.current.type == EventType.Layout)
        return;
      float width = position1.width;
      float num = (float) ((double) position1.height - (double) this.m_FilterHeight - (!this.dragHandleEnabled ? 0.0 : (double) this.m_DragHeight));
      float height = position1.height;
      position1.height = this.m_FilterHeight;
      Rect r = position1;
      position1.height = num;
      position1.y += this.m_FilterHeight;
      Rect rect = position1;
      Profiler.BeginSample("TreeView.OnGUI");
      this.m_TreeView.OnGUI(rect);
      Profiler.EndSample();
      if (this.dragHandleEnabled)
      {
        position1.y += num + 1f;
        position1.height = 1f;
        Rect position2 = position1;
        position1.height = 10f;
        position1.y += 10f;
        position1.x += (float) (((double) position1.width - (double) this.m_DragWidth) * 0.5);
        position1.width = this.m_DragWidth;
        this.m_TableHeight = EditorGUI.HeightResizer(position1, this.m_TableHeight, this.GetMinHeight(), float.MaxValue);
        if ((double) this.m_MultiColumnHeaderState.widthOfAllVisibleColumns <= (double) width)
        {
          Rect texCoords = new Rect(0.0f, 1f, 1f, (float) (1.0 - 1.0 / (double) EditorStyles.inspectorTitlebar.normal.background.height));
          GUI.DrawTextureWithTexCoords(position2, (Texture) EditorStyles.inspectorTitlebar.normal.background, texCoords);
        }
        if (Event.current.type == EventType.Repaint)
          SerializedPropertyTable.Styles.DragHandle.Draw(position1, false, false, false, false);
      }
      this.m_TreeView.OnFilterGUI(r);
      if (this.m_TreeView.IsFilteredDirty())
        this.m_TreeView.Reload();
      Profiler.EndSample();
    }

    public void OnEnable()
    {
      this.m_TableHeight = SessionState.GetFloat(this.m_SerializationUID + SerializedPropertyTable.s_TableHeight, 200f);
    }

    public void OnDisable()
    {
      if (this.m_TreeView != null)
        this.m_TreeView.SerializeState(this.m_SerializationUID);
      SessionState.SetFloat(this.m_SerializationUID + SerializedPropertyTable.s_TableHeight, this.m_TableHeight);
    }

    private static class Styles
    {
      public static readonly GUIStyle DragHandle = (GUIStyle) "RL DragHandle";
    }

    internal delegate SerializedPropertyTreeView.Column[] HeaderDelegate(out string[] propNames);
  }
}
