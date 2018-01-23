// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GraphViewPresenter
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
  internal abstract class GraphViewPresenter : ScriptableObject
  {
    [SerializeField]
    protected List<GraphElementPresenter> m_Elements = new List<GraphElementPresenter>();
    [SerializeField]
    private List<GraphElementPresenter> m_TempElements = new List<GraphElementPresenter>();
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public Vector3 scale;

    public IEnumerable<GraphElementPresenter> allChildren
    {
      get
      {
        return this.m_Elements.SelectMany<GraphElementPresenter, GraphElementPresenter>((Func<GraphElementPresenter, IEnumerable<GraphElementPresenter>>) (e => e.allElements));
      }
    }

    public virtual void AddElement(GraphElementPresenter element)
    {
      this.m_Elements.Add(element);
    }

    public virtual void AddElement(EdgePresenter edge)
    {
      this.AddElement((GraphElementPresenter) edge);
    }

    public virtual void RemoveElement(GraphElementPresenter element)
    {
      element.OnRemoveFromGraph();
      this.m_Elements.Remove(element);
    }

    protected void OnEnable()
    {
      this.m_Elements.Clear();
      this.m_TempElements.Clear();
    }

    public IEnumerable<GraphElementPresenter> elements
    {
      get
      {
        return this.m_Elements.Union<GraphElementPresenter>((IEnumerable<GraphElementPresenter>) this.m_TempElements);
      }
    }

    public void AddTempElement(GraphElementPresenter element)
    {
      this.m_TempElements.Add(element);
    }

    public void RemoveTempElement(GraphElementPresenter element)
    {
      element.OnRemoveFromGraph();
      this.m_TempElements.Remove(element);
    }

    public void ClearTempElements()
    {
      this.m_TempElements.Clear();
    }

    public virtual List<NodeAnchorPresenter> GetCompatibleAnchors(NodeAnchorPresenter startAnchor, NodeAdapter nodeAdapter)
    {
      return this.allChildren.OfType<NodeAnchorPresenter>().Where<NodeAnchorPresenter>((Func<NodeAnchorPresenter, bool>) (nap => nap.IsConnectable() && nap.orientation == startAnchor.orientation && nap.direction != startAnchor.direction && nodeAdapter.GetAdapter(nap.source, startAnchor.source) != null)).ToList<NodeAnchorPresenter>();
    }
  }
}
