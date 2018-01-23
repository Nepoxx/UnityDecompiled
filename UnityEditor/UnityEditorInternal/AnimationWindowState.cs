// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class AnimationWindowState : ScriptableObject, ICurveEditorState
  {
    [SerializeField]
    private float m_FrameRate = 60f;
    [SerializeField]
    private TimeArea.TimeFormat m_TimeFormat = TimeArea.TimeFormat.TimeFrame;
    private HashSet<int> m_ModifiedCurves = new HashSet<int>();
    private AnimationWindowState.RefreshType m_Refresh = AnimationWindowState.RefreshType.None;
    [SerializeField]
    public AnimationWindowHierarchyState hierarchyState;
    [SerializeField]
    public AnimEditor animEditor;
    [SerializeField]
    public bool showCurveEditor;
    [SerializeField]
    public bool linkedWithSequencer;
    [SerializeField]
    private TimeArea m_TimeArea;
    [SerializeField]
    private AnimationWindowSelection m_Selection;
    [SerializeField]
    private AnimationWindowKeySelection m_KeySelection;
    [SerializeField]
    private int m_ActiveKeyframeHash;
    [SerializeField]
    private AnimationWindowControl m_ControlInterface;
    [SerializeField]
    private IAnimationWindowControl m_OverrideControlInterface;
    [NonSerialized]
    public Action onStartLiveEdit;
    [NonSerialized]
    public Action onEndLiveEdit;
    [NonSerialized]
    public Action<float> onFrameRateChange;
    private static List<AnimationWindowKeyframe> s_KeyframeClipboard;
    [NonSerialized]
    public AnimationWindowHierarchyDataSource hierarchyData;
    private List<AnimationWindowCurve> m_ActiveCurvesCache;
    private List<DopeLine> m_dopelinesCache;
    private List<AnimationWindowKeyframe> m_SelectedKeysCache;
    private Bounds? m_SelectionBoundsCache;
    private CurveWrapper[] m_ActiveCurveWrappersCache;
    private AnimationWindowKeyframe m_ActiveKeyframeCache;
    private EditorCurveBinding? m_lastAddedCurveBinding;
    private int m_PreviousRefreshHash;
    private List<AnimationWindowState.LiveEditCurve> m_LiveEditSnapshot;
    public const float kDefaultFrameRate = 60f;
    public const string kEditCurveUndoLabel = "Edit Curve";

    public AnimationWindowSelection selection
    {
      get
      {
        if (this.m_Selection == null)
          this.m_Selection = new AnimationWindowSelection();
        return this.m_Selection;
      }
    }

    public AnimationWindowSelectionItem selectedItem
    {
      get
      {
        if (this.m_Selection != null && this.m_Selection.count > 0)
          return this.m_Selection.First();
        return (AnimationWindowSelectionItem) null;
      }
      set
      {
        if (this.m_Selection == null)
          this.m_Selection = new AnimationWindowSelection();
        if ((UnityEngine.Object) value == (UnityEngine.Object) null)
          this.m_Selection.Clear();
        else
          this.m_Selection.Set(value);
      }
    }

    public AnimationClip activeAnimationClip
    {
      get
      {
        if ((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null)
          return this.selectedItem.animationClip;
        return (AnimationClip) null;
      }
    }

    public GameObject activeGameObject
    {
      get
      {
        if ((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null)
          return this.selectedItem.gameObject;
        return (GameObject) null;
      }
    }

    public GameObject activeRootGameObject
    {
      get
      {
        if ((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null)
          return this.selectedItem.rootGameObject;
        return (GameObject) null;
      }
    }

    public Component activeAnimationPlayer
    {
      get
      {
        if ((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null)
          return this.selectedItem.animationPlayer;
        return (Component) null;
      }
    }

    public bool animatorIsOptimized
    {
      get
      {
        if ((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null)
          return this.selectedItem.objectIsOptimized;
        return false;
      }
    }

    public bool disabled
    {
      get
      {
        return this.selection.disabled;
      }
    }

    public IAnimationWindowControl controlInterface
    {
      get
      {
        if ((UnityEngine.Object) this.m_OverrideControlInterface != (UnityEngine.Object) null)
          return this.m_OverrideControlInterface;
        return (IAnimationWindowControl) this.m_ControlInterface;
      }
    }

    public IAnimationWindowControl overrideControlInterface
    {
      get
      {
        return this.m_OverrideControlInterface;
      }
      set
      {
        if ((UnityEngine.Object) this.m_OverrideControlInterface != (UnityEngine.Object) null)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_OverrideControlInterface);
        this.m_OverrideControlInterface = value;
      }
    }

    public void OnGUI()
    {
      this.RefreshHashCheck();
      this.Refresh();
    }

    private void RefreshHashCheck()
    {
      int refreshHash = this.GetRefreshHash();
      if (this.m_PreviousRefreshHash == refreshHash)
        return;
      this.refresh = AnimationWindowState.RefreshType.Everything;
      this.m_PreviousRefreshHash = refreshHash;
    }

    private void Refresh()
    {
      this.selection.Synchronize();
      if (this.refresh == AnimationWindowState.RefreshType.Everything)
      {
        this.selection.Refresh();
        this.m_ActiveKeyframeCache = (AnimationWindowKeyframe) null;
        this.m_ActiveCurvesCache = (List<AnimationWindowCurve>) null;
        this.m_dopelinesCache = (List<DopeLine>) null;
        this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
        this.m_SelectionBoundsCache = new Bounds?();
        this.ClearCurveWrapperCache();
        if (this.hierarchyData != null)
          this.hierarchyData.UpdateData();
        if (this.m_lastAddedCurveBinding.HasValue)
          this.OnNewCurveAdded(this.m_lastAddedCurveBinding.Value);
        if (this.activeCurves.Count == 0 && this.dopelines.Count > 0)
          this.SelectHierarchyItem(this.dopelines[0], false, false);
        this.m_Refresh = AnimationWindowState.RefreshType.None;
      }
      else
      {
        if (this.refresh != AnimationWindowState.RefreshType.CurvesOnly)
          return;
        this.m_ActiveKeyframeCache = (AnimationWindowKeyframe) null;
        this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
        this.m_SelectionBoundsCache = new Bounds?();
        this.ReloadModifiedAnimationCurveCache();
        this.ReloadModifiedDopelineCache();
        this.ReloadModifiedCurveWrapperCache();
        this.m_Refresh = AnimationWindowState.RefreshType.None;
        this.m_ModifiedCurves.Clear();
      }
    }

    private int GetRefreshHash()
    {
      return this.selection.GetRefreshHash() ^ (this.hierarchyState == null ? 0 : this.hierarchyState.expandedIDs.Count) ^ (this.hierarchyState == null ? 0 : this.hierarchyState.GetTallInstancesCount()) ^ (!this.showCurveEditor ? 0 : 1);
    }

    public void ForceRefresh()
    {
      this.refresh = AnimationWindowState.RefreshType.Everything;
    }

    public void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
      AnimationUtility.onCurveWasModified += new AnimationUtility.OnCurveWasModified(this.CurveWasModified);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.onStartLiveEdit += (Action) (() => {});
      this.onEndLiveEdit += (Action) (() => {});
      this.onFrameRateChange += (Action<float>) (frameRate => {});
      if ((UnityEngine.Object) this.m_ControlInterface == (UnityEngine.Object) null)
        this.m_ControlInterface = ScriptableObject.CreateInstance(typeof (AnimationWindowControl)) as AnimationWindowControl;
      this.m_ControlInterface.state = this;
    }

    public void OnDisable()
    {
      AnimationUtility.onCurveWasModified -= new AnimationUtility.OnCurveWasModified(this.CurveWasModified);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.m_ControlInterface.OnDisable();
    }

    public void OnDestroy()
    {
      if (this.m_Selection != null)
        this.m_Selection.Clear();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_KeySelection);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ControlInterface);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_OverrideControlInterface);
    }

    public void OnSelectionChanged()
    {
      this.onFrameRateChange(this.frameRate);
      this.controlInterface.OnSelectionChanged();
    }

    public AnimationWindowState.RefreshType refresh
    {
      get
      {
        return this.m_Refresh;
      }
      set
      {
        if (this.m_Refresh >= value)
          return;
        this.m_Refresh = value;
      }
    }

    public void UndoRedoPerformed()
    {
      this.refresh = AnimationWindowState.RefreshType.Everything;
      this.controlInterface.ResampleAnimation();
    }

    private void CurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type)
    {
      AnimationWindowSelectionItem[] all = Array.FindAll<AnimationWindowSelectionItem>(this.selection.ToArray(), (Predicate<AnimationWindowSelectionItem>) (item => (UnityEngine.Object) item.animationClip == (UnityEngine.Object) clip));
      if (all.Length == 0)
        return;
      if (type == AnimationUtility.CurveModifiedType.CurveModified)
      {
        bool flag1 = false;
        bool flag2 = false;
        int hashCode = binding.GetHashCode();
        for (int index1 = 0; index1 < all.Length; ++index1)
        {
          List<AnimationWindowCurve> curves = all[index1].curves;
          for (int index2 = 0; index2 < curves.Count; ++index2)
          {
            AnimationWindowCurve animationWindowCurve = curves[index2];
            if (animationWindowCurve.GetBindingHashCode() == hashCode)
            {
              this.m_ModifiedCurves.Add(animationWindowCurve.GetHashCode());
              flag1 = true;
              flag2 |= animationWindowCurve.binding.isPhantom;
            }
          }
        }
        if (flag1 && !flag2)
        {
          this.refresh = AnimationWindowState.RefreshType.CurvesOnly;
        }
        else
        {
          this.m_lastAddedCurveBinding = new EditorCurveBinding?(binding);
          this.refresh = AnimationWindowState.RefreshType.Everything;
        }
      }
      else
        this.refresh = AnimationWindowState.RefreshType.Everything;
    }

    public void SaveKeySelection(string undoLabel)
    {
      if (!((UnityEngine.Object) this.m_KeySelection != (UnityEngine.Object) null))
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_KeySelection, undoLabel);
    }

    public void SaveCurve(AnimationWindowCurve curve)
    {
      this.SaveCurve(curve, "Edit Curve");
    }

    public void SaveCurve(AnimationWindowCurve curve, string undoLabel)
    {
      if (!curve.animationIsEditable)
        Debug.LogError((object) "Curve is not editable and shouldn't be saved.");
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) curve.clip, undoLabel);
      AnimationRecording.SaveModifiedCurve(curve, curve.clip);
      this.Repaint();
    }

    private void SaveSelectedKeys(string undoLabel)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      foreach (AnimationWindowState.LiveEditCurve liveEditCurve in this.m_LiveEditSnapshot)
      {
        if (liveEditCurve.curve.animationIsEditable)
        {
          if (!animationWindowCurveList.Contains(liveEditCurve.curve))
            animationWindowCurveList.Add(liveEditCurve.curve);
          List<AnimationWindowKeyframe> animationWindowKeyframeList = new List<AnimationWindowKeyframe>();
          foreach (AnimationWindowKeyframe keyframe in liveEditCurve.curve.m_Keyframes)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            AnimationWindowState.\u003CSaveSelectedKeys\u003Ec__AnonStorey1 keysCAnonStorey1 = new AnimationWindowState.\u003CSaveSelectedKeys\u003Ec__AnonStorey1();
            // ISSUE: reference to a compiler-generated field
            keysCAnonStorey1.other = keyframe;
            // ISSUE: reference to a compiler-generated field
            keysCAnonStorey1.\u0024this = this;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (!liveEditCurve.selectedKeys.Exists(new Predicate<AnimationWindowState.LiveEditKeyframe>(keysCAnonStorey1.\u003C\u003Em__0)) && liveEditCurve.selectedKeys.Exists(new Predicate<AnimationWindowState.LiveEditKeyframe>(keysCAnonStorey1.\u003C\u003Em__1)))
            {
              // ISSUE: reference to a compiler-generated field
              animationWindowKeyframeList.Add(keysCAnonStorey1.other);
            }
          }
          foreach (AnimationWindowKeyframe animationWindowKeyframe in animationWindowKeyframeList)
            liveEditCurve.curve.m_Keyframes.Remove(animationWindowKeyframe);
        }
      }
      foreach (AnimationWindowCurve curve in animationWindowCurveList)
        this.SaveCurve(curve, undoLabel);
    }

    public void RemoveCurve(AnimationWindowCurve curve, string undoLabel)
    {
      if (!curve.animationIsEditable)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) curve.clip, undoLabel);
      if (curve.isPPtrCurve)
        AnimationUtility.SetObjectReferenceCurve(curve.clip, curve.binding, (ObjectReferenceKeyframe[]) null);
      else
        AnimationUtility.SetEditorCurve(curve.clip, curve.binding, (AnimationCurve) null);
    }

    public bool previewing
    {
      get
      {
        return this.controlInterface.previewing;
      }
    }

    public bool canPreview
    {
      get
      {
        return this.controlInterface.canPreview;
      }
    }

    public void StartPreview()
    {
      this.controlInterface.StartPreview();
      this.controlInterface.ResampleAnimation();
    }

    public void StopPreview()
    {
      this.controlInterface.StopPreview();
    }

    public bool recording
    {
      get
      {
        return this.controlInterface.recording;
      }
    }

    public bool canRecord
    {
      get
      {
        return this.controlInterface.canRecord;
      }
    }

    public void StartRecording()
    {
      if (!((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null))
        return;
      this.controlInterface.StartRecording(this.selectedItem.sourceObject);
      this.controlInterface.ResampleAnimation();
    }

    public void StopRecording()
    {
      this.controlInterface.StopRecording();
    }

    public bool playing
    {
      get
      {
        return this.controlInterface.playing;
      }
    }

    public void StartPlayback()
    {
      this.controlInterface.StartPlayback();
    }

    public void StopPlayback()
    {
      this.controlInterface.StopPlayback();
    }

    public void ResampleAnimation()
    {
      this.controlInterface.ResampleAnimation();
    }

    public List<AnimationWindowCurve> allCurves
    {
      get
      {
        return this.selection.curves;
      }
    }

    public List<AnimationWindowCurve> activeCurves
    {
      get
      {
        if (this.m_ActiveCurvesCache == null)
        {
          this.m_ActiveCurvesCache = new List<AnimationWindowCurve>();
          if (this.hierarchyState != null && this.hierarchyData != null)
          {
            foreach (int selectedId in this.hierarchyState.selectedIDs)
            {
              AnimationWindowHierarchyNode windowHierarchyNode = this.hierarchyData.FindItem(selectedId) as AnimationWindowHierarchyNode;
              if (windowHierarchyNode != null)
              {
                AnimationWindowCurve[] curves = windowHierarchyNode.curves;
                if (curves != null)
                {
                  foreach (AnimationWindowCurve animationWindowCurve in curves)
                  {
                    if (!this.m_ActiveCurvesCache.Contains(animationWindowCurve))
                      this.m_ActiveCurvesCache.Add(animationWindowCurve);
                  }
                }
              }
            }
            this.m_ActiveCurvesCache.Sort();
          }
        }
        return this.m_ActiveCurvesCache;
      }
    }

    public CurveWrapper[] activeCurveWrappers
    {
      get
      {
        if (this.m_ActiveCurveWrappersCache == null || this.m_ActiveCurvesCache == null)
        {
          List<CurveWrapper> source = new List<CurveWrapper>();
          foreach (AnimationWindowCurve activeCurve in this.activeCurves)
          {
            if (!activeCurve.isDiscreteCurve)
              source.Add(AnimationWindowUtility.GetCurveWrapper(activeCurve, activeCurve.clip));
          }
          if (!source.Any<CurveWrapper>())
          {
            foreach (AnimationWindowCurve allCurve in this.allCurves)
            {
              if (!allCurve.isDiscreteCurve)
                source.Add(AnimationWindowUtility.GetCurveWrapper(allCurve, allCurve.clip));
            }
          }
          this.m_ActiveCurveWrappersCache = source.ToArray();
        }
        return this.m_ActiveCurveWrappersCache;
      }
    }

    public List<DopeLine> dopelines
    {
      get
      {
        if (this.m_dopelinesCache == null)
        {
          this.m_dopelinesCache = new List<DopeLine>();
          if (this.hierarchyData != null)
          {
            foreach (TreeViewItem row in (IEnumerable<TreeViewItem>) this.hierarchyData.GetRows())
            {
              AnimationWindowHierarchyNode node = row as AnimationWindowHierarchyNode;
              if (node != null && !(node is AnimationWindowHierarchyAddButtonNode))
              {
                AnimationWindowCurve[] curves = node.curves;
                if (curves != null)
                  this.m_dopelinesCache.Add(new DopeLine(row.id, curves)
                  {
                    tallMode = this.hierarchyState.GetTallMode(node),
                    objectType = node.animatableObjectType,
                    hasChildren = !(node is AnimationWindowHierarchyPropertyNode),
                    isMasterDopeline = row is AnimationWindowHierarchyMasterNode
                  });
              }
            }
          }
        }
        return this.m_dopelinesCache;
      }
    }

    public List<AnimationWindowHierarchyNode> selectedHierarchyNodes
    {
      get
      {
        List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
        if ((UnityEngine.Object) this.activeAnimationClip != (UnityEngine.Object) null && this.hierarchyData != null)
        {
          foreach (int selectedId in this.hierarchyState.selectedIDs)
          {
            AnimationWindowHierarchyNode windowHierarchyNode = (AnimationWindowHierarchyNode) this.hierarchyData.FindItem(selectedId);
            if (windowHierarchyNode != null && !(windowHierarchyNode is AnimationWindowHierarchyAddButtonNode))
              windowHierarchyNodeList.Add(windowHierarchyNode);
          }
        }
        return windowHierarchyNodeList;
      }
    }

    public AnimationWindowKeyframe activeKeyframe
    {
      get
      {
        if (this.m_ActiveKeyframeCache == null)
        {
          foreach (AnimationWindowCurve allCurve in this.allCurves)
          {
            foreach (AnimationWindowKeyframe keyframe in allCurve.m_Keyframes)
            {
              if (keyframe.GetHash() == this.m_ActiveKeyframeHash)
                this.m_ActiveKeyframeCache = keyframe;
            }
          }
        }
        return this.m_ActiveKeyframeCache;
      }
      set
      {
        this.m_ActiveKeyframeCache = (AnimationWindowKeyframe) null;
        this.m_ActiveKeyframeHash = value == null ? 0 : value.GetHash();
      }
    }

    public List<AnimationWindowKeyframe> selectedKeys
    {
      get
      {
        if (this.m_SelectedKeysCache == null)
        {
          this.m_SelectedKeysCache = new List<AnimationWindowKeyframe>();
          foreach (AnimationWindowCurve allCurve in this.allCurves)
          {
            foreach (AnimationWindowKeyframe keyframe in allCurve.m_Keyframes)
            {
              if (this.KeyIsSelected(keyframe))
                this.m_SelectedKeysCache.Add(keyframe);
            }
          }
        }
        return this.m_SelectedKeysCache;
      }
    }

    public Bounds selectionBounds
    {
      get
      {
        if (!this.m_SelectionBoundsCache.HasValue)
        {
          List<AnimationWindowKeyframe> selectedKeys = this.selectedKeys;
          if (selectedKeys.Count > 0)
          {
            AnimationWindowKeyframe animationWindowKeyframe1 = selectedKeys[0];
            Bounds bounds = new Bounds((Vector3) new Vector2(animationWindowKeyframe1.time + animationWindowKeyframe1.curve.timeOffset, !animationWindowKeyframe1.isPPtrCurve ? (float) animationWindowKeyframe1.value : 0.0f), (Vector3) Vector2.zero);
            for (int index = 1; index < selectedKeys.Count; ++index)
            {
              AnimationWindowKeyframe animationWindowKeyframe2 = selectedKeys[index];
              float x = animationWindowKeyframe2.time + animationWindowKeyframe2.curve.timeOffset;
              float y = !animationWindowKeyframe2.isPPtrCurve ? (float) animationWindowKeyframe2.value : 0.0f;
              bounds.Encapsulate((Vector3) new Vector2(x, y));
            }
            this.m_SelectionBoundsCache = new Bounds?(bounds);
          }
          else
            this.m_SelectionBoundsCache = new Bounds?(new Bounds((Vector3) Vector2.zero, (Vector3) Vector2.zero));
        }
        return this.m_SelectionBoundsCache.Value;
      }
    }

    private HashSet<int> selectedKeyHashes
    {
      get
      {
        if ((UnityEngine.Object) this.m_KeySelection == (UnityEngine.Object) null)
        {
          this.m_KeySelection = ScriptableObject.CreateInstance<AnimationWindowKeySelection>();
          this.m_KeySelection.hideFlags = HideFlags.HideAndDontSave;
        }
        return this.m_KeySelection.selectedKeyHashes;
      }
      set
      {
        if ((UnityEngine.Object) this.m_KeySelection == (UnityEngine.Object) null)
        {
          this.m_KeySelection = ScriptableObject.CreateInstance<AnimationWindowKeySelection>();
          this.m_KeySelection.hideFlags = HideFlags.HideAndDontSave;
        }
        this.m_KeySelection.selectedKeyHashes = value;
      }
    }

    public bool AnyKeyIsSelected(DopeLine dopeline)
    {
      foreach (AnimationWindowKeyframe key in dopeline.keys)
      {
        if (this.KeyIsSelected(key))
          return true;
      }
      return false;
    }

    public bool KeyIsSelected(AnimationWindowKeyframe keyframe)
    {
      return this.selectedKeyHashes.Contains(keyframe.GetHash());
    }

    public void SelectKey(AnimationWindowKeyframe keyframe)
    {
      int hash = keyframe.GetHash();
      if (!this.selectedKeyHashes.Contains(hash))
        this.selectedKeyHashes.Add(hash);
      this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
      this.m_SelectionBoundsCache = new Bounds?();
    }

    public void SelectKeysFromDopeline(DopeLine dopeline)
    {
      if (dopeline == null)
        return;
      foreach (AnimationWindowKeyframe key in dopeline.keys)
        this.SelectKey(key);
    }

    public void UnselectKey(AnimationWindowKeyframe keyframe)
    {
      int hash = keyframe.GetHash();
      if (this.selectedKeyHashes.Contains(hash))
        this.selectedKeyHashes.Remove(hash);
      this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
      this.m_SelectionBoundsCache = new Bounds?();
    }

    public void UnselectKeysFromDopeline(DopeLine dopeline)
    {
      if (dopeline == null)
        return;
      foreach (AnimationWindowKeyframe key in dopeline.keys)
        this.UnselectKey(key);
    }

    public void DeleteSelectedKeys()
    {
      if (this.selectedKeys.Count == 0)
        return;
      this.DeleteKeys(this.selectedKeys);
    }

    public void DeleteKeys(List<AnimationWindowKeyframe> keys)
    {
      this.SaveKeySelection("Edit Curve");
      foreach (AnimationWindowKeyframe key in keys)
      {
        if (key.curve.animationIsEditable)
        {
          this.UnselectKey(key);
          key.curve.m_Keyframes.Remove(key);
          this.SaveCurve(key.curve, "Edit Curve");
        }
      }
      this.ResampleAnimation();
    }

    public void StartLiveEdit()
    {
      if (this.onStartLiveEdit != null)
        this.onStartLiveEdit();
      this.m_LiveEditSnapshot = new List<AnimationWindowState.LiveEditCurve>();
      this.SaveKeySelection("Edit Curve");
      foreach (AnimationWindowKeyframe selectedKey in this.selectedKeys)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowState.\u003CStartLiveEdit\u003Ec__AnonStorey2 editCAnonStorey2 = new AnimationWindowState.\u003CStartLiveEdit\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        editCAnonStorey2.selectedKey = selectedKey;
        // ISSUE: reference to a compiler-generated method
        if (!this.m_LiveEditSnapshot.Exists(new Predicate<AnimationWindowState.LiveEditCurve>(editCAnonStorey2.\u003C\u003Em__0)))
        {
          AnimationWindowState.LiveEditCurve liveEditCurve = new AnimationWindowState.LiveEditCurve();
          // ISSUE: reference to a compiler-generated field
          liveEditCurve.curve = editCAnonStorey2.selectedKey.curve;
          // ISSUE: reference to a compiler-generated field
          foreach (AnimationWindowKeyframe keyframe in editCAnonStorey2.selectedKey.curve.m_Keyframes)
          {
            AnimationWindowState.LiveEditKeyframe liveEditKeyframe = new AnimationWindowState.LiveEditKeyframe();
            liveEditKeyframe.keySnapshot = new AnimationWindowKeyframe(keyframe);
            liveEditKeyframe.key = keyframe;
            if (this.KeyIsSelected(keyframe))
              liveEditCurve.selectedKeys.Add(liveEditKeyframe);
            else
              liveEditCurve.unselectedKeys.Add(liveEditKeyframe);
          }
          this.m_LiveEditSnapshot.Add(liveEditCurve);
        }
      }
    }

    public void EndLiveEdit()
    {
      this.SaveSelectedKeys("Edit Curve");
      this.m_LiveEditSnapshot = (List<AnimationWindowState.LiveEditCurve>) null;
      if (this.onEndLiveEdit == null)
        return;
      this.onEndLiveEdit();
    }

    public bool InLiveEdit()
    {
      return this.m_LiveEditSnapshot != null;
    }

    public void MoveSelectedKeys(float deltaTime, bool snapToFrame)
    {
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.ClearKeySelections();
      foreach (AnimationWindowState.LiveEditCurve liveEditCurve in this.m_LiveEditSnapshot)
      {
        foreach (AnimationWindowState.LiveEditKeyframe selectedKey in liveEditCurve.selectedKeys)
        {
          if (liveEditCurve.curve.animationIsEditable)
          {
            selectedKey.key.time = Mathf.Max(selectedKey.keySnapshot.time + deltaTime, 0.0f);
            if (snapToFrame)
              selectedKey.key.time = this.SnapToFrame(selectedKey.key.time, liveEditCurve.curve.clip.frameRate);
          }
          this.SelectKey(selectedKey.key);
        }
      }
      if (flag)
        return;
      this.EndLiveEdit();
    }

    public void TransformSelectedKeys(Matrix4x4 matrix, bool flipX, bool flipY, bool snapToFrame)
    {
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.ClearKeySelections();
      foreach (AnimationWindowState.LiveEditCurve liveEditCurve in this.m_LiveEditSnapshot)
      {
        foreach (AnimationWindowState.LiveEditKeyframe selectedKey in liveEditCurve.selectedKeys)
        {
          if (liveEditCurve.curve.animationIsEditable)
          {
            Vector3 point = new Vector3(selectedKey.keySnapshot.time, !selectedKey.keySnapshot.isPPtrCurve ? (float) selectedKey.keySnapshot.value : 0.0f, 0.0f);
            point = matrix.MultiplyPoint3x4(point);
            selectedKey.key.time = Mathf.Max(!snapToFrame ? point.x : this.SnapToFrame(point.x, liveEditCurve.curve.clip.frameRate), 0.0f);
            if (flipX)
            {
              selectedKey.key.inTangent = (double) selectedKey.keySnapshot.outTangent == double.PositiveInfinity ? float.PositiveInfinity : -selectedKey.keySnapshot.outTangent;
              selectedKey.key.outTangent = (double) selectedKey.keySnapshot.inTangent == double.PositiveInfinity ? float.PositiveInfinity : -selectedKey.keySnapshot.inTangent;
            }
            if (!selectedKey.key.isPPtrCurve)
            {
              selectedKey.key.value = (object) point.y;
              if (flipY)
              {
                selectedKey.key.inTangent = (double) selectedKey.key.inTangent == double.PositiveInfinity ? float.PositiveInfinity : -selectedKey.key.inTangent;
                selectedKey.key.outTangent = (double) selectedKey.key.outTangent == double.PositiveInfinity ? float.PositiveInfinity : -selectedKey.key.outTangent;
              }
            }
          }
          this.SelectKey(selectedKey.key);
        }
      }
      if (flag)
        return;
      this.EndLiveEdit();
    }

    public void TransformRippleKeys(Matrix4x4 matrix, float t1, float t2, bool flipX, bool snapToFrame)
    {
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.ClearKeySelections();
      foreach (AnimationWindowState.LiveEditCurve liveEditCurve in this.m_LiveEditSnapshot)
      {
        foreach (AnimationWindowState.LiveEditKeyframe selectedKey in liveEditCurve.selectedKeys)
        {
          if (liveEditCurve.curve.animationIsEditable)
          {
            Vector3 point = new Vector3(selectedKey.keySnapshot.time, 0.0f, 0.0f);
            point = matrix.MultiplyPoint3x4(point);
            selectedKey.key.time = Mathf.Max(!snapToFrame ? point.x : this.SnapToFrame(point.x, liveEditCurve.curve.clip.frameRate), 0.0f);
            if (flipX)
            {
              selectedKey.key.inTangent = (double) selectedKey.keySnapshot.outTangent == double.PositiveInfinity ? float.PositiveInfinity : -selectedKey.keySnapshot.outTangent;
              selectedKey.key.outTangent = (double) selectedKey.keySnapshot.inTangent == double.PositiveInfinity ? float.PositiveInfinity : -selectedKey.keySnapshot.inTangent;
            }
          }
          this.SelectKey(selectedKey.key);
        }
        if (liveEditCurve.curve.animationIsEditable)
        {
          foreach (AnimationWindowState.LiveEditKeyframe unselectedKey in liveEditCurve.unselectedKeys)
          {
            if ((double) unselectedKey.keySnapshot.time > (double) t2)
            {
              Vector3 point = new Vector3(!flipX ? t2 : t1, 0.0f, 0.0f);
              point = matrix.MultiplyPoint3x4(point);
              float num = point.x - t2;
              if ((double) num > 0.0)
              {
                float time = unselectedKey.keySnapshot.time + num;
                unselectedKey.key.time = Mathf.Max(!snapToFrame ? time : this.SnapToFrame(time, liveEditCurve.curve.clip.frameRate), 0.0f);
              }
              else
                unselectedKey.key.time = unselectedKey.keySnapshot.time;
            }
            else if ((double) unselectedKey.keySnapshot.time < (double) t1)
            {
              Vector3 point = new Vector3(!flipX ? t1 : t2, 0.0f, 0.0f);
              point = matrix.MultiplyPoint3x4(point);
              float num = point.x - t1;
              if ((double) num < 0.0)
              {
                float time = unselectedKey.keySnapshot.time + num;
                unselectedKey.key.time = Mathf.Max(!snapToFrame ? time : this.SnapToFrame(time, liveEditCurve.curve.clip.frameRate), 0.0f);
              }
              else
                unselectedKey.key.time = unselectedKey.keySnapshot.time;
            }
          }
        }
      }
      if (flag)
        return;
      this.EndLiveEdit();
    }

    public void CopyKeys()
    {
      if (AnimationWindowState.s_KeyframeClipboard == null)
        AnimationWindowState.s_KeyframeClipboard = new List<AnimationWindowKeyframe>();
      float num1 = float.MaxValue;
      AnimationWindowState.s_KeyframeClipboard.Clear();
      foreach (AnimationWindowKeyframe selectedKey in this.selectedKeys)
      {
        AnimationWindowState.s_KeyframeClipboard.Add(new AnimationWindowKeyframe(selectedKey));
        float num2 = selectedKey.time + selectedKey.curve.timeOffset;
        if ((double) num2 < (double) num1)
          num1 = num2;
      }
      if (AnimationWindowState.s_KeyframeClipboard.Count > 0)
      {
        foreach (AnimationWindowKeyframe animationWindowKeyframe in AnimationWindowState.s_KeyframeClipboard)
          animationWindowKeyframe.time -= num1 - animationWindowKeyframe.curve.timeOffset;
      }
      else
        this.CopyAllActiveCurves();
    }

    public void CopyAllActiveCurves()
    {
      foreach (AnimationWindowCurve activeCurve in this.activeCurves)
      {
        foreach (AnimationWindowKeyframe keyframe in activeCurve.m_Keyframes)
          AnimationWindowState.s_KeyframeClipboard.Add(new AnimationWindowKeyframe(keyframe));
      }
    }

    public void PasteKeys()
    {
      if (AnimationWindowState.s_KeyframeClipboard == null)
        AnimationWindowState.s_KeyframeClipboard = new List<AnimationWindowKeyframe>();
      this.SaveKeySelection("Edit Curve");
      HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this.selectedKeyHashes);
      this.ClearKeySelections();
      AnimationWindowCurve animationWindowCurve1 = (AnimationWindowCurve) null;
      AnimationWindowCurve animationWindowCurve2 = (AnimationWindowCurve) null;
      float startTime = 0.0f;
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      foreach (AnimationWindowKeyframe animationWindowKeyframe in AnimationWindowState.s_KeyframeClipboard)
      {
        if (!animationWindowCurveList.Any<AnimationWindowCurve>() || animationWindowCurveList.Last<AnimationWindowCurve>() != animationWindowKeyframe.curve)
          animationWindowCurveList.Add(animationWindowKeyframe.curve);
      }
      bool flag = animationWindowCurveList.Count<AnimationWindowCurve>() == this.activeCurves.Count<AnimationWindowCurve>();
      int index = 0;
      foreach (AnimationWindowKeyframe key in AnimationWindowState.s_KeyframeClipboard)
      {
        if (animationWindowCurve2 != null && key.curve != animationWindowCurve2)
          ++index;
        AnimationWindowKeyframe keyframe = new AnimationWindowKeyframe(key);
        keyframe.curve = !flag ? AnimationWindowUtility.BestMatchForPaste(keyframe.curve.binding, animationWindowCurveList, this.activeCurves) : this.activeCurves[index];
        if (keyframe.curve == null)
        {
          if (this.activeCurves.Count > 0)
          {
            AnimationWindowCurve activeCurve = this.activeCurves[0];
            if (activeCurve.animationIsEditable)
            {
              keyframe.curve = new AnimationWindowCurve(activeCurve.clip, key.curve.binding, key.curve.valueType);
              keyframe.curve.selectionBinding = activeCurve.selectionBinding;
              keyframe.time = key.time;
            }
          }
          else
          {
            AnimationWindowSelectionItem windowSelectionItem = this.selection.First();
            if (windowSelectionItem.animationIsEditable)
            {
              keyframe.curve = new AnimationWindowCurve(windowSelectionItem.animationClip, key.curve.binding, key.curve.valueType);
              keyframe.curve.selectionBinding = windowSelectionItem;
              keyframe.time = key.time;
            }
          }
        }
        if (keyframe.curve != null && keyframe.curve.animationIsEditable)
        {
          keyframe.time += this.time.time - keyframe.curve.timeOffset;
          if ((double) keyframe.time >= 0.0 && keyframe.curve != null && keyframe.curve.isPPtrCurve == key.curve.isPPtrCurve)
          {
            if (keyframe.curve.HasKeyframe(AnimationKeyTime.Time(keyframe.time, keyframe.curve.clip.frameRate)))
              keyframe.curve.RemoveKeyframe(AnimationKeyTime.Time(keyframe.time, keyframe.curve.clip.frameRate));
            if (animationWindowCurve1 == keyframe.curve)
              keyframe.curve.RemoveKeysAtRange(startTime, keyframe.time);
            keyframe.curve.m_Keyframes.Add(keyframe);
            this.SelectKey(keyframe);
            this.SaveCurve(keyframe.curve, "Edit Curve");
            animationWindowCurve1 = keyframe.curve;
            startTime = keyframe.time;
          }
          animationWindowCurve2 = key.curve;
        }
      }
      if (this.selectedKeyHashes.Count == 0)
        this.selectedKeyHashes = intSet;
      else
        this.ResampleAnimation();
    }

    public void ClearSelections()
    {
      this.ClearKeySelections();
      this.ClearHierarchySelection();
    }

    public void ClearKeySelections()
    {
      this.selectedKeyHashes.Clear();
      this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
      this.m_SelectionBoundsCache = new Bounds?();
    }

    public void ClearHierarchySelection()
    {
      this.hierarchyState.selectedIDs.Clear();
      this.m_ActiveCurvesCache = (List<AnimationWindowCurve>) null;
    }

    private void ClearCurveWrapperCache()
    {
      if (this.m_ActiveCurveWrappersCache == null)
        return;
      for (int index = 0; index < this.m_ActiveCurveWrappersCache.Length; ++index)
      {
        CurveWrapper curveWrapper = this.m_ActiveCurveWrappersCache[index];
        if (curveWrapper.renderer != null)
          curveWrapper.renderer.FlushCache();
      }
      this.m_ActiveCurveWrappersCache = (CurveWrapper[]) null;
    }

    private void ReloadModifiedDopelineCache()
    {
      if (this.m_dopelinesCache == null)
        return;
      for (int index = 0; index < this.m_dopelinesCache.Count; ++index)
      {
        DopeLine dopeLine = this.m_dopelinesCache[index];
        foreach (object curve in dopeLine.curves)
        {
          if (this.m_ModifiedCurves.Contains(curve.GetHashCode()))
          {
            dopeLine.InvalidateKeyframes();
            break;
          }
        }
      }
    }

    private void ReloadModifiedCurveWrapperCache()
    {
      if (this.m_ActiveCurveWrappersCache == null)
        return;
      Dictionary<int, AnimationWindowCurve> source = new Dictionary<int, AnimationWindowCurve>();
      for (int index = 0; index < this.m_ActiveCurveWrappersCache.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowState.\u003CReloadModifiedCurveWrapperCache\u003Ec__AnonStorey3 cacheCAnonStorey3 = new AnimationWindowState.\u003CReloadModifiedCurveWrapperCache\u003Ec__AnonStorey3();
        // ISSUE: reference to a compiler-generated field
        cacheCAnonStorey3.curveWrapper = this.m_ActiveCurveWrappersCache[index];
        // ISSUE: reference to a compiler-generated field
        if (this.m_ModifiedCurves.Contains(cacheCAnonStorey3.curveWrapper.id))
        {
          // ISSUE: reference to a compiler-generated method
          AnimationWindowCurve animationWindowCurve = this.allCurves.Find(new Predicate<AnimationWindowCurve>(cacheCAnonStorey3.\u003C\u003Em__0));
          if (animationWindowCurve != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) animationWindowCurve.clip.startTime != (double) cacheCAnonStorey3.curveWrapper.renderer.RangeStart() || (double) animationWindowCurve.clip.stopTime != (double) cacheCAnonStorey3.curveWrapper.renderer.RangeEnd())
            {
              this.ClearCurveWrapperCache();
              return;
            }
            source[index] = animationWindowCurve;
          }
        }
      }
      for (int index = 0; index < source.Count; ++index)
      {
        KeyValuePair<int, AnimationWindowCurve> keyValuePair = source.ElementAt<KeyValuePair<int, AnimationWindowCurve>>(index);
        CurveWrapper curveWrapper = this.m_ActiveCurveWrappersCache[keyValuePair.Key];
        if (curveWrapper.renderer != null)
          curveWrapper.renderer.FlushCache();
        this.m_ActiveCurveWrappersCache[keyValuePair.Key] = AnimationWindowUtility.GetCurveWrapper(keyValuePair.Value, keyValuePair.Value.clip);
      }
    }

    private void ReloadModifiedAnimationCurveCache()
    {
      for (int index = 0; index < this.allCurves.Count; ++index)
      {
        AnimationWindowCurve allCurve = this.allCurves[index];
        if (this.m_ModifiedCurves.Contains(allCurve.GetHashCode()))
          allCurve.LoadKeyframes(allCurve.clip);
      }
    }

    private void OnNewCurveAdded(EditorCurveBinding newCurve)
    {
      string propertyName = newCurve.propertyName;
      string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(newCurve.propertyName);
      if (this.hierarchyData == null)
        return;
      if (this.HasHierarchySelection())
      {
        foreach (AnimationWindowHierarchyNode row in (IEnumerable<TreeViewItem>) this.hierarchyData.GetRows())
        {
          if (!(row.path != newCurve.path) && row.animatableObjectType == newCurve.type && (!(row.propertyName != propertyName) || !(row.propertyName != propertyGroupName)))
          {
            this.SelectHierarchyItem(row.id, true, false);
            if (newCurve.isPPtrCurve)
              this.hierarchyState.AddTallInstance(row.id);
          }
        }
      }
      this.controlInterface.ResampleAnimation();
      this.m_lastAddedCurveBinding = new EditorCurveBinding?();
    }

    public void Repaint()
    {
      if (!((UnityEngine.Object) this.animEditor != (UnityEngine.Object) null))
        return;
      this.animEditor.Repaint();
    }

    public List<AnimationWindowKeyframe> GetAggregateKeys(AnimationWindowHierarchyNode hierarchyNode)
    {
      DopeLine dopeLine = this.dopelines.FirstOrDefault<DopeLine>((Func<DopeLine, bool>) (e => e.hierarchyNodeID == hierarchyNode.id));
      if (dopeLine == null)
        return (List<AnimationWindowKeyframe>) null;
      return dopeLine.keys;
    }

    public void OnHierarchySelectionChanged(int[] selectedInstanceIDs)
    {
      this.HandleHierarchySelectionChanged(selectedInstanceIDs, true);
    }

    public void HandleHierarchySelectionChanged(int[] selectedInstanceIDs, bool triggerSceneSelectionSync)
    {
      this.m_ActiveCurvesCache = (List<AnimationWindowCurve>) null;
      if (!triggerSceneSelectionSync)
        return;
      this.SyncSceneSelection(selectedInstanceIDs);
    }

    public void SelectHierarchyItem(DopeLine dopeline, bool additive)
    {
      this.SelectHierarchyItem(dopeline.hierarchyNodeID, additive, true);
    }

    public void SelectHierarchyItem(DopeLine dopeline, bool additive, bool triggerSceneSelectionSync)
    {
      this.SelectHierarchyItem(dopeline.hierarchyNodeID, additive, triggerSceneSelectionSync);
    }

    public void SelectHierarchyItem(int hierarchyNodeID, bool additive, bool triggerSceneSelectionSync)
    {
      if (!additive)
        this.ClearHierarchySelection();
      this.hierarchyState.selectedIDs.Add(hierarchyNodeID);
      this.HandleHierarchySelectionChanged(this.hierarchyState.selectedIDs.ToArray(), triggerSceneSelectionSync);
    }

    public void UnSelectHierarchyItem(DopeLine dopeline)
    {
      this.UnSelectHierarchyItem(dopeline.hierarchyNodeID);
    }

    public void UnSelectHierarchyItem(int hierarchyNodeID)
    {
      this.hierarchyState.selectedIDs.Remove(hierarchyNodeID);
    }

    public bool HasHierarchySelection()
    {
      if (this.hierarchyState.selectedIDs.Count == 0)
        return false;
      if (this.hierarchyState.selectedIDs.Count == 1)
        return this.hierarchyState.selectedIDs[0] != 0;
      return true;
    }

    public List<int> GetAffectedHierarchyIDs(List<AnimationWindowKeyframe> keyframes)
    {
      List<int> intList = new List<int>();
      foreach (DopeLine affectedDopeline in this.GetAffectedDopelines(keyframes))
      {
        if (!intList.Contains(affectedDopeline.hierarchyNodeID))
          intList.Add(affectedDopeline.hierarchyNodeID);
      }
      return intList;
    }

    public List<DopeLine> GetAffectedDopelines(List<AnimationWindowKeyframe> keyframes)
    {
      List<DopeLine> dopeLineList = new List<DopeLine>();
      foreach (AnimationWindowCurve affectedCurve in this.GetAffectedCurves(keyframes))
      {
        foreach (DopeLine dopeline in this.dopelines)
        {
          if (!dopeLineList.Contains(dopeline) && ((IEnumerable<AnimationWindowCurve>) dopeline.curves).Contains<AnimationWindowCurve>(affectedCurve))
            dopeLineList.Add(dopeline);
        }
      }
      return dopeLineList;
    }

    public List<AnimationWindowCurve> GetAffectedCurves(List<AnimationWindowKeyframe> keyframes)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      foreach (AnimationWindowKeyframe keyframe in keyframes)
      {
        if (!animationWindowCurveList.Contains(keyframe.curve))
          animationWindowCurveList.Add(keyframe.curve);
      }
      return animationWindowCurveList;
    }

    public DopeLine GetDopeline(int selectedInstanceID)
    {
      foreach (DopeLine dopeline in this.dopelines)
      {
        if (dopeline.hierarchyNodeID == selectedInstanceID)
          return dopeline;
      }
      return (DopeLine) null;
    }

    private void SyncSceneSelection(int[] selectedNodeIDs)
    {
      if ((UnityEngine.Object) this.selectedItem == (UnityEngine.Object) null || !this.selectedItem.canSyncSceneSelection)
        return;
      GameObject rootGameObject = this.selectedItem.rootGameObject;
      if ((UnityEngine.Object) rootGameObject == (UnityEngine.Object) null)
        return;
      List<int> intList = new List<int>();
      foreach (int selectedNodeId in selectedNodeIDs)
      {
        AnimationWindowHierarchyNode windowHierarchyNode = this.hierarchyData.FindItem(selectedNodeId) as AnimationWindowHierarchyNode;
        if (windowHierarchyNode != null && !(windowHierarchyNode is AnimationWindowHierarchyMasterNode))
        {
          Transform tr = rootGameObject.transform.Find(windowHierarchyNode.path);
          if ((UnityEngine.Object) tr != (UnityEngine.Object) null && (UnityEngine.Object) rootGameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.activeAnimationPlayer == (UnityEngine.Object) AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(tr))
            intList.Add(tr.gameObject.GetInstanceID());
        }
      }
      if (intList.Count > 0)
        Selection.instanceIDs = intList.ToArray();
      else
        Selection.activeGameObject = rootGameObject;
    }

    public float clipFrameRate
    {
      get
      {
        if ((UnityEngine.Object) this.activeAnimationClip == (UnityEngine.Object) null)
          return 60f;
        return this.activeAnimationClip.frameRate;
      }
      set
      {
        if (!((UnityEngine.Object) this.activeAnimationClip != (UnityEngine.Object) null) || (double) value <= 0.0 || (double) value > 10000.0)
          return;
        this.ClearKeySelections();
        this.SaveKeySelection("Edit Curve");
        foreach (AnimationWindowCurve allCurve in this.allCurves)
        {
          foreach (AnimationWindowKeyframe keyframe in allCurve.m_Keyframes)
          {
            int frame = AnimationKeyTime.Time(keyframe.time, this.clipFrameRate).frame;
            keyframe.time = AnimationKeyTime.Frame(frame, value).time;
          }
          this.SaveCurve(allCurve, "Edit Curve");
        }
        AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents(this.activeAnimationClip);
        foreach (AnimationEvent animationEvent in animationEvents)
        {
          int frame = AnimationKeyTime.Time(animationEvent.time, this.clipFrameRate).frame;
          animationEvent.time = AnimationKeyTime.Frame(frame, value).time;
        }
        AnimationUtility.SetAnimationEvents(this.activeAnimationClip, animationEvents);
        this.activeAnimationClip.frameRate = value;
      }
    }

    public float frameRate
    {
      get
      {
        return this.m_FrameRate;
      }
      set
      {
        if ((double) this.m_FrameRate == (double) value)
          return;
        this.m_FrameRate = value;
        this.onFrameRateChange(this.m_FrameRate);
      }
    }

    public AnimationKeyTime time
    {
      get
      {
        return this.controlInterface.time;
      }
    }

    public int currentFrame
    {
      get
      {
        return this.time.frame;
      }
      set
      {
        this.controlInterface.GoToFrame(value);
      }
    }

    public float currentTime
    {
      get
      {
        return this.time.time;
      }
      set
      {
        this.controlInterface.GoToTime(value);
      }
    }

    public TimeArea.TimeFormat timeFormat
    {
      get
      {
        return this.m_TimeFormat;
      }
      set
      {
        this.m_TimeFormat = value;
      }
    }

    public TimeArea timeArea
    {
      get
      {
        return this.m_TimeArea;
      }
      set
      {
        this.m_TimeArea = value;
      }
    }

    public float pixelPerSecond
    {
      get
      {
        return this.timeArea.m_Scale.x;
      }
    }

    public float zeroTimePixel
    {
      get
      {
        return (float) ((double) this.timeArea.shownArea.xMin * (double) this.timeArea.m_Scale.x * -1.0);
      }
    }

    public float PixelToTime(float pixel)
    {
      return this.PixelToTime(pixel, AnimationWindowState.SnapMode.Disabled);
    }

    public float PixelToTime(float pixel, AnimationWindowState.SnapMode snap)
    {
      return this.SnapToFrame((pixel - this.zeroTimePixel) / this.pixelPerSecond, snap);
    }

    public float TimeToPixel(float time)
    {
      return this.TimeToPixel(time, AnimationWindowState.SnapMode.Disabled);
    }

    public float TimeToPixel(float time, AnimationWindowState.SnapMode snap)
    {
      return this.SnapToFrame(time, snap) * this.pixelPerSecond + this.zeroTimePixel;
    }

    public float SnapToFrame(float time, AnimationWindowState.SnapMode snap)
    {
      double num;
      switch (snap)
      {
        case AnimationWindowState.SnapMode.Disabled:
          return time;
        case AnimationWindowState.SnapMode.SnapToFrame:
          num = (double) this.frameRate;
          break;
        default:
          num = (double) this.clipFrameRate;
          break;
      }
      float fps = (float) num;
      return this.SnapToFrame(time, fps);
    }

    public float SnapToFrame(float time, float fps)
    {
      return Mathf.Round(time * fps) / fps;
    }

    public float minVisibleTime
    {
      get
      {
        return this.m_TimeArea.shownArea.xMin;
      }
    }

    public float maxVisibleTime
    {
      get
      {
        return this.m_TimeArea.shownArea.xMax;
      }
    }

    public float visibleTimeSpan
    {
      get
      {
        return this.maxVisibleTime - this.minVisibleTime;
      }
    }

    public float minVisibleFrame
    {
      get
      {
        return this.minVisibleTime * this.frameRate;
      }
    }

    public float maxVisibleFrame
    {
      get
      {
        return this.maxVisibleTime * this.frameRate;
      }
    }

    public float visibleFrameSpan
    {
      get
      {
        return this.visibleTimeSpan * this.frameRate;
      }
    }

    public float minTime
    {
      get
      {
        return this.timeRange.x;
      }
    }

    public float maxTime
    {
      get
      {
        return this.timeRange.y;
      }
    }

    public Vector2 timeRange
    {
      get
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        if (this.selection.count > 0)
        {
          num1 = float.MaxValue;
          num2 = float.MinValue;
          foreach (AnimationWindowSelectionItem windowSelectionItem in this.selection.ToArray())
          {
            num1 = Mathf.Min(num1, windowSelectionItem.animationClip.startTime + windowSelectionItem.timeOffset);
            num2 = Mathf.Max(num2, windowSelectionItem.animationClip.stopTime + windowSelectionItem.timeOffset);
          }
        }
        return new Vector2(num1, num2);
      }
    }

    public string FormatFrame(int frame, int frameDigits)
    {
      return (frame / (int) this.frameRate).ToString() + ":" + ((float) frame % this.frameRate).ToString().PadLeft(frameDigits, '0');
    }

    public float TimeToFrame(float time)
    {
      return time * this.frameRate;
    }

    public float FrameToTime(float frame)
    {
      return frame / this.frameRate;
    }

    public int TimeToFrameFloor(float time)
    {
      return Mathf.FloorToInt(this.TimeToFrame(time));
    }

    public int TimeToFrameRound(float time)
    {
      return Mathf.RoundToInt(this.TimeToFrame(time));
    }

    public float FrameToPixel(float i, Rect rect)
    {
      return (i - this.minVisibleFrame) * rect.width / this.visibleFrameSpan;
    }

    public float FrameDeltaToPixel(Rect rect)
    {
      return rect.width / this.visibleFrameSpan;
    }

    public float TimeToPixel(float time, Rect rect)
    {
      return this.FrameToPixel(time * this.frameRate, rect);
    }

    public float PixelToTime(float pixelX, Rect rect)
    {
      return pixelX * this.visibleTimeSpan / rect.width + this.minVisibleTime;
    }

    public float PixelDeltaToTime(Rect rect)
    {
      return this.visibleTimeSpan / rect.width;
    }

    public enum RefreshType
    {
      None,
      CurvesOnly,
      Everything,
    }

    public enum SnapMode
    {
      Disabled,
      SnapToFrame,
      SnapToClipFrame,
    }

    private struct LiveEditKeyframe
    {
      public AnimationWindowKeyframe keySnapshot;
      public AnimationWindowKeyframe key;
    }

    private class LiveEditCurve
    {
      public List<AnimationWindowState.LiveEditKeyframe> selectedKeys = new List<AnimationWindowState.LiveEditKeyframe>();
      public List<AnimationWindowState.LiveEditKeyframe> unselectedKeys = new List<AnimationWindowState.LiveEditKeyframe>();
      public AnimationWindowCurve curve;
    }
  }
}
