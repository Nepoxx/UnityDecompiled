// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatorOverrideControllerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AnimatorOverrideController))]
  internal class AnimatorOverrideControllerInspector : Editor
  {
    private SerializedProperty m_Controller;
    private List<KeyValuePair<AnimationClip, AnimationClip>> m_Clips;
    private ReorderableList m_ClipList;
    private string m_Search;

    private void OnEnable()
    {
      AnimatorOverrideController target = this.target as AnimatorOverrideController;
      this.m_Controller = this.serializedObject.FindProperty("m_Controller");
      this.m_Search = "";
      if (this.m_Clips == null)
        this.m_Clips = new List<KeyValuePair<AnimationClip, AnimationClip>>();
      if (this.m_ClipList == null)
      {
        target.GetOverrides(this.m_Clips);
        this.m_Clips.Sort((IComparer<KeyValuePair<AnimationClip, AnimationClip>>) new AnimationClipOverrideComparer());
        this.m_ClipList = new ReorderableList((IList) this.m_Clips, typeof (KeyValuePair<AnimationClip, AnimationClip>), false, true, false, false);
        this.m_ClipList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawClipElement);
        this.m_ClipList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawClipHeader);
        this.m_ClipList.onSelectCallback = new ReorderableList.SelectCallbackDelegate(this.SelectClip);
        this.m_ClipList.elementHeight = 16f;
      }
      target.OnOverrideControllerDirty += new AnimatorOverrideController.OnOverrideControllerDirtyCallback(((Editor) this).Repaint);
    }

    private void OnDisable()
    {
      (this.target as AnimatorOverrideController).OnOverrideControllerDirty -= new AnimatorOverrideController.OnOverrideControllerDirtyCallback(((Editor) this).Repaint);
    }

    public override void OnInspectorGUI()
    {
      bool flag1 = this.targets.Length > 1;
      bool flag2 = false;
      this.serializedObject.UpdateIfRequiredOrScript();
      AnimatorOverrideController target = this.target as AnimatorOverrideController;
      RuntimeAnimatorController animatorController1 = !this.m_Controller.hasMultipleDifferentValues ? target.runtimeAnimatorController : (RuntimeAnimatorController) null;
      EditorGUI.BeginChangeCheck();
      RuntimeAnimatorController animatorController2 = EditorGUILayout.ObjectField("Controller", (Object) animatorController1, typeof (UnityEditor.Animations.AnimatorController), false, new GUILayoutOption[0]) as RuntimeAnimatorController;
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.targets.Length; ++index)
          (this.targets[index] as AnimatorOverrideController).runtimeAnimatorController = animatorController2;
        flag2 = true;
      }
      GUI.SetNextControlName("OverridesSearch");
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && GUI.GetNameOfFocusedControl() == "OverridesSearch")
        this.m_Search = "";
      EditorGUI.BeginChangeCheck();
      string str = EditorGUILayout.ToolbarSearchField(this.m_Search);
      if (EditorGUI.EndChangeCheck())
        this.m_Search = str;
      using (new EditorGUI.DisabledScope(this.m_Controller == null || flag1 && this.m_Controller.hasMultipleDifferentValues || (Object) animatorController2 == (Object) null))
      {
        EditorGUI.BeginChangeCheck();
        target.GetOverrides(this.m_Clips);
        if (this.m_Search.Length > 0)
          this.FilterOverrides();
        else
          this.m_Clips.Sort((IComparer<KeyValuePair<AnimationClip, AnimationClip>>) new AnimationClipOverrideComparer());
        this.m_ClipList.list = (IList) this.m_Clips;
        this.m_ClipList.DoLayoutList();
        if (EditorGUI.EndChangeCheck())
        {
          for (int index = 0; index < this.targets.Length; ++index)
            (this.targets[index] as AnimatorOverrideController).ApplyOverrides((IList<KeyValuePair<AnimationClip, AnimationClip>>) this.m_Clips);
          flag2 = true;
        }
      }
      if (!flag2)
        return;
      target.PerformOverrideClipListCleanup();
    }

    private void FilterOverrides()
    {
      if (this.m_Search.Length == 0)
        return;
      string[] strArray = this.m_Search.ToLower().Split(' ');
      List<KeyValuePair<AnimationClip, AnimationClip>> keyValuePairList1 = new List<KeyValuePair<AnimationClip, AnimationClip>>();
      List<KeyValuePair<AnimationClip, AnimationClip>> keyValuePairList2 = new List<KeyValuePair<AnimationClip, AnimationClip>>();
      foreach (KeyValuePair<AnimationClip, AnimationClip> clip in this.m_Clips)
      {
        string str1 = clip.Key.name.ToLower().Replace(" ", "");
        bool flag1 = true;
        bool flag2 = false;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str2 = strArray[index];
          if (str1.Contains(str2))
          {
            if (index == 0 && str1.StartsWith(str2))
              flag2 = true;
          }
          else
          {
            flag1 = false;
            break;
          }
        }
        if (flag1)
        {
          if (flag2)
            keyValuePairList1.Add(clip);
          else
            keyValuePairList2.Add(clip);
        }
      }
      this.m_Clips.Clear();
      keyValuePairList1.Sort((IComparer<KeyValuePair<AnimationClip, AnimationClip>>) new AnimationClipOverrideComparer());
      keyValuePairList2.Sort((IComparer<KeyValuePair<AnimationClip, AnimationClip>>) new AnimationClipOverrideComparer());
      this.m_Clips.AddRange((IEnumerable<KeyValuePair<AnimationClip, AnimationClip>>) keyValuePairList1);
      this.m_Clips.AddRange((IEnumerable<KeyValuePair<AnimationClip, AnimationClip>>) keyValuePairList2);
    }

    private void DrawClipElement(Rect rect, int index, bool selected, bool focused)
    {
      AnimationClip key = this.m_Clips[index].Key;
      AnimationClip animationClip1 = this.m_Clips[index].Value;
      rect.xMax /= 2f;
      GUI.Label(rect, key.name, EditorStyles.label);
      rect.xMin = rect.xMax;
      rect.xMax *= 2f;
      EditorGUI.BeginChangeCheck();
      AnimationClip animationClip2 = EditorGUI.ObjectField(rect, "", (Object) animationClip1, typeof (AnimationClip), false) as AnimationClip;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_Clips[index] = new KeyValuePair<AnimationClip, AnimationClip>(key, animationClip2);
    }

    private void DrawClipHeader(Rect rect)
    {
      rect.xMax /= 2f;
      GUI.Label(rect, "Original", EditorStyles.label);
      rect.xMin = rect.xMax;
      rect.xMax *= 2f;
      GUI.Label(rect, "Override", EditorStyles.label);
    }

    private void SelectClip(ReorderableList list)
    {
      if (0 > list.index || list.index >= this.m_Clips.Count)
        return;
      EditorGUIUtility.PingObject((Object) this.m_Clips[list.index].Key);
    }
  }
}
