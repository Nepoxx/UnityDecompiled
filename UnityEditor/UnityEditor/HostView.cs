// Decompiled with JetBrains decompiler
// Type: UnityEditor.HostView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor
{
  internal class HostView : GUIView
  {
    internal static Color kViewColor = new Color(0.76f, 0.76f, 0.76f, 1f);
    internal static PrefColor kPlayModeDarken = new PrefColor("Playmode tint", 0.8f, 0.8f, 0.8f, 1f);
    [NonSerialized]
    private Rect m_BackgroundClearRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    [NonSerialized]
    protected readonly RectOffset m_BorderSize = new RectOffset();
    internal GUIStyle background;
    [SerializeField]
    private EditorWindow m_ActualView;

    internal static event Action<HostView> actualViewChanged;

    internal EditorWindow actualView
    {
      get
      {
        return this.m_ActualView;
      }
      set
      {
        if ((UnityEngine.Object) this.m_ActualView == (UnityEngine.Object) value)
          return;
        this.DeregisterSelectedPane(true);
        this.m_ActualView = value;
        this.RegisterSelectedPane();
        if (HostView.actualViewChanged == null)
          return;
        HostView.actualViewChanged(this);
      }
    }

    protected virtual void UpdateViewMargins(EditorWindow view)
    {
    }

    protected override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      this.SetActualViewPosition(newPos);
    }

    protected virtual void SetActualViewPosition(Rect newPos)
    {
      if (!((UnityEngine.Object) this.m_ActualView != (UnityEngine.Object) null))
        return;
      this.m_ActualView.m_Pos = newPos;
      this.UpdateViewMargins(this.m_ActualView);
      this.m_ActualView.OnResized();
    }

    protected override void SetWindow(ContainerWindow win)
    {
      base.SetWindow(win);
      if (!((UnityEngine.Object) this.m_ActualView != (UnityEngine.Object) null))
        return;
      this.UpdateViewMargins(this.m_ActualView);
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.background = (GUIStyle) null;
      this.RegisterSelectedPane();
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.DeregisterSelectedPane(false);
    }

    protected override void OldOnGUI()
    {
      this.ClearBackground();
      EditorGUIUtility.ResetGUIState();
      this.DoWindowDecorationStart();
      if (this.background == null)
      {
        this.background = (GUIStyle) "hostview";
        this.background.padding.top = 0;
      }
      GUILayout.BeginVertical(this.background, new GUILayoutOption[0]);
      if ((bool) ((UnityEngine.Object) this.actualView))
        this.actualView.m_Pos = this.screenPosition;
      this.Invoke("OnGUI");
      EditorGUIUtility.ResetGUIState();
      if ((UnityEngine.Object) this.m_ActualView != (UnityEngine.Object) null && (double) this.m_ActualView.m_FadeoutTime != 0.0 && Event.current.type == EventType.Repaint)
        this.m_ActualView.DrawNotification();
      GUILayout.EndVertical();
      this.DoWindowDecorationEnd();
      EditorGUI.ShowRepaints();
    }

    protected override bool OnFocus()
    {
      this.Invoke(nameof (OnFocus));
      if ((UnityEngine.Object) this == (UnityEngine.Object) null)
        return false;
      this.Repaint();
      return true;
    }

    private void OnLostFocus()
    {
      EditorGUI.EndEditingActiveTextField();
      this.Invoke(nameof (OnLostFocus));
      this.Repaint();
    }

    protected override void OnDestroy()
    {
      if ((bool) ((UnityEngine.Object) this.m_ActualView))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ActualView, true);
      base.OnDestroy();
    }

    protected System.Type[] GetPaneTypes()
    {
      return new System.Type[7]{ typeof (SceneView), typeof (GameView), typeof (InspectorWindow), typeof (SceneHierarchyWindow), typeof (ProjectBrowser), typeof (ProfilerWindow), typeof (AnimationWindow) };
    }

    internal void OnProjectChange()
    {
      this.Invoke(nameof (OnProjectChange));
    }

    internal void OnSelectionChange()
    {
      this.Invoke(nameof (OnSelectionChange));
    }

    internal void OnDidOpenScene()
    {
      this.Invoke(nameof (OnDidOpenScene));
    }

    internal void OnInspectorUpdate()
    {
      this.Invoke(nameof (OnInspectorUpdate));
    }

    internal void OnHierarchyChange()
    {
      this.Invoke(nameof (OnHierarchyChange));
    }

    private MethodInfo GetPaneMethod(string methodName)
    {
      return this.GetPaneMethod(methodName, (object) this.m_ActualView);
    }

    private MethodInfo GetPaneMethod(string methodName, object obj)
    {
      if (obj == null)
        return (MethodInfo) null;
      System.Type type = obj.GetType();
      for (; type != null; type = type.BaseType)
      {
        MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
          return method;
      }
      return (MethodInfo) null;
    }

    public static void EndOffsetArea()
    {
      if (Event.current.type == EventType.Used)
        return;
      GUILayoutUtility.EndLayoutGroup();
      GUI.EndGroup();
    }

    public static void BeginOffsetArea(Rect screenRect, GUIContent content, GUIStyle style)
    {
      GUILayoutGroup guiLayoutGroup = EditorGUILayoutUtilityInternal.BeginLayoutArea(style, typeof (GUILayoutGroup));
      if (Event.current.type == EventType.Layout)
      {
        guiLayoutGroup.resetCoords = false;
        guiLayoutGroup.minWidth = guiLayoutGroup.maxWidth = screenRect.width + 1f;
        guiLayoutGroup.minHeight = guiLayoutGroup.maxHeight = screenRect.height + 2f;
        guiLayoutGroup.rect = Rect.MinMaxRect(-1f, -1f, guiLayoutGroup.rect.xMax, guiLayoutGroup.rect.yMax - 10f);
      }
      GUI.BeginGroup(screenRect, content, style);
    }

    public void InvokeOnGUI(Rect onGUIPosition)
    {
      if (Unsupported.IsDeveloperBuild() && (UnityEngine.Object) this.actualView != (UnityEngine.Object) null && (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.F5))
      {
        this.Reload((object) this.actualView);
      }
      else
      {
        this.DoWindowDecorationStart();
        GUIStyle style = (GUIStyle) "dockareaoverlay";
        if (this.actualView is GameView)
          GUI.Box(onGUIPosition, GUIContent.none, style);
        HostView.BeginOffsetArea(new Rect(onGUIPosition.x + 2f, onGUIPosition.y + 17f, onGUIPosition.width - 4f, (float) ((double) onGUIPosition.height - 17.0 - 2.0)), GUIContent.none, (GUIStyle) "TabWindowBackground");
        EditorGUIUtility.ResetGUIState();
        bool flag = false;
        try
        {
          this.Invoke("OnGUI");
        }
        catch (TargetInvocationException ex)
        {
          if (ex.InnerException is ExitGUIException)
            flag = true;
          throw;
        }
        finally
        {
          if (!flag)
          {
            if ((UnityEngine.Object) this.actualView != (UnityEngine.Object) null && (double) this.actualView.m_FadeoutTime != 0.0 && (Event.current != null && Event.current.type == EventType.Repaint))
              this.actualView.DrawNotification();
            HostView.EndOffsetArea();
            EditorGUIUtility.ResetGUIState();
            this.DoWindowDecorationEnd();
            if (Event.current.type == EventType.Repaint)
              style.Draw(onGUIPosition, GUIContent.none, 0);
          }
        }
      }
    }

    protected void Invoke(string methodName)
    {
      this.Invoke(methodName, (object) this.m_ActualView);
    }

    protected void Invoke(string methodName, object obj)
    {
      MethodInfo paneMethod = this.GetPaneMethod(methodName, obj);
      if (paneMethod == null)
        return;
      paneMethod.Invoke(obj, (object[]) null);
    }

    protected void RegisterSelectedPane()
    {
      if (!(bool) ((UnityEngine.Object) this.m_ActualView))
        return;
      this.m_ActualView.m_Parent = this;
      this.visualTree.Add(this.m_ActualView.rootVisualContainer);
      this.panel.getViewDataDictionary = new GetViewDataDictionary(this.m_ActualView.GetViewDataDictionary);
      this.panel.savePersistentViewData = new SavePersistentViewData(this.m_ActualView.SavePersistentViewData);
      if (this.GetPaneMethod("Update") != null)
        EditorApplication.update += new EditorApplication.CallbackFunction(this.SendUpdate);
      if (this.GetPaneMethod("ModifierKeysChanged") != null)
        EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(this.SendModKeysChanged);
      this.m_ActualView.MakeParentsSettingsMatchMe();
      if ((double) this.m_ActualView.m_FadeoutTime != 0.0)
        EditorApplication.update += new EditorApplication.CallbackFunction(this.m_ActualView.CheckForWindowRepaint);
      try
      {
        this.Invoke("OnBecameVisible");
        this.Invoke("OnFocus");
      }
      catch (TargetInvocationException ex)
      {
        Debug.LogError((object) (ex.InnerException.GetType().Name + ":" + ex.InnerException.Message));
      }
      this.UpdateViewMargins(this.m_ActualView);
    }

    protected void DeregisterSelectedPane(bool clearActualView)
    {
      if (!(bool) ((UnityEngine.Object) this.m_ActualView))
        return;
      if (this.m_ActualView.rootVisualContainer.shadow.parent == this.visualTree)
      {
        this.visualTree.Remove(this.m_ActualView.rootVisualContainer);
        this.panel.getViewDataDictionary = (GetViewDataDictionary) null;
        this.panel.savePersistentViewData = (SavePersistentViewData) null;
      }
      if (this.GetPaneMethod("Update") != null)
        EditorApplication.update -= new EditorApplication.CallbackFunction(this.SendUpdate);
      if (this.GetPaneMethod("ModifierKeysChanged") != null)
        EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(this.SendModKeysChanged);
      if ((double) this.m_ActualView.m_FadeoutTime != 0.0)
        EditorApplication.update -= new EditorApplication.CallbackFunction(this.m_ActualView.CheckForWindowRepaint);
      if (!clearActualView)
        return;
      EditorWindow actualView = this.m_ActualView;
      this.m_ActualView = (EditorWindow) null;
      this.Invoke("OnLostFocus", (object) actualView);
      this.Invoke("OnBecameInvisible", (object) actualView);
    }

    private void SendUpdate()
    {
      this.Invoke("Update");
    }

    private void SendModKeysChanged()
    {
      this.Invoke("ModifierKeysChanged");
    }

    internal RectOffset borderSize
    {
      get
      {
        return this.GetBorderSize();
      }
    }

    protected virtual RectOffset GetBorderSize()
    {
      return this.m_BorderSize;
    }

    protected void ShowGenericMenu()
    {
      GUIStyle guiStyle = (GUIStyle) "PaneOptions";
      Rect rect = new Rect((float) ((double) this.position.width - (double) guiStyle.fixedWidth - 4.0), Mathf.Floor((float) (this.background.margin.top + 20) - guiStyle.fixedHeight), guiStyle.fixedWidth, guiStyle.fixedHeight);
      if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, (GUIStyle) "PaneOptions"))
        this.PopupGenericMenu(this.m_ActualView, rect);
      MethodInfo paneMethod = this.GetPaneMethod("ShowButton", (object) this.m_ActualView);
      if (paneMethod == null)
        return;
      object[] parameters = new object[1]{ (object) new Rect((float) ((double) this.position.width - (double) guiStyle.fixedWidth - 20.0), Mathf.Floor((float) (this.background.margin.top + 4)), 16f, 16f) };
      paneMethod.Invoke((object) this.m_ActualView, parameters);
    }

    public void PopupGenericMenu(EditorWindow view, Rect pos)
    {
      GenericMenu menu = new GenericMenu();
      IHasCustomMenu hasCustomMenu = view as IHasCustomMenu;
      if (hasCustomMenu != null)
        hasCustomMenu.AddItemsToMenu(menu);
      this.AddDefaultItemsToMenu(menu, view);
      menu.DropDown(pos);
      Event.current.Use();
    }

    private void Inspect(object userData)
    {
      Selection.activeObject = (UnityEngine.Object) userData;
    }

    private void Reload(object userData)
    {
      EditorWindow pane = userData as EditorWindow;
      if ((UnityEngine.Object) pane == (UnityEngine.Object) null)
        return;
      System.Type type = pane.GetType();
      string json = EditorJsonUtility.ToJson((object) pane);
      DockArea parent = pane.m_Parent as DockArea;
      EditorWindow instance;
      if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      {
        int idx = parent.m_Panes.IndexOf(pane);
        parent.RemoveTab(pane, false);
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) pane, true);
        instance = ScriptableObject.CreateInstance(type) as EditorWindow;
        parent.AddTab(idx, instance);
      }
      else
      {
        pane.Close();
        instance = ScriptableObject.CreateInstance(type) as EditorWindow;
        if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
          instance.Show();
      }
      EditorJsonUtility.FromJsonOverwrite(json, (object) instance);
    }

    protected virtual void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow window)
    {
      if (menu.GetItemCount() != 0)
        menu.AddSeparator("");
      if (!Unsupported.IsDeveloperBuild())
        return;
      menu.AddItem(EditorGUIUtility.TextContent("Inspect Window"), false, new GenericMenu.MenuFunction2(this.Inspect), (object) window);
      menu.AddItem(EditorGUIUtility.TextContent("Inspect View"), false, new GenericMenu.MenuFunction2(this.Inspect), (object) window.m_Parent);
      menu.AddItem(EditorGUIUtility.TextContent("Reload Window _f5"), false, new GenericMenu.MenuFunction2(this.Reload), (object) window);
      menu.AddSeparator("");
    }

    protected void ClearBackground()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      EditorWindow actualView = this.actualView;
      if ((UnityEngine.Object) actualView != (UnityEngine.Object) null && actualView.dontClearBackground && (this.backgroundValid && this.position == this.m_BackgroundClearRect))
        return;
      Color color = !EditorGUIUtility.isProSkin ? HostView.kViewColor : EditorGUIUtility.kDarkViewBackground;
      GL.Clear(true, true, !EditorApplication.isPlayingOrWillChangePlaymode ? color : color * (Color) HostView.kPlayModeDarken);
      this.backgroundValid = true;
      this.m_BackgroundClearRect = this.position;
    }
  }
}
