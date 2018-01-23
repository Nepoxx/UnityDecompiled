// Decompiled with JetBrains decompiler
// Type: UnityEditor.DopeSheetEditorRectangleTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class DopeSheetEditorRectangleTool : RectangleTool
  {
    private static Rect g_EmptyRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private const float kDefaultFrameRate = 60f;
    private const int kScaleLeftWidth = 17;
    private const int kScaleLeftMarginHorizontal = 0;
    private const float kScaleLeftMarginVertical = 4f;
    private const int kScaleRightWidth = 17;
    private const int kScaleRightMarginHorizontal = 0;
    private const float kScaleRightMarginVertical = 4f;
    private const int kHLabelMarginHorizontal = 8;
    private const int kHLabelMarginVertical = 1;
    private DopeSheetEditor m_DopeSheetEditor;
    private AnimationWindowState m_State;
    private DopeSheetEditorRectangleTool.ToolLayout m_Layout;
    private Vector2 m_Pivot;
    private Vector2 m_Previous;
    private Vector2 m_MouseOffset;
    private bool m_IsDragging;
    private bool m_RippleTime;
    private float m_RippleTimeStart;
    private float m_RippleTimeEnd;
    private AreaManipulator[] m_SelectionBoxes;
    private AreaManipulator m_SelectionScaleLeft;
    private AreaManipulator m_SelectionScaleRight;

    private bool hasSelection
    {
      get
      {
        return this.m_State.selectedKeys.Count > 0;
      }
    }

    private Bounds selectionBounds
    {
      get
      {
        return this.m_State.selectionBounds;
      }
    }

    private float frameRate
    {
      get
      {
        return this.m_State.frameRate;
      }
    }

    private bool isDragging
    {
      get
      {
        return this.m_IsDragging || this.m_DopeSheetEditor.isDragging;
      }
    }

    public override void Initialize(TimeArea timeArea)
    {
      base.Initialize(timeArea);
      this.m_DopeSheetEditor = timeArea as DopeSheetEditor;
      this.m_State = this.m_DopeSheetEditor.state;
      if (this.m_SelectionBoxes == null)
      {
        this.m_SelectionBoxes = new AreaManipulator[2];
        for (int index = 0; index < 2; ++index)
        {
          this.m_SelectionBoxes[index] = new AreaManipulator(this.styles.rectangleToolSelection, MouseCursor.MoveArrow);
          AreaManipulator selectionBox1 = this.m_SelectionBoxes[index];
          selectionBox1.onStartDrag = selectionBox1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
          {
            if (evt.shift || EditorGUI.actionKey || !this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
              return false;
            this.OnStartMove(new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), this.rippleTimeClutch);
            return true;
          });
          AreaManipulator selectionBox2 = this.m_SelectionBoxes[index];
          selectionBox2.onDrag = selectionBox2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) =>
          {
            this.OnMove(new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f));
            return true;
          });
          AreaManipulator selectionBox3 = this.m_SelectionBoxes[index];
          selectionBox3.onEndDrag = selectionBox3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) =>
          {
            this.OnEndMove();
            return true;
          });
        }
      }
      if (this.m_SelectionScaleLeft == null)
      {
        this.m_SelectionScaleLeft = new AreaManipulator(this.styles.dopesheetScaleLeft, MouseCursor.ResizeHorizontal);
        AreaManipulator selectionScaleLeft1 = this.m_SelectionScaleLeft;
        selectionScaleLeft1.onStartDrag = selectionScaleLeft1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
        {
          if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
            return false;
          this.OnStartScale(RectangleTool.ToolCoord.Right, RectangleTool.ToolCoord.Left, new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), this.rippleTimeClutch);
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
      if (this.m_SelectionScaleRight != null)
        return;
      this.m_SelectionScaleRight = new AreaManipulator(this.styles.dopesheetScaleRight, MouseCursor.ResizeHorizontal);
      AreaManipulator selectionScaleRight1 = this.m_SelectionScaleRight;
      selectionScaleRight1.onStartDrag = selectionScaleRight1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
      {
        if (!this.hasSelection || !manipulator.rect.Contains(evt.mousePosition))
          return false;
        this.OnStartScale(RectangleTool.ToolCoord.Left, RectangleTool.ToolCoord.Right, new Vector2(this.PixelToTime(evt.mousePosition.x, this.frameRate), 0.0f), this.rippleTimeClutch);
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

    public void OnGUI()
    {
      if (!this.hasSelection || Event.current.type != EventType.Repaint)
        return;
      this.m_Layout = this.CalculateLayout();
      this.m_SelectionBoxes[0].OnGUI(this.m_Layout.summaryRect);
      this.m_SelectionBoxes[1].OnGUI(this.m_Layout.selectionRect);
      this.m_SelectionScaleLeft.OnGUI(this.m_Layout.scaleLeftRect);
      this.m_SelectionScaleRight.OnGUI(this.m_Layout.scaleRightRect);
      this.DrawLabels();
    }

    public void HandleEvents()
    {
      this.HandleClutchKeys();
      this.m_SelectionScaleLeft.HandleEvents();
      this.m_SelectionScaleRight.HandleEvents();
      this.m_SelectionBoxes[0].HandleEvents();
      this.m_SelectionBoxes[1].HandleEvents();
    }

    private DopeSheetEditorRectangleTool.ToolLayout CalculateLayout()
    {
      DopeSheetEditorRectangleTool.ToolLayout toolLayout = new DopeSheetEditorRectangleTool.ToolLayout();
      Bounds selectionBounds = this.selectionBounds;
      bool flag1 = !Mathf.Approximately(selectionBounds.size.x, 0.0f);
      float pixel1 = this.TimeToPixel(selectionBounds.min.x);
      float pixel2 = this.TimeToPixel(selectionBounds.max.x);
      float y = 0.0f;
      float num1 = 0.0f;
      bool flag2 = true;
      float num2 = 0.0f;
      List<DopeLine> dopelines = this.m_State.dopelines;
      for (int index1 = 0; index1 < dopelines.Count; ++index1)
      {
        DopeLine dopeLine = dopelines[index1];
        float num3 = !dopeLine.tallMode ? 16f : 32f;
        if (!dopeLine.isMasterDopeline)
        {
          int count = dopeLine.keys.Count;
          for (int index2 = 0; index2 < count; ++index2)
          {
            if (this.m_State.KeyIsSelected(dopeLine.keys[index2]))
            {
              if (flag2)
              {
                y = num2;
                flag2 = false;
              }
              num1 = num2 + num3;
              break;
            }
          }
        }
        num2 += num3;
      }
      toolLayout.summaryRect = new Rect(pixel1, 0.0f, pixel2 - pixel1, 16f);
      toolLayout.selectionRect = new Rect(pixel1, y, pixel2 - pixel1, num1 - y);
      if (flag1)
      {
        toolLayout.scaleLeftRect = new Rect(toolLayout.selectionRect.xMin - 17f, toolLayout.selectionRect.yMin + 4f, 17f, toolLayout.selectionRect.height - 8f);
        toolLayout.scaleRightRect = new Rect(toolLayout.selectionRect.xMax, toolLayout.selectionRect.yMin + 4f, 17f, toolLayout.selectionRect.height - 8f);
      }
      else
      {
        toolLayout.scaleLeftRect = DopeSheetEditorRectangleTool.g_EmptyRect;
        toolLayout.scaleRightRect = DopeSheetEditorRectangleTool.g_EmptyRect;
      }
      if (flag1)
      {
        toolLayout.leftLabelAnchor = new Vector2(toolLayout.summaryRect.xMin - 8f, this.contentRect.yMin + 1f);
        toolLayout.rightLabelAnchor = new Vector2(toolLayout.summaryRect.xMax + 8f, this.contentRect.yMin + 1f);
      }
      else
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        DopeSheetEditorRectangleTool.ToolLayout& local1 = @toolLayout;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        DopeSheetEditorRectangleTool.ToolLayout& local2 = @toolLayout;
        double num3 = (double) toolLayout.summaryRect.center.x + 8.0;
        double num4 = (double) this.contentRect.yMin + 1.0;
        Vector2 vector2_1;
        Vector2 vector2_2 = vector2_1 = new Vector2((float) num3, (float) num4);
        // ISSUE: explicit reference operation
        (^local2).rightLabelAnchor = vector2_1;
        Vector2 vector2_3 = vector2_2;
        // ISSUE: explicit reference operation
        (^local1).leftLabelAnchor = vector2_3;
      }
      return toolLayout;
    }

    private void DrawLabels()
    {
      if (!this.isDragging)
        return;
      if (!Mathf.Approximately(this.selectionBounds.size.x, 0.0f))
      {
        GUIContent content1 = new GUIContent(string.Format("{0}", (object) this.m_DopeSheetEditor.FormatTime(this.selectionBounds.min.x, this.m_State.frameRate, this.m_State.timeFormat)));
        GUIContent content2 = new GUIContent(string.Format("{0}", (object) this.m_DopeSheetEditor.FormatTime(this.selectionBounds.max.x, this.m_State.frameRate, this.m_State.timeFormat)));
        Vector2 vector2_1 = this.styles.dragLabel.CalcSize(content1);
        Vector2 vector2_2 = this.styles.dragLabel.CalcSize(content2);
        EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.leftLabelAnchor.x - vector2_1.x, this.m_Layout.leftLabelAnchor.y, vector2_1.x, vector2_1.y), content1, this.styles.dragLabel, 0.3f);
        EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.rightLabelAnchor.x, this.m_Layout.rightLabelAnchor.y, vector2_2.x, vector2_2.y), content2, this.styles.dragLabel, 0.3f);
      }
      else
      {
        GUIContent content = new GUIContent(string.Format("{0}", (object) this.m_DopeSheetEditor.FormatTime(this.selectionBounds.center.x, this.m_State.frameRate, this.m_State.timeFormat)));
        Vector2 vector2 = this.styles.dragLabel.CalcSize(content);
        EditorGUI.DoDropShadowLabel(new Rect(this.m_Layout.leftLabelAnchor.x, this.m_Layout.leftLabelAnchor.y, vector2.x, vector2.y), content, this.styles.dragLabel, 0.3f);
      }
    }

    private void OnStartScale(RectangleTool.ToolCoord pivotCoord, RectangleTool.ToolCoord pickedCoord, Vector2 mousePos, bool rippleTime)
    {
      Bounds selectionBounds = this.selectionBounds;
      this.m_IsDragging = true;
      this.m_Pivot = this.ToolCoordToPosition(pivotCoord, selectionBounds);
      this.m_Previous = this.ToolCoordToPosition(pickedCoord, selectionBounds);
      this.m_MouseOffset = mousePos - this.m_Previous;
      this.m_RippleTime = rippleTime;
      this.m_RippleTimeStart = selectionBounds.min.x;
      this.m_RippleTimeEnd = selectionBounds.max.x;
      this.m_State.StartLiveEdit();
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
      this.m_State.EndLiveEdit();
      this.m_IsDragging = false;
    }

    internal void OnStartMove(Vector2 position, bool rippleTime)
    {
      Bounds selectionBounds = this.selectionBounds;
      this.m_IsDragging = true;
      this.m_Previous = position;
      this.m_RippleTime = rippleTime;
      this.m_RippleTimeStart = selectionBounds.min.x;
      this.m_RippleTimeEnd = selectionBounds.max.x;
      this.m_State.StartLiveEdit();
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
      this.m_State.EndLiveEdit();
      this.m_IsDragging = false;
    }

    private void TransformKeys(Matrix4x4 matrix, bool flipX, bool flipY)
    {
      if (this.m_RippleTime)
        this.m_State.TransformRippleKeys(matrix, this.m_RippleTimeStart, this.m_RippleTimeEnd, flipX, true);
      else
        this.m_State.TransformSelectedKeys(matrix, flipX, flipY, true);
    }

    private struct ToolLayout
    {
      public Rect summaryRect;
      public Rect selectionRect;
      public Rect scaleLeftRect;
      public Rect scaleRightRect;
      public Vector2 leftLabelAnchor;
      public Vector2 rightLabelAnchor;
    }
  }
}
