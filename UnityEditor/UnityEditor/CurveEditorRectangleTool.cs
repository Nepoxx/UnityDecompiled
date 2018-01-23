// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditorRectangleTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class CurveEditorRectangleTool : RectangleTool
  {
    private static Rect g_EmptyRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private const int kHBarMinWidth = 14;
    private const int kHBarHeight = 13;
    private const int kHBarLeftWidth = 15;
    private const int kHBarLeftHeight = 13;
    private const int kHBarRightWidth = 15;
    private const int kHBarRightHeight = 13;
    private const int kHLabelMarginHorizontal = 3;
    private const int kHLabelMarginVertical = 1;
    private const int kVBarMinHeight = 15;
    private const int kVBarWidth = 13;
    private const int kVBarBottomWidth = 13;
    private const int kVBarBottomHeight = 15;
    private const int kVBarTopWidth = 13;
    private const int kVBarTopHeight = 15;
    private const int kVLabelMarginHorizontal = 1;
    private const int kVLabelMarginVertical = 2;
    private const int kScaleLeftWidth = 17;
    private const int kScaleLeftMarginHorizontal = 0;
    private const float kScaleLeftRatio = 0.8f;
    private const int kScaleRightWidth = 17;
    private const int kScaleRightMarginHorizontal = 0;
    private const float kScaleRightRatio = 0.8f;
    private const int kScaleBottomHeight = 17;
    private const int kScaleBottomMarginVertical = 0;
    private const float kScaleBottomRatio = 0.8f;
    private const int kScaleTopHeight = 17;
    private const int kScaleTopMarginVertical = 0;
    private const float kScaleTopRatio = 0.8f;
    private CurveEditor m_CurveEditor;
    private CurveEditorRectangleTool.ToolLayout m_Layout;
    private Vector2 m_Pivot;
    private Vector2 m_Previous;
    private Vector2 m_MouseOffset;
    private CurveEditorRectangleTool.DragMode m_DragMode;
    private bool m_RippleTime;
    private float m_RippleTimeStart;
    private float m_RippleTimeEnd;
    private AreaManipulator m_HBarLeft;
    private AreaManipulator m_HBarRight;
    private AreaManipulator m_HBar;
    private AreaManipulator m_VBarBottom;
    private AreaManipulator m_VBarTop;
    private AreaManipulator m_VBar;
    private AreaManipulator m_SelectionBox;
    private AreaManipulator m_SelectionScaleLeft;
    private AreaManipulator m_SelectionScaleRight;
    private AreaManipulator m_SelectionScaleBottom;
    private AreaManipulator m_SelectionScaleTop;

    private bool hasSelection
    {
      get
      {
        return this.m_CurveEditor.hasSelection && !this.m_CurveEditor.IsDraggingCurveOrRegion();
      }
    }

    private Bounds selectionBounds
    {
      get
      {
        return this.m_CurveEditor.selectionBounds;
      }
    }

    private float frameRate
    {
      get
      {
        return this.m_CurveEditor.invSnap;
      }
    }

    private CurveEditorRectangleTool.DragMode dragMode
    {
      get
      {
        if (this.m_DragMode != CurveEditorRectangleTool.DragMode.None)
          return this.m_DragMode;
        return this.m_CurveEditor.IsDraggingKey() ? CurveEditorRectangleTool.DragMode.MoveBothAxis : CurveEditorRectangleTool.DragMode.None;
      }
    }

    public override void Initialize(TimeArea timeArea)
    {
      base.Initialize(timeArea);
      this.m_CurveEditor = timeArea as CurveEditor;
      if (this.m_HBarLeft == null)
      {
        this.m_HBarLeft = new AreaManipulator(this.styles.rectangleToolHBarLeft, MouseCursor.ResizeHorizontal);
        AreaManipulator hbarLeft1 = this.m_HBarLeft;
        hbarLeft1.onStartDrag = hbarLeft1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Right, RectangleTool.ToolCoord.Left, new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), CurveEditorRectangleTool.DragMode.ScaleHorizontal, this.rippleTimeClutch);
          return true;
        });
        AreaManipulator hbarLeft2 = this.m_HBarLeft;
        hbarLeft2.onDrag = hbarLeft2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleTime(this.PixelToTime(evt.mousePosition.x, this.frameRate));
          return true;
        });
        AreaManipulator hbarLeft3 = this.m_HBarLeft;
        hbarLeft3.onEndDrag = hbarLeft3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_HBarRight == null)
      {
        this.m_HBarRight = new AreaManipulator(this.styles.rectangleToolHBarRight, MouseCursor.ResizeHorizontal);
        AreaManipulator hbarRight1 = this.m_HBarRight;
        hbarRight1.onStartDrag = hbarRight1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Left, RectangleTool.ToolCoord.Right, new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), CurveEditorRectangleTool.DragMode.ScaleHorizontal, this.rippleTimeClutch);
          return true;
        });
        AreaManipulator hbarRight2 = this.m_HBarRight;
        hbarRight2.onDrag = hbarRight2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleTime(this.PixelToTime(evt.mousePosition.x, this.frameRate));
          return true;
        });
        AreaManipulator hbarRight3 = this.m_HBarRight;
        hbarRight3.onEndDrag = hbarRight3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_HBar == null)
      {
        this.m_HBar = new AreaManipulator(this.styles.rectangleToolHBar, MouseCursor.MoveArrow);
        AreaManipulator hbar1 = this.m_HBar;
        hbar1.onStartDrag = hbar1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartMove(new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), CurveEditorRectangleTool.DragMode.MoveHorizontal, this.rippleTimeClutch);
          return true;
        });
        AreaManipulator hbar2 = this.m_HBar;
        hbar2.onDrag = hbar2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnMove(new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f));
          return true;
        });
        AreaManipulator hbar3 = this.m_HBar;
        hbar3.onEndDrag = hbar3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndMove();
          return true;
        });
      }
      if (this.m_VBarBottom == null)
      {
        this.m_VBarBottom = new AreaManipulator(this.styles.rectangleToolVBarBottom, MouseCursor.ResizeVertical);
        AreaManipulator vbarBottom1 = this.m_VBarBottom;
        vbarBottom1.onStartDrag = vbarBottom1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Top, RectangleTool.ToolCoord.Bottom, new Vector2(0.0f, this.PixelToValue(evt.mousePosition.y)), CurveEditorRectangleTool.DragMode.ScaleVertical, false);
          return true;
        });
        AreaManipulator vbarBottom2 = this.m_VBarBottom;
        vbarBottom2.onDrag = vbarBottom2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleValue(this.PixelToValue(evt.mousePosition.y));
          return true;
        });
        AreaManipulator vbarBottom3 = this.m_VBarBottom;
        vbarBottom3.onEndDrag = vbarBottom3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_VBarTop == null)
      {
        this.m_VBarTop = new AreaManipulator(this.styles.rectangleToolVBarTop, MouseCursor.ResizeVertical);
        AreaManipulator vbarTop1 = this.m_VBarTop;
        vbarTop1.onStartDrag = vbarTop1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Bottom, RectangleTool.ToolCoord.Top, new Vector2(0.0f, this.PixelToValue(evt.mousePosition.y)), CurveEditorRectangleTool.DragMode.ScaleVertical, false);
          return true;
        });
        AreaManipulator vbarTop2 = this.m_VBarTop;
        vbarTop2.onDrag = vbarTop2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleValue(this.PixelToValue(evt.mousePosition.y));
          return true;
        });
        AreaManipulator vbarTop3 = this.m_VBarTop;
        vbarTop3.onEndDrag = vbarTop3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_VBar == null)
      {
        this.m_VBar = new AreaManipulator(this.styles.rectangleToolVBar, MouseCursor.MoveArrow);
        AreaManipulator vbar1 = this.m_VBar;
        vbar1.onStartDrag = vbar1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartMove(new Vector2(0.0f, this.PixelToValue(evt.mousePosition.y)), CurveEditorRectangleTool.DragMode.MoveVertical, false);
          return true;
        });
        AreaManipulator vbar2 = this.m_VBar;
        vbar2.onDrag = vbar2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnMove(new Vector2(0.0f, this.PixelToValue(evt.mousePosition.y)));
          return true;
        });
        AreaManipulator vbar3 = this.m_VBar;
        vbar3.onEndDrag = vbar3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndMove();
          return true;
        });
      }
      if (this.m_SelectionBox == null)
      {
        this.m_SelectionBox = new AreaManipulator(this.styles.rectangleToolSelection, MouseCursor.MoveArrow);
        AreaManipulator selectionBox1 = this.m_SelectionBox;
        selectionBox1.onStartDrag = selectionBox1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (evt.shift || EditorGUI.actionKey || !this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartMove(new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), this.PixelToValue(evt.mousePosition.y)), !this.rippleTimeClutch ? CurveEditorRectangleTool.DragMode.MoveBothAxis : CurveEditorRectangleTool.DragMode.MoveHorizontal, this.rippleTimeClutch);
          return true;
        });
        AreaManipulator selectionBox2 = this.m_SelectionBox;
        selectionBox2.onDrag = selectionBox2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          if (evt.shift && this.m_DragMode == CurveEditorRectangleTool.DragMode.MoveBothAxis)
            this.m_DragMode = (double) Mathf.Abs(evt.mousePosition.x - this.TimeToPixel(this.m_Previous.x)) <= (double) Mathf.Abs(evt.mousePosition.y - this.ValueToPixel(this.m_Previous.y)) ? CurveEditorRectangleTool.DragMode.MoveVertical : CurveEditorRectangleTool.DragMode.MoveHorizontal;
          this.OnMove(new Vector2((this.m_DragMode & CurveEditorRectangleTool.DragMode.MoveHorizontal) == CurveEditorRectangleTool.DragMode.None ? this.m_Previous.x : this.PixelToTime(evt.mousePosition.x, this.frameRate), (this.m_DragMode & CurveEditorRectangleTool.DragMode.MoveVertical) == CurveEditorRectangleTool.DragMode.None ? this.m_Previous.y : this.PixelToValue(evt.mousePosition.y)));
          return true;
        });
        AreaManipulator selectionBox3 = this.m_SelectionBox;
        selectionBox3.onEndDrag = selectionBox3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndMove();
          return true;
        });
      }
      if (this.m_SelectionScaleLeft == null)
      {
        this.m_SelectionScaleLeft = new AreaManipulator(this.styles.rectangleToolScaleLeft, MouseCursor.ResizeHorizontal);
        AreaManipulator selectionScaleLeft1 = this.m_SelectionScaleLeft;
        selectionScaleLeft1.onStartDrag = selectionScaleLeft1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Right, RectangleTool.ToolCoord.Left, new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), CurveEditorRectangleTool.DragMode.ScaleHorizontal, this.rippleTimeClutch);
          return true;
        });
        AreaManipulator selectionScaleLeft2 = this.m_SelectionScaleLeft;
        selectionScaleLeft2.onDrag = selectionScaleLeft2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleTime(this.PixelToTime(evt.mousePosition.x, this.frameRate));
          return true;
        });
        AreaManipulator selectionScaleLeft3 = this.m_SelectionScaleLeft;
        selectionScaleLeft3.onEndDrag = selectionScaleLeft3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_SelectionScaleRight == null)
      {
        this.m_SelectionScaleRight = new AreaManipulator(this.styles.rectangleToolScaleRight, MouseCursor.ResizeHorizontal);
        AreaManipulator selectionScaleRight1 = this.m_SelectionScaleRight;
        selectionScaleRight1.onStartDrag = selectionScaleRight1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Left, RectangleTool.ToolCoord.Right, new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), CurveEditorRectangleTool.DragMode.ScaleHorizontal, this.rippleTimeClutch);
          return true;
        });
        AreaManipulator selectionScaleRight2 = this.m_SelectionScaleRight;
        selectionScaleRight2.onDrag = selectionScaleRight2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleTime(this.PixelToTime(evt.mousePosition.x, this.frameRate));
          return true;
        });
        AreaManipulator selectionScaleRight3 = this.m_SelectionScaleRight;
        selectionScaleRight3.onEndDrag = selectionScaleRight3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_SelectionScaleBottom == null)
      {
        this.m_SelectionScaleBottom = new AreaManipulator(this.styles.rectangleToolScaleBottom, MouseCursor.ResizeVertical);
        AreaManipulator selectionScaleBottom1 = this.m_SelectionScaleBottom;
        selectionScaleBottom1.onStartDrag = selectionScaleBottom1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Top, RectangleTool.ToolCoord.Bottom, new Vector2(0.0f, this.PixelToValue(evt.mousePosition.y)), CurveEditorRectangleTool.DragMode.ScaleVertical, false);
          return true;
        });
        AreaManipulator selectionScaleBottom2 = this.m_SelectionScaleBottom;
        selectionScaleBottom2.onDrag = selectionScaleBottom2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
        {
          this.OnScaleValue(this.PixelToValue(evt.mousePosition.y));
          return true;
        });
        AreaManipulator selectionScaleBottom3 = this.m_SelectionScaleBottom;
        selectionScaleBottom3.onEndDrag = selectionScaleBottom3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
        {
          this.OnEndScale();
          return true;
        });
      }
      if (this.m_SelectionScaleTop != null)
        return;
      this.m_SelectionScaleTop = new AreaManipulator(this.styles.rectangleToolScaleTop, MouseCursor.ResizeVertical);
      AreaManipulator selectionScaleTop1 = this.m_SelectionScaleTop;
      selectionScaleTop1.onStartDrag = selectionScaleTop1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
      {
        if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
          return false;
        this.OnStartScale(RectangleTool.ToolCoord.Bottom, RectangleTool.ToolCoord.Top, new Vector2(0.0f, this.PixelToValue(evt.mousePosition.y)), CurveEditorRectangleTool.DragMode.ScaleVertical, false);
        return true;
      });
      AreaManipulator selectionScaleTop2 = this.m_SelectionScaleTop;
      selectionScaleTop2.onDrag = selectionScaleTop2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
      {
        this.OnScaleValue(this.PixelToValue(evt.mousePosition.y));
        return true;
      });
      AreaManipulator selectionScaleTop3 = this.m_SelectionScaleTop;
      selectionScaleTop3.onEndDrag = selectionScaleTop3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
      {
        this.OnEndScale();
        return true;
      });
    }

    public void OnGUI()
    {
      if (!this.hasSelection || Event.current.type != EventType.Repaint)
        return;
      CurveEditorSettings.RectangleToolFlags rectangleToolFlags = this.m_CurveEditor.settings.rectangleToolFlags;
      if (rectangleToolFlags == CurveEditorSettings.RectangleToolFlags.NoRectangleTool)
        return;
      Color color = GUI.color;
      GUI.color = Color.white;
      this.m_Layout = this.CalculateLayout();
      if (rectangleToolFlags == CurveEditorSettings.RectangleToolFlags.FullRectangleTool)
      {
        GUI.Label(this.m_Layout.selectionLeftRect, GUIContent.none, this.styles.rectangleToolHighlight);
        GUI.Label(this.m_Layout.selectionTopRect, GUIContent.none, this.styles.rectangleToolHighlight);
        GUI.Label(this.m_Layout.underlayLeftRect, GUIContent.none, this.styles.rectangleToolHighlight);
        GUI.Label(this.m_Layout.underlayTopRect, GUIContent.none, this.styles.rectangleToolHighlight);
      }
      this.m_SelectionBox.OnGUI(this.m_Layout.selectionRect);
      this.m_SelectionScaleTop.OnGUI(this.m_Layout.scaleTopRect);
      this.m_SelectionScaleBottom.OnGUI(this.m_Layout.scaleBottomRect);
      this.m_SelectionScaleLeft.OnGUI(this.m_Layout.scaleLeftRect);
      this.m_SelectionScaleRight.OnGUI(this.m_Layout.scaleRightRect);
      GUI.color = color;
    }

    public void OverlayOnGUI()
    {
      if (!this.hasSelection || Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      if (this.m_CurveEditor.settings.rectangleToolFlags == CurveEditorSettings.RectangleToolFlags.FullRectangleTool)
      {
        GUI.color = Color.white;
        this.m_HBar.OnGUI(this.m_Layout.hBarRect);
        this.m_HBarLeft.OnGUI(this.m_Layout.hBarLeftRect);
        this.m_HBarRight.OnGUI(this.m_Layout.hBarRightRect);
        this.m_VBar.OnGUI(this.m_Layout.vBarRect);
        this.m_VBarBottom.OnGUI(this.m_Layout.vBarBottomRect);
        this.m_VBarTop.OnGUI(this.m_Layout.vBarTopRect);
      }
      this.DrawLabels();
      GUI.color = color;
    }

    public void HandleEvents()
    {
      if (this.m_CurveEditor.settings.rectangleToolFlags == CurveEditorSettings.RectangleToolFlags.NoRectangleTool)
        return;
      this.m_SelectionScaleTop.HandleEvents();
      this.m_SelectionScaleBottom.HandleEvents();
      this.m_SelectionScaleLeft.HandleEvents();
      this.m_SelectionScaleRight.HandleEvents();
      this.m_SelectionBox.HandleEvents();
    }

    public void HandleOverlayEvents()
    {
      this.HandleClutchKeys();
      switch (this.m_CurveEditor.settings.rectangleToolFlags)
      {
        case CurveEditorSettings.RectangleToolFlags.FullRectangleTool:
          this.m_VBarBottom.HandleEvents();
          this.m_VBarTop.HandleEvents();
          this.m_VBar.HandleEvents();
          this.m_HBarLeft.HandleEvents();
          this.m_HBarRight.HandleEvents();
          this.m_HBar.HandleEvents();
          break;
      }
    }

    private CurveEditorRectangleTool.ToolLayout CalculateLayout()
    {
      CurveEditorRectangleTool.ToolLayout toolLayout = new CurveEditorRectangleTool.ToolLayout();
      bool flag1 = !Mathf.Approximately(this.selectionBounds.size.x, 0.0f);
      bool flag2 = !Mathf.Approximately(this.selectionBounds.size.y, 0.0f);
      float pixel1 = this.TimeToPixel(this.selectionBounds.min.x);
      float pixel2 = this.TimeToPixel(this.selectionBounds.max.x);
      float pixel3 = this.ValueToPixel(this.selectionBounds.max.y);
      float pixel4 = this.ValueToPixel(this.selectionBounds.min.y);
      toolLayout.selectionRect = new Rect(pixel1, pixel3, pixel2 - pixel1, pixel4 - pixel3);
      toolLayout.displayHScale = true;
      float width = (float) ((double) toolLayout.selectionRect.width - 15.0 - 15.0);
      if ((double) width < 14.0)
      {
        toolLayout.displayHScale = false;
        width = toolLayout.selectionRect.width;
        if ((double) width < 14.0)
        {
          toolLayout.selectionRect.x = toolLayout.selectionRect.center.x - 7f;
          toolLayout.selectionRect.width = 14f;
          width = 14f;
        }
      }
      if (toolLayout.displayHScale)
      {
        toolLayout.hBarLeftRect = new Rect(toolLayout.selectionRect.xMin, this.contentRect.yMin, 15f, 13f);
        toolLayout.hBarRect = new Rect(toolLayout.hBarLeftRect.xMax, this.contentRect.yMin, width, 13f);
        toolLayout.hBarRightRect = new Rect(toolLayout.hBarRect.xMax, this.contentRect.yMin, 15f, 13f);
      }
      else
      {
        toolLayout.hBarRect = new Rect(toolLayout.selectionRect.xMin, this.contentRect.yMin, width, 13f);
        toolLayout.hBarLeftRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        toolLayout.hBarRightRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      }
      toolLayout.displayVScale = true;
      float height = (float) ((double) toolLayout.selectionRect.height - 15.0 - 15.0);
      if ((double) height < 15.0)
      {
        toolLayout.displayVScale = false;
        height = toolLayout.selectionRect.height;
        if ((double) height < 15.0)
        {
          toolLayout.selectionRect.y = toolLayout.selectionRect.center.y - 7.5f;
          toolLayout.selectionRect.height = 15f;
          height = 15f;
        }
      }
      if (toolLayout.displayVScale)
      {
        toolLayout.vBarTopRect = new Rect(this.contentRect.xMin, toolLayout.selectionRect.yMin, 13f, 15f);
        toolLayout.vBarRect = new Rect(this.contentRect.xMin, toolLayout.vBarTopRect.yMax, 13f, height);
        toolLayout.vBarBottomRect = new Rect(this.contentRect.xMin, toolLayout.vBarRect.yMax, 13f, 15f);
      }
      else
      {
        toolLayout.vBarRect = new Rect(this.contentRect.xMin, toolLayout.selectionRect.yMin, 13f, height);
        toolLayout.vBarTopRect = CurveEditorRectangleTool.g_EmptyRect;
        toolLayout.vBarBottomRect = CurveEditorRectangleTool.g_EmptyRect;
      }
      if (flag1)
      {
        float num1 = 0.09999999f;
        float num2 = 0.09999999f;
        toolLayout.scaleLeftRect = new Rect(toolLayout.selectionRect.xMin - 17f, toolLayout.selectionRect.yMin + toolLayout.selectionRect.height * num1, 17f, toolLayout.selectionRect.height * 0.8f);
        toolLayout.scaleRightRect = new Rect(toolLayout.selectionRect.xMax, toolLayout.selectionRect.yMin + toolLayout.selectionRect.height * num2, 17f, toolLayout.selectionRect.height * 0.8f);
      }
      else
      {
        toolLayout.scaleLeftRect = CurveEditorRectangleTool.g_EmptyRect;
        toolLayout.scaleRightRect = CurveEditorRectangleTool.g_EmptyRect;
      }
      if (flag2)
      {
        float num1 = 0.09999999f;
        float num2 = 0.09999999f;
        toolLayout.scaleTopRect = new Rect(toolLayout.selectionRect.xMin + toolLayout.selectionRect.width * num2, toolLayout.selectionRect.yMin - 17f, toolLayout.selectionRect.width * 0.8f, 17f);
        toolLayout.scaleBottomRect = new Rect(toolLayout.selectionRect.xMin + toolLayout.selectionRect.width * num1, toolLayout.selectionRect.yMax, toolLayout.selectionRect.width * 0.8f, 17f);
      }
      else
      {
        toolLayout.scaleTopRect = CurveEditorRectangleTool.g_EmptyRect;
        toolLayout.scaleBottomRect = CurveEditorRectangleTool.g_EmptyRect;
      }
      if (flag1)
      {
        toolLayout.leftLabelAnchor = new Vector2(toolLayout.selectionRect.xMin - 3f, this.contentRect.yMin + 1f);
        toolLayout.rightLabelAnchor = new Vector2(toolLayout.selectionRect.xMax + 3f, this.contentRect.yMin + 1f);
      }
      else
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        CurveEditorRectangleTool.ToolLayout& local1 = @toolLayout;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        CurveEditorRectangleTool.ToolLayout& local2 = @toolLayout;
        double num1 = (double) toolLayout.selectionRect.xMax + 3.0;
        double num2 = (double) this.contentRect.yMin + 1.0;
        Vector2 vector2_1;
        Vector2 vector2_2 = vector2_1 = new Vector2((float) num1, (float) num2);
        // ISSUE: explicit reference operation
        (^local2).rightLabelAnchor = vector2_1;
        Vector2 vector2_3 = vector2_2;
        // ISSUE: explicit reference operation
        (^local1).leftLabelAnchor = vector2_3;
      }
      if (flag2)
      {
        toolLayout.bottomLabelAnchor = new Vector2(this.contentRect.xMin + 1f, toolLayout.selectionRect.yMax + 2f);
        toolLayout.topLabelAnchor = new Vector2(this.contentRect.xMin + 1f, toolLayout.selectionRect.yMin - 2f);
      }
      else
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        CurveEditorRectangleTool.ToolLayout& local1 = @toolLayout;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        CurveEditorRectangleTool.ToolLayout& local2 = @toolLayout;
        Rect contentRect = this.contentRect;
        Vector2 vector2_1;
        Vector2 vector2_2 = vector2_1 = new Vector2(contentRect.xMin + 1f, toolLayout.selectionRect.yMin - 2f);
        // ISSUE: explicit reference operation
        (^local2).topLabelAnchor = vector2_1;
        Vector2 vector2_3 = vector2_2;
        // ISSUE: explicit reference operation
        (^local1).bottomLabelAnchor = vector2_3;
      }
      toolLayout.selectionLeftRect = new Rect(this.contentRect.xMin + 13f, toolLayout.selectionRect.yMin, toolLayout.selectionRect.xMin - 13f, toolLayout.selectionRect.height);
      toolLayout.selectionTopRect = new Rect(toolLayout.selectionRect.xMin, this.contentRect.yMin + 13f, toolLayout.selectionRect.width, toolLayout.selectionRect.yMin - 13f);
      toolLayout.underlayTopRect = new Rect(this.contentRect.xMin, this.contentRect.yMin, this.contentRect.width, 13f);
      toolLayout.underlayLeftRect = new Rect(this.contentRect.xMin, this.contentRect.yMin + 13f, 13f, this.contentRect.height - 13f);
      return toolLayout;
    }

    private void DrawLabels()
    {
      if (this.dragMode == CurveEditorRectangleTool.DragMode.None)
        return;
      CurveEditorSettings.RectangleToolFlags rectangleToolFlags = this.m_CurveEditor.settings.rectangleToolFlags;
      bool flag1 = !Mathf.Approximately(this.selectionBounds.size.x, 0.0f);
      bool flag2 = !Mathf.Approximately(this.selectionBounds.size.y, 0.0f);
      switch (rectangleToolFlags)
      {
        case CurveEditorSettings.RectangleToolFlags.MiniRectangleTool:
          if ((this.dragMode & CurveEditorRectangleTool.DragMode.MoveBothAxis) != CurveEditorRectangleTool.DragMode.None)
          {
            Vector2 vector2_1 = flag1 || flag2 ? new Vector2(this.PixelToTime(Event.current.mousePosition.x, this.frameRate), this.PixelToValue(Event.current.mousePosition.y)) : (Vector2) this.selectionBounds.center;
            Vector2 vector2_2 = new Vector2(this.TimeToPixel(vector2_1.x), this.ValueToPixel(vector2_1.y));
            GUIContent content = new GUIContent(string.Format("{0}, {1}", (object) this.m_CurveEditor.FormatTime(vector2_1.x, this.m_CurveEditor.invSnap, this.m_CurveEditor.timeFormat), (object) this.m_CurveEditor.FormatValue(vector2_1.y)));
            Vector2 vector2_3 = this.styles.dragLabel.CalcSize(content);
            EditorGUI.DoDropShadowLabel(new Rect(vector2_2.x, vector2_2.y - vector2_3.y, vector2_3.x, vector2_3.y), content, this.styles.dragLabel, 0.3f);
          }
          break;
        case CurveEditorSettings.RectangleToolFlags.FullRectangleTool:
          if ((this.dragMode & CurveEditorRectangleTool.DragMode.MoveScaleHorizontal) != CurveEditorRectangleTool.DragMode.None)
          {
            if (flag1)
            {
              GUIContent content1 = new GUIContent(string.Format("{0}", (object) this.m_CurveEditor.FormatTime(this.selectionBounds.min.x, this.m_CurveEditor.invSnap, this.m_CurveEditor.timeFormat)));
              GUIContent content2 = new GUIContent(string.Format("{0}", (object) this.m_CurveEditor.FormatTime(this.selectionBounds.max.x, this.m_CurveEditor.invSnap, this.m_CurveEditor.timeFormat)));
              Vector2 vector2_1 = this.styles.dragLabel.CalcSize(content1);
              Vector2 vector2_2 = this.styles.dragLabel.CalcSize(content2);
              EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.leftLabelAnchor.x - vector2_1.x, this.m_Layout.leftLabelAnchor.y, vector2_1.x, vector2_1.y), content1, this.styles.dragLabel, 0.3f);
              EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.rightLabelAnchor.x, this.m_Layout.rightLabelAnchor.y, vector2_2.x, vector2_2.y), content2, this.styles.dragLabel, 0.3f);
            }
            else
            {
              GUIContent content = new GUIContent(string.Format("{0}", (object) this.m_CurveEditor.FormatTime(this.selectionBounds.center.x, this.m_CurveEditor.invSnap, this.m_CurveEditor.timeFormat)));
              Vector2 vector2 = this.styles.dragLabel.CalcSize(content);
              EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.leftLabelAnchor.x, this.m_Layout.leftLabelAnchor.y, vector2.x, vector2.y), content, this.styles.dragLabel, 0.3f);
            }
          }
          if ((this.dragMode & CurveEditorRectangleTool.DragMode.MoveScaleVertical) == CurveEditorRectangleTool.DragMode.None)
            break;
          if (flag2)
          {
            GUIContent content1 = new GUIContent(string.Format("{0}", (object) this.m_CurveEditor.FormatValue(this.selectionBounds.min.y)));
            GUIContent content2 = new GUIContent(string.Format("{0}", (object) this.m_CurveEditor.FormatValue(this.selectionBounds.max.y)));
            Vector2 vector2_1 = this.styles.dragLabel.CalcSize(content1);
            Vector2 vector2_2 = this.styles.dragLabel.CalcSize(content2);
            EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.bottomLabelAnchor.x, this.m_Layout.bottomLabelAnchor.y, vector2_1.x, vector2_1.y), content1, this.styles.dragLabel, 0.3f);
            EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.topLabelAnchor.x, this.m_Layout.topLabelAnchor.y - vector2_2.y, vector2_2.x, vector2_2.y), content2, this.styles.dragLabel, 0.3f);
          }
          else
          {
            GUIContent content = new GUIContent(string.Format("{0}", (object) this.m_CurveEditor.FormatValue(this.selectionBounds.center.y)));
            Vector2 vector2 = this.styles.dragLabel.CalcSize(content);
            EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.topLabelAnchor.x, this.m_Layout.topLabelAnchor.y - vector2.y, vector2.x, vector2.y), content, this.styles.dragLabel, 0.3f);
          }
          break;
      }
    }

    private void OnStartScale(RectangleTool.ToolCoord pivotCoord, RectangleTool.ToolCoord pickedCoord, Vector2 mousePos, CurveEditorRectangleTool.DragMode dragMode, bool rippleTime)
    {
      Bounds selectionBounds = this.selectionBounds;
      this.m_DragMode = dragMode;
      this.m_Pivot = this.ToolCoordToPosition(pivotCoord, selectionBounds);
      this.m_Previous = this.ToolCoordToPosition(pickedCoord, selectionBounds);
      this.m_MouseOffset = mousePos - this.m_Previous;
      this.m_RippleTime = rippleTime;
      this.m_RippleTimeStart = selectionBounds.min.x;
      this.m_RippleTimeEnd = selectionBounds.max.x;
      this.m_CurveEditor.StartLiveEdit();
    }

    private void OnScaleTime(float time)
    {
      Matrix4x4 transform;
      bool flipKeys;
      if (!this.CalculateScaleTimeMatrix(this.m_Previous.x, time, this.m_MouseOffset.x, this.m_Pivot.x, this.frameRate, out transform, out flipKeys))
        return;
      this.TransformKeys(transform, flipKeys, false);
    }

    private void OnScaleValue(float val)
    {
      Matrix4x4 transform;
      bool flipKeys;
      if (!this.CalculateScaleValueMatrix(this.m_Previous.y, val, this.m_MouseOffset.y, this.m_Pivot.y, out transform, out flipKeys))
        return;
      this.TransformKeys(transform, false, flipKeys);
    }

    private void OnEndScale()
    {
      this.m_CurveEditor.EndLiveEdit();
      this.m_DragMode = CurveEditorRectangleTool.DragMode.None;
      GUI.changed = true;
    }

    internal void OnStartMove(Vector2 position, bool rippleTime)
    {
      this.OnStartMove(position, CurveEditorRectangleTool.DragMode.None, rippleTime);
    }

    private void OnStartMove(Vector2 position, CurveEditorRectangleTool.DragMode dragMode, bool rippleTime)
    {
      Bounds selectionBounds = this.selectionBounds;
      this.m_DragMode = dragMode;
      this.m_Previous = position;
      this.m_RippleTime = rippleTime;
      this.m_RippleTimeStart = selectionBounds.min.x;
      this.m_RippleTimeEnd = selectionBounds.max.x;
      this.m_CurveEditor.StartLiveEdit();
    }

    internal void OnMove(Vector2 position)
    {
      Vector2 vector2 = position - this.m_Previous;
      Matrix4x4 identity = Matrix4x4.identity;
      identity.SetTRS(new Vector3(vector2.x, vector2.y, 0.0f), Quaternion.identity, Vector3.one);
      this.TransformKeys(identity, false, false);
    }

    internal void OnEndMove()
    {
      this.m_CurveEditor.EndLiveEdit();
      this.m_DragMode = CurveEditorRectangleTool.DragMode.None;
      GUI.changed = true;
    }

    private void TransformKeys(Matrix4x4 matrix, bool flipX, bool flipY)
    {
      if (this.m_RippleTime)
      {
        this.m_CurveEditor.TransformRippleKeys(matrix, this.m_RippleTimeStart, this.m_RippleTimeEnd, flipX);
        GUI.changed = true;
      }
      else
      {
        this.m_CurveEditor.TransformSelectedKeys(matrix, flipX, flipY);
        GUI.changed = true;
      }
    }

    private struct ToolLayout
    {
      public Rect selectionRect;
      public Rect hBarRect;
      public Rect hBarLeftRect;
      public Rect hBarRightRect;
      public bool displayHScale;
      public Rect vBarRect;
      public Rect vBarBottomRect;
      public Rect vBarTopRect;
      public bool displayVScale;
      public Rect selectionLeftRect;
      public Rect selectionTopRect;
      public Rect underlayTopRect;
      public Rect underlayLeftRect;
      public Rect scaleLeftRect;
      public Rect scaleRightRect;
      public Rect scaleTopRect;
      public Rect scaleBottomRect;
      public Vector2 leftLabelAnchor;
      public Vector2 rightLabelAnchor;
      public Vector2 bottomLabelAnchor;
      public Vector2 topLabelAnchor;
    }

    private enum DragMode
    {
      None = 0,
      MoveHorizontal = 1,
      MoveVertical = 2,
      MoveBothAxis = 3,
      ScaleHorizontal = 4,
      MoveScaleHorizontal = 5,
      ScaleVertical = 8,
      MoveScaleVertical = 10, // 0x0000000A
      ScaleBothAxis = 12, // 0x0000000C
    }
  }
}
