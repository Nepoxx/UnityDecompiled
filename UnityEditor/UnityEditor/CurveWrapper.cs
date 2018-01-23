// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveWrapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class CurveWrapper
  {
    public Color wrapColorMultiplier = Color.white;
    public float vRangeMin = float.NegativeInfinity;
    public float vRangeMax = float.PositiveInfinity;
    private CurveRenderer m_Renderer;
    private ISelectionBinding m_SelectionBinding;
    public int id;
    public EditorCurveBinding binding;
    public int groupId;
    public int regionId;
    public Color color;
    public bool readOnly;
    public bool hidden;
    public CurveWrapper.GetAxisScalarsCallback getAxisUiScalarsCallback;
    public CurveWrapper.SetAxisScalarsCallback setAxisUiScalarsCallback;
    public CurveWrapper.PreProcessKeyMovement preProcessKeyMovementDelegate;
    public CurveWrapper.SelectionMode selected;
    public int listIndex;
    private bool m_Changed;

    public CurveWrapper()
    {
      this.id = 0;
      this.groupId = -1;
      this.regionId = -1;
      this.hidden = false;
      this.readOnly = false;
      this.listIndex = -1;
      this.getAxisUiScalarsCallback = (CurveWrapper.GetAxisScalarsCallback) null;
      this.setAxisUiScalarsCallback = (CurveWrapper.SetAxisScalarsCallback) null;
    }

    public CurveRenderer renderer
    {
      get
      {
        return this.m_Renderer;
      }
      set
      {
        this.m_Renderer = value;
      }
    }

    public AnimationCurve curve
    {
      get
      {
        return this.renderer.GetCurve();
      }
    }

    public GameObject rootGameObjet
    {
      get
      {
        return this.m_SelectionBinding == null ? (GameObject) null : this.m_SelectionBinding.rootGameObject;
      }
    }

    public AnimationClip animationClip
    {
      get
      {
        return this.m_SelectionBinding == null ? (AnimationClip) null : this.m_SelectionBinding.animationClip;
      }
    }

    public float timeOffset
    {
      get
      {
        return this.m_SelectionBinding == null ? 0.0f : this.m_SelectionBinding.timeOffset;
      }
    }

    public bool clipIsEditable
    {
      get
      {
        return this.m_SelectionBinding == null || this.m_SelectionBinding.clipIsEditable;
      }
    }

    public bool animationIsEditable
    {
      get
      {
        return this.m_SelectionBinding == null || this.m_SelectionBinding.animationIsEditable;
      }
    }

    public int selectionID
    {
      get
      {
        return this.m_SelectionBinding == null ? 0 : this.m_SelectionBinding.id;
      }
    }

    public ISelectionBinding selectionBindingInterface
    {
      get
      {
        return this.m_SelectionBinding;
      }
      set
      {
        this.m_SelectionBinding = value;
      }
    }

    public Bounds bounds
    {
      get
      {
        return this.renderer.GetBounds();
      }
    }

    public bool changed
    {
      get
      {
        return this.m_Changed;
      }
      set
      {
        this.m_Changed = value;
        if (!value || this.renderer == null)
          return;
        this.renderer.FlushCache();
      }
    }

    public int AddKey(Keyframe key)
    {
      this.PreProcessKey(ref key);
      return this.curve.AddKey(key);
    }

    public void PreProcessKey(ref Keyframe key)
    {
      if (this.preProcessKeyMovementDelegate == null)
        return;
      this.preProcessKeyMovementDelegate(ref key);
    }

    public int MoveKey(int index, ref Keyframe key)
    {
      this.PreProcessKey(ref key);
      return this.curve.MoveKey(index, key);
    }

    public delegate Vector2 GetAxisScalarsCallback();

    public delegate void SetAxisScalarsCallback(Vector2 newAxisScalars);

    public delegate void PreProcessKeyMovement(ref Keyframe key);

    internal enum SelectionMode
    {
      None,
      Selected,
      SemiSelected,
    }
  }
}
