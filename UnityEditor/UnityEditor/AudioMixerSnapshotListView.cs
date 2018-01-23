// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerSnapshotListView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor
{
  internal class AudioMixerSnapshotListView
  {
    private ReorderableListWithRenameAndScrollView m_ReorderableListWithRenameAndScrollView;
    private AudioMixerController m_Controller;
    private List<AudioMixerSnapshotController> m_Snapshots;
    private ReorderableListWithRenameAndScrollView.State m_State;
    private static AudioMixerSnapshotListView.Styles s_Styles;

    public AudioMixerSnapshotListView(ReorderableListWithRenameAndScrollView.State state)
    {
      this.m_State = state;
    }

    public void OnMixerControllerChanged(AudioMixerController controller)
    {
      this.m_Controller = controller;
      this.RecreateListControl();
    }

    private int GetSnapshotIndex(AudioMixerSnapshotController snapshot)
    {
      for (int index = 0; index < this.m_Snapshots.Count; ++index)
      {
        if ((UnityEngine.Object) this.m_Snapshots[index] == (UnityEngine.Object) snapshot)
          return index;
      }
      return 0;
    }

    private void RecreateListControl()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_Snapshots = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.m_Controller.snapshots);
      this.m_ReorderableListWithRenameAndScrollView = new ReorderableListWithRenameAndScrollView(new ReorderableList((IList) this.m_Snapshots, typeof (AudioMixerSnapshotController), true, false, false, false)
      {
        onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.EndDragChild),
        elementHeight = 16f,
        headerHeight = 0.0f,
        footerHeight = 0.0f,
        showDefaultBackground = false,
        index = this.GetSnapshotIndex(this.m_Controller.TargetSnapshot)
      }, this.m_State);
      this.m_ReorderableListWithRenameAndScrollView.onSelectionChanged += new Action<int>(this.SelectionChanged);
      this.m_ReorderableListWithRenameAndScrollView.onNameChangedAtIndex += new Action<int, string>(this.NameChanged);
      this.m_ReorderableListWithRenameAndScrollView.onDeleteItemAtIndex += new Action<int>(this.Delete);
      this.m_ReorderableListWithRenameAndScrollView.onGetNameAtIndex += new Func<int, string>(this.GetNameOfElement);
      this.m_ReorderableListWithRenameAndScrollView.onCustomDrawElement += new ReorderableList.ElementCallbackDelegate(this.CustomDrawElement);
    }

    private void SaveToBackend()
    {
      this.m_Controller.snapshots = this.m_Snapshots.ToArray();
      this.m_Controller.OnSubAssetChanged();
    }

    public void LoadFromBackend()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_Snapshots.Clear();
      this.m_Snapshots.AddRange((IEnumerable<AudioMixerSnapshotController>) this.m_Controller.snapshots);
    }

    public void OnEvent()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_ReorderableListWithRenameAndScrollView.OnEvent();
    }

    public void CustomDrawElement(Rect r, int index, bool isActive, bool isFocused)
    {
      Event current = Event.current;
      if (current.type == EventType.MouseUp && current.button == 1 && r.Contains(current.mousePosition))
      {
        AudioMixerSnapshotListView.SnapshotMenu.Show(r, this.m_Snapshots[index], this);
        current.Use();
      }
      bool isSelected = index == this.m_ReorderableListWithRenameAndScrollView.list.index && !this.m_ReorderableListWithRenameAndScrollView.IsRenamingIndex(index);
      r.width -= 19f;
      this.m_ReorderableListWithRenameAndScrollView.DrawElementText(r, index, isActive, isSelected, isFocused);
      if (!((UnityEngine.Object) this.m_Controller.startSnapshot == (UnityEngine.Object) this.m_Snapshots[index]))
        return;
      r.x = (float) ((double) r.xMax + 5.0 + 5.0);
      r.y += (float) (((double) r.height - 14.0) / 2.0);
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Rect& local = @r;
      float num1 = 14f;
      r.height = num1;
      double num2 = (double) num1;
      // ISSUE: explicit reference operation
      (^local).width = (float) num2;
      GUI.Label(r, AudioMixerSnapshotListView.s_Styles.starIcon, GUIStyle.none);
    }

    public float GetTotalHeight()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return 0.0f;
      return this.m_ReorderableListWithRenameAndScrollView.list.GetHeight() + 22f;
    }

    public void OnGUI(Rect rect)
    {
      if (AudioMixerSnapshotListView.s_Styles == null)
        AudioMixerSnapshotListView.s_Styles = new AudioMixerSnapshotListView.Styles();
      Rect headerRect;
      Rect contentRect;
      using (new EditorGUI.DisabledScope((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null))
      {
        AudioMixerDrawUtils.DrawRegionBg(rect, out headerRect, out contentRect);
        AudioMixerDrawUtils.HeaderLabel(headerRect, AudioMixerSnapshotListView.s_Styles.header, AudioMixerSnapshotListView.s_Styles.snapshotsIcon);
      }
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      int snapshotIndex = this.GetSnapshotIndex(this.m_Controller.TargetSnapshot);
      if (snapshotIndex != this.m_ReorderableListWithRenameAndScrollView.list.index)
      {
        this.m_ReorderableListWithRenameAndScrollView.list.index = snapshotIndex;
        this.m_ReorderableListWithRenameAndScrollView.FrameItem(snapshotIndex);
      }
      this.m_ReorderableListWithRenameAndScrollView.OnGUI(contentRect);
      if (GUI.Button(new Rect(headerRect.xMax - 15f, headerRect.y + 3f, 15f, 15f), AudioMixerSnapshotListView.s_Styles.addButton, EditorStyles.label))
        this.Add();
    }

    public void SelectionChanged(int index)
    {
      if (index >= this.m_Snapshots.Count)
        index = this.m_Snapshots.Count - 1;
      this.m_Controller.TargetSnapshot = this.m_Snapshots[index];
      this.UpdateViews();
    }

    private string GetNameOfElement(int index)
    {
      return this.m_Snapshots[index].name;
    }

    public void NameChanged(int index, string newName)
    {
      this.m_Snapshots[index].name = newName;
      this.SaveToBackend();
    }

    private void DuplicateCurrentSnapshot()
    {
      Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Duplicate current snapshot");
      this.m_Controller.CloneNewSnapshotFromTarget(true);
      this.LoadFromBackend();
      this.UpdateViews();
    }

    private void Add()
    {
      Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Add new snapshot");
      this.m_Controller.CloneNewSnapshotFromTarget(true);
      this.LoadFromBackend();
      this.Rename(this.m_Controller.TargetSnapshot);
      this.UpdateViews();
    }

    private void DeleteSnapshot(AudioMixerSnapshotController snapshot)
    {
      if (this.m_Controller.snapshots.Length <= 1)
      {
        Debug.Log((object) "You must have at least 1 snapshot in an AudioMixer.");
      }
      else
      {
        this.m_Controller.RemoveSnapshot(snapshot);
        this.LoadFromBackend();
        this.m_ReorderableListWithRenameAndScrollView.list.index = this.GetSnapshotIndex(this.m_Controller.TargetSnapshot);
        this.UpdateViews();
      }
    }

    private void Delete(int index)
    {
      this.DeleteSnapshot(this.m_Snapshots[index]);
    }

    public void EndDragChild(ReorderableList list)
    {
      this.m_Snapshots = this.m_ReorderableListWithRenameAndScrollView.list.list as List<AudioMixerSnapshotController>;
      this.SaveToBackend();
    }

    private void UpdateViews()
    {
      AudioMixerWindow editorWindowOfType = (AudioMixerWindow) WindowLayout.FindEditorWindowOfType(typeof (AudioMixerWindow));
      if ((UnityEngine.Object) editorWindowOfType != (UnityEngine.Object) null)
        editorWindowOfType.Repaint();
      InspectorWindow.RepaintAllInspectors();
    }

    private void SetAsStartupSnapshot(AudioMixerSnapshotController snapshot)
    {
      Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Set start snapshot");
      this.m_Controller.startSnapshot = (AudioMixerSnapshot) snapshot;
    }

    private void Rename(AudioMixerSnapshotController snapshot)
    {
      this.m_ReorderableListWithRenameAndScrollView.BeginRename(this.GetSnapshotIndex(snapshot), 0.0f);
    }

    public void OnUndoRedoPerformed()
    {
      this.LoadFromBackend();
    }

    private class Styles
    {
      public GUIContent starIcon = new GUIContent((Texture) EditorGUIUtility.FindTexture("Favorite"), "Start snapshot");
      public GUIContent header = new GUIContent("Snapshots", "A snapshot is a set of values for all parameters in the mixer. When using the mixer you modify parameters in the selected snapshot. Blend between multiple snapshots at runtime.");
      public GUIContent addButton = new GUIContent("+");
      public Texture2D snapshotsIcon = EditorGUIUtility.FindTexture("AudioMixerSnapshot Icon");
    }

    internal class SnapshotMenu
    {
      public static void Show(Rect buttonRect, AudioMixerSnapshotController snapshot, AudioMixerSnapshotListView list)
      {
        GenericMenu genericMenu1 = new GenericMenu();
        AudioMixerSnapshotListView.SnapshotMenu.data data1 = new AudioMixerSnapshotListView.SnapshotMenu.data() { snapshot = snapshot, list = list };
        GenericMenu genericMenu2 = genericMenu1;
        GUIContent content1 = new GUIContent("Set as start Snapshot");
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.SetAsStartupSnapshot);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache0 = AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache0;
        AudioMixerSnapshotListView.SnapshotMenu.data data2 = data1;
        genericMenu2.AddItem(content1, num1 != 0, fMgCache0, (object) data2);
        genericMenu1.AddSeparator("");
        GenericMenu genericMenu3 = genericMenu1;
        GUIContent content2 = new GUIContent("Rename");
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache1 = new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.Rename);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache1 = AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache1;
        AudioMixerSnapshotListView.SnapshotMenu.data data3 = data1;
        genericMenu3.AddItem(content2, num2 != 0, fMgCache1, (object) data3);
        GenericMenu genericMenu4 = genericMenu1;
        GUIContent content3 = new GUIContent("Duplicate");
        int num3 = 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache2 = new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.Duplicate);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache2 = AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache2;
        AudioMixerSnapshotListView.SnapshotMenu.data data4 = data1;
        genericMenu4.AddItem(content3, num3 != 0, fMgCache2, (object) data4);
        GenericMenu genericMenu5 = genericMenu1;
        GUIContent content4 = new GUIContent("Delete");
        int num4 = 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache3 = new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.Delete);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache3 = AudioMixerSnapshotListView.SnapshotMenu.\u003C\u003Ef__mg\u0024cache3;
        AudioMixerSnapshotListView.SnapshotMenu.data data5 = data1;
        genericMenu5.AddItem(content4, num4 != 0, fMgCache3, (object) data5);
        genericMenu1.DropDown(buttonRect);
      }

      private static void SetAsStartupSnapshot(object userData)
      {
        AudioMixerSnapshotListView.SnapshotMenu.data data = userData as AudioMixerSnapshotListView.SnapshotMenu.data;
        data.list.SetAsStartupSnapshot(data.snapshot);
      }

      private static void Rename(object userData)
      {
        AudioMixerSnapshotListView.SnapshotMenu.data data = userData as AudioMixerSnapshotListView.SnapshotMenu.data;
        data.list.Rename(data.snapshot);
      }

      private static void Duplicate(object userData)
      {
        (userData as AudioMixerSnapshotListView.SnapshotMenu.data).list.DuplicateCurrentSnapshot();
      }

      private static void Delete(object userData)
      {
        AudioMixerSnapshotListView.SnapshotMenu.data data = userData as AudioMixerSnapshotListView.SnapshotMenu.data;
        data.list.DeleteSnapshot(data.snapshot);
      }

      private class data
      {
        public AudioMixerSnapshotController snapshot;
        public AudioMixerSnapshotListView list;
      }
    }
  }
}
