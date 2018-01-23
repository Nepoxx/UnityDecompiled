// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.MiniMap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class MiniMap : GraphElement
  {
    private float m_PreviousContainerWidth = -1f;
    private float m_PreviousContainerHeight = -1f;
    private readonly Color m_ViewportColor = new Color(1f, 1f, 0.0f, 0.35f);
    private readonly Color m_SelectedChildrenColor = new Color(1f, 1f, 1f, 0.5f);
    private readonly Label m_Label;
    private Dragger m_Dragger;
    private Rect m_ViewportRect;
    private Rect m_ContentRect;
    private Rect m_ContentRectLocal;

    public MiniMap()
    {
      this.clippingOptions = VisualElement.ClippingOptions.NoClipping;
      this.m_Label = new Label("Floating Minimap");
      this.Add((VisualElement) this.m_Label);
      this.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.ShowContextualMenu), Capture.NoCapture);
      this.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
    }

    private int titleBarOffset
    {
      get
      {
        return (int) (float) this.style.paddingTop;
      }
    }

    protected void ShowContextualMenu(MouseUpEvent e)
    {
      if (e.button != 1)
        return;
      MiniMapPresenter presenter1 = this.GetPresenter<MiniMapPresenter>();
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent(!presenter1.anchored ? "Anchor" : "Make floating"), false, (GenericMenu.MenuFunction2) (contentView =>
      {
        MiniMapPresenter presenter2 = this.GetPresenter<MiniMapPresenter>();
        presenter2.anchored = !presenter2.anchored;
      }), (object) this);
      genericMenu.DropDown(new Rect(e.mousePosition.x, e.mousePosition.y, 0.0f, 0.0f));
      e.StopPropagation();
    }

    public override void OnDataChanged()
    {
      base.OnDataChanged();
      this.AdjustAnchoring();
      this.Resize();
    }

    private void AdjustAnchoring()
    {
      MiniMapPresenter presenter = this.GetPresenter<MiniMapPresenter>();
      if ((UnityEngine.Object) presenter == (UnityEngine.Object) null)
        return;
      if (presenter.anchored)
      {
        presenter.capabilities &= ~Capabilities.Movable;
        this.ResetPositionProperties();
        this.AddToClassList("anchored");
      }
      else
      {
        if (this.m_Dragger == null)
        {
          this.m_Dragger = new Dragger()
          {
            clampToParentEdges = true
          };
          this.AddManipulator((IManipulator) this.m_Dragger);
        }
        this.presenter.capabilities |= Capabilities.Movable;
        this.RemoveFromClassList("anchored");
      }
    }

    private void Resize()
    {
      if (this.parent == null)
        return;
      MiniMapPresenter presenter = this.GetPresenter<MiniMapPresenter>();
      this.style.width = (StyleValue<float>) presenter.maxWidth;
      this.style.height = (StyleValue<float>) presenter.maxHeight;
      if ((double) (float) this.style.positionLeft + (double) (float) this.style.width > (double) this.parent.layout.x + (double) this.parent.layout.width)
      {
        Rect position = presenter.position;
        position.x -= (float) ((double) (float) this.style.positionLeft + (double) (float) this.style.width - ((double) this.parent.layout.x + (double) this.parent.layout.width));
        presenter.position = position;
      }
      if ((double) (float) this.style.positionTop + (double) (float) this.style.height > (double) this.parent.layout.y + (double) this.parent.layout.height)
      {
        Rect position = presenter.position;
        position.y -= (float) ((double) (float) this.style.positionTop + (double) (float) this.style.height - ((double) this.parent.layout.y + (double) this.parent.layout.height));
        presenter.position = position;
      }
      Rect position1 = presenter.position;
      position1.width = (float) this.style.width;
      position1.height = (float) this.style.height;
      position1.x = Mathf.Max(this.parent.layout.x, position1.x);
      position1.y = Mathf.Max(this.parent.layout.y, position1.y);
      presenter.position = position1;
      if (presenter.anchored)
        return;
      this.layout = presenter.position;
    }

    private static void ChangeToMiniMapCoords(ref Rect rect, float factor, Vector3 translation)
    {
      rect.width *= factor;
      rect.height *= factor;
      rect.x *= factor;
      rect.y *= factor;
      rect.x += translation.x;
      rect.y += translation.y;
    }

    private void CalculateRects(UnityEditor.Experimental.UIElements.GraphView.GraphView gView)
    {
      this.m_ContentRect = gView.CalculateRectToFitAll();
      this.m_ContentRectLocal = this.m_ContentRect;
      Matrix4x4 inverse = gView.contentViewContainer.worldTransform.inverse;
      Vector4 column = inverse.GetColumn(3);
      Vector2 vector2 = new Vector2(inverse.m00, inverse.m11);
      this.m_ViewportRect = this.parent.layout;
      this.m_ViewportRect.x += column.x;
      this.m_ViewportRect.y += column.y;
      this.m_ViewportRect.width *= vector2.x;
      this.m_ViewportRect.height *= vector2.y;
      this.m_Label.text = "MiniMap v: " + string.Format("{0:0}", (object) this.m_ViewportRect.width) + "x" + string.Format("{0:0}", (object) this.m_ViewportRect.height);
      Rect rect = RectUtils.Encompass(this.m_ContentRect, this.m_ViewportRect);
      float factor = this.layout.width / rect.width;
      MiniMap.ChangeToMiniMapCoords(ref rect, factor, Vector3.zero);
      Vector3 translation = new Vector3(this.layout.x - rect.x, this.layout.y + (float) this.titleBarOffset - rect.y);
      MiniMap.ChangeToMiniMapCoords(ref this.m_ViewportRect, factor, translation);
      MiniMap.ChangeToMiniMapCoords(ref this.m_ContentRect, factor, translation);
      if ((double) rect.height <= (double) this.layout.height - (double) this.titleBarOffset)
        return;
      float num1 = (this.layout.height - (float) this.titleBarOffset) / rect.height;
      float num2 = (float) (((double) this.layout.width - (double) rect.width * (double) num1) / 2.0);
      float num3 = (float) ((double) this.layout.y + (double) this.titleBarOffset - ((double) rect.y + (double) translation.y) * (double) num1);
      this.m_ContentRect.width *= num1;
      this.m_ContentRect.height *= num1;
      this.m_ContentRect.y *= num1;
      this.m_ContentRect.x += num2;
      this.m_ContentRect.y += num3;
      this.m_ViewportRect.width *= num1;
      this.m_ViewportRect.height *= num1;
      this.m_ViewportRect.y *= num1;
      this.m_ViewportRect.x += num2;
      this.m_ViewportRect.y += num3;
    }

    private Rect CalculateElementRect(GraphElement elem)
    {
      GraphElementPresenter presenter = elem.GetPresenter<GraphElementPresenter>();
      if ((presenter.capabilities & Capabilities.Floating) != (Capabilities) 0 || presenter is EdgePresenter)
        return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      Rect localBound = elem.localBound;
      localBound.x = this.m_ContentRect.x + (localBound.x - this.m_ContentRectLocal.x) * this.m_ContentRect.width / this.m_ContentRectLocal.width;
      localBound.y = this.m_ContentRect.y + (localBound.y - this.m_ContentRectLocal.y) * this.m_ContentRect.height / this.m_ContentRectLocal.height;
      localBound.width *= this.m_ContentRect.width / this.m_ContentRectLocal.width;
      localBound.height *= this.m_ContentRect.height / this.m_ContentRectLocal.height;
      float num1 = this.layout.xMin + 2f;
      float num2 = this.layout.xMax - 2f;
      float num3 = this.layout.yMax - 2f;
      if ((double) localBound.x < (double) num1)
      {
        if ((double) localBound.x < (double) num1 - (double) localBound.width)
          return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        localBound.width -= num1 - localBound.x;
        localBound.x = num1;
      }
      if ((double) localBound.x + (double) localBound.width >= (double) num2)
      {
        if ((double) localBound.x >= (double) num2)
          return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        localBound.width -= localBound.x + localBound.width - num2;
      }
      if ((double) localBound.y < (double) this.layout.yMin + (double) this.titleBarOffset)
      {
        if ((double) localBound.y < (double) this.layout.yMin + (double) this.titleBarOffset - (double) localBound.height)
          return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        localBound.height -= this.layout.yMin + (float) this.titleBarOffset - localBound.y;
        localBound.y = this.layout.yMin + (float) this.titleBarOffset;
      }
      if ((double) localBound.y + (double) localBound.height >= (double) num3)
      {
        if ((double) localBound.y >= (double) num3)
          return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        localBound.height -= localBound.y + localBound.height - num3;
      }
      return localBound;
    }

    public override void DoRepaint()
    {
      UnityEditor.Experimental.UIElements.GraphView.GraphView firstAncestorOfType = this.GetFirstAncestorOfType<UnityEditor.Experimental.UIElements.GraphView.GraphView>();
      Matrix4x4 matrix = firstAncestorOfType.viewTransform.matrix;
      Vector2 vector2 = new Vector2(matrix.m00, matrix.m11);
      float num1 = this.parent.layout.width / vector2.x;
      float num2 = this.parent.layout.height / vector2.y;
      if ((double) Mathf.Abs(num1 - this.m_PreviousContainerWidth) > (double) Mathf.Epsilon || (double) Mathf.Abs(num2 - this.m_PreviousContainerHeight) > (double) Mathf.Epsilon)
      {
        this.m_PreviousContainerWidth = num1;
        this.m_PreviousContainerHeight = num2;
        this.Resize();
      }
      this.CalculateRects(firstAncestorOfType);
      base.DoRepaint();
      Color color = Handles.color;
      firstAncestorOfType.graphElements.ForEach((System.Action<GraphElement>) (elem =>
      {
        Rect elementRect = this.CalculateElementRect(elem);
        Handles.color = elem.elementTypeColor;
        Handles.DrawSolidRectangleWithOutline(elementRect, elem.elementTypeColor, elem.elementTypeColor);
        GraphElementPresenter presenter = elem.GetPresenter<GraphElementPresenter>();
        if (!((UnityEngine.Object) presenter != (UnityEngine.Object) null) || !presenter.selected)
          return;
        this.DrawRectangleOutline(elementRect, this.m_SelectedChildrenColor);
      }));
      this.DrawRectangleOutline(this.m_ViewportRect, this.m_ViewportColor);
      Handles.color = color;
    }

    private void DrawRectangleOutline(Rect rect, Color color)
    {
      Color color1 = Handles.color;
      Handles.color = color;
      Handles.DrawPolyLine(new Vector3(rect.x, rect.y, 0.0f), new Vector3(rect.x + rect.width, rect.y, 0.0f), new Vector3(rect.x + rect.width, rect.y + rect.height, 0.0f), new Vector3(rect.x, rect.y + rect.height, 0.0f), new Vector3(rect.x, rect.y, 0.0f));
      Handles.color = color1;
    }

    protected void OnMouseDown(MouseDownEvent e)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MiniMap.\u003COnMouseDown\u003Ec__AnonStorey0 downCAnonStorey0 = new MiniMap.\u003COnMouseDown\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      downCAnonStorey0.e = e;
      // ISSUE: reference to a compiler-generated field
      downCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      downCAnonStorey0.gView = this.GetFirstAncestorOfType<UnityEditor.Experimental.UIElements.GraphView.GraphView>();
      // ISSUE: reference to a compiler-generated field
      this.CalculateRects(downCAnonStorey0.gView);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      downCAnonStorey0.mousePosition = downCAnonStorey0.e.localMousePosition;
      // ISSUE: reference to a compiler-generated field
      downCAnonStorey0.mousePosition.x += this.layout.x;
      // ISSUE: reference to a compiler-generated field
      downCAnonStorey0.mousePosition.y += this.layout.y;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      downCAnonStorey0.gView.graphElements.ForEach(new System.Action<GraphElement>(downCAnonStorey0.\u003C\u003Em__0));
    }
  }
}
