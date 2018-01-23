// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GraphElementPresenter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  [Serializable]
  internal abstract class GraphElementPresenter : ScriptableObject
  {
    [SerializeField]
    private bool m_Selected;
    [SerializeField]
    private Rect m_Position;
    [SerializeField]
    private Capabilities m_Capabilities;

    public virtual Rect position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }

    public Capabilities capabilities
    {
      get
      {
        return this.m_Capabilities;
      }
      set
      {
        this.m_Capabilities = value;
      }
    }

    public bool selected
    {
      get
      {
        return this.m_Selected;
      }
      set
      {
        if ((this.capabilities & Capabilities.Selectable) != Capabilities.Selectable)
          return;
        this.m_Selected = value;
      }
    }

    public virtual UnityEngine.Object[] GetObjectsToWatch()
    {
      return new UnityEngine.Object[1]{ (UnityEngine.Object) this };
    }

    protected virtual void OnEnable()
    {
      this.capabilities = Capabilities.Normal | Capabilities.Selectable | Capabilities.Movable;
    }

    public virtual void OnRemoveFromGraph()
    {
    }

    public virtual void CommitChanges()
    {
    }

    public virtual IEnumerable<GraphElementPresenter> allChildren
    {
      get
      {
        return Enumerable.Empty<GraphElementPresenter>();
      }
    }

    public virtual IEnumerable<GraphElementPresenter> allElements
    {
      get
      {
        GraphElementPresenter.\u003C\u003Ec__Iterator0 cIterator0 = new GraphElementPresenter.\u003C\u003Ec__Iterator0() { \u0024this = this };
        cIterator0.\u0024PC = -2;
        return (IEnumerable<GraphElementPresenter>) cIterator0;
      }
    }
  }
}
