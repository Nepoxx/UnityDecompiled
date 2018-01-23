// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.Resizer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class Resizer : VisualElement
  {
    private GUIContent m_LabelText = new GUIContent();
    private readonly Rect k_WidgetTextOffset = new Rect(0.0f, 0.0f, 5f, 5f);
    private Vector2 m_Start;
    private Rect m_StartPos;
    private Vector2 m_MinimumSize;
    private GUIStyle m_StyleWidget;
    private GUIStyle m_StyleLabel;
    private bool m_Active;

    public Resizer()
      : this(new Vector2(30f, 30f))
    {
    }

    public Resizer(Vector2 minimumSize)
    {
      this.m_MinimumSize = minimumSize;
      this.style.positionType = (StyleValue<PositionType>) PositionType.Absolute;
      this.style.positionTop = (StyleValue<float>) float.NaN;
      this.style.positionLeft = (StyleValue<float>) float.NaN;
      this.style.positionBottom = (StyleValue<float>) 0.0f;
      this.style.positionRight = (StyleValue<float>) 0.0f;
      this.style.paddingLeft = (StyleValue<float>) 10f;
      this.style.paddingTop = (StyleValue<float>) 14f;
      this.style.width = (StyleValue<float>) 20f;
      this.style.height = (StyleValue<float>) 20f;
      this.m_Active = false;
      this.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
    }

    public MouseButton activateButton { get; set; }

    private Texture image { get; set; }

    private void OnMouseDown(MouseDownEvent e)
    {
      GraphElement parent = this.parent as GraphElement;
      if (parent == null)
        return;
      GraphElementPresenter presenter = parent.presenter;
      if ((Object) presenter == (Object) null || (presenter.capabilities & Capabilities.Resizable) != Capabilities.Resizable || (MouseButton) e.button != this.activateButton)
        return;
      this.m_Start = this.ChangeCoordinatesTo(this.parent, e.localMousePosition);
      this.m_StartPos = this.parent.layout;
      if ((PositionType) this.parent.style.positionType != PositionType.Manual)
        Debug.LogWarning((object) "Attempting to resize an object with a non manual position");
      this.m_Active = true;
      this.TakeCapture();
      e.StopPropagation();
    }

    private void OnMouseUp(MouseUpEvent e)
    {
      GraphElement parent = this.parent as GraphElement;
      if (parent == null)
        return;
      GraphElementPresenter presenter = parent.presenter;
      if ((Object) presenter == (Object) null || (presenter.capabilities & Capabilities.Resizable) != Capabilities.Resizable || (!this.m_Active || (MouseButton) e.button != this.activateButton) || !this.m_Active)
        return;
      this.m_Active = false;
      this.ReleaseCapture();
      e.StopPropagation();
    }

    private void OnMouseMove(MouseMoveEvent e)
    {
      GraphElement parent = this.parent as GraphElement;
      if (parent == null)
        return;
      GraphElementPresenter presenter = parent.presenter;
      if ((Object) presenter == (Object) null || (presenter.capabilities & Capabilities.Resizable) != Capabilities.Resizable || (!this.m_Active || (PositionType) this.parent.style.positionType != PositionType.Manual))
        return;
      Vector2 vector2_1 = this.ChangeCoordinatesTo(this.parent, e.localMousePosition) - this.m_Start;
      Vector2 vector2_2 = new Vector2(this.m_StartPos.width + vector2_1.x, this.m_StartPos.height + vector2_1.y);
      if ((double) vector2_2.x < (double) this.m_MinimumSize.x)
        vector2_2.x = this.m_MinimumSize.x;
      if ((double) vector2_2.y < (double) this.m_MinimumSize.y)
        vector2_2.y = this.m_MinimumSize.y;
      presenter.position = new Rect(presenter.position.x, presenter.position.y, vector2_2.x, vector2_2.y);
      this.m_LabelText.text = string.Format("{0:0}", (object) this.parent.layout.width) + "x" + string.Format("{0:0}", (object) this.parent.layout.height);
      e.StopPropagation();
    }

    public override void DoRepaint()
    {
      if (this.m_StyleWidget == null)
      {
        this.m_StyleWidget = new GUIStyle((GUIStyle) "WindowBottomResize")
        {
          fixedHeight = 0.0f
        };
        this.image = (Texture) this.m_StyleWidget.normal.background;
      }
      if ((Object) this.image == (Object) null)
      {
        Debug.LogWarning((object) "null texture passed to GUI.DrawTexture");
      }
      else
      {
        GUI.DrawTexture(this.contentRect, this.image, ScaleMode.ScaleAndCrop, true, 0.0f, GUI.color, 0.0f, 0.0f);
        if (this.m_StyleLabel == null)
          this.m_StyleLabel = new GUIStyle((GUIStyle) "Label");
        if (!this.m_Active)
          return;
        Rect widgetTextOffset = this.k_WidgetTextOffset;
        this.m_StyleLabel.Draw(new Rect(this.layout.max.x + widgetTextOffset.width, this.layout.max.y + widgetTextOffset.height, 200f, 20f), this.m_LabelText, false, false, false, false);
      }
    }
  }
}
