// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GraphElement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal abstract class GraphElement : DataWatchContainer, ISelectable
  {
    private GraphElementPresenter m_Presenter;
    private ClickSelector m_ClickSelector;

    protected GraphElement()
    {
      this.ClearClassList();
      this.AddToClassList("graphElement");
      this.elementTypeColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);
    }

    public Color elementTypeColor { get; set; }

    public virtual int layer
    {
      get
      {
        return 0;
      }
    }

    public T GetPresenter<T>() where T : GraphElementPresenter
    {
      return this.presenter as T;
    }

    public GraphElementPresenter presenter
    {
      get
      {
        return this.m_Presenter;
      }
      set
      {
        if ((Object) this.m_Presenter == (Object) value)
          return;
        this.RemoveWatch();
        if (this.m_ClickSelector != null)
        {
          this.RemoveManipulator((IManipulator) this.m_ClickSelector);
          this.m_ClickSelector = (ClickSelector) null;
        }
        this.m_Presenter = value;
        if (this.IsSelectable())
        {
          this.m_ClickSelector = new ClickSelector();
          this.AddManipulator((IManipulator) this.m_ClickSelector);
        }
        this.OnDataChanged();
        this.AddWatch();
      }
    }

    protected override Object[] toWatch
    {
      get
      {
        return this.presenter.GetObjectsToWatch();
      }
    }

    public override void OnDataChanged()
    {
      if ((Object) this.presenter == (Object) null)
        return;
      foreach (VisualElement visualElement in (VisualElement) this)
      {
        GraphElement graphElement = visualElement as GraphElement;
        if (graphElement != null)
        {
          GraphElementPresenter presenter = graphElement.presenter;
          if ((Object) presenter != (Object) null)
            presenter.selected = this.presenter.selected;
        }
      }
      if (this.presenter.selected)
        this.AddToClassList("selected");
      else
        this.RemoveFromClassList("selected");
      this.SetPosition(this.presenter.position);
    }

    public virtual bool IsSelectable()
    {
      return (this.presenter.capabilities & Capabilities.Selectable) == Capabilities.Selectable;
    }

    public virtual Vector3 GetGlobalCenter()
    {
      Vector2 center = this.layout.center;
      return this.parent.worldTransform.MultiplyPoint3x4(new Vector3(center.x + this.parent.layout.x, center.y + this.parent.layout.y));
    }

    public virtual void SetPosition(Rect newPos)
    {
      this.layout = newPos;
    }

    public virtual void OnSelected()
    {
    }

    public virtual void Select(UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer, bool additive)
    {
      if (selectionContainer == null || selectionContainer.selection.Contains((ISelectable) this))
        return;
      if (!additive)
        selectionContainer.ClearSelection();
      selectionContainer.AddToSelection((ISelectable) this);
    }

    public virtual void Unselect(UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer)
    {
      if (selectionContainer == null || this.parent != selectionContainer.contentViewContainer || !selectionContainer.selection.Contains((ISelectable) this))
        return;
      selectionContainer.RemoveFromSelection((ISelectable) this);
    }

    public virtual bool IsSelected(UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer)
    {
      return selectionContainer != null && this.parent == selectionContainer.contentViewContainer && selectionContainer.selection.Contains((ISelectable) this);
    }
  }
}
