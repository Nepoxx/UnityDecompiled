// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.LayoutGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Abstract base class to use for layout groups.</para>
  /// </summary>
  [DisallowMultipleComponent]
  [ExecuteInEditMode]
  [RequireComponent(typeof (RectTransform))]
  public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutGroup, ILayoutController
  {
    [SerializeField]
    protected RectOffset m_Padding = new RectOffset();
    [FormerlySerializedAs("m_Alignment")]
    [SerializeField]
    protected TextAnchor m_ChildAlignment = TextAnchor.UpperLeft;
    private Vector2 m_TotalMinSize = Vector2.zero;
    private Vector2 m_TotalPreferredSize = Vector2.zero;
    private Vector2 m_TotalFlexibleSize = Vector2.zero;
    [NonSerialized]
    private List<RectTransform> m_RectChildren = new List<RectTransform>();
    [NonSerialized]
    private RectTransform m_Rect;
    protected DrivenRectTransformTracker m_Tracker;

    protected LayoutGroup()
    {
      if (this.m_Padding != null)
        return;
      this.m_Padding = new RectOffset();
    }

    /// <summary>
    ///   <para>The padding to add around the child layout elements.</para>
    /// </summary>
    public RectOffset padding
    {
      get
      {
        return this.m_Padding;
      }
      set
      {
        this.SetProperty<RectOffset>(ref this.m_Padding, value);
      }
    }

    /// <summary>
    ///   <para>The alignment to use for the child layout elements in the layout group.</para>
    /// </summary>
    public TextAnchor childAlignment
    {
      get
      {
        return this.m_ChildAlignment;
      }
      set
      {
        this.SetProperty<TextAnchor>(ref this.m_ChildAlignment, value);
      }
    }

    protected RectTransform rectTransform
    {
      get
      {
        if ((UnityEngine.Object) this.m_Rect == (UnityEngine.Object) null)
          this.m_Rect = this.GetComponent<RectTransform>();
        return this.m_Rect;
      }
    }

    protected List<RectTransform> rectChildren
    {
      get
      {
        return this.m_RectChildren;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual void CalculateLayoutInputHorizontal()
    {
      this.m_RectChildren.Clear();
      List<Component> componentList = ListPool<Component>.Get();
      for (int index1 = 0; index1 < this.rectTransform.childCount; ++index1)
      {
        RectTransform child = this.rectTransform.GetChild(index1) as RectTransform;
        if (!((UnityEngine.Object) child == (UnityEngine.Object) null) && child.gameObject.activeInHierarchy)
        {
          child.GetComponents(typeof (ILayoutIgnorer), componentList);
          if (componentList.Count == 0)
          {
            this.m_RectChildren.Add(child);
          }
          else
          {
            for (int index2 = 0; index2 < componentList.Count; ++index2)
            {
              if (!((ILayoutIgnorer) componentList[index2]).ignoreLayout)
              {
                this.m_RectChildren.Add(child);
                break;
              }
            }
          }
        }
      }
      ListPool<Component>.Release(componentList);
      this.m_Tracker.Clear();
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public abstract void CalculateLayoutInputVertical();

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float minWidth
    {
      get
      {
        return this.GetTotalMinSize(0);
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float preferredWidth
    {
      get
      {
        return this.GetTotalPreferredSize(0);
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float flexibleWidth
    {
      get
      {
        return this.GetTotalFlexibleSize(0);
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float minHeight
    {
      get
      {
        return this.GetTotalMinSize(1);
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float preferredHeight
    {
      get
      {
        return this.GetTotalPreferredSize(1);
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float flexibleHeight
    {
      get
      {
        return this.GetTotalFlexibleSize(1);
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual int layoutPriority
    {
      get
      {
        return 0;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public abstract void SetLayoutHorizontal();

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public abstract void SetLayoutVertical();

    protected override void OnEnable()
    {
      base.OnEnable();
      this.SetDirty();
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      this.m_Tracker.Clear();
      LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
      base.OnDisable();
    }

    /// <summary>
    ///   <para>Callback for when properties have been changed by animation.</para>
    /// </summary>
    protected override void OnDidApplyAnimationProperties()
    {
      this.SetDirty();
    }

    /// <summary>
    ///   <para>The min size for the layout group on the given axis.</para>
    /// </summary>
    /// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
    /// <returns>
    ///   <para>The min size.</para>
    /// </returns>
    protected float GetTotalMinSize(int axis)
    {
      return this.m_TotalMinSize[axis];
    }

    /// <summary>
    ///   <para>The preferred size for the layout group on the given axis.</para>
    /// </summary>
    /// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
    /// <returns>
    ///   <para>The preferred size.</para>
    /// </returns>
    protected float GetTotalPreferredSize(int axis)
    {
      return this.m_TotalPreferredSize[axis];
    }

    /// <summary>
    ///   <para>The flexible size for the layout group on the given axis.</para>
    /// </summary>
    /// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
    /// <returns>
    ///   <para>The flexible size.</para>
    /// </returns>
    protected float GetTotalFlexibleSize(int axis)
    {
      return this.m_TotalFlexibleSize[axis];
    }

    /// <summary>
    ///   <para>Returns the calculated position of the first child layout element along the given axis.</para>
    /// </summary>
    /// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
    /// <param name="requiredSpaceWithoutPadding">The total space required on the given axis for all the layout elements including spacing and excluding padding.</param>
    /// <returns>
    ///   <para>The position of the first child along the given axis.</para>
    /// </returns>
    protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
    {
      float num1 = requiredSpaceWithoutPadding + (axis != 0 ? (float) this.padding.vertical : (float) this.padding.horizontal);
      float num2 = this.rectTransform.rect.size[axis] - num1;
      float alignmentOnAxis = this.GetAlignmentOnAxis(axis);
      return (axis != 0 ? (float) this.padding.top : (float) this.padding.left) + num2 * alignmentOnAxis;
    }

    /// <summary>
    ///   <para>Returns the alignment on the specified axis as a fraction where 0 is lefttop, 0.5 is middle, and 1 is rightbottom.</para>
    /// </summary>
    /// <param name="axis">The axis to get alignment along. 0 is horizontal and 1 is vertical.</param>
    /// <returns>
    ///   <para>The alignment as a fraction where 0 is lefttop, 0.5 is middle, and 1 is rightbottom.</para>
    /// </returns>
    protected float GetAlignmentOnAxis(int axis)
    {
      if (axis == 0)
        return (float) ((int) this.childAlignment % 3) * 0.5f;
      return (float) ((int) this.childAlignment / 3) * 0.5f;
    }

    /// <summary>
    ///   <para>Used to set the calculated layout properties for the given axis.</para>
    /// </summary>
    /// <param name="totalMin">The min size for the layout group.</param>
    /// <param name="totalPreferred">The preferred size for the layout group.</param>
    /// <param name="totalFlexible">The flexible size for the layout group.</param>
    /// <param name="axis">The axis to set sizes for. 0 is horizontal and 1 is vertical.</param>
    protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
    {
      this.m_TotalMinSize[axis] = totalMin;
      this.m_TotalPreferredSize[axis] = totalPreferred;
      this.m_TotalFlexibleSize[axis] = totalFlexible;
    }

    protected void SetChildAlongAxis(RectTransform rect, int axis, float pos)
    {
      if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
        return;
      this.m_Tracker.Add((UnityEngine.Object) this, rect, (DrivenTransformProperties) (3840 | (axis != 0 ? 4 : 2)));
      rect.SetInsetAndSizeFromParentEdge(axis != 0 ? RectTransform.Edge.Top : RectTransform.Edge.Left, pos, rect.sizeDelta[axis]);
    }

    /// <summary>
    ///   <para>Set the position and size of a child layout element along the given axis.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the child layout element.</param>
    /// <param name="axis">The axis to set the position and size along. 0 is horizontal and 1 is vertical.</param>
    /// <param name="pos">The position from the left side or top.</param>
    /// <param name="size">The size.</param>
    protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
    {
      if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
        return;
      this.m_Tracker.Add((UnityEngine.Object) this, rect, (DrivenTransformProperties) (3840 | (axis != 0 ? 8196 : 4098)));
      rect.SetInsetAndSizeFromParentEdge(axis != 0 ? RectTransform.Edge.Top : RectTransform.Edge.Left, pos, size);
    }

    private bool isRootLayoutGroup
    {
      get
      {
        if ((UnityEngine.Object) this.transform.parent == (UnityEngine.Object) null)
          return true;
        return (UnityEngine.Object) this.transform.parent.GetComponent(typeof (ILayoutGroup)) == (UnityEngine.Object) null;
      }
    }

    protected override void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      if (!this.isRootLayoutGroup)
        return;
      this.SetDirty();
    }

    protected virtual void OnTransformChildrenChanged()
    {
      this.SetDirty();
    }

    protected void SetProperty<T>(ref T currentValue, T newValue)
    {
      if ((object) currentValue == null && (object) newValue == null || (object) currentValue != null && currentValue.Equals((object) newValue))
        return;
      currentValue = newValue;
      this.SetDirty();
    }

    /// <summary>
    ///   <para>Mark the LayoutGroup as dirty.</para>
    /// </summary>
    protected void SetDirty()
    {
      if (!this.IsActive())
        return;
      if (!CanvasUpdateRegistry.IsRebuildingLayout())
        LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
      else
        this.StartCoroutine(this.DelayedSetDirty(this.rectTransform));
    }

    [DebuggerHidden]
    private IEnumerator DelayedSetDirty(RectTransform rectTransform)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new LayoutGroup.\u003CDelayedSetDirty\u003Ec__Iterator0() { rectTransform = rectTransform };
    }

    protected override void OnValidate()
    {
      this.SetDirty();
    }
  }
}
