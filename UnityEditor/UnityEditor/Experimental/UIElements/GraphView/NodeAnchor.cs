// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.NodeAnchor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class NodeAnchor : GraphElement
  {
    protected EdgeConnector m_EdgeConnector;
    protected VisualElement m_ConnectorBox;
    protected VisualElement m_ConnectorText;

    protected NodeAnchor()
    {
      this.ClearClassList();
      (EditorGUIUtility.Load("UXML/GraphView/NodeAnchor.uxml") as VisualTreeAsset).CloneTree((VisualElement) this, (Dictionary<string, VisualElement>) null);
      this.m_ConnectorBox = this.Q("connector", (string) null);
      this.m_ConnectorBox.AddToClassList("connector");
      this.m_ConnectorText = this.Q("type", (string) null);
      this.m_ConnectorText.AddToClassList("type");
    }

    public Direction direction { get; private set; }

    public static NodeAnchor Create<TEdgePresenter>(NodeAnchorPresenter presenter) where TEdgePresenter : EdgePresenter
    {
      NodeAnchor nodeAnchor = new NodeAnchor();
      nodeAnchor.m_EdgeConnector = (EdgeConnector) new EdgeConnector<TEdgePresenter>((IEdgeConnectorListener) null);
      nodeAnchor.presenter = (GraphElementPresenter) presenter;
      NodeAnchor ele = nodeAnchor;
      ele.AddManipulator((IManipulator) ele.m_EdgeConnector);
      return ele;
    }

    public virtual void UpdateClasses(bool fakeConnection)
    {
      if (this.GetPresenter<NodeAnchorPresenter>().connected || fakeConnection)
        this.AddToClassList("connected");
      else
        this.RemoveFromClassList("connected");
    }

    protected virtual VisualElement CreateConnector()
    {
      return new VisualElement();
    }

    private void UpdateConnector()
    {
      if (this.m_EdgeConnector == null)
        return;
      NodeAnchorPresenter presenter = this.GetPresenter<NodeAnchorPresenter>();
      if (this.m_EdgeConnector.target != null && this.m_EdgeConnector.target.HasCapture())
        return;
      if (!presenter.connected || presenter.direction != Direction.Input)
        this.AddManipulator((IManipulator) this.m_EdgeConnector);
      else
        this.RemoveManipulator((IManipulator) this.m_EdgeConnector);
    }

    public Node node
    {
      get
      {
        return this.GetFirstAncestorOfType<Node>();
      }
    }

    public override void OnDataChanged()
    {
      this.UpdateConnector();
      this.UpdateClasses(false);
      NodeAnchorPresenter presenter = this.GetPresenter<NodeAnchorPresenter>();
      System.Type anchorType = presenter.anchorType;
      System.Type type1 = typeof (PortSource<>);
      try
      {
        System.Type type2 = type1.MakeGenericType(anchorType);
        presenter.source = Activator.CreateInstance(type2);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Couldn't build PortSouce<" + (anchorType != null ? anchorType.Name : "null") + "> " + ex.Message));
      }
      if (presenter.highlight)
        this.m_ConnectorBox.AddToClassList("anchorHighlight");
      else
        this.m_ConnectorBox.RemoveFromClassList("anchorHighlight");
      this.m_ConnectorText.text = !string.IsNullOrEmpty(presenter.name) ? presenter.name : anchorType.Name;
      presenter.capabilities &= ~Capabilities.Selectable;
      this.direction = presenter.direction;
    }

    public override Vector3 GetGlobalCenter()
    {
      return (Vector3) this.LocalToWorld((Vector2) this.m_ConnectorBox.transform.matrix.MultiplyPoint3x4((Vector3) this.m_ConnectorBox.layout.center));
    }

    public override bool ContainsPoint(Vector2 localPoint)
    {
      localPoint -= this.layout.position;
      return this.m_ConnectorBox.ContainsPoint((Vector2) this.m_ConnectorBox.transform.matrix.MultiplyPoint3x4((Vector3) localPoint));
    }
  }
}
