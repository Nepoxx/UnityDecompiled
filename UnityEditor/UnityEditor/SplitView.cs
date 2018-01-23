// Decompiled with JetBrains decompiler
// Type: UnityEditor.SplitView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SplitView : View, ICleanuppable, IDropArea
  {
    public bool vertical = false;
    public int controlID = 0;
    private SplitterState splitState = (SplitterState) null;
    private const float kRootDropZoneThickness = 70f;
    private const float kRootDropZoneOffset = 50f;
    private const float kRootDropDestinationThickness = 200f;
    private const float kMaxViewDropZoneThickness = 300f;
    private const float kMinViewDropDestinationThickness = 100f;
    internal const float kGrabDist = 5f;

    private Rect RectFromEdge(Rect rect, SplitView.ViewEdge edge, float thickness, float offset)
    {
      switch (edge)
      {
        case SplitView.ViewEdge.Left:
          return new Rect(rect.x - offset, rect.y, thickness, rect.height);
        case SplitView.ViewEdge.Bottom:
          return new Rect(rect.x, rect.yMax - thickness + offset, rect.width, thickness);
        case SplitView.ViewEdge.Top:
          return new Rect(rect.x, rect.y - offset, rect.width, thickness);
        case SplitView.ViewEdge.Right:
          return new Rect(rect.xMax - thickness + offset, rect.y, thickness, rect.height);
        default:
          throw new ArgumentException("Specify exactly one edge");
      }
    }

    private void SetupSplitter()
    {
      int[] realSizes = new int[this.children.Length];
      int[] minSizes = new int[this.children.Length];
      for (int index = 0; index < this.children.Length; ++index)
      {
        View child = this.children[index];
        realSizes[index] = !this.vertical ? (int) child.position.width : (int) child.position.height;
        minSizes[index] = !this.vertical ? (int) child.minSize.x : (int) child.minSize.y;
      }
      this.splitState = new SplitterState(realSizes, minSizes, (int[]) null);
      this.splitState.splitSize = 10;
    }

    private void SetupRectsFromSplitter()
    {
      if (this.children.Length == 0)
        return;
      int num1 = 0;
      int num2 = 0;
      foreach (int realSize in this.splitState.realSizes)
        num2 += realSize;
      float num3 = 1f;
      if ((double) num2 > (!this.vertical ? (double) this.position.width : (double) this.position.height))
        num3 = (!this.vertical ? this.position.width : this.position.height) / (float) num2;
      SavedGUIState savedGuiState = SavedGUIState.Create();
      for (int index = 0; index < this.children.Length; ++index)
      {
        int num4 = (int) Mathf.Round((float) this.splitState.realSizes[index] * num3);
        if (this.vertical)
          this.children[index].position = new Rect(0.0f, (float) num1, this.position.width, (float) num4);
        else
          this.children[index].position = new Rect((float) num1, 0.0f, (float) num4, this.position.height);
        num1 += num4;
      }
      savedGuiState.ApplyAndForget();
    }

    private static void RecalcMinMaxAndReflowAll(SplitView start)
    {
      SplitView splitView = start;
      SplitView node;
      do
      {
        node = splitView;
        splitView = node.parent as SplitView;
      }
      while ((bool) ((UnityEngine.Object) splitView));
      SplitView.RecalcMinMaxRecurse(node);
      SplitView.ReflowRecurse(node);
    }

    private static void RecalcMinMaxRecurse(SplitView node)
    {
      foreach (View child in node.children)
      {
        SplitView node1 = child as SplitView;
        if ((bool) ((UnityEngine.Object) node1))
          SplitView.RecalcMinMaxRecurse(node1);
      }
      node.ChildrenMinMaxChanged();
    }

    private static void ReflowRecurse(SplitView node)
    {
      node.Reflow();
      foreach (View child in node.children)
      {
        SplitView node1 = child as SplitView;
        if ((bool) ((UnityEngine.Object) node1))
          SplitView.RecalcMinMaxRecurse(node1);
      }
    }

    internal override void Reflow()
    {
      this.SetupSplitter();
      for (int i1 = 0; i1 < this.children.Length - 1; ++i1)
        this.splitState.DoSplitter(i1, i1 + 1, 0);
      this.splitState.RelativeToRealSizes(!this.vertical ? (int) this.position.width : (int) this.position.height);
      this.SetupRectsFromSplitter();
    }

    private void PlaceView(int i, float pos, float size)
    {
      float num = Mathf.Round(pos);
      if (this.vertical)
        this.children[i].position = new Rect(0.0f, num, this.position.width, Mathf.Round(pos + size) - num);
      else
        this.children[i].position = new Rect(num, 0.0f, Mathf.Round(pos + size) - num, this.position.height);
    }

    public override void AddChild(View child, int idx)
    {
      base.AddChild(child, idx);
      this.ChildrenMinMaxChanged();
      this.splitState = (SplitterState) null;
    }

    public void RemoveChildNice(View child)
    {
      if (this.children.Length != 1)
      {
        int num1 = this.IndexOfChild(child);
        float t = num1 != 0 ? (num1 != this.children.Length - 1 ? 0.5f : 1f) : 0.0f;
        float num2 = !this.vertical ? Mathf.Lerp(child.position.xMin, child.position.xMax, t) : Mathf.Lerp(child.position.yMin, child.position.yMax, t);
        if (num1 > 0)
        {
          View child1 = this.children[num1 - 1];
          Rect position = child1.position;
          if (this.vertical)
            position.yMax = num2;
          else
            position.xMax = num2;
          child1.position = position;
          if (child1 is SplitView)
            child1.Reflow();
        }
        if (num1 < this.children.Length - 1)
        {
          View child1 = this.children[num1 + 1];
          Rect position = child1.position;
          child1.position = !this.vertical ? new Rect(num2, position.y, position.xMax - num2, position.height) : new Rect(position.x, num2, position.width, position.yMax - num2);
          if (child1 is SplitView)
            child1.Reflow();
        }
      }
      this.RemoveChild(child);
    }

    public override void RemoveChild(View child)
    {
      this.splitState = (SplitterState) null;
      base.RemoveChild(child);
    }

    private DropInfo RootViewDropZone(SplitView.ViewEdge edge, Vector2 mousePos, Rect screenRect)
    {
      float offset = (edge & SplitView.ViewEdge.FitsVertical) == SplitView.ViewEdge.None ? 50f : 70f;
      if (!this.RectFromEdge(screenRect, edge, 70f, offset).Contains(mousePos))
        return (DropInfo) null;
      return new DropInfo((IDropArea) this) { type = DropInfo.Type.Pane, userData = (object) new SplitView.ExtraDropInfo(true, edge, 0), rect = this.RectFromEdge(screenRect, edge, 200f, 0.0f) };
    }

    public DropInfo DragOverRootView(Vector2 mouseScreenPosition)
    {
      if (this.children.Length == 1 && (UnityEngine.Object) DockArea.s_IgnoreDockingForView == (UnityEngine.Object) this.children[0])
        return (DropInfo) null;
      return this.RootViewDropZone(SplitView.ViewEdge.Bottom, mouseScreenPosition, this.screenPosition) ?? this.RootViewDropZone(SplitView.ViewEdge.Top, mouseScreenPosition, this.screenPosition) ?? this.RootViewDropZone(SplitView.ViewEdge.Left, mouseScreenPosition, this.screenPosition) ?? this.RootViewDropZone(SplitView.ViewEdge.Right, mouseScreenPosition, this.screenPosition);
    }

    public DropInfo DragOver(EditorWindow w, Vector2 mouseScreenPosition)
    {
      for (int index = 0; index < this.children.Length; ++index)
      {
        View child = this.children[index];
        if (!((UnityEngine.Object) child == (UnityEngine.Object) DockArea.s_IgnoreDockingForView) && !(child is SplitView))
        {
          SplitView.ViewEdge viewEdge1 = SplitView.ViewEdge.None;
          Rect screenPosition = child.screenPosition;
          Rect rect1 = this.RectFromEdge(screenPosition, SplitView.ViewEdge.Bottom, screenPosition.height - 39f, 0.0f);
          float num1 = Mathf.Min(Mathf.Round(rect1.width / 3f), 300f);
          float num2 = Mathf.Min(Mathf.Round(rect1.height / 3f), 300f);
          Rect rect2 = this.RectFromEdge(rect1, SplitView.ViewEdge.Left, num1, 0.0f);
          Rect rect3 = this.RectFromEdge(rect1, SplitView.ViewEdge.Right, num1, 0.0f);
          Rect rect4 = this.RectFromEdge(rect1, SplitView.ViewEdge.Bottom, num2, 0.0f);
          Rect rect5 = this.RectFromEdge(rect1, SplitView.ViewEdge.Top, num2, 0.0f);
          if (rect2.Contains(mouseScreenPosition))
            viewEdge1 |= SplitView.ViewEdge.Left;
          if (rect3.Contains(mouseScreenPosition))
            viewEdge1 |= SplitView.ViewEdge.Right;
          if (rect4.Contains(mouseScreenPosition))
            viewEdge1 |= SplitView.ViewEdge.Bottom;
          if (rect5.Contains(mouseScreenPosition))
            viewEdge1 |= SplitView.ViewEdge.Top;
          Vector2 vector2_1 = Vector2.zero;
          Vector2 vector2_2 = Vector2.zero;
          SplitView.ViewEdge viewEdge2 = viewEdge1;
          SplitView.ViewEdge viewEdge3 = viewEdge1;
          switch (viewEdge1)
          {
            case SplitView.ViewEdge.BottomLeft:
              viewEdge2 = SplitView.ViewEdge.Bottom;
              viewEdge3 = SplitView.ViewEdge.Left;
              vector2_1 = new Vector2(rect1.x, rect1.yMax) - mouseScreenPosition;
              vector2_2 = new Vector2(-num1, num2);
              break;
            case SplitView.ViewEdge.TopLeft:
              viewEdge2 = SplitView.ViewEdge.Left;
              viewEdge3 = SplitView.ViewEdge.Top;
              vector2_1 = new Vector2(rect1.x, rect1.y) - mouseScreenPosition;
              vector2_2 = new Vector2(-num1, -num2);
              break;
            default:
              switch (viewEdge1 - 10)
              {
                case SplitView.ViewEdge.None:
                  viewEdge2 = SplitView.ViewEdge.Right;
                  viewEdge3 = SplitView.ViewEdge.Bottom;
                  vector2_1 = new Vector2(rect1.xMax, rect1.yMax) - mouseScreenPosition;
                  vector2_2 = new Vector2(num1, num2);
                  break;
                case SplitView.ViewEdge.Bottom:
                  viewEdge2 = SplitView.ViewEdge.Top;
                  viewEdge3 = SplitView.ViewEdge.Right;
                  vector2_1 = new Vector2(rect1.xMax, rect1.y) - mouseScreenPosition;
                  vector2_2 = new Vector2(num1, -num2);
                  break;
              }
          }
          SplitView.ViewEdge edge = (double) vector2_1.x * (double) vector2_2.y - (double) vector2_1.y * (double) vector2_2.x >= 0.0 ? viewEdge3 : viewEdge2;
          if (edge != SplitView.ViewEdge.None)
          {
            float thickness = Mathf.Max(Mathf.Round((float) (((edge & SplitView.ViewEdge.FitsHorizontal) == SplitView.ViewEdge.None ? (double) screenPosition.height : (double) screenPosition.width) / 3.0)), 100f);
            return new DropInfo((IDropArea) this) { userData = (object) new SplitView.ExtraDropInfo(false, edge, index), type = DropInfo.Type.Pane, rect = this.RectFromEdge(screenPosition, edge, thickness, 0.0f) };
          }
        }
      }
      if (this.screenPosition.Contains(mouseScreenPosition) && !(this.parent is SplitView))
        return new DropInfo((IDropArea) null);
      return (DropInfo) null;
    }

    protected override void ChildrenMinMaxChanged()
    {
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      if (this.vertical)
      {
        foreach (View child in this.children)
        {
          zero1.x = Mathf.Max(child.minSize.x, zero1.x);
          zero2.x = Mathf.Max(child.maxSize.x, zero2.x);
          zero1.y += child.minSize.y;
          zero2.y += child.maxSize.y;
        }
      }
      else
      {
        foreach (View child in this.children)
        {
          zero1.x += child.minSize.x;
          zero2.x += child.maxSize.x;
          zero1.y = Mathf.Max(child.minSize.y, zero1.y);
          zero2.y = Mathf.Max(child.maxSize.y, zero2.y);
        }
      }
      this.splitState = (SplitterState) null;
      this.SetMinMaxSizes(zero1, zero2);
    }

    public override string ToString()
    {
      return !this.vertical ? "SplitView (horiz)" : "SplitView (vert)";
    }

    public bool PerformDrop(EditorWindow dropWindow, DropInfo dropInfo, Vector2 screenPos)
    {
      SplitView.ExtraDropInfo userData = dropInfo.userData as SplitView.ExtraDropInfo;
      bool rootWindow = userData.rootWindow;
      SplitView.ViewEdge edge = userData.edge;
      int idx = userData.index;
      Rect rect = dropInfo.rect;
      bool flag1 = (edge & SplitView.ViewEdge.TopLeft) != SplitView.ViewEdge.None;
      bool flag2 = (edge & SplitView.ViewEdge.FitsVertical) != SplitView.ViewEdge.None;
      SplitView splitView;
      if (this.vertical == flag2 || this.children.Length < 2)
      {
        if (!flag1)
        {
          if (rootWindow)
            idx = this.children.Length;
          else
            ++idx;
        }
        splitView = this;
      }
      else if (rootWindow)
      {
        SplitView instance = ScriptableObject.CreateInstance<SplitView>();
        instance.position = this.position;
        if ((UnityEngine.Object) this.window.rootView == (UnityEngine.Object) this)
          this.window.rootView = (View) instance;
        else
          this.parent.AddChild((View) instance, this.parent.IndexOfChild((View) this));
        instance.AddChild((View) this);
        this.position = new Rect(Vector2.zero, this.position.size);
        idx = !flag1 ? 1 : 0;
        splitView = instance;
      }
      else
      {
        SplitView instance = ScriptableObject.CreateInstance<SplitView>();
        instance.AddChild(this.children[idx]);
        this.AddChild((View) instance, idx);
        instance.position = instance.children[0].position;
        instance.children[0].position = new Rect(Vector2.zero, instance.position.size);
        idx = !flag1 ? 1 : 0;
        splitView = instance;
      }
      rect.position -= this.screenPosition.position;
      DockArea instance1 = ScriptableObject.CreateInstance<DockArea>();
      splitView.vertical = flag2;
      splitView.MakeRoomForRect(rect);
      splitView.AddChild((View) instance1, idx);
      instance1.position = rect;
      DockArea.s_OriginalDragSource.RemoveTab(dropWindow);
      dropWindow.m_Parent = (HostView) instance1;
      instance1.AddTab(dropWindow);
      this.Reflow();
      SplitView.RecalcMinMaxAndReflowAll(this);
      instance1.MakeVistaDWMHappyDance();
      return true;
    }

    private static string PosVals(float[] posVals)
    {
      string str = "[";
      foreach (float posVal in posVals)
        str = str + "" + (object) posVal + ", ";
      return str + "]";
    }

    private void MakeRoomForRect(Rect r)
    {
      Rect[] sources = new Rect[this.children.Length];
      for (int index = 0; index < sources.Length; ++index)
        sources[index] = this.children[index].position;
      this.CalcRoomForRect(sources, r);
      for (int index = 0; index < sources.Length; ++index)
        this.children[index].position = sources[index];
    }

    private void CalcRoomForRect(Rect[] sources, Rect r)
    {
      float num1 = !this.vertical ? r.x : r.y;
      float num2 = num1 + (!this.vertical ? r.width : r.height);
      float num3 = (float) (((double) num1 + (double) num2) * 0.5);
      int index1 = 0;
      while (index1 < sources.Length && (!this.vertical ? (double) sources[index1].x + (double) sources[index1].width * 0.5 : (double) sources[index1].y + (double) sources[index1].height * 0.5) <= (double) num3)
        ++index1;
      float num4 = num1;
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        if (this.vertical)
        {
          sources[index2].yMax = num4;
          if ((double) sources[index2].height < (double) this.children[index2].minSize.y)
          {
            float num5 = sources[index2].yMax - this.children[index2].minSize.y;
            sources[index2].yMin = num5;
            num4 = num5;
          }
          else
            break;
        }
        else
        {
          sources[index2].xMax = num4;
          if ((double) sources[index2].width < (double) this.children[index2].minSize.x)
          {
            float num5 = sources[index2].xMax - this.children[index2].minSize.x;
            sources[index2].xMin = num5;
            num4 = num5;
          }
          else
            break;
        }
      }
      if ((double) num4 < 0.0)
      {
        float num5 = -num4;
        for (int index2 = 0; index2 < index1 - 1; ++index2)
        {
          if (this.vertical)
            sources[index2].y += num5;
          else
            sources[index2].x += num5;
        }
        num2 += num5;
      }
      float num6 = num2;
      for (int index2 = index1; index2 < sources.Length; ++index2)
      {
        if (this.vertical)
        {
          float yMax = sources[index2].yMax;
          sources[index2].yMin = num6;
          sources[index2].yMax = yMax;
          if ((double) sources[index2].height < (double) this.children[index2].minSize.y)
          {
            float num5 = sources[index2].yMin + this.children[index2].minSize.y;
            sources[index2].yMax = num5;
            num6 = num5;
          }
          else
            break;
        }
        else
        {
          float xMax = sources[index2].xMax;
          sources[index2].xMin = num6;
          sources[index2].xMax = xMax;
          if ((double) sources[index2].width < (double) this.children[index2].minSize.x)
          {
            float num5 = sources[index2].xMin + this.children[index2].minSize.x;
            sources[index2].xMax = num5;
            num6 = num5;
          }
          else
            break;
        }
      }
      float num7 = !this.vertical ? this.position.width : this.position.height;
      if ((double) num6 <= (double) num7)
        return;
      float num8 = num7 - num6;
      for (int index2 = 0; index2 < index1 - 1; ++index2)
      {
        if (this.vertical)
          sources[index2].y += num8;
        else
          sources[index2].x += num8;
      }
      float num9 = num2 + num8;
    }

    public void Cleanup()
    {
      SplitView parent1 = this.parent as SplitView;
      if (this.children.Length == 1 && (UnityEngine.Object) parent1 != (UnityEngine.Object) null)
      {
        View child = this.children[0];
        child.position = this.position;
        if ((UnityEngine.Object) this.parent != (UnityEngine.Object) null)
        {
          this.parent.AddChild(child, this.parent.IndexOfChild((View) this));
          this.parent.RemoveChild((View) this);
          if ((bool) ((UnityEngine.Object) parent1))
            parent1.Cleanup();
          child.position = this.position;
          if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
            return;
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this);
          return;
        }
        if (child is SplitView)
        {
          this.RemoveChild(child);
          this.window.rootView = child;
          child.position = new Rect(0.0f, 0.0f, child.window.position.width, this.window.position.height);
          child.Reflow();
          if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
            return;
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this);
          return;
        }
      }
      if ((bool) ((UnityEngine.Object) parent1))
      {
        parent1.Cleanup();
        SplitView parent2 = this.parent as SplitView;
        if ((bool) ((UnityEngine.Object) parent2) && parent2.vertical == this.vertical)
        {
          int num = new List<View>((IEnumerable<View>) this.parent.children).IndexOf((View) this);
          foreach (View child in this.children)
          {
            parent2.AddChild(child, num++);
            child.position = new Rect(this.position.x + child.position.x, this.position.y + child.position.y, child.position.width, child.position.height);
          }
        }
      }
      if (this.children.Length == 0)
      {
        if ((UnityEngine.Object) this.parent == (UnityEngine.Object) null && (UnityEngine.Object) this.window != (UnityEngine.Object) null)
        {
          this.window.Close();
        }
        else
        {
          ICleanuppable parent2 = this.parent as ICleanuppable;
          if (this.parent is SplitView)
          {
            ((SplitView) this.parent).RemoveChildNice((View) this);
            if (!Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
              UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this, true);
          }
          parent2.Cleanup();
        }
      }
      else
      {
        this.splitState = (SplitterState) null;
        this.Reflow();
      }
    }

    public void SplitGUI(Event evt)
    {
      if (this.splitState == null)
        this.SetupSplitter();
      SplitView parent = this.parent as SplitView;
      if ((bool) ((UnityEngine.Object) parent))
      {
        Event evt1 = new Event(evt);
        evt1.mousePosition += new Vector2(this.position.x, this.position.y);
        parent.SplitGUI(evt1);
        if (evt1.type == EventType.Used)
          evt.Use();
      }
      float num1 = !this.vertical ? evt.mousePosition.x : evt.mousePosition.y;
      int controlId = GUIUtility.GetControlID(546739, FocusType.Passive);
      this.controlID = controlId;
      switch (evt.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (this.children.Length == 1)
            break;
          int num2 = !this.vertical ? (int) this.children[0].position.x : (int) this.children[0].position.y;
          for (int index = 0; index < this.children.Length - 1; ++index)
          {
            if (index >= this.splitState.realSizes.Length)
            {
              DockArea current = GUIView.current as DockArea;
              string str = "Non-dock area " + (object) GUIView.current.GetType();
              if ((bool) ((UnityEngine.Object) current) && current.m_Selected < current.m_Panes.Count && (bool) ((UnityEngine.Object) current.m_Panes[current.m_Selected]))
                str = current.m_Panes[current.m_Selected].GetType().ToString();
              if (Unsupported.IsDeveloperBuild())
                Debug.LogError((object) ("Real sizes out of bounds for: " + str + " index: " + (object) index + " RealSizes: " + (object) this.splitState.realSizes.Length));
              this.SetupSplitter();
            }
            if ((!this.vertical ? new Rect((float) (num2 + this.splitState.realSizes[index] - this.splitState.splitSize / 2), this.children[0].position.y, (float) this.splitState.splitSize, this.children[0].position.height) : new Rect(this.children[0].position.x, (float) (num2 + this.splitState.realSizes[index] - this.splitState.splitSize / 2), this.children[0].position.width, (float) this.splitState.splitSize)).Contains(evt.mousePosition))
            {
              this.splitState.splitterInitialOffset = (int) num1;
              this.splitState.currentActiveSplitter = index;
              GUIUtility.hotControl = controlId;
              evt.Use();
              break;
            }
            num2 += this.splitState.realSizes[index];
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseDrag:
          if (this.children.Length <= 1 || GUIUtility.hotControl != controlId || this.splitState.currentActiveSplitter < 0)
            break;
          int diff = (int) num1 - this.splitState.splitterInitialOffset;
          if (diff != 0)
          {
            this.splitState.splitterInitialOffset = (int) num1;
            this.splitState.DoSplitter(this.splitState.currentActiveSplitter, this.splitState.currentActiveSplitter + 1, diff);
          }
          this.SetupRectsFromSplitter();
          evt.Use();
          break;
      }
    }

    protected override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      this.Reflow();
    }

    [System.Flags]
    internal enum ViewEdge
    {
      None = 0,
      Left = 1,
      Bottom = 2,
      Top = 4,
      Right = 8,
      BottomLeft = Bottom | Left, // 0x00000003
      BottomRight = Right | Bottom, // 0x0000000A
      TopLeft = Top | Left, // 0x00000005
      TopRight = Right | Top, // 0x0000000C
      FitsVertical = Top | Bottom, // 0x00000006
      FitsHorizontal = Right | Left, // 0x00000009
      Before = TopLeft, // 0x00000005
      After = BottomRight, // 0x0000000A
    }

    internal class ExtraDropInfo
    {
      public bool rootWindow;
      public SplitView.ViewEdge edge;
      public int index;

      public ExtraDropInfo(bool rootWindow, SplitView.ViewEdge edge, int index)
      {
        this.rootWindow = rootWindow;
        this.edge = edge;
        this.index = index;
      }
    }
  }
}
