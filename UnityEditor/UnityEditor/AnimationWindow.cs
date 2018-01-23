// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Animation", useTypeNameAsIconName = true)]
  internal class AnimationWindow : EditorWindow
  {
    private static List<AnimationWindow> s_AnimationWindows = new List<AnimationWindow>();
    [SerializeField]
    private bool m_Locked = false;
    [SerializeField]
    private AnimEditor m_AnimEditor;
    private GUIStyle m_LockButtonStyle;
    private GUIContent m_DefaultTitleContent;
    private GUIContent m_RecordTitleContent;

    public static List<AnimationWindow> GetAllAnimationWindows()
    {
      return AnimationWindow.s_AnimationWindows;
    }

    internal AnimationWindowState state
    {
      get
      {
        if ((UnityEngine.Object) this.m_AnimEditor != (UnityEngine.Object) null)
          return this.m_AnimEditor.state;
        return (AnimationWindowState) null;
      }
    }

    public void ForceRefresh()
    {
      if (!((UnityEngine.Object) this.m_AnimEditor != (UnityEngine.Object) null))
        return;
      this.m_AnimEditor.state.ForceRefresh();
    }

    public void OnEnable()
    {
      if ((UnityEngine.Object) this.m_AnimEditor == (UnityEngine.Object) null)
      {
        this.m_AnimEditor = ScriptableObject.CreateInstance(typeof (AnimEditor)) as AnimEditor;
        this.m_AnimEditor.hideFlags = HideFlags.HideAndDontSave;
      }
      AnimationWindow.s_AnimationWindows.Add(this);
      this.titleContent = this.GetLocalizedTitleContent();
      this.m_DefaultTitleContent = this.titleContent;
      this.m_RecordTitleContent = EditorGUIUtility.TextContentWithIcon(this.titleContent.text, "Animation.Record");
      this.OnSelectionChange();
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void OnDisable()
    {
      AnimationWindow.s_AnimationWindows.Remove(this);
      this.m_AnimEditor.OnDisable();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void OnDestroy()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_AnimEditor);
    }

    public void Update()
    {
      this.m_AnimEditor.Update();
    }

    public void OnGUI()
    {
      Profiler.BeginSample("AnimationWindow.OnGUI");
      this.titleContent = !this.m_AnimEditor.state.recording ? this.m_DefaultTitleContent : this.m_RecordTitleContent;
      this.m_AnimEditor.OnAnimEditorGUI((EditorWindow) this, this.position);
      Profiler.EndSample();
    }

    public void OnSelectionChange()
    {
      if ((UnityEngine.Object) this.m_AnimEditor == (UnityEngine.Object) null)
        return;
      GameObject activeGameObject = Selection.activeGameObject;
      if ((UnityEngine.Object) activeGameObject != (UnityEngine.Object) null)
      {
        this.EditGameObject(activeGameObject);
      }
      else
      {
        AnimationClip activeObject = Selection.activeObject as AnimationClip;
        if ((UnityEngine.Object) activeObject != (UnityEngine.Object) null)
          this.EditAnimationClip(activeObject);
      }
    }

    public void OnFocus()
    {
      this.OnSelectionChange();
    }

    public void OnControllerChange()
    {
      this.OnSelectionChange();
    }

    public void OnLostFocus()
    {
      if (!((UnityEngine.Object) this.m_AnimEditor != (UnityEngine.Object) null))
        return;
      this.m_AnimEditor.OnLostFocus();
    }

    public bool EditGameObject(GameObject gameObject)
    {
      if (this.state.linkedWithSequencer)
        return false;
      return this.EditGameObjectInternal(gameObject, (IAnimationWindowControl) null);
    }

    public bool EditAnimationClip(AnimationClip animationClip)
    {
      if (this.state.linkedWithSequencer)
        return false;
      return this.EditAnimationClipInternal(animationClip, (UnityEngine.Object) null, (IAnimationWindowControl) null);
    }

    public bool EditSequencerClip(AnimationClip animationClip, UnityEngine.Object sourceObject, IAnimationWindowControl controlInterface)
    {
      if (!this.EditAnimationClipInternal(animationClip, sourceObject, controlInterface))
        return false;
      this.state.linkedWithSequencer = true;
      return true;
    }

    public void UnlinkSequencer()
    {
      if (!this.state.linkedWithSequencer)
        return;
      this.state.linkedWithSequencer = false;
      this.EditAnimationClip((AnimationClip) null);
      this.OnSelectionChange();
    }

    private bool EditGameObjectInternal(GameObject gameObject, IAnimationWindowControl controlInterface)
    {
      if (EditorUtility.IsPersistent((UnityEngine.Object) gameObject) || (gameObject.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        return false;
      GameObjectSelectionItem selectedItem = GameObjectSelectionItem.Create(gameObject);
      if (this.ShouldUpdateGameObjectSelection(selectedItem))
      {
        this.m_AnimEditor.selectedItem = (AnimationWindowSelectionItem) selectedItem;
        this.m_AnimEditor.overrideControlInterface = controlInterface;
        return true;
      }
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) selectedItem);
      return false;
    }

    private bool EditAnimationClipInternal(AnimationClip animationClip, UnityEngine.Object sourceObject, IAnimationWindowControl controlInterface)
    {
      AnimationClipSelectionItem clipSelectionItem = AnimationClipSelectionItem.Create(animationClip, sourceObject);
      if (this.ShouldUpdateSelection((AnimationWindowSelectionItem) clipSelectionItem))
      {
        this.m_AnimEditor.selectedItem = (AnimationWindowSelectionItem) clipSelectionItem;
        this.m_AnimEditor.overrideControlInterface = controlInterface;
        return true;
      }
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) clipSelectionItem);
      return false;
    }

    protected virtual void ShowButton(Rect r)
    {
      if (this.m_LockButtonStyle == null)
        this.m_LockButtonStyle = (GUIStyle) "IN LockButton";
      if (this.m_AnimEditor.stateDisabled)
        this.m_Locked = false;
      EditorGUI.BeginChangeCheck();
      using (new EditorGUI.DisabledScope(this.m_AnimEditor.stateDisabled))
        this.m_Locked = GUI.Toggle(r, this.m_Locked, GUIContent.none, this.m_LockButtonStyle);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.OnSelectionChange();
    }

    private bool ShouldUpdateGameObjectSelection(GameObjectSelectionItem selectedItem)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimationWindow.\u003CShouldUpdateGameObjectSelection\u003Ec__AnonStorey0 selectionCAnonStorey0 = new AnimationWindow.\u003CShouldUpdateGameObjectSelection\u003Ec__AnonStorey0();
      if (this.m_Locked)
        return false;
      if ((UnityEngine.Object) selectedItem.rootGameObject == (UnityEngine.Object) null)
        return true;
      // ISSUE: reference to a compiler-generated field
      selectionCAnonStorey0.currentlySelectedItem = this.m_AnimEditor.selectedItem;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return !((UnityEngine.Object) selectionCAnonStorey0.currentlySelectedItem != (UnityEngine.Object) null) || (UnityEngine.Object) selectedItem.rootGameObject != (UnityEngine.Object) selectionCAnonStorey0.currentlySelectedItem.rootGameObject || ((UnityEngine.Object) selectionCAnonStorey0.currentlySelectedItem.animationClip == (UnityEngine.Object) null || (UnityEngine.Object) selectionCAnonStorey0.currentlySelectedItem.rootGameObject != (UnityEngine.Object) null && !Array.Exists<AnimationClip>(AnimationUtility.GetAnimationClips(selectionCAnonStorey0.currentlySelectedItem.rootGameObject), new Predicate<AnimationClip>(selectionCAnonStorey0.\u003C\u003Em__0)));
    }

    private bool ShouldUpdateSelection(AnimationWindowSelectionItem selectedItem)
    {
      if (this.m_Locked)
        return false;
      AnimationWindowSelectionItem selectedItem1 = this.m_AnimEditor.selectedItem;
      if ((UnityEngine.Object) selectedItem1 != (UnityEngine.Object) null)
        return selectedItem.GetRefreshHash() != selectedItem1.GetRefreshHash();
      return true;
    }

    private void UndoRedoPerformed()
    {
      this.Repaint();
    }
  }
}
