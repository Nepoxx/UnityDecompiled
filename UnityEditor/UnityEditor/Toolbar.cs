// Decompiled with JetBrains decompiler
// Type: UnityEditor.Toolbar
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Collaboration;
using UnityEditor.Connect;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor
{
  internal class Toolbar : GUIView
  {
    private static readonly string[] s_ToolControlNames = new string[6]{ "ToolbarPersistentToolsPan", "ToolbarPersistentToolsTranslate", "ToolbarPersistentToolsRotate", "ToolbarPersistentToolsScale", "ToolbarPersistentToolsRect", "ToolbarPersistentToolsTransform" };
    private static bool m_ShowCollabTooltip = false;
    private static GUIContent[] s_ShownToolIcons = new GUIContent[6];
    public static Toolbar get = (Toolbar) null;
    public static bool requestShowCollabToolbar = false;
    private Toolbar.CollabToolbarState m_CollabToolbarState = Toolbar.CollabToolbarState.UpToDate;
    private static GUIContent[] s_ToolIcons;
    private static GUIContent[] s_ViewToolIcons;
    private static GUIContent[] s_PivotIcons;
    private static GUIContent[] s_PivotRotation;
    private static GUIContent s_LayerContent;
    private static GUIContent[] s_PlayIcons;
    private static GUIContent s_AccountContent;
    private static GUIContent s_CloudIcon;
    private static GUIContent[] s_CollabIcons;
    private const float kCollabButtonWidth = 78f;
    private ButtonWithAnimatedIconRotation m_CollabButton;
    private string m_DynamicTooltip;
    [SerializeField]
    private string m_LastLoadedLayoutName;

    private void InitializeToolIcons()
    {
      if (Toolbar.s_ToolIcons != null)
        return;
      Toolbar.s_ToolIcons = new GUIContent[10]
      {
        EditorGUIUtility.IconContent("MoveTool", "|Move Tool"),
        EditorGUIUtility.IconContent("RotateTool", "|Rotate Tool"),
        EditorGUIUtility.IconContent("ScaleTool", "|Scale Tool"),
        EditorGUIUtility.IconContent("RectTool", "|Rect Tool"),
        EditorGUIUtility.IconContent("TransformTool", "|Move, Rotate or Scale selected objects."),
        EditorGUIUtility.IconContent("MoveTool On"),
        EditorGUIUtility.IconContent("RotateTool On"),
        EditorGUIUtility.IconContent("ScaleTool On"),
        EditorGUIUtility.IconContent("RectTool On"),
        EditorGUIUtility.IconContent("TransformTool On")
      };
      string text = "|Hand Tool";
      Toolbar.s_ViewToolIcons = new GUIContent[10]
      {
        EditorGUIUtility.IconContent("ViewToolOrbit", text),
        EditorGUIUtility.IconContent("ViewToolMove", text),
        EditorGUIUtility.IconContent("ViewToolZoom", text),
        EditorGUIUtility.IconContent("ViewToolOrbit", text),
        EditorGUIUtility.IconContent("ViewToolOrbit", "|Orbit the Scene view."),
        EditorGUIUtility.IconContent("ViewToolOrbit On", text),
        EditorGUIUtility.IconContent("ViewToolMove On", text),
        EditorGUIUtility.IconContent("ViewToolZoom On", text),
        EditorGUIUtility.IconContent("ViewToolOrbit On"),
        EditorGUIUtility.IconContent("ViewToolOrbit On", text)
      };
      Toolbar.s_PivotIcons = new GUIContent[2]
      {
        EditorGUIUtility.TextContentWithIcon("Center|Toggle Tool Handle Position\n\nThe tool handle is placed at the center of the selection.", "ToolHandleCenter"),
        EditorGUIUtility.TextContentWithIcon("Pivot|Toggle Tool Handle Position\n\nThe tool handle is placed at the active object's pivot point.", "ToolHandlePivot")
      };
      Toolbar.s_PivotRotation = new GUIContent[2]
      {
        EditorGUIUtility.TextContentWithIcon("Local|Toggle Tool Handle Rotation\n\nTool handles are in the active object's rotation.", "ToolHandleLocal"),
        EditorGUIUtility.TextContentWithIcon("Global|Toggle Tool Handle Rotation\n\nTool handles are in global rotation.", "ToolHandleGlobal")
      };
      Toolbar.s_LayerContent = EditorGUIUtility.TextContent("Layers|Which layers are visible in the Scene views.");
      Toolbar.s_PlayIcons = new GUIContent[8]
      {
        EditorGUIUtility.IconContent("PlayButton", "|Play"),
        EditorGUIUtility.IconContent("PauseButton", "|Pause"),
        EditorGUIUtility.IconContent("StepButton", "|Step"),
        EditorGUIUtility.IconContent("PlayButtonProfile", "|Profiler Play"),
        EditorGUIUtility.IconContent("PlayButton On"),
        EditorGUIUtility.IconContent("PauseButton On"),
        EditorGUIUtility.IconContent("StepButton On"),
        EditorGUIUtility.IconContent("PlayButtonProfile On")
      };
      Toolbar.s_CloudIcon = EditorGUIUtility.IconContent("CloudConnect");
      Toolbar.s_AccountContent = new GUIContent("Account");
      Toolbar.s_CollabIcons = new GUIContent[9]
      {
        EditorGUIUtility.TextContentWithIcon("Collab| You need to enable collab.", "CollabNew"),
        EditorGUIUtility.TextContentWithIcon("Collab| You are up to date.", "Collab"),
        EditorGUIUtility.TextContentWithIcon("Collab| Please fix your conflicts prior to publishing.", "CollabConflict"),
        EditorGUIUtility.TextContentWithIcon("Collab| Last operation failed. Please retry later.", "CollabError"),
        EditorGUIUtility.TextContentWithIcon("Collab| Please update, there are server changes.", "CollabPull"),
        EditorGUIUtility.TextContentWithIcon("Collab| You have files to publish.", "CollabPush"),
        EditorGUIUtility.TextContentWithIcon("Collab| Operation in progress.", "CollabProgress"),
        EditorGUIUtility.TextContentWithIcon("Collab| Collab is disabled.", "CollabNew"),
        EditorGUIUtility.TextContentWithIcon("Collab| Please check your network connection.", "CollabNew")
      };
    }

    private GUIContent currentCollabContent
    {
      get
      {
        GUIContent guiContent = new GUIContent(Toolbar.s_CollabIcons[(int) this.m_CollabToolbarState]);
        if (!Toolbar.m_ShowCollabTooltip)
          guiContent.tooltip = (string) null;
        else if (this.m_DynamicTooltip != "")
          guiContent.tooltip = this.m_DynamicTooltip;
        if (Collab.instance.AreTestsRunning())
          guiContent.text = "CTF";
        return guiContent;
      }
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.visualTree.clippingOptions = VisualElement.ClippingOptions.NoClipping;
      EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(((GUIView) this).Repaint);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.OnSelectionChange);
      UnityConnect.instance.StateChanged += new UnityEditor.Connect.StateChangedDelegate(this.OnUnityConnectStateChanged);
      UnityConnect.instance.UserStateChanged += new UserStateChangedDelegate(this.OnUnityConnectUserStateChanged);
      Toolbar.get = this;
      Collab.instance.StateChanged += new UnityEditor.Collaboration.StateChangedDelegate(this.OnCollabStateChanged);
      if (this.m_CollabButton != null)
        return;
      this.m_CollabButton = new ButtonWithAnimatedIconRotation((Func<float>) (() => (float) EditorApplication.timeSinceStartup * 500f), new Action(((GUIView) this).Repaint), 20f, true);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(((GUIView) this).Repaint);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.OnSelectionChange);
      UnityConnect.instance.StateChanged -= new UnityEditor.Connect.StateChangedDelegate(this.OnUnityConnectStateChanged);
      UnityConnect.instance.UserStateChanged -= new UserStateChangedDelegate(this.OnUnityConnectUserStateChanged);
      Collab.instance.StateChanged -= new UnityEditor.Collaboration.StateChangedDelegate(this.OnCollabStateChanged);
      if (this.m_CollabButton == null)
        return;
      this.m_CollabButton.Clear();
    }

    internal static string lastLoadedLayoutName
    {
      get
      {
        return !string.IsNullOrEmpty(Toolbar.get.m_LastLoadedLayoutName) ? Toolbar.get.m_LastLoadedLayoutName : "Layout";
      }
      set
      {
        Toolbar.get.m_LastLoadedLayoutName = value;
        Toolbar.get.Repaint();
      }
    }

    protected override bool OnFocus()
    {
      return false;
    }

    private void OnSelectionChange()
    {
      Tools.OnSelectionChange();
      this.Repaint();
    }

    protected void OnUnityConnectStateChanged(ConnectInfo state)
    {
      this.UpdateCollabToolbarState();
      Toolbar.RepaintToolbar();
    }

    protected void OnUnityConnectUserStateChanged(UserInfo state)
    {
      this.UpdateCollabToolbarState();
    }

    private Rect GetThinArea(Rect pos)
    {
      return new Rect(pos.x, 7f, pos.width, 18f);
    }

    private Rect GetThickArea(Rect pos)
    {
      return new Rect(pos.x, 5f, pos.width, 24f);
    }

    private void ReserveWidthLeft(float width, ref Rect pos)
    {
      pos.x -= width;
      pos.width = width;
    }

    private void ReserveWidthRight(float width, ref Rect pos)
    {
      pos.x += pos.width;
      pos.width = width;
    }

    private void ReserveRight(float width, ref Rect pos)
    {
      pos.x += width;
    }

    private void ReserveBottom(float height, ref Rect pos)
    {
      pos.y += height;
    }

    protected override void OldOnGUI()
    {
      float width1 = 10f;
      float width2 = 20f;
      float num1 = 32f;
      float num2 = 64f;
      float width3 = 80f;
      this.InitializeToolIcons();
      bool willChangePlaymode = EditorApplication.isPlayingOrWillChangePlaymode;
      if (willChangePlaymode)
        GUI.color = (Color) HostView.kPlayModeDarken;
      if (Event.current.type == EventType.Repaint)
        Toolbar.Styles.appToolbar.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), false, false, false, false);
      Rect pos = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      this.ReserveWidthRight(width1, ref pos);
      this.ReserveWidthRight(num1 * (float) Toolbar.s_ShownToolIcons.Length, ref pos);
      this.DoToolButtons(this.GetThickArea(pos));
      this.ReserveWidthRight(width2, ref pos);
      this.ReserveWidthRight(num2 * 2f, ref pos);
      this.DoPivotButtons(this.GetThinArea(pos));
      pos = new Rect((float) Mathf.RoundToInt((float) (((double) this.position.width - 100.0) / 2.0)), 0.0f, 140f, 0.0f);
      GUILayout.BeginArea(this.GetThickArea(pos));
      GUILayout.BeginHorizontal();
      this.DoPlayButtons(willChangePlaymode);
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
      pos = new Rect(this.position.width, 0.0f, 0.0f, 0.0f);
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(width3, ref pos);
      this.DoLayoutDropDown(this.GetThinArea(pos));
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(width3, ref pos);
      this.DoLayersDropDown(this.GetThinArea(pos));
      this.ReserveWidthLeft(width2, ref pos);
      this.ReserveWidthLeft(width3, ref pos);
      if (EditorGUI.DropdownButton(this.GetThinArea(pos), Toolbar.s_AccountContent, FocusType.Passive, Toolbar.Styles.dropdown))
        this.ShowUserMenu(this.GetThinArea(pos));
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(32f, ref pos);
      if (GUI.Button(this.GetThinArea(pos), Toolbar.s_CloudIcon))
        UnityConnectServiceCollection.instance.ShowService("Hub", true, "cloud_icon");
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(78f, ref pos);
      this.DoCollabDropDown(this.GetThinArea(pos));
      EditorGUI.ShowRepaints();
      Highlighter.ControlHighlightGUI((GUIView) this);
    }

    private void ShowUserMenu(Rect dropDownRect)
    {
      GenericMenu genericMenu = new GenericMenu();
      if (!UnityConnect.instance.online)
      {
        genericMenu.AddDisabledItem(new GUIContent("Go to account"));
        genericMenu.AddDisabledItem(new GUIContent("Sign in..."));
        if (!Application.HasProLicense())
        {
          genericMenu.AddSeparator("");
          genericMenu.AddDisabledItem(new GUIContent("Upgrade to Pro"));
        }
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Toolbar.\u003CShowUserMenu\u003Ec__AnonStorey0 menuCAnonStorey0 = new Toolbar.\u003CShowUserMenu\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        menuCAnonStorey0.accountUrl = UnityConnect.instance.GetConfigurationURL(CloudConfigUrl.CloudPortal);
        if (UnityConnect.instance.loggedIn)
        {
          // ISSUE: reference to a compiler-generated method
          genericMenu.AddItem(new GUIContent("Go to account"), false, new GenericMenu.MenuFunction(menuCAnonStorey0.\u003C\u003Em__0));
        }
        else
          genericMenu.AddDisabledItem(new GUIContent("Go to account"));
        if (UnityConnect.instance.loggedIn)
        {
          string text = "Sign out " + UnityConnect.instance.userInfo.displayName;
          genericMenu.AddItem(new GUIContent(text), false, (GenericMenu.MenuFunction) (() => UnityConnect.instance.Logout()));
        }
        else
          genericMenu.AddItem(new GUIContent("Sign in..."), false, (GenericMenu.MenuFunction) (() => UnityConnect.instance.ShowLogin()));
        if (!Application.HasProLicense())
        {
          genericMenu.AddSeparator("");
          genericMenu.AddItem(new GUIContent("Upgrade to Pro"), false, (GenericMenu.MenuFunction) (() => Application.OpenURL("https://store.unity3d.com/")));
        }
      }
      genericMenu.DropDown(dropDownRect);
    }

    private void DoToolButtons(Rect rect)
    {
      GUI.changed = false;
      int selected = !Tools.viewToolActive ? (int) Tools.current : 0;
      for (int index = 1; index < Toolbar.s_ShownToolIcons.Length; ++index)
      {
        Toolbar.s_ShownToolIcons[index] = Toolbar.s_ToolIcons[index - 1 + (index != selected ? 0 : Toolbar.s_ShownToolIcons.Length - 1)];
        Toolbar.s_ShownToolIcons[index].tooltip = Toolbar.s_ToolIcons[index - 1].tooltip;
      }
      Toolbar.s_ShownToolIcons[0] = Toolbar.s_ViewToolIcons[(int) (Tools.viewTool + (selected != 0 ? 0 : Toolbar.s_ShownToolIcons.Length - 1))];
      int num = GUI.Toolbar(rect, selected, Toolbar.s_ShownToolIcons, Toolbar.s_ToolControlNames, (GUIStyle) "Command", GUI.ToolbarButtonSize.FitToContents);
      if (!GUI.changed)
        return;
      Tools.current = (Tool) num;
      Tools.ResetGlobalHandleRotation();
    }

    private void DoPivotButtons(Rect rect)
    {
      GUI.SetNextControlName("ToolbarToolPivotPositionButton");
      Tools.pivotMode = (PivotMode) EditorGUI.CycleButton(new Rect(rect.x, rect.y, rect.width / 2f, rect.height), (int) Tools.pivotMode, Toolbar.s_PivotIcons, (GUIStyle) "ButtonLeft");
      if (Tools.current == Tool.Scale && Selection.transforms.Length < 2)
        GUI.enabled = false;
      GUI.SetNextControlName("ToolbarToolPivotOrientationButton");
      PivotRotation pivotRotation = (PivotRotation) EditorGUI.CycleButton(new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height), (int) Tools.pivotRotation, Toolbar.s_PivotRotation, (GUIStyle) "ButtonRight");
      if (Tools.pivotRotation != pivotRotation)
      {
        Tools.pivotRotation = pivotRotation;
        if (pivotRotation == PivotRotation.Global)
          Tools.ResetGlobalHandleRotation();
      }
      if (Tools.current == Tool.Scale)
        GUI.enabled = true;
      if (!GUI.changed)
        return;
      Tools.RepaintAllToolViews();
    }

    private void DoPlayButtons(bool isOrWillEnterPlaymode)
    {
      bool isPlaying = EditorApplication.isPlaying;
      GUI.changed = false;
      int index = !isPlaying ? 0 : 4;
      Color color = GUI.color + new Color(0.01f, 0.01f, 0.01f, 0.01f);
      GUI.contentColor = new Color(1f / color.r, 1f / color.g, 1f / color.g, 1f / color.a);
      GUI.SetNextControlName("ToolbarPlayModePlayButton");
      GUILayout.Toggle(isOrWillEnterPlaymode, Toolbar.s_PlayIcons[index], (GUIStyle) "CommandLeft", new GUILayoutOption[0]);
      GUI.backgroundColor = Color.white;
      if (GUI.changed)
      {
        Toolbar.TogglePlaying();
        GUIUtility.ExitGUI();
      }
      GUI.changed = false;
      GUI.SetNextControlName("ToolbarPlayModePauseButton");
      bool flag = GUILayout.Toggle(EditorApplication.isPaused, Toolbar.s_PlayIcons[index + 1], (GUIStyle) "CommandMid", new GUILayoutOption[0]);
      if (GUI.changed)
      {
        EditorApplication.isPaused = flag;
        GUIUtility.ExitGUI();
      }
      GUI.SetNextControlName("ToolbarPlayModeStepButton");
      if (!GUILayout.Button(Toolbar.s_PlayIcons[index + 2], (GUIStyle) "CommandRight", new GUILayoutOption[0]))
        return;
      EditorApplication.Step();
      GUIUtility.ExitGUI();
    }

    private void DoLayersDropDown(Rect rect)
    {
      GUIStyle style = (GUIStyle) "DropDown";
      if (!EditorGUI.DropdownButton(rect, Toolbar.s_LayerContent, FocusType.Passive, style) || !LayerVisibilityWindow.ShowAtPosition(rect))
        return;
      GUIUtility.ExitGUI();
    }

    private void DoLayoutDropDown(Rect rect)
    {
      if (!EditorGUI.DropdownButton(rect, GUIContent.Temp(Toolbar.lastLoadedLayoutName), FocusType.Passive, (GUIStyle) "DropDown"))
        return;
      Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
      rect.x = screenPoint.x;
      rect.y = screenPoint.y;
      EditorUtility.Internal_DisplayPopupMenu(rect, "Window/Layouts", (UnityEngine.Object) this, 0);
    }

    private void ShowPopup(Rect rect)
    {
      this.ReserveRight(39f, ref rect);
      this.ReserveBottom(5f, ref rect);
      Rect screenRect = GUIUtility.GUIToScreenRect(rect);
      AssetDatabase.SaveAssets();
      if (!CollabToolbarWindow.ShowCenteredAtPosition(screenRect))
        return;
      GUIUtility.ExitGUI();
    }

    private void DoCollabDropDown(Rect rect)
    {
      this.UpdateCollabToolbarState();
      bool flag = Toolbar.requestShowCollabToolbar;
      Toolbar.requestShowCollabToolbar = false;
      using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
      {
        bool animate = this.m_CollabToolbarState == Toolbar.CollabToolbarState.InProgress;
        EditorGUIUtility.SetIconSize(new Vector2(12f, 12f));
        if (this.m_CollabButton.OnGUI(rect, this.currentCollabContent, animate, Toolbar.Styles.collabButtonStyle))
          flag = true;
        EditorGUIUtility.SetIconSize(Vector2.zero);
      }
      if (!flag)
        return;
      this.ShowPopup(rect);
    }

    public void OnCollabStateChanged(CollabInfo info)
    {
      this.UpdateCollabToolbarState();
    }

    public void UpdateCollabToolbarState()
    {
      Toolbar.CollabToolbarState collabToolbarState = Toolbar.CollabToolbarState.UpToDate;
      bool flag1 = UnityConnect.instance.connectInfo.online && UnityConnect.instance.connectInfo.loggedIn;
      this.m_DynamicTooltip = "";
      if (flag1)
      {
        Collab instance = Collab.instance;
        CollabInfo collabInfo = instance.collabInfo;
        int code = 0;
        int priority = 4;
        int behaviour = 2;
        string errorMsg = "";
        string errorShortMsg = "";
        bool flag2 = false;
        if (instance.GetError(5, out code, out priority, out behaviour, out errorMsg, out errorShortMsg))
        {
          flag2 = priority <= 1;
          this.m_DynamicTooltip = errorShortMsg;
        }
        if (!collabInfo.ready)
          collabToolbarState = Toolbar.CollabToolbarState.InProgress;
        else if (flag2)
          collabToolbarState = Toolbar.CollabToolbarState.OperationError;
        else if (collabInfo.inProgress)
          collabToolbarState = Toolbar.CollabToolbarState.InProgress;
        else if (!UnityConnect.instance.projectInfo.projectBound || !Collab.instance.IsCollabEnabledForCurrentProject())
          collabToolbarState = Toolbar.CollabToolbarState.NeedToEnableCollab;
        else if (collabInfo.update)
          collabToolbarState = Toolbar.CollabToolbarState.ServerHasChanges;
        else if (collabInfo.conflict)
          collabToolbarState = Toolbar.CollabToolbarState.Conflict;
        else if (collabInfo.publish)
          collabToolbarState = Toolbar.CollabToolbarState.FilesToPush;
      }
      else
        collabToolbarState = Toolbar.CollabToolbarState.Offline;
      if (collabToolbarState == this.m_CollabToolbarState && CollabToolbarWindow.s_ToolbarIsVisible != Toolbar.m_ShowCollabTooltip)
        return;
      this.m_CollabToolbarState = collabToolbarState;
      Toolbar.m_ShowCollabTooltip = !CollabToolbarWindow.s_ToolbarIsVisible;
      Toolbar.RepaintToolbar();
    }

    private static void InternalWillTogglePlaymode()
    {
      InternalEditorUtility.RepaintAllViews();
    }

    private static void TogglePlaying()
    {
      EditorApplication.isPlaying = !EditorApplication.isPlaying;
      Toolbar.InternalWillTogglePlaymode();
    }

    internal static void RepaintToolbar()
    {
      if (!((UnityEngine.Object) Toolbar.get != (UnityEngine.Object) null))
        return;
      Toolbar.get.Repaint();
    }

    public float CalcHeight()
    {
      return 30f;
    }

    private enum CollabToolbarState
    {
      NeedToEnableCollab,
      UpToDate,
      Conflict,
      OperationError,
      ServerHasChanges,
      FilesToPush,
      InProgress,
      Disabled,
      Offline,
    }

    private static class Styles
    {
      public static readonly GUIStyle collabButtonStyle = new GUIStyle((GUIStyle) "Dropdown") { padding = { left = 24 } };
      public static readonly GUIStyle dropdown = (GUIStyle) "Dropdown";
      public static readonly GUIStyle appToolbar = (GUIStyle) "AppToolbar";
    }
  }
}
