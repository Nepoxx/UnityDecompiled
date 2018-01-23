// Decompiled with JetBrains decompiler
// Type: UnityEditor.DockArea
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor
{
  internal class DockArea : HostView, IDropArea
  {
    internal static View s_IgnoreDockingForView = (View) null;
    private static DropInfo s_DropInfo = (DropInfo) null;
    [SerializeField]
    internal List<EditorWindow> m_Panes = new List<EditorWindow>();
    [NonSerialized]
    internal GUIStyle tabStyle = (GUIStyle) null;
    internal const float kTabHeight = 17f;
    internal const float kDockHeight = 39f;
    private const float kSideBorders = 2f;
    private const float kBottomBorders = 2f;
    private const float kWindowButtonsWidth = 40f;
    private static int s_PlaceholderPos;
    private static EditorWindow s_DragPane;
    internal static DockArea s_OriginalDragSource;
    private static Vector2 s_StartDragPosition;
    private static int s_DragMode;
    [SerializeField]
    internal int m_Selected;
    [SerializeField]
    internal int m_LastSelected;
    private bool m_IsBeingDestroyed;

    public DockArea()
    {
      if (this.m_Panes == null || this.m_Panes.Count == 0)
        return;
      Debug.LogError((object) "m_Panes is filled in DockArea constructor.");
    }

    public int selected
    {
      get
      {
        return this.m_Selected;
      }
      set
      {
        if (this.m_Selected != value)
          this.m_LastSelected = this.m_Selected;
        this.m_Selected = value;
        if (this.m_Selected < 0 || this.m_Selected >= this.m_Panes.Count)
          return;
        this.actualView = this.m_Panes[this.m_Selected];
      }
    }

    private void RemoveNullWindows()
    {
      List<EditorWindow> editorWindowList = new List<EditorWindow>();
      foreach (EditorWindow pane in this.m_Panes)
      {
        if ((UnityEngine.Object) pane != (UnityEngine.Object) null)
          editorWindowList.Add(pane);
      }
      this.m_Panes = editorWindowList;
    }

    protected override void OnDestroy()
    {
      this.m_IsBeingDestroyed = true;
      if (this.hasFocus)
        this.Invoke("OnLostFocus");
      this.actualView = (EditorWindow) null;
      foreach (UnityEngine.Object pane in this.m_Panes)
        UnityEngine.Object.DestroyImmediate(pane, true);
      base.OnDestroy();
    }

    protected override void OnEnable()
    {
      if (this.m_Panes != null)
      {
        if (this.m_Panes.Count == 0)
        {
          this.m_Selected = 0;
        }
        else
        {
          this.m_Selected = Math.Min(this.m_Selected, this.m_Panes.Count - 1);
          this.actualView = this.m_Panes[this.m_Selected];
        }
      }
      base.OnEnable();
      this.imguiContainer.name = VisualElementUtils.GetUniqueName("Dockarea");
    }

    protected override void UpdateViewMargins(EditorWindow view)
    {
      base.UpdateViewMargins(view);
      if ((UnityEngine.Object) view == (UnityEngine.Object) null)
        return;
      RectOffset borderSize = this.GetBorderSize();
      IStyle style = view.rootVisualContainer.style;
      style.positionTop = (StyleValue<float>) ((float) borderSize.top);
      style.positionBottom = (StyleValue<float>) ((float) borderSize.bottom);
      style.positionLeft = (StyleValue<float>) ((float) borderSize.left);
      style.positionRight = (StyleValue<float>) ((float) borderSize.right);
      style.positionType = (StyleValue<PositionType>) PositionType.Absolute;
    }

    public void AddTab(EditorWindow pane)
    {
      this.AddTab(this.m_Panes.Count, pane);
    }

    public void AddTab(int idx, EditorWindow pane)
    {
      this.DeregisterSelectedPane(true);
      this.m_Panes.Insert(idx, pane);
      this.selected = idx;
      SplitView parent = this.parent as SplitView;
      if ((bool) ((UnityEngine.Object) parent))
        parent.Reflow();
      this.Repaint();
    }

    public void RemoveTab(EditorWindow pane)
    {
      this.RemoveTab(pane, true);
    }

    public void RemoveTab(EditorWindow pane, bool killIfEmpty)
    {
      if ((UnityEngine.Object) this.actualView == (UnityEngine.Object) pane)
        this.DeregisterSelectedPane(true);
      int num = this.m_Panes.IndexOf(pane);
      if (num == -1)
        return;
      this.m_Panes.Remove(pane);
      if (num == this.m_LastSelected)
        this.m_LastSelected = this.m_Panes.Count - 1;
      else if (num < this.m_LastSelected || this.m_LastSelected == this.m_Panes.Count)
        --this.m_LastSelected;
      this.m_LastSelected = Mathf.Clamp(this.m_LastSelected, 0, this.m_Panes.Count - 1);
      this.m_Selected = num != this.m_Selected ? this.m_Panes.IndexOf(this.actualView) : this.m_LastSelected;
      if (this.m_Selected >= 0 && this.m_Selected < this.m_Panes.Count)
        this.actualView = this.m_Panes[this.m_Selected];
      this.Repaint();
      pane.m_Parent = (HostView) null;
      if (killIfEmpty)
        this.KillIfEmpty();
      this.RegisterSelectedPane();
    }

    private void KillIfEmpty()
    {
      if (this.m_Panes.Count != 0)
        return;
      if ((UnityEngine.Object) this.parent == (UnityEngine.Object) null)
      {
        this.window.InternalCloseWindow();
      }
      else
      {
        SplitView parent1 = this.parent as SplitView;
        ICleanuppable parent2 = this.parent as ICleanuppable;
        parent1.RemoveChildNice((View) this);
        if (!this.m_IsBeingDestroyed)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this, true);
        if (parent2 == null)
          return;
        parent2.Cleanup();
      }
    }

    private Rect tabRect
    {
      get
      {
        return new Rect(0.0f, 0.0f, this.position.width, 17f);
      }
    }

    public DropInfo DragOver(EditorWindow window, Vector2 mouseScreenPosition)
    {
      Rect screenPosition = this.screenPosition;
      screenPosition.height = 39f;
      if (!screenPosition.Contains(mouseScreenPosition))
        return (DropInfo) null;
      if (this.background == null)
        this.background = (GUIStyle) "hostview";
      Rect rect = this.background.margin.Remove(this.screenPosition);
      Vector2 mousePos = mouseScreenPosition - new Vector2(rect.x, rect.y);
      Rect tabRect = this.tabRect;
      int tabAtMousePos = this.GetTabAtMousePos(mousePos, tabRect);
      float tabWidth = this.GetTabWidth(tabRect.width);
      if (DockArea.s_PlaceholderPos != tabAtMousePos)
      {
        this.Repaint();
        DockArea.s_PlaceholderPos = tabAtMousePos;
      }
      return new DropInfo((IDropArea) this) { type = DropInfo.Type.Tab, rect = new Rect(mousePos.x - tabWidth * 0.25f + rect.x, tabRect.y + rect.y, tabWidth, tabRect.height) };
    }

    public bool PerformDrop(EditorWindow w, DropInfo info, Vector2 screenPos)
    {
      DockArea.s_OriginalDragSource.RemoveTab(w, (UnityEngine.Object) DockArea.s_OriginalDragSource != (UnityEngine.Object) this);
      int idx = DockArea.s_PlaceholderPos <= this.m_Panes.Count ? DockArea.s_PlaceholderPos : this.m_Panes.Count;
      this.AddTab(idx, w);
      this.selected = idx;
      return true;
    }

    protected override void OldOnGUI()
    {
      this.ClearBackground();
      EditorGUIUtility.ResetGUIState();
      if ((UnityEngine.Object) this.window == (UnityEngine.Object) null)
        return;
      SplitView parent = this.parent as SplitView;
      if (Event.current.type == EventType.Repaint && (bool) ((UnityEngine.Object) parent))
      {
        View child = (View) this;
        for (; (bool) ((UnityEngine.Object) parent); parent = parent.parent as SplitView)
        {
          int controlId = parent.controlID;
          if (controlId == GUIUtility.hotControl || GUIUtility.hotControl == 0)
          {
            int num = parent.IndexOfChild(child);
            if (parent.vertical)
            {
              if (num != 0)
                EditorGUIUtility.AddCursorRect(new Rect(0.0f, 0.0f, this.position.width, 5f), MouseCursor.SplitResizeUpDown, controlId);
              if (num != parent.children.Length - 1)
                EditorGUIUtility.AddCursorRect(new Rect(0.0f, this.position.height - 5f, this.position.width, 5f), MouseCursor.SplitResizeUpDown, controlId);
            }
            else
            {
              if (num != 0)
                EditorGUIUtility.AddCursorRect(new Rect(0.0f, 0.0f, 5f, this.position.height), MouseCursor.SplitResizeLeftRight, controlId);
              if (num != parent.children.Length - 1)
                EditorGUIUtility.AddCursorRect(new Rect(this.position.width - 5f, 0.0f, 5f, this.position.height), MouseCursor.SplitResizeLeftRight, controlId);
            }
          }
          child = (View) parent;
        }
        parent = this.parent as SplitView;
      }
      bool flag = false;
      if (this.window.rootView.GetType() != typeof (MainView))
      {
        flag = true;
        if ((double) this.windowPosition.y == 0.0)
          this.background = (GUIStyle) "dockareaStandalone";
        else
          this.background = (GUIStyle) "dockarea";
      }
      else
        this.background = (GUIStyle) "dockarea";
      if ((bool) ((UnityEngine.Object) parent))
      {
        Event evt = new Event(Event.current);
        evt.mousePosition += new Vector2(this.position.x, this.position.y);
        parent.SplitGUI(evt);
        if (evt.type == EventType.Used)
          Event.current.Use();
      }
      Rect rect1 = this.background.margin.Remove(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
      rect1.x = (float) this.background.margin.left;
      rect1.y = (float) this.background.margin.top;
      Rect windowPosition = this.windowPosition;
      float num1 = 2f;
      if ((double) windowPosition.x == 0.0)
      {
        rect1.x -= num1;
        rect1.width += num1;
      }
      if ((double) windowPosition.xMax == (double) this.window.position.width)
        rect1.width += num1;
      if ((double) windowPosition.yMax == (double) this.window.position.height)
        rect1.height += !flag ? 2f : 2f;
      if (Event.current.type == EventType.Repaint)
        this.background.Draw(rect1, GUIContent.none, 0);
      if (this.tabStyle == null)
        this.tabStyle = (GUIStyle) "dragtab";
      if (this.m_Panes.Count > 0)
      {
        HostView.BeginOffsetArea(new Rect(rect1.x + 2f, rect1.y + 17f, rect1.width - 4f, (float) ((double) rect1.height - 17.0 - 2.0)), GUIContent.none, (GUIStyle) "TabWindowBackground");
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint(Vector2.zero);
        Rect rect2 = this.borderSize.Remove(this.position);
        rect2.x = screenPoint.x;
        rect2.y = screenPoint.y;
        if (this.selected >= 0 && this.selected < this.m_Panes.Count)
          this.m_Panes[this.selected].m_Pos = rect2;
        HostView.EndOffsetArea();
      }
      this.DragTab(new Rect(rect1.x + 1f, rect1.y, rect1.width - 40f, 17f), this.tabStyle);
      this.tabStyle = (GUIStyle) "dragtab";
      this.ShowGenericMenu();
      if (this.m_Panes.Count > 0)
        this.InvokeOnGUI(rect1);
      EditorGUI.ShowRepaints();
      Highlighter.ControlHighlightGUI((GUIView) this);
    }

    protected override void SetActualViewPosition(Rect newPos)
    {
      base.SetActualViewPosition(this.borderSize.Remove(newPos));
    }

    private void Maximize(object userData)
    {
      EditorWindow win = userData as EditorWindow;
      if (!((UnityEngine.Object) win != (UnityEngine.Object) null))
        return;
      WindowLayout.Maximize(win);
    }

    internal void Close(object userData)
    {
      EditorWindow editorWindow = userData as EditorWindow;
      if ((UnityEngine.Object) editorWindow != (UnityEngine.Object) null)
      {
        editorWindow.Close();
      }
      else
      {
        this.RemoveTab((EditorWindow) null, false);
        this.KillIfEmpty();
      }
    }

    private bool AllowTabAction()
    {
      int num = 0;
      ContainerWindow containerWindow = ((IEnumerable<ContainerWindow>) ContainerWindow.windows).First<ContainerWindow>((Func<ContainerWindow, bool>) (e => e.showMode == ShowMode.MainWindow));
      if ((UnityEngine.Object) containerWindow != (UnityEngine.Object) null)
      {
        foreach (View allChild in containerWindow.rootView.allChildren)
        {
          DockArea dockArea = allChild as DockArea;
          if ((UnityEngine.Object) dockArea != (UnityEngine.Object) null)
          {
            num += dockArea.m_Panes.Count;
            if (num > 1)
              return true;
          }
        }
      }
      return false;
    }

    protected override void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow view)
    {
      base.AddDefaultItemsToMenu(menu, view);
      if (this.parent.window.showMode == ShowMode.MainWindow)
        menu.AddItem(EditorGUIUtility.TextContent("Maximize"), !(this.parent is SplitView), new GenericMenu.MenuFunction2(this.Maximize), (object) view);
      else
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Maximize"));
      if (this.window.showMode != ShowMode.MainWindow || this.AllowTabAction())
        menu.AddItem(EditorGUIUtility.TextContent("Close Tab"), false, new GenericMenu.MenuFunction2(this.Close), (object) view);
      else
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Close Tab"));
      menu.AddSeparator("");
      System.Type[] paneTypes = this.GetPaneTypes();
      GUIContent guiContent = EditorGUIUtility.TextContent("Add Tab");
      foreach (System.Type t in paneTypes)
      {
        if (t != null)
        {
          GUIContent content = new GUIContent(EditorWindow.GetLocalizedTitleContentFromType(t));
          content.text = guiContent.text + "/" + content.text;
          menu.AddItem(content, false, new GenericMenu.MenuFunction2(this.AddTabToHere), (object) t);
        }
      }
    }

    private void AddTabToHere(object userData)
    {
      this.AddTab((EditorWindow) ScriptableObject.CreateInstance((System.Type) userData));
    }

    private float GetTabWidth(float width)
    {
      int count = this.m_Panes.Count;
      if (DockArea.s_DropInfo != null && object.ReferenceEquals((object) DockArea.s_DropInfo.dropArea, (object) this))
        ++count;
      if (this.m_Panes.IndexOf(DockArea.s_DragPane) != -1)
        --count;
      return Mathf.Min(width / (float) count, 100f);
    }

    private int GetTabAtMousePos(Vector2 mousePos, Rect position)
    {
      return (int) Mathf.Min((mousePos.x - position.xMin) / this.GetTabWidth(position.width), 100f);
    }

    internal override void Initialize(ContainerWindow win)
    {
      base.Initialize(win);
      this.RemoveNullWindows();
      foreach (EditorWindow pane in this.m_Panes)
        pane.m_Parent = (HostView) this;
    }

    private static void CheckDragWindowExists()
    {
      if (DockArea.s_DragMode != 1 || (bool) ((UnityEngine.Object) PaneDragTab.get.m_Window))
        return;
      DockArea.s_OriginalDragSource.RemoveTab(DockArea.s_DragPane);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) DockArea.s_DragPane);
      PaneDragTab.get.Close();
      GUIUtility.hotControl = 0;
      DockArea.ResetDragVars();
    }

    private void DragTab(Rect pos, GUIStyle tabStyle)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      float tabWidth = this.GetTabWidth(pos.width);
      Event current = Event.current;
      if (DockArea.s_DragMode != 0 && GUIUtility.hotControl == 0)
      {
        PaneDragTab.get.Close();
        DockArea.ResetDragVars();
      }
      EventType typeForControl = current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (pos.Contains(current.mousePosition) && GUIUtility.hotControl == 0)
          {
            int tabAtMousePos = this.GetTabAtMousePos(current.mousePosition, pos);
            if (tabAtMousePos < this.m_Panes.Count)
            {
              switch (current.button)
              {
                case 0:
                  if (tabAtMousePos != this.selected)
                    this.selected = tabAtMousePos;
                  GUIUtility.hotControl = controlId;
                  DockArea.s_StartDragPosition = current.mousePosition;
                  DockArea.s_DragMode = 0;
                  current.Use();
                  break;
                case 2:
                  this.m_Panes[tabAtMousePos].Close();
                  current.Use();
                  break;
              }
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 screenPoint = GUIUtility.GUIToScreenPoint(current.mousePosition);
            if (DockArea.s_DragMode != 0)
            {
              DockArea.s_DragMode = 0;
              PaneDragTab.get.Close();
              EditorApplication.CallbackFunction update = EditorApplication.update;
              // ISSUE: reference to a compiler-generated field
              if (DockArea.\u003C\u003Ef__mg\u0024cache1 == null)
              {
                // ISSUE: reference to a compiler-generated field
                DockArea.\u003C\u003Ef__mg\u0024cache1 = new EditorApplication.CallbackFunction(DockArea.CheckDragWindowExists);
              }
              // ISSUE: reference to a compiler-generated field
              EditorApplication.CallbackFunction fMgCache1 = DockArea.\u003C\u003Ef__mg\u0024cache1;
              EditorApplication.update = update - fMgCache1;
              if (DockArea.s_DropInfo != null && DockArea.s_DropInfo.dropArea != null)
              {
                DockArea.s_DropInfo.dropArea.PerformDrop(DockArea.s_DragPane, DockArea.s_DropInfo, screenPoint);
              }
              else
              {
                EditorWindow dragPane = DockArea.s_DragPane;
                DockArea.ResetDragVars();
                this.RemoveTab(dragPane);
                Rect position = dragPane.position;
                position.x = screenPoint.x - position.width * 0.5f;
                position.y = screenPoint.y - position.height * 0.5f;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                  position.y = Mathf.Max(InternalEditorUtility.GetBoundsOfDesktopAtPoint(screenPoint).y, position.y);
                EditorWindow.CreateNewWindowForEditorWindow(dragPane, false, false);
                dragPane.position = dragPane.m_Parent.window.FitWindowRectToScreen(position, true, true);
                GUIUtility.hotControl = 0;
                GUIUtility.ExitGUI();
              }
              DockArea.ResetDragVars();
            }
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 vector2 = current.mousePosition - DockArea.s_StartDragPosition;
            current.Use();
            Rect screenPosition = this.screenPosition;
            bool flag = this.window.showMode != ShowMode.MainWindow || this.AllowTabAction();
            if (DockArea.s_DragMode == 0 && (double) vector2.sqrMagnitude > 99.0 && flag)
            {
              DockArea.s_DragMode = 1;
              DockArea.s_PlaceholderPos = this.selected;
              DockArea.s_DragPane = this.m_Panes[this.selected];
              DockArea.s_IgnoreDockingForView = this.m_Panes.Count != 1 ? (View) null : (View) this;
              DockArea.s_OriginalDragSource = this;
              PaneDragTab.get.Show(new Rect((float) ((double) pos.x + (double) screenPosition.x + (double) tabWidth * (double) this.selected), pos.y + screenPosition.y, tabWidth, pos.height), DockArea.s_DragPane.titleContent, this.position.size, GUIUtility.GUIToScreenPoint(current.mousePosition));
              EditorApplication.CallbackFunction update = EditorApplication.update;
              // ISSUE: reference to a compiler-generated field
              if (DockArea.\u003C\u003Ef__mg\u0024cache0 == null)
              {
                // ISSUE: reference to a compiler-generated field
                DockArea.\u003C\u003Ef__mg\u0024cache0 = new EditorApplication.CallbackFunction(DockArea.CheckDragWindowExists);
              }
              // ISSUE: reference to a compiler-generated field
              EditorApplication.CallbackFunction fMgCache0 = DockArea.\u003C\u003Ef__mg\u0024cache0;
              EditorApplication.update = update + fMgCache0;
              GUIUtility.ExitGUI();
            }
            if (DockArea.s_DragMode == 1)
            {
              DropInfo di = (DropInfo) null;
              ContainerWindow[] windows = ContainerWindow.windows;
              Vector2 screenPoint = GUIUtility.GUIToScreenPoint(current.mousePosition);
              ContainerWindow inFrontOf = (ContainerWindow) null;
              foreach (ContainerWindow containerWindow in windows)
              {
                SplitView rootSplitView = containerWindow.rootSplitView;
                if (!((UnityEngine.Object) rootSplitView == (UnityEngine.Object) null))
                {
                  di = rootSplitView.DragOverRootView(screenPoint);
                  if (di == null)
                  {
                    foreach (View allChild in containerWindow.rootView.allChildren)
                    {
                      IDropArea dropArea = allChild as IDropArea;
                      if (dropArea != null)
                        di = dropArea.DragOver(DockArea.s_DragPane, screenPoint);
                      if (di != null)
                        break;
                    }
                  }
                  if (di != null)
                  {
                    inFrontOf = containerWindow;
                    break;
                  }
                }
              }
              if (di == null)
                di = new DropInfo((IDropArea) null);
              if (di.type != DropInfo.Type.Tab)
                DockArea.s_PlaceholderPos = -1;
              DockArea.s_DropInfo = di;
              if ((bool) ((UnityEngine.Object) PaneDragTab.get.m_Window))
                PaneDragTab.get.SetDropInfo(di, screenPoint, inFrontOf);
            }
            break;
          }
          break;
        case EventType.Repaint:
          float xMin = pos.xMin;
          int num = 0;
          if ((bool) ((UnityEngine.Object) this.actualView))
          {
            for (int index = 0; index < this.m_Panes.Count; ++index)
            {
              if (!((UnityEngine.Object) DockArea.s_DragPane == (UnityEngine.Object) this.m_Panes[index]))
              {
                if (DockArea.s_DropInfo != null && object.ReferenceEquals((object) DockArea.s_DropInfo.dropArea, (object) this) && DockArea.s_PlaceholderPos == num)
                  xMin += tabWidth;
                Rect rect = new Rect(xMin, pos.yMin, tabWidth, pos.height);
                float x = Mathf.Round(rect.x);
                Rect position = new Rect(x, rect.y, Mathf.Round(rect.x + rect.width) - x, rect.height);
                tabStyle.Draw(position, this.m_Panes[index].titleContent, false, false, index == this.selected, this.hasFocus);
                xMin += tabWidth;
                ++num;
              }
            }
            break;
          }
          Rect rect1 = new Rect(xMin, pos.yMin, tabWidth, pos.height);
          float x1 = Mathf.Round(rect1.x);
          Rect position1 = new Rect(x1, rect1.y, Mathf.Round(rect1.x + rect1.width) - x1, rect1.height);
          tabStyle.Draw(position1, "Failed to load", false, false, true, false);
          break;
        default:
          if (typeForControl == EventType.ContextClick && pos.Contains(current.mousePosition) && GUIUtility.hotControl == 0)
          {
            int tabAtMousePos = this.GetTabAtMousePos(current.mousePosition, pos);
            if (tabAtMousePos < this.m_Panes.Count)
              this.PopupGenericMenu(this.m_Panes[tabAtMousePos], new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f));
            break;
          }
          break;
      }
      this.selected = Mathf.Clamp(this.selected, 0, this.m_Panes.Count - 1);
    }

    protected override RectOffset GetBorderSize()
    {
      if (!(bool) ((UnityEngine.Object) this.window))
        return this.m_BorderSize;
      RectOffset borderSize = this.m_BorderSize;
      int num1 = 0;
      this.m_BorderSize.bottom = num1;
      int num2 = num1;
      this.m_BorderSize.top = num2;
      int num3 = num2;
      this.m_BorderSize.right = num3;
      int num4 = num3;
      borderSize.left = num4;
      Rect windowPosition = this.windowPosition;
      if ((double) windowPosition.xMin != 0.0)
        this.m_BorderSize.left += 2;
      if ((double) windowPosition.xMax != (double) this.window.position.width)
        this.m_BorderSize.right += 2;
      this.m_BorderSize.top = 17 + (!((UnityEngine.Object) this.window != (UnityEngine.Object) null) || this.window.showMode == ShowMode.MainWindow ? 2 : 5);
      this.m_BorderSize.bottom = !((UnityEngine.Object) this.window != (UnityEngine.Object) null) || this.window.showMode == ShowMode.MainWindow ? 2 : 0;
      return this.m_BorderSize;
    }

    private static void ResetDragVars()
    {
      DockArea.s_DragPane = (EditorWindow) null;
      DockArea.s_DropInfo = (DropInfo) null;
      DockArea.s_PlaceholderPos = -1;
      DockArea.s_DragMode = 0;
      DockArea.s_OriginalDragSource = (DockArea) null;
    }
  }
}
