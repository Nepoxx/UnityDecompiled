// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindowClipPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AnimationWindowClipPopup
  {
    [SerializeField]
    public AnimationWindowState state;
    [SerializeField]
    private int selectedIndex;

    public void OnGUI()
    {
      AnimationWindowSelectionItem selectedItem = this.state.selectedItem;
      if ((UnityEngine.Object) selectedItem == (UnityEngine.Object) null)
        return;
      if (selectedItem.canChangeAnimationClip)
      {
        string[] clipMenuContent = this.GetClipMenuContent();
        EditorGUI.BeginChangeCheck();
        this.selectedIndex = EditorGUILayout.Popup(this.ClipToIndex(this.state.activeAnimationClip), clipMenuContent, EditorStyles.toolbarPopup, new GUILayoutOption[0]);
        if (!EditorGUI.EndChangeCheck())
          return;
        if (clipMenuContent[this.selectedIndex] == AnimationWindowStyles.createNewClip.text)
        {
          AnimationClip newClip = AnimationWindowUtility.CreateNewClip(selectedItem.rootGameObject.name);
          if ((bool) ((UnityEngine.Object) newClip))
          {
            AnimationWindowUtility.AddClipToAnimationPlayerComponent(this.state.activeAnimationPlayer, newClip);
            this.state.selection.UpdateClip(this.state.selectedItem, newClip);
            GUIUtility.ExitGUI();
          }
        }
        else
          this.state.selection.UpdateClip(this.state.selectedItem, this.IndexToClip(this.selectedIndex));
      }
      else
      {
        if (!((UnityEngine.Object) this.state.activeAnimationClip != (UnityEngine.Object) null))
          return;
        EditorGUI.LabelField(EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, AnimationWindowStyles.toolbarLabel, new GUILayoutOption[0]), CurveUtility.GetClipName(this.state.activeAnimationClip), AnimationWindowStyles.toolbarLabel);
      }
    }

    private string[] GetClipMenuContent()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.GetClipNames());
      AnimationWindowSelectionItem selectedItem = this.state.selectedItem;
      if ((UnityEngine.Object) selectedItem.rootGameObject != (UnityEngine.Object) null && selectedItem.animationIsEditable)
      {
        stringList.Add("");
        stringList.Add(AnimationWindowStyles.createNewClip.text);
      }
      return stringList.ToArray();
    }

    private AnimationClip[] GetOrderedClipList()
    {
      AnimationClip[] array = new AnimationClip[0];
      if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
        array = AnimationUtility.GetAnimationClips(this.state.activeRootGameObject);
      Array.Sort<AnimationClip>(array, (Comparison<AnimationClip>) ((clip1, clip2) => CurveUtility.GetClipName(clip1).CompareTo(CurveUtility.GetClipName(clip2))));
      return array;
    }

    private string[] GetClipNames()
    {
      AnimationClip[] orderedClipList = this.GetOrderedClipList();
      string[] strArray = new string[orderedClipList.Length];
      for (int index = 0; index < orderedClipList.Length; ++index)
        strArray[index] = CurveUtility.GetClipName(orderedClipList[index]);
      return strArray;
    }

    private AnimationClip IndexToClip(int index)
    {
      if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
      {
        AnimationClip[] orderedClipList = this.GetOrderedClipList();
        if (index >= 0 && index < orderedClipList.Length)
          return orderedClipList[index];
      }
      return (AnimationClip) null;
    }

    private int ClipToIndex(AnimationClip clip)
    {
      if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
      {
        int num = 0;
        foreach (AnimationClip orderedClip in this.GetOrderedClipList())
        {
          if ((UnityEngine.Object) clip == (UnityEngine.Object) orderedClip)
            return num;
          ++num;
        }
      }
      return 0;
    }
  }
}
