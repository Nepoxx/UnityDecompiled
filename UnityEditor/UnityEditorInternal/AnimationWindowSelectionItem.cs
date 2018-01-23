// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowSelectionItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowSelectionItem : ScriptableObject, IEquatable<AnimationWindowSelectionItem>, ISelectionBinding
  {
    private List<AnimationWindowCurve> m_CurvesCache = (List<AnimationWindowCurve>) null;
    [SerializeField]
    private float m_TimeOffset;
    [SerializeField]
    private int m_Id;
    [SerializeField]
    private GameObject m_GameObject;
    [SerializeField]
    private ScriptableObject m_ScriptableObject;
    [SerializeField]
    private AnimationClip m_AnimationClip;

    public virtual float timeOffset
    {
      get
      {
        return this.m_TimeOffset;
      }
      set
      {
        this.m_TimeOffset = value;
      }
    }

    public virtual int id
    {
      get
      {
        return this.m_Id;
      }
      set
      {
        this.m_Id = value;
      }
    }

    public virtual GameObject gameObject
    {
      get
      {
        return this.m_GameObject;
      }
      set
      {
        this.m_GameObject = value;
      }
    }

    public virtual ScriptableObject scriptableObject
    {
      get
      {
        return this.m_ScriptableObject;
      }
      set
      {
        this.m_ScriptableObject = value;
      }
    }

    public virtual UnityEngine.Object sourceObject
    {
      get
      {
        return !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) ? (UnityEngine.Object) this.scriptableObject : (UnityEngine.Object) this.gameObject;
      }
    }

    public virtual AnimationClip animationClip
    {
      get
      {
        return this.m_AnimationClip;
      }
      set
      {
        this.m_AnimationClip = value;
      }
    }

    public virtual GameObject rootGameObject
    {
      get
      {
        Component animationPlayer = this.animationPlayer;
        if ((UnityEngine.Object) animationPlayer != (UnityEngine.Object) null)
          return animationPlayer.gameObject;
        return (GameObject) null;
      }
    }

    public virtual Component animationPlayer
    {
      get
      {
        if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
          return AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(this.gameObject.transform);
        return (Component) null;
      }
    }

    public virtual bool animationIsEditable
    {
      get
      {
        return (!(bool) ((UnityEngine.Object) this.animationClip) || (this.animationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None) && !this.objectIsPrefab;
      }
    }

    public virtual bool clipIsEditable
    {
      get
      {
        return (bool) ((UnityEngine.Object) this.animationClip) && (this.animationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None && AssetDatabase.IsOpenForEdit((UnityEngine.Object) this.animationClip, StatusQueryOptions.UseCachedIfPossible);
      }
    }

    public virtual bool objectIsPrefab
    {
      get
      {
        return (bool) ((UnityEngine.Object) this.gameObject) && (EditorUtility.IsPersistent((UnityEngine.Object) this.gameObject) || (this.gameObject.hideFlags & HideFlags.NotEditable) != HideFlags.None);
      }
    }

    public virtual bool objectIsOptimized
    {
      get
      {
        Animator animationPlayer = this.animationPlayer as Animator;
        if ((UnityEngine.Object) animationPlayer == (UnityEngine.Object) null)
          return false;
        return animationPlayer.isOptimizable && !animationPlayer.hasTransformHierarchy;
      }
    }

    public virtual bool canPreview
    {
      get
      {
        if ((UnityEngine.Object) this.rootGameObject != (UnityEngine.Object) null)
          return !this.objectIsOptimized;
        return false;
      }
    }

    public virtual bool canRecord
    {
      get
      {
        if (!this.animationIsEditable)
          return false;
        return this.canPreview;
      }
    }

    public virtual bool canChangeAnimationClip
    {
      get
      {
        return (UnityEngine.Object) this.rootGameObject != (UnityEngine.Object) null;
      }
    }

    public virtual bool canAddCurves
    {
      get
      {
        if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
          return !this.objectIsPrefab && this.clipIsEditable;
        return (UnityEngine.Object) this.scriptableObject != (UnityEngine.Object) null;
      }
    }

    public virtual bool canSyncSceneSelection
    {
      get
      {
        return true;
      }
    }

    public List<AnimationWindowCurve> curves
    {
      get
      {
        if (this.m_CurvesCache == null)
        {
          this.m_CurvesCache = new List<AnimationWindowCurve>();
          if ((UnityEngine.Object) this.animationClip != (UnityEngine.Object) null)
          {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(this.animationClip);
            EditorCurveBinding[] referenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(this.animationClip);
            List<AnimationWindowCurve> transformCurves = new List<AnimationWindowCurve>();
            foreach (EditorCurveBinding editorCurveBinding in curveBindings)
            {
              if (AnimationWindowUtility.ShouldShowAnimationWindowCurve(editorCurveBinding))
              {
                AnimationWindowCurve animationWindowCurve = new AnimationWindowCurve(this.animationClip, editorCurveBinding, this.GetEditorCurveValueType(editorCurveBinding));
                animationWindowCurve.selectionBinding = this;
                this.m_CurvesCache.Add(animationWindowCurve);
                if (AnimationWindowUtility.IsTransformType(editorCurveBinding.type))
                  transformCurves.Add(animationWindowCurve);
              }
            }
            foreach (EditorCurveBinding editorCurveBinding in referenceCurveBindings)
              this.m_CurvesCache.Add(new AnimationWindowCurve(this.animationClip, editorCurveBinding, this.GetEditorCurveValueType(editorCurveBinding))
              {
                selectionBinding = this
              });
            transformCurves.Sort();
            if (transformCurves.Count > 0)
              this.FillInMissingTransformCurves(transformCurves, ref this.m_CurvesCache);
          }
          this.m_CurvesCache.Sort();
        }
        return this.m_CurvesCache;
      }
    }

    private void FillInMissingTransformCurves(List<AnimationWindowCurve> transformCurves, ref List<AnimationWindowCurve> curvesCache)
    {
      EditorCurveBinding lastBinding = transformCurves[0].binding;
      EditorCurveBinding?[] propertyGroup = new EditorCurveBinding?[3];
      foreach (AnimationWindowCurve transformCurve in transformCurves)
      {
        EditorCurveBinding binding = transformCurve.binding;
        if (binding.path != lastBinding.path || AnimationWindowUtility.GetPropertyGroupName(binding.propertyName) != AnimationWindowUtility.GetPropertyGroupName(lastBinding.propertyName))
        {
          string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(lastBinding.propertyName);
          this.FillPropertyGroup(ref propertyGroup, lastBinding, propertyGroupName, ref curvesCache);
          lastBinding = binding;
          propertyGroup = new EditorCurveBinding?[3];
        }
        this.AssignBindingToRightSlot(binding, ref propertyGroup);
      }
      this.FillPropertyGroup(ref propertyGroup, lastBinding, AnimationWindowUtility.GetPropertyGroupName(lastBinding.propertyName), ref curvesCache);
    }

    private void FillPropertyGroup(ref EditorCurveBinding?[] propertyGroup, EditorCurveBinding lastBinding, string propertyGroupName, ref List<AnimationWindowCurve> curvesCache)
    {
      EditorCurveBinding editorCurveBinding = lastBinding;
      editorCurveBinding.isPhantom = true;
      if (!propertyGroup[0].HasValue)
      {
        editorCurveBinding.propertyName = propertyGroupName + ".x";
        curvesCache.Add(new AnimationWindowCurve(this.animationClip, editorCurveBinding, this.GetEditorCurveValueType(editorCurveBinding))
        {
          selectionBinding = this
        });
      }
      if (!propertyGroup[1].HasValue)
      {
        editorCurveBinding.propertyName = propertyGroupName + ".y";
        curvesCache.Add(new AnimationWindowCurve(this.animationClip, editorCurveBinding, this.GetEditorCurveValueType(editorCurveBinding))
        {
          selectionBinding = this
        });
      }
      if (propertyGroup[2].HasValue)
        return;
      editorCurveBinding.propertyName = propertyGroupName + ".z";
      curvesCache.Add(new AnimationWindowCurve(this.animationClip, editorCurveBinding, this.GetEditorCurveValueType(editorCurveBinding))
      {
        selectionBinding = this
      });
    }

    private void AssignBindingToRightSlot(EditorCurveBinding transformBinding, ref EditorCurveBinding?[] propertyGroup)
    {
      if (transformBinding.propertyName.EndsWith(".x"))
        propertyGroup[0] = new EditorCurveBinding?(transformBinding);
      else if (transformBinding.propertyName.EndsWith(".y"))
      {
        propertyGroup[1] = new EditorCurveBinding?(transformBinding);
      }
      else
      {
        if (!transformBinding.propertyName.EndsWith(".z"))
          return;
        propertyGroup[2] = new EditorCurveBinding?(transformBinding);
      }
    }

    public static AnimationWindowSelectionItem Create()
    {
      AnimationWindowSelectionItem instance = ScriptableObject.CreateInstance(typeof (AnimationWindowSelectionItem)) as AnimationWindowSelectionItem;
      instance.hideFlags = HideFlags.HideAndDontSave;
      return instance;
    }

    public int GetRefreshHash()
    {
      return this.id * 19603 ^ (!((UnityEngine.Object) this.animationClip != (UnityEngine.Object) null) ? 0 : 729 * this.animationClip.GetHashCode()) ^ (!((UnityEngine.Object) this.rootGameObject != (UnityEngine.Object) null) ? 0 : 27 * this.rootGameObject.GetHashCode()) ^ (!((UnityEngine.Object) this.scriptableObject != (UnityEngine.Object) null) ? 0 : this.scriptableObject.GetHashCode());
    }

    public void ClearCache()
    {
      this.m_CurvesCache = (List<AnimationWindowCurve>) null;
    }

    public virtual void Synchronize()
    {
    }

    public bool Equals(AnimationWindowSelectionItem other)
    {
      return this.id == other.id && (UnityEngine.Object) this.animationClip == (UnityEngine.Object) other.animationClip && (UnityEngine.Object) this.gameObject == (UnityEngine.Object) other.gameObject && (UnityEngine.Object) this.scriptableObject == (UnityEngine.Object) other.scriptableObject;
    }

    public System.Type GetEditorCurveValueType(EditorCurveBinding curveBinding)
    {
      if ((UnityEngine.Object) this.rootGameObject != (UnityEngine.Object) null)
        return AnimationUtility.GetEditorCurveValueType(this.rootGameObject, curveBinding);
      if ((UnityEngine.Object) this.scriptableObject != (UnityEngine.Object) null)
        return AnimationUtility.GetScriptableObjectEditorCurveValueType(this.scriptableObject, curveBinding);
      if (curveBinding.isPPtrCurve)
        return (System.Type) null;
      return typeof (float);
    }
  }
}
