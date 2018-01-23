// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.NodePresenter
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
  internal class NodePresenter : SimpleElementPresenter
  {
    [SerializeField]
    protected List<NodeAnchorPresenter> m_InputAnchors;
    [SerializeField]
    protected List<NodeAnchorPresenter> m_OutputAnchors;
    [SerializeField]
    private bool m_expanded;
    protected Orientation m_Orientation;

    protected NodePresenter()
    {
      this.m_expanded = true;
      this.m_Orientation = Orientation.Horizontal;
    }

    public List<NodeAnchorPresenter> inputAnchors
    {
      get
      {
        return this.m_InputAnchors ?? (this.m_InputAnchors = new List<NodeAnchorPresenter>());
      }
    }

    public List<NodeAnchorPresenter> outputAnchors
    {
      get
      {
        return this.m_OutputAnchors ?? (this.m_OutputAnchors = new List<NodeAnchorPresenter>());
      }
    }

    public virtual bool expanded
    {
      get
      {
        return this.m_expanded;
      }
      set
      {
        this.m_expanded = value;
      }
    }

    public virtual Orientation orientation
    {
      get
      {
        return this.m_Orientation;
      }
    }

    protected new void OnEnable()
    {
      base.OnEnable();
      this.capabilities |= Capabilities.Deletable;
    }

    public override IEnumerable<GraphElementPresenter> allChildren
    {
      get
      {
        return this.inputAnchors.Concat<NodeAnchorPresenter>((IEnumerable<NodeAnchorPresenter>) this.outputAnchors).Cast<GraphElementPresenter>();
      }
    }

    public override IEnumerable<GraphElementPresenter> allElements
    {
      get
      {
        NodePresenter.\u003C\u003Ec__Iterator0 cIterator0 = new NodePresenter.\u003C\u003Ec__Iterator0() { \u0024this = this };
        cIterator0.\u0024PC = -2;
        return (IEnumerable<GraphElementPresenter>) cIterator0;
      }
    }
  }
}
