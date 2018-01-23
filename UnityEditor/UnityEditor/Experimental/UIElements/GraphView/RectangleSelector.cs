// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.RectangleSelector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class RectangleSelector : MouseManipulator
  {
    private readonly RectangleSelector.RectangleSelect m_Rectangle;
    private bool m_Active;

    public RectangleSelector()
    {
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
      this.m_Rectangle = new RectangleSelector.RectangleSelect();
      this.m_Rectangle.style.positionType = (StyleValue<PositionType>) PositionType.Absolute;
      this.m_Rectangle.style.positionTop = (StyleValue<float>) 0.0f;
      this.m_Rectangle.style.positionLeft = (StyleValue<float>) 0.0f;
      this.m_Rectangle.style.positionBottom = (StyleValue<float>) 0.0f;
      this.m_Rectangle.style.positionRight = (StyleValue<float>) 0.0f;
      this.m_Active = false;
    }

    public Rect ComputeAxisAlignedBound(Rect position, Matrix4x4 transform)
    {
      Vector3 vector3_1 = transform.MultiplyPoint3x4((Vector3) position.min);
      Vector3 vector3_2 = transform.MultiplyPoint3x4((Vector3) position.max);
      return Rect.MinMaxRect(Math.Min(vector3_1.x, vector3_2.x), Math.Min(vector3_1.y, vector3_2.y), Math.Max(vector3_1.x, vector3_2.x), Math.Max(vector3_1.y, vector3_2.y));
    }

    protected override void RegisterCallbacksOnTarget()
    {
      if (!(this.target is UnityEditor.Experimental.UIElements.GraphView.GraphView))
        throw new InvalidOperationException("Manipulator can only be added to a GraphView");
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
    }

    private void OnMouseDown(MouseDownEvent e)
    {
      if (e.target != this.target)
        return;
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null || !this.CanStartManipulation((IMouseEvent) e))
        return;
      if (!e.ctrlKey)
        target.ClearSelection();
      target.Add((VisualElement) this.m_Rectangle);
      this.m_Rectangle.start = e.localMousePosition;
      this.m_Rectangle.end = this.m_Rectangle.start;
      this.m_Active = true;
      this.target.TakeCapture();
      e.StopPropagation();
    }

    private void OnMouseUp(MouseUpEvent e)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RectangleSelector.\u003COnMouseUp\u003Ec__AnonStorey0 mouseUpCAnonStorey0 = new RectangleSelector.\u003COnMouseUp\u003Ec__AnonStorey0();
      if (!this.m_Active)
        return;
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null || !this.CanStopManipulation((IMouseEvent) e))
        return;
      target.Remove((VisualElement) this.m_Rectangle);
      this.m_Rectangle.end = e.localMousePosition;
      Rect rect = new Rect() { min = new Vector2(Math.Min(this.m_Rectangle.start.x, this.m_Rectangle.end.x), Math.Min(this.m_Rectangle.start.y, this.m_Rectangle.end.y)), max = new Vector2(Math.Max(this.m_Rectangle.start.x, this.m_Rectangle.end.x), Math.Max(this.m_Rectangle.start.y, this.m_Rectangle.end.y)) };
      // ISSUE: reference to a compiler-generated field
      mouseUpCAnonStorey0.selectionRect = rect;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      mouseUpCAnonStorey0.selectionRect = this.ComputeAxisAlignedBound(mouseUpCAnonStorey0.selectionRect, target.viewTransform.matrix.inverse);
      List<ISelectable> selection = target.selection;
      // ISSUE: reference to a compiler-generated field
      mouseUpCAnonStorey0.newSelection = new List<ISelectable>();
      // ISSUE: reference to a compiler-generated method
      target.graphElements.ForEach(new System.Action<GraphElement>(mouseUpCAnonStorey0.\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      foreach (ISelectable selectable in mouseUpCAnonStorey0.newSelection)
      {
        if (selection.Contains(selectable))
        {
          if (e.ctrlKey)
            target.RemoveFromSelection(selectable);
        }
        else
          target.AddToSelection(selectable);
      }
      this.m_Active = false;
      this.target.ReleaseCapture();
      e.StopPropagation();
    }

    private void OnMouseMove(MouseMoveEvent e)
    {
      if (!this.m_Active)
        return;
      this.m_Rectangle.end = e.localMousePosition;
      e.StopPropagation();
    }

    private class RectangleSelect : VisualElement
    {
      public Vector2 start { get; set; }

      public Vector2 end { get; set; }

      public override void DoRepaint()
      {
        VisualElement parent = this.parent;
        Vector2 start = this.start;
        Vector2 end = this.end;
        if (this.start == this.end)
          return;
        Vector2 vector2_1 = start + parent.layout.position;
        Vector2 vector2_2 = end + parent.layout.position;
        Rect rect = new Rect() { min = new Vector2(Math.Min(vector2_1.x, vector2_2.x), Math.Min(vector2_1.y, vector2_2.y)), max = new Vector2(Math.Max(vector2_1.x, vector2_2.x), Math.Max(vector2_1.y, vector2_2.y)) };
        Color col = new Color(1f, 0.6f, 0.0f, 1f);
        float segmentsLength = 5f;
        Vector3[] vector3Array = new Vector3[4]{ new Vector3(rect.xMin, rect.yMin, 0.0f), new Vector3(rect.xMax, rect.yMin, 0.0f), new Vector3(rect.xMax, rect.yMax, 0.0f), new Vector3(rect.xMin, rect.yMax, 0.0f) };
        this.DrawDottedLine(vector3Array[0], vector3Array[1], segmentsLength, col);
        this.DrawDottedLine(vector3Array[1], vector3Array[2], segmentsLength, col);
        this.DrawDottedLine(vector3Array[2], vector3Array[3], segmentsLength, col);
        this.DrawDottedLine(vector3Array[3], vector3Array[0], segmentsLength, col);
        string text1 = "(" + string.Format("{0:0}", (object) this.start.x) + ", " + string.Format("{0:0}", (object) this.start.y) + ")";
        GUI.skin.label.Draw(new Rect(vector2_1.x, vector2_1.y - 18f, 200f, 20f), new GUIContent(text1), 0);
        string text2 = "(" + string.Format("{0:0}", (object) this.end.x) + ", " + string.Format("{0:0}", (object) this.end.y) + ")";
        GUI.skin.label.Draw(new Rect(vector2_2.x - 80f, vector2_2.y + 5f, 200f, 20f), new GUIContent(text2), 0);
      }

      private void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength, Color col)
      {
        HandleUtility.ApplyWireMaterial();
        GL.Begin(1);
        GL.Color(col);
        float num1 = Vector3.Distance(p1, p2);
        int num2 = Mathf.CeilToInt(num1 / segmentsLength);
        int num3 = 0;
        while (num3 < num2)
        {
          GL.Vertex(Vector3.Lerp(p1, p2, (float) num3 * segmentsLength / num1));
          GL.Vertex(Vector3.Lerp(p1, p2, (float) (num3 + 1) * segmentsLength / num1));
          num3 += 2;
        }
        GL.End();
      }
    }
  }
}
