// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.Edge
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class Edge : GraphElement
  {
    private const float k_EndPointRadius = 4f;
    private const float k_InterceptWidth = 3f;
    private const string k_EdgeWidthProperty = "edge-width";
    private const string k_SelectedEdgeColorProperty = "selected-edge-color";
    private const string k_EdgeColorProperty = "edge-color";
    private UnityEditor.Experimental.UIElements.GraphView.GraphView m_GraphView;
    private NodeAnchorPresenter m_OutputPresenter;
    private NodeAnchorPresenter m_InputPresenter;
    private NodeAnchor m_LeftAnchor;
    private NodeAnchor m_RightAnchor;
    private EdgeControl m_EdgeControl;
    private StyleValue<int> m_EdgeWidth;
    private StyleValue<Color> m_SelectedColor;
    private StyleValue<Color> m_DefaultColor;

    public Edge()
    {
      this.clippingOptions = VisualElement.ClippingOptions.NoClipping;
      this.ClearClassList();
      this.AddToClassList("edge");
      this.Add((VisualElement) this.edgeControl);
    }

    public EdgeControl edgeControl
    {
      get
      {
        if (this.m_EdgeControl == null)
          this.m_EdgeControl = this.CreateEdgeControl();
        return this.m_EdgeControl;
      }
    }

    public int edgeWidth
    {
      get
      {
        return this.m_EdgeWidth.GetSpecifiedValueOrDefault(2);
      }
    }

    public Color selectedColor
    {
      get
      {
        return this.m_SelectedColor.GetSpecifiedValueOrDefault(new Color(0.9411765f, 0.9411765f, 0.9411765f));
      }
    }

    public Color defaultColor
    {
      get
      {
        return this.m_DefaultColor.GetSpecifiedValueOrDefault(new Color(0.572549f, 0.572549f, 0.572549f));
      }
    }

    protected Vector3[] PointsAndTangents
    {
      get
      {
        return this.edgeControl.controlPoints;
      }
    }

    public override bool Overlaps(Rect rectangle)
    {
      if (!this.UpdateEdgeControl())
        return false;
      return this.edgeControl.Overlaps(rectangle);
    }

    public override bool ContainsPoint(Vector2 localPoint)
    {
      if (!this.UpdateEdgeControl())
        return false;
      return this.edgeControl.ContainsPoint(localPoint);
    }

    protected bool UpdateEdgeControl()
    {
      EdgePresenter presenter = this.GetPresenter<EdgePresenter>();
      NodeAnchorPresenter output = presenter.output;
      NodeAnchorPresenter input = presenter.input;
      if ((UnityEngine.Object) output == (UnityEngine.Object) null && (UnityEngine.Object) input == (UnityEngine.Object) null)
        return false;
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      this.GetFromToPoints(presenter, output, input, ref zero1, ref zero2);
      this.edgeControl.from = zero1;
      this.edgeControl.to = zero2;
      this.edgeControl.orientation = !((UnityEngine.Object) output != (UnityEngine.Object) null) ? input.orientation : output.orientation;
      return true;
    }

    public override void DoRepaint()
    {
      this.DrawEdge();
    }

    protected void GetFromToPoints(EdgePresenter edgePresenter, NodeAnchorPresenter outputPresenter, NodeAnchorPresenter inputPresenter, ref Vector2 from, ref Vector2 to)
    {
      if ((UnityEngine.Object) outputPresenter == (UnityEngine.Object) null && (UnityEngine.Object) inputPresenter == (UnityEngine.Object) null)
        return;
      if (this.m_GraphView == null)
        this.m_GraphView = this.GetFirstOfType<UnityEditor.Experimental.UIElements.GraphView.GraphView>();
      if ((UnityEngine.Object) outputPresenter != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.m_OutputPresenter != (UnityEngine.Object) outputPresenter)
        {
          this.m_LeftAnchor = this.m_GraphView.Query<NodeAnchor>((string) null, (string) null).Where((Func<NodeAnchor, bool>) (e => e.direction == Direction.Output && (UnityEngine.Object) e.GetPresenter<NodeAnchorPresenter>() == (UnityEngine.Object) outputPresenter)).First();
          this.m_OutputPresenter = outputPresenter;
        }
        if (this.m_LeftAnchor != null)
        {
          from = (Vector2) this.m_LeftAnchor.GetGlobalCenter();
          from = (Vector2) this.worldTransform.inverse.MultiplyPoint3x4((Vector3) from);
        }
      }
      else
        from = (Vector2) this.worldTransform.inverse.MultiplyPoint3x4(new Vector3(edgePresenter.candidatePosition.x, edgePresenter.candidatePosition.y));
      if ((UnityEngine.Object) inputPresenter != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.m_InputPresenter != (UnityEngine.Object) inputPresenter)
        {
          this.m_RightAnchor = this.m_GraphView.Query<NodeAnchor>((string) null, (string) null).Where((Func<NodeAnchor, bool>) (e => e.direction == Direction.Input && (UnityEngine.Object) e.GetPresenter<NodeAnchorPresenter>() == (UnityEngine.Object) inputPresenter)).First();
          this.m_InputPresenter = inputPresenter;
        }
        if (this.m_RightAnchor == null)
          return;
        to = (Vector2) this.m_RightAnchor.GetGlobalCenter();
        to = (Vector2) this.worldTransform.inverse.MultiplyPoint3x4((Vector3) to);
      }
      else
        to = (Vector2) this.worldTransform.inverse.MultiplyPoint3x4(new Vector3(edgePresenter.candidatePosition.x, edgePresenter.candidatePosition.y));
    }

    public override void OnStyleResolved(ICustomStyle styles)
    {
      base.OnStyleResolved(styles);
      styles.ApplyCustomProperty("edge-width", ref this.m_EdgeWidth);
      styles.ApplyCustomProperty("selected-edge-color", ref this.m_SelectedColor);
      styles.ApplyCustomProperty("edge-color", ref this.m_DefaultColor);
    }

    protected virtual void DrawEdge()
    {
      if (!this.UpdateEdgeControl())
        return;
      EdgePresenter presenter = this.GetPresenter<EdgePresenter>();
      Color color = !presenter.selected ? this.defaultColor : this.selectedColor;
      this.edgeControl.edgeColor = color;
      this.edgeControl.startCapColor = color;
      this.edgeControl.endCapColor = !((UnityEngine.Object) presenter.input == (UnityEngine.Object) null) ? color : this.edgeControl.startCapColor;
    }

    protected virtual EdgeControl CreateEdgeControl()
    {
      return new EdgeControl() { capRadius = 4f, interceptWidth = 3f };
    }
  }
}
