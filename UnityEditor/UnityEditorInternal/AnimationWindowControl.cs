// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowControl : IAnimationWindowControl, IAnimationContextualResponder
  {
    [SerializeField]
    private AnimationKeyTime m_Time;
    [NonSerialized]
    private float m_PreviousUpdateTime;
    [NonSerialized]
    public AnimationWindowState state;
    [SerializeField]
    private AnimationClip m_CandidateClip;
    [SerializeField]
    private AnimationModeDriver m_Driver;
    [SerializeField]
    private AnimationModeDriver m_CandidateDriver;

    public AnimEditor animEditor
    {
      get
      {
        return this.state.animEditor;
      }
    }

    public override void OnEnable()
    {
      base.OnEnable();
    }

    public void OnDisable()
    {
      this.StopPreview();
      this.StopPlayback();
      if (!AnimationMode.InAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver()))
        return;
      AnimationMode.StopAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver());
    }

    public override void OnSelectionChanged()
    {
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
        this.m_Time = AnimationKeyTime.Time(0.0f, this.state.frameRate);
      this.StopPreview();
      this.StopPlayback();
    }

    public override AnimationKeyTime time
    {
      get
      {
        return this.m_Time;
      }
    }

    public override void GoToTime(float time)
    {
      this.SetCurrentTime(time);
    }

    public override void GoToFrame(int frame)
    {
      this.SetCurrentFrame(frame);
    }

    public override void StartScrubTime()
    {
    }

    public override void ScrubTime(float time)
    {
      this.SetCurrentTime(time);
    }

    public override void EndScrubTime()
    {
    }

    public override void GoToPreviousFrame()
    {
      this.SetCurrentFrame(this.time.frame - 1);
    }

    public override void GoToNextFrame()
    {
      this.SetCurrentFrame(this.time.frame + 1);
    }

    public override void GoToPreviousKeyframe()
    {
      this.SetCurrentTime(this.state.SnapToFrame(AnimationWindowUtility.GetPreviousKeyframeTime((!this.state.showCurveEditor || this.state.activeCurves.Count <= 0 ? this.state.allCurves : this.state.activeCurves).ToArray(), this.time.time, this.state.clipFrameRate), AnimationWindowState.SnapMode.SnapToClipFrame));
    }

    public void GoToPreviousKeyframe(PropertyModification[] modifications)
    {
      EditorCurveBinding[] editorCurveBindings = AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(modifications, this.state.activeRootGameObject, this.state.activeAnimationClip);
      if (editorCurveBindings.Length == 0)
        return;
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      for (int index = 0; index < this.state.allCurves.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowControl.\u003CGoToPreviousKeyframe\u003Ec__AnonStorey0 keyframeCAnonStorey0 = new AnimationWindowControl.\u003CGoToPreviousKeyframe\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        keyframeCAnonStorey0.curve = this.state.allCurves[index];
        // ISSUE: reference to a compiler-generated method
        if (Array.Exists<EditorCurveBinding>(editorCurveBindings, new Predicate<EditorCurveBinding>(keyframeCAnonStorey0.\u003C\u003Em__0)))
        {
          // ISSUE: reference to a compiler-generated field
          animationWindowCurveList.Add(keyframeCAnonStorey0.curve);
        }
      }
      this.SetCurrentTime(this.state.SnapToFrame(AnimationWindowUtility.GetPreviousKeyframeTime(animationWindowCurveList.ToArray(), this.time.time, this.state.clipFrameRate), AnimationWindowState.SnapMode.SnapToClipFrame));
      this.state.Repaint();
    }

    public override void GoToNextKeyframe()
    {
      this.SetCurrentTime(this.state.SnapToFrame(AnimationWindowUtility.GetNextKeyframeTime((!this.state.showCurveEditor || this.state.activeCurves.Count <= 0 ? this.state.allCurves : this.state.activeCurves).ToArray(), this.time.time, this.state.clipFrameRate), AnimationWindowState.SnapMode.SnapToClipFrame));
    }

    public void GoToNextKeyframe(PropertyModification[] modifications)
    {
      EditorCurveBinding[] editorCurveBindings = AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(modifications, this.state.activeRootGameObject, this.state.activeAnimationClip);
      if (editorCurveBindings.Length == 0)
        return;
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      for (int index = 0; index < this.state.allCurves.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowControl.\u003CGoToNextKeyframe\u003Ec__AnonStorey1 keyframeCAnonStorey1 = new AnimationWindowControl.\u003CGoToNextKeyframe\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        keyframeCAnonStorey1.curve = this.state.allCurves[index];
        // ISSUE: reference to a compiler-generated method
        if (Array.Exists<EditorCurveBinding>(editorCurveBindings, new Predicate<EditorCurveBinding>(keyframeCAnonStorey1.\u003C\u003Em__0)))
        {
          // ISSUE: reference to a compiler-generated field
          animationWindowCurveList.Add(keyframeCAnonStorey1.curve);
        }
      }
      this.SetCurrentTime(this.state.SnapToFrame(AnimationWindowUtility.GetNextKeyframeTime(animationWindowCurveList.ToArray(), this.time.time, this.state.clipFrameRate), AnimationWindowState.SnapMode.SnapToClipFrame));
      this.state.Repaint();
    }

    public override void GoToFirstKeyframe()
    {
      if (!(bool) ((UnityEngine.Object) this.state.activeAnimationClip))
        return;
      this.SetCurrentTime(this.state.activeAnimationClip.startTime);
    }

    public override void GoToLastKeyframe()
    {
      if (!(bool) ((UnityEngine.Object) this.state.activeAnimationClip))
        return;
      this.SetCurrentTime(this.state.activeAnimationClip.stopTime);
    }

    private void SnapTimeToFrame()
    {
      this.SetCurrentTime(this.state.FrameToTime((float) this.time.frame));
    }

    private void SetCurrentTime(float value)
    {
      if (Mathf.Approximately(value, this.time.time))
        return;
      this.m_Time = AnimationKeyTime.Time(value, this.state.frameRate);
      this.StartPreview();
      this.ClearCandidates();
      this.ResampleAnimation();
    }

    private void SetCurrentFrame(int value)
    {
      if (value == this.time.frame)
        return;
      this.m_Time = AnimationKeyTime.Frame(value, this.state.frameRate);
      this.StartPreview();
      this.ClearCandidates();
      this.ResampleAnimation();
    }

    public override bool canPlay
    {
      get
      {
        return this.canPreview;
      }
    }

    public override bool playing
    {
      get
      {
        return AnimationMode.InAnimationPlaybackMode() && this.previewing;
      }
    }

    public override bool StartPlayback()
    {
      if (!this.canPlay)
        return false;
      if (!this.playing)
      {
        AnimationMode.StartAnimationPlaybackMode();
        this.m_PreviousUpdateTime = Time.realtimeSinceStartup;
        this.StartPreview();
        this.ClearCandidates();
      }
      return true;
    }

    public override void StopPlayback()
    {
      if (!AnimationMode.InAnimationPlaybackMode())
        return;
      AnimationMode.StopAnimationPlaybackMode();
      this.SnapTimeToFrame();
    }

    public override bool PlaybackUpdate()
    {
      if (!this.playing)
        return false;
      float num1 = Time.realtimeSinceStartup - this.m_PreviousUpdateTime;
      this.m_PreviousUpdateTime = Time.realtimeSinceStartup;
      float num2 = this.time.time + num1;
      if ((double) num2 > (double) this.state.maxTime)
        num2 = this.state.minTime;
      this.m_Time = AnimationKeyTime.Time(Mathf.Clamp(num2, this.state.minTime, this.state.maxTime), this.state.frameRate);
      this.ResampleAnimation();
      return true;
    }

    public override bool canPreview
    {
      get
      {
        if (!this.state.selection.canPreview)
          return false;
        return AnimationMode.InAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver()) || !AnimationMode.InAnimationMode();
      }
    }

    public override bool previewing
    {
      get
      {
        return AnimationMode.InAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver());
      }
    }

    public override bool StartPreview()
    {
      if (this.previewing)
        return true;
      if (!this.canPreview)
        return false;
      AnimationMode.StartAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver());
      AnimationPropertyContextualMenu.Instance.SetResponder((IAnimationContextualResponder) this);
      Undo.postprocessModifications += new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications);
      return true;
    }

    public override void StopPreview()
    {
      this.StopPlayback();
      this.StopRecording();
      this.ClearCandidates();
      AnimationMode.StopAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver());
      if (AnimationPropertyContextualMenu.Instance.IsResponder((IAnimationContextualResponder) this))
        AnimationPropertyContextualMenu.Instance.SetResponder((IAnimationContextualResponder) null);
      Undo.postprocessModifications -= new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications);
    }

    public override bool canRecord
    {
      get
      {
        if (!this.state.selection.canRecord)
          return false;
        return this.canPreview;
      }
    }

    public override bool recording
    {
      get
      {
        if (this.previewing)
          return AnimationMode.InAnimationRecording();
        return false;
      }
    }

    public override bool StartRecording(UnityEngine.Object targetObject)
    {
      return this.StartRecording();
    }

    private bool StartRecording()
    {
      if (this.recording)
        return true;
      if (!this.canRecord || !this.StartPreview())
        return false;
      AnimationMode.StartAnimationRecording();
      this.ClearCandidates();
      return true;
    }

    public override void StopRecording()
    {
      if (!this.recording)
        return;
      AnimationMode.StopAnimationRecording();
    }

    private void StartCandidateRecording()
    {
      AnimationMode.StartCandidateRecording((UnityEngine.Object) this.GetCandidateDriver());
    }

    private void StopCandidateRecording()
    {
      AnimationMode.StopCandidateRecording();
    }

    public override void ResampleAnimation()
    {
      if (this.state.disabled || !this.previewing || !this.canPreview)
        return;
      bool flag = false;
      AnimationMode.BeginSampling();
      foreach (AnimationWindowSelectionItem windowSelectionItem in this.state.selection.ToArray())
      {
        if ((UnityEngine.Object) windowSelectionItem.animationClip != (UnityEngine.Object) null)
        {
          Undo.FlushUndoRecordObjects();
          AnimationMode.SampleAnimationClip(windowSelectionItem.rootGameObject, windowSelectionItem.animationClip, this.time.time - windowSelectionItem.timeOffset);
          if ((UnityEngine.Object) this.m_CandidateClip != (UnityEngine.Object) null)
            AnimationMode.SampleCandidateClip(windowSelectionItem.rootGameObject, this.m_CandidateClip, 0.0f);
          flag = true;
        }
      }
      AnimationMode.EndSampling();
      if (!flag)
        return;
      SceneView.RepaintAll();
      InspectorWindow.RepaintAllInspectors();
      ParticleSystemWindow instance = ParticleSystemWindow.GetInstance();
      if ((bool) ((UnityEngine.Object) instance))
        instance.Repaint();
    }

    private AnimationModeDriver GetAnimationModeDriver()
    {
      if ((UnityEngine.Object) this.m_Driver == (UnityEngine.Object) null)
      {
        this.m_Driver = ScriptableObject.CreateInstance<AnimationModeDriver>();
        this.m_Driver.name = "AnimationWindowDriver";
        this.m_Driver.isKeyCallback += (AnimationModeDriver.IsKeyCallback) ((target, propertyPath) =>
        {
          if (!AnimationMode.IsPropertyAnimated(target, propertyPath))
            return false;
          return this.KeyExists(new PropertyModification[1]{ new PropertyModification() { target = target, propertyPath = propertyPath } });
        });
      }
      return this.m_Driver;
    }

    private AnimationModeDriver GetCandidateDriver()
    {
      if ((UnityEngine.Object) this.m_CandidateDriver == (UnityEngine.Object) null)
      {
        this.m_CandidateDriver = ScriptableObject.CreateInstance<AnimationModeDriver>();
        this.m_CandidateDriver.name = "AnimationWindowCandidateDriver";
      }
      return this.m_CandidateDriver;
    }

    private UndoPropertyModification[] PostprocessAnimationRecordingModifications(UndoPropertyModification[] modifications)
    {
      if (!AnimationMode.InAnimationMode((UnityEngine.Object) this.GetAnimationModeDriver()))
      {
        Undo.postprocessModifications -= new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications);
        return modifications;
      }
      if (this.recording)
        return this.ProcessAutoKey(modifications);
      if (this.previewing)
        return this.RegisterCandidates(modifications);
      return modifications;
    }

    private UndoPropertyModification[] ProcessAutoKey(UndoPropertyModification[] modifications)
    {
      this.BeginKeyModification();
      UndoPropertyModification[] propertyModificationArray = AnimationRecording.Process((IAnimationRecordingState) new AnimationWindowControl.RecordingState(this.state, AnimationWindowControl.RecordingStateMode.AutoKey), modifications);
      this.EndKeyModification();
      return propertyModificationArray;
    }

    private UndoPropertyModification[] RegisterCandidates(UndoPropertyModification[] modifications)
    {
      bool flag = (UnityEngine.Object) this.m_CandidateClip == (UnityEngine.Object) null;
      if (flag)
      {
        this.m_CandidateClip = new AnimationClip();
        this.m_CandidateClip.legacy = this.state.activeAnimationClip.legacy;
        this.m_CandidateClip.name = "CandidateClip";
        this.StartCandidateRecording();
      }
      UndoPropertyModification[] propertyModificationArray = AnimationRecording.Process((IAnimationRecordingState) new AnimationWindowControl.CandidateRecordingState(this.state, this.m_CandidateClip), modifications);
      if (flag && propertyModificationArray.Length == modifications.Length)
        this.ClearCandidates();
      InspectorWindow.RepaintAllInspectors();
      return propertyModificationArray;
    }

    private void RemoveFromCandidates(PropertyModification[] modifications)
    {
      if ((UnityEngine.Object) this.m_CandidateClip == (UnityEngine.Object) null)
        return;
      EditorCurveBinding[] editorCurveBindings = AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(modifications, this.state.activeRootGameObject, this.m_CandidateClip);
      if (editorCurveBindings.Length == 0)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_CandidateClip, "Edit Candidate Curve");
      for (int index = 0; index < editorCurveBindings.Length; ++index)
      {
        EditorCurveBinding binding = editorCurveBindings[index];
        if (binding.isPPtrCurve)
          AnimationUtility.SetObjectReferenceCurve(this.m_CandidateClip, binding, (ObjectReferenceKeyframe[]) null);
        else
          AnimationUtility.SetEditorCurve(this.m_CandidateClip, binding, (AnimationCurve) null);
      }
      if (AnimationUtility.GetCurveBindings(this.m_CandidateClip).Length != 0 || AnimationUtility.GetObjectReferenceCurveBindings(this.m_CandidateClip).Length != 0)
        return;
      this.ClearCandidates();
    }

    public override void ClearCandidates()
    {
      this.m_CandidateClip = (AnimationClip) null;
      this.StopCandidateRecording();
    }

    public override void ProcessCandidates()
    {
      if ((UnityEngine.Object) this.m_CandidateClip == (UnityEngine.Object) null)
        return;
      this.BeginKeyModification();
      EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(this.m_CandidateClip);
      EditorCurveBinding[] referenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(this.m_CandidateClip);
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      for (int index = 0; index < this.state.allCurves.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowControl.\u003CProcessCandidates\u003Ec__AnonStorey2 candidatesCAnonStorey2 = new AnimationWindowControl.\u003CProcessCandidates\u003Ec__AnonStorey2();
        AnimationWindowCurve allCurve = this.state.allCurves[index];
        // ISSUE: reference to a compiler-generated field
        candidatesCAnonStorey2.remappedBinding = RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(allCurve.binding, this.m_CandidateClip);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (Array.Exists<EditorCurveBinding>(curveBindings, new Predicate<EditorCurveBinding>(candidatesCAnonStorey2.\u003C\u003Em__0)) || Array.Exists<EditorCurveBinding>(referenceCurveBindings, new Predicate<EditorCurveBinding>(candidatesCAnonStorey2.\u003C\u003Em__1)))
          animationWindowCurveList.Add(allCurve);
      }
      AnimationWindowUtility.AddKeyframes(this.state, animationWindowCurveList.ToArray(), this.time);
      this.EndKeyModification();
      this.ClearCandidates();
    }

    private List<AnimationWindowKeyframe> GetKeys(PropertyModification[] modifications)
    {
      List<AnimationWindowKeyframe> animationWindowKeyframeList = new List<AnimationWindowKeyframe>();
      EditorCurveBinding[] editorCurveBindings = AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(modifications, this.state.activeRootGameObject, this.state.activeAnimationClip);
      if (editorCurveBindings.Length == 0)
        return animationWindowKeyframeList;
      for (int index = 0; index < this.state.allCurves.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowControl.\u003CGetKeys\u003Ec__AnonStorey3 keysCAnonStorey3 = new AnimationWindowControl.\u003CGetKeys\u003Ec__AnonStorey3();
        // ISSUE: reference to a compiler-generated field
        keysCAnonStorey3.curve = this.state.allCurves[index];
        // ISSUE: reference to a compiler-generated method
        if (Array.Exists<EditorCurveBinding>(editorCurveBindings, new Predicate<EditorCurveBinding>(keysCAnonStorey3.\u003C\u003Em__0)))
        {
          // ISSUE: reference to a compiler-generated field
          int keyframeIndex = keysCAnonStorey3.curve.GetKeyframeIndex(this.state.time);
          if (keyframeIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            animationWindowKeyframeList.Add(keysCAnonStorey3.curve.m_Keyframes[keyframeIndex]);
          }
        }
      }
      return animationWindowKeyframeList;
    }

    public bool IsAnimatable(PropertyModification[] modifications)
    {
      for (int index = 0; index < modifications.Length; ++index)
      {
        PropertyModification modification = modifications[index];
        if (AnimationWindowUtility.PropertyIsAnimatable(modification.target, modification.propertyPath, (UnityEngine.Object) this.state.activeRootGameObject))
          return true;
      }
      return false;
    }

    public bool IsEditable(UnityEngine.Object targetObject)
    {
      if (this.state.selection.disabled || !this.previewing)
        return false;
      AnimationWindowSelectionItem selectedItem = this.state.selectedItem;
      if ((UnityEngine.Object) selectedItem != (UnityEngine.Object) null)
      {
        GameObject gameObject = (GameObject) null;
        if (targetObject is Component)
          gameObject = ((Component) targetObject).gameObject;
        else if (targetObject is GameObject)
          gameObject = (GameObject) targetObject;
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Component componentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(gameObject.transform);
          if ((UnityEngine.Object) selectedItem.animationPlayer == (UnityEngine.Object) componentInParents)
            return selectedItem.animationIsEditable;
        }
      }
      return false;
    }

    public bool KeyExists(PropertyModification[] modifications)
    {
      return this.GetKeys(modifications).Count > 0;
    }

    public bool CandidateExists(PropertyModification[] modifications)
    {
      if (!this.HasAnyCandidates())
        return false;
      for (int index = 0; index < modifications.Length; ++index)
      {
        PropertyModification modification = modifications[index];
        if (AnimationMode.IsPropertyCandidate(modification.target, modification.propertyPath))
          return true;
      }
      return false;
    }

    public bool CurveExists(PropertyModification[] modifications)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimationWindowControl.\u003CCurveExists\u003Ec__AnonStorey4 existsCAnonStorey4 = new AnimationWindowControl.\u003CCurveExists\u003Ec__AnonStorey4();
      EditorCurveBinding[] editorCurveBindings = AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(modifications, this.state.activeRootGameObject, this.state.activeAnimationClip);
      if (editorCurveBindings.Length == 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      existsCAnonStorey4.clipBindings = AnimationUtility.GetCurveBindings(this.state.activeAnimationClip);
      // ISSUE: reference to a compiler-generated field
      if (existsCAnonStorey4.clipBindings.Length == 0)
        return false;
      // ISSUE: reference to a compiler-generated method
      if (Array.Exists<EditorCurveBinding>(editorCurveBindings, new Predicate<EditorCurveBinding>(existsCAnonStorey4.\u003C\u003Em__0)))
        return true;
      EditorCurveBinding[] referenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(this.state.activeAnimationClip);
      if (referenceCurveBindings.Length == 0)
        return false;
      // ISSUE: reference to a compiler-generated method
      return Array.Exists<EditorCurveBinding>(referenceCurveBindings, new Predicate<EditorCurveBinding>(existsCAnonStorey4.\u003C\u003Em__1));
    }

    public bool HasAnyCandidates()
    {
      return (UnityEngine.Object) this.m_CandidateClip != (UnityEngine.Object) null;
    }

    public bool HasAnyCurves()
    {
      return this.state.allCurves.Count > 0;
    }

    public void AddKey(SerializedProperty property)
    {
      this.AddKey(AnimationWindowUtility.SerializedPropertyToPropertyModifications(property));
    }

    public void AddKey(PropertyModification[] modifications)
    {
      UndoPropertyModification[] modifications1 = new UndoPropertyModification[modifications.Length];
      for (int index = 0; index < modifications.Length; ++index)
      {
        PropertyModification modification = modifications[index];
        modifications1[index].previousValue = modification;
        modifications1[index].currentValue = modification;
      }
      this.BeginKeyModification();
      AnimationRecording.Process((IAnimationRecordingState) new AnimationWindowControl.RecordingState(this.state, AnimationWindowControl.RecordingStateMode.ManualKey), modifications1);
      this.EndKeyModification();
      this.RemoveFromCandidates(modifications);
      this.ResampleAnimation();
      this.state.Repaint();
    }

    public void RemoveKey(SerializedProperty property)
    {
      this.RemoveKey(AnimationWindowUtility.SerializedPropertyToPropertyModifications(property));
    }

    public void RemoveKey(PropertyModification[] modifications)
    {
      this.BeginKeyModification();
      this.state.DeleteKeys(this.GetKeys(modifications));
      this.RemoveFromCandidates(modifications);
      this.EndKeyModification();
      this.ResampleAnimation();
      this.state.Repaint();
    }

    public void RemoveCurve(SerializedProperty property)
    {
      this.RemoveCurve(AnimationWindowUtility.SerializedPropertyToPropertyModifications(property));
    }

    public void RemoveCurve(PropertyModification[] modifications)
    {
      EditorCurveBinding[] editorCurveBindings = AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(modifications, this.state.activeRootGameObject, this.state.activeAnimationClip);
      if (editorCurveBindings.Length == 0)
        return;
      this.BeginKeyModification();
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.state.activeAnimationClip, "Remove Curve");
      for (int index = 0; index < editorCurveBindings.Length; ++index)
      {
        EditorCurveBinding binding = editorCurveBindings[index];
        if (binding.isPPtrCurve)
          AnimationUtility.SetObjectReferenceCurve(this.state.activeAnimationClip, binding, (ObjectReferenceKeyframe[]) null);
        else
          AnimationUtility.SetEditorCurve(this.state.activeAnimationClip, binding, (AnimationCurve) null);
      }
      this.EndKeyModification();
      this.RemoveFromCandidates(modifications);
      this.ResampleAnimation();
      this.state.Repaint();
    }

    public void AddCandidateKeys()
    {
      this.ProcessCandidates();
      this.ResampleAnimation();
      this.state.Repaint();
    }

    public void AddAnimatedKeys()
    {
      this.BeginKeyModification();
      AnimationWindowUtility.AddKeyframes(this.state, this.state.allCurves.ToArray(), this.time);
      this.ClearCandidates();
      this.EndKeyModification();
      this.ResampleAnimation();
      this.state.Repaint();
    }

    private void BeginKeyModification()
    {
      if (!((UnityEngine.Object) this.animEditor != (UnityEngine.Object) null))
        return;
      this.animEditor.BeginKeyModification();
    }

    private void EndKeyModification()
    {
      if (!((UnityEngine.Object) this.animEditor != (UnityEngine.Object) null))
        return;
      this.animEditor.EndKeyModification();
    }

    private class CandidateRecordingState : IAnimationRecordingState
    {
      public CandidateRecordingState(AnimationWindowState state, AnimationClip candidateClip)
      {
        this.activeGameObject = state.activeGameObject;
        this.activeRootGameObject = state.activeRootGameObject;
        this.activeAnimationClip = candidateClip;
      }

      public GameObject activeGameObject { get; private set; }

      public GameObject activeRootGameObject { get; private set; }

      public AnimationClip activeAnimationClip { get; private set; }

      public int currentFrame
      {
        get
        {
          return 0;
        }
      }

      public bool addZeroFrame
      {
        get
        {
          return false;
        }
      }

      public bool DiscardModification(PropertyModification modification)
      {
        return !AnimationMode.IsPropertyAnimated(modification.target, modification.propertyPath);
      }

      public void SaveCurve(AnimationWindowCurve curve)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) curve.clip, "Edit Candidate Curve");
        AnimationRecording.SaveModifiedCurve(curve, curve.clip);
      }

      public void AddPropertyModification(EditorCurveBinding binding, PropertyModification propertyModification, bool keepPrefabOverride)
      {
        AnimationMode.AddCandidate(binding, propertyModification, keepPrefabOverride);
      }
    }

    private enum RecordingStateMode
    {
      ManualKey,
      AutoKey,
    }

    private class RecordingState : IAnimationRecordingState
    {
      private AnimationWindowState m_State;
      private AnimationWindowControl.RecordingStateMode m_Mode;

      public RecordingState(AnimationWindowState state, AnimationWindowControl.RecordingStateMode mode)
      {
        this.m_State = state;
        this.m_Mode = mode;
      }

      public GameObject activeGameObject
      {
        get
        {
          return this.m_State.activeGameObject;
        }
      }

      public GameObject activeRootGameObject
      {
        get
        {
          return this.m_State.activeRootGameObject;
        }
      }

      public AnimationClip activeAnimationClip
      {
        get
        {
          return this.m_State.activeAnimationClip;
        }
      }

      public int currentFrame
      {
        get
        {
          return this.m_State.currentFrame;
        }
      }

      public bool addZeroFrame
      {
        get
        {
          return this.m_Mode == AnimationWindowControl.RecordingStateMode.AutoKey;
        }
      }

      public bool addPropertyModification
      {
        get
        {
          return this.m_State.previewing;
        }
      }

      public bool DiscardModification(PropertyModification modification)
      {
        return false;
      }

      public void SaveCurve(AnimationWindowCurve curve)
      {
        this.m_State.SaveCurve(curve);
      }

      public void AddPropertyModification(EditorCurveBinding binding, PropertyModification propertyModification, bool keepPrefabOverride)
      {
        AnimationMode.AddPropertyModification(binding, propertyModification, keepPrefabOverride);
      }
    }
  }
}
