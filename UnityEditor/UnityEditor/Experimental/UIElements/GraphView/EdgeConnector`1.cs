// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.EdgeConnector`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class EdgeConnector<TEdgePresenter> : EdgeConnector where TEdgePresenter : EdgePresenter
  {
    private static NodeAdapter s_nodeAdapter = new NodeAdapter();
    private List<NodeAnchorPresenter> m_CompatibleAnchors;
    private TEdgePresenter m_EdgePresenterCandidate;
    private GraphViewPresenter m_GraphViewPresenter;
    private UnityEditor.Experimental.UIElements.GraphView.GraphView m_GraphView;
    private bool m_Active;
    private readonly IEdgeConnectorListener m_Listener;

    public EdgeConnector(IEdgeConnectorListener listener)
    {
      this.m_Listener = listener;
      this.m_Active = false;
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
    }

    protected void OnMouseDown(MouseDownEvent e)
    {
      if (!this.CanStartManipulation((IMouseEvent) e))
        return;
      NodeAnchor target = e.target as NodeAnchor;
      if (target == null)
        return;
      NodeAnchorPresenter presenter = target.GetPresenter<NodeAnchorPresenter>();
      this.m_GraphView = target.GetFirstAncestorOfType<UnityEditor.Experimental.UIElements.GraphView.GraphView>();
      if ((UnityEngine.Object) presenter == (UnityEngine.Object) null || this.m_GraphView == null)
        return;
      this.m_GraphViewPresenter = this.m_GraphView.presenter;
      if ((UnityEngine.Object) this.m_GraphViewPresenter == (UnityEngine.Object) null)
        return;
      this.m_Active = true;
      this.target.TakeCapture();
      this.m_CompatibleAnchors = this.m_GraphViewPresenter.GetCompatibleAnchors(presenter, EdgeConnector<TEdgePresenter>.s_nodeAdapter);
      foreach (NodeAnchorPresenter compatibleAnchor in this.m_CompatibleAnchors)
        compatibleAnchor.highlight = true;
      this.m_EdgePresenterCandidate = ScriptableObject.CreateInstance<TEdgePresenter>();
      this.m_EdgePresenterCandidate.position = new Rect(0.0f, 0.0f, 1f, 1f);
      if (presenter.direction == Direction.Output)
      {
        this.m_EdgePresenterCandidate.output = target.GetPresenter<NodeAnchorPresenter>();
        this.m_EdgePresenterCandidate.input = (NodeAnchorPresenter) null;
      }
      else
      {
        this.m_EdgePresenterCandidate.output = (NodeAnchorPresenter) null;
        this.m_EdgePresenterCandidate.input = target.GetPresenter<NodeAnchorPresenter>();
      }
      this.m_EdgePresenterCandidate.candidate = true;
      this.m_EdgePresenterCandidate.candidatePosition = e.mousePosition;
      this.m_GraphViewPresenter.AddTempElement((GraphElementPresenter) this.m_EdgePresenterCandidate);
      e.StopPropagation();
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
      if (!this.m_Active)
        return;
      this.m_EdgePresenterCandidate.candidatePosition = e.mousePosition;
      e.StopPropagation();
    }

    protected void OnMouseUp(MouseUpEvent e)
    {
      if (this.m_Active && this.CanStopManipulation((IMouseEvent) e))
      {
        NodeAnchorPresenter nodeAnchorPresenter = (NodeAnchorPresenter) null;
        if (this.m_GraphView != null)
        {
          foreach (NodeAnchorPresenter compatibleAnchor in this.m_CompatibleAnchors)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            EdgeConnector<TEdgePresenter>.\u003COnMouseUp\u003Ec__AnonStorey0 mouseUpCAnonStorey0 = new EdgeConnector<TEdgePresenter>.\u003COnMouseUp\u003Ec__AnonStorey0();
            // ISSUE: reference to a compiler-generated field
            mouseUpCAnonStorey0.compatibleAnchor = compatibleAnchor;
            // ISSUE: reference to a compiler-generated field
            mouseUpCAnonStorey0.compatibleAnchor.highlight = false;
            // ISSUE: reference to a compiler-generated method
            NodeAnchor nodeAnchor = this.m_GraphView.Query<NodeAnchor>((string) null, (string) null).Where(new Func<NodeAnchor, bool>(mouseUpCAnonStorey0.\u003C\u003Em__0)).First();
            if (nodeAnchor != null && nodeAnchor.worldBound.Contains(e.mousePosition))
            {
              // ISSUE: reference to a compiler-generated field
              nodeAnchorPresenter = mouseUpCAnonStorey0.compatibleAnchor;
            }
          }
        }
        if ((UnityEngine.Object) nodeAnchorPresenter == (UnityEngine.Object) null && this.m_Listener != null)
          this.m_Listener.OnDropOutsideAnchor((EdgePresenter) this.m_EdgePresenterCandidate, e.mousePosition);
        this.m_GraphViewPresenter.RemoveTempElement((GraphElementPresenter) this.m_EdgePresenterCandidate);
        if ((UnityEngine.Object) this.m_EdgePresenterCandidate != (UnityEngine.Object) null && (UnityEngine.Object) this.m_GraphViewPresenter != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) nodeAnchorPresenter != (UnityEngine.Object) null)
          {
            if ((UnityEngine.Object) this.m_EdgePresenterCandidate.output == (UnityEngine.Object) null)
              this.m_EdgePresenterCandidate.output = nodeAnchorPresenter;
            else
              this.m_EdgePresenterCandidate.input = nodeAnchorPresenter;
            this.m_EdgePresenterCandidate.output.Connect((EdgePresenter) this.m_EdgePresenterCandidate);
            this.m_EdgePresenterCandidate.input.Connect((EdgePresenter) this.m_EdgePresenterCandidate);
            this.m_GraphViewPresenter.AddElement((EdgePresenter) this.m_EdgePresenterCandidate);
          }
          this.m_EdgePresenterCandidate.candidate = false;
        }
        this.m_EdgePresenterCandidate = (TEdgePresenter) null;
        this.m_GraphViewPresenter = (GraphViewPresenter) null;
        this.m_Active = false;
        e.StopPropagation();
      }
      this.target.ReleaseCapture();
    }
  }
}
