// Decompiled with JetBrains decompiler
// Type: UnityEditor.CollabToolbarWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Web;
using UnityEngine;

namespace UnityEditor
{
  internal class CollabToolbarWindow : WebViewEditorStaticWindow, IHasCustomMenu
  {
    public static bool s_ToolbarIsVisible = false;
    private const string kWindowName = "Unity Collab Toolbar";
    private static long s_LastClosedTime;
    private static CollabToolbarWindow s_CollabToolbarWindow;
    private const int kWindowWidth = 320;
    private const int kWindowHeight = 350;

    internal override WebView webView
    {
      get
      {
        return WebViewStatic.GetWebView();
      }
      set
      {
        WebViewStatic.SetWebView(value);
      }
    }

    public static void CloseToolbarWindows()
    {
      // ISSUE: reference to a compiler-generated field
      if (CollabToolbarWindow.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CollabToolbarWindow.\u003C\u003Ef__mg\u0024cache0 = new EditorApplication.CallbackFunction(CollabToolbarWindow.CloseToolbarWindowsImmediately);
      }
      // ISSUE: reference to a compiler-generated field
      EditorApplication.CallDelayed(CollabToolbarWindow.\u003C\u003Ef__mg\u0024cache0, 1f);
    }

    public static void CloseToolbarWindowsImmediately()
    {
      foreach (EditorWindow editorWindow in UnityEngine.Resources.FindObjectsOfTypeAll<CollabToolbarWindow>())
        editorWindow.Close();
    }

    [MenuItem("Window/Collab Toolbar", false, 2011, true)]
    public static CollabToolbarWindow ShowToolbarWindow()
    {
      return EditorWindow.GetWindow<CollabToolbarWindow>(false, "Unity Collab Toolbar");
    }

    [MenuItem("Window/Collab Toolbar", true)]
    public static bool ValidateShowToolbarWindow()
    {
      return true;
    }

    internal static bool ShowCenteredAtPosition(Rect buttonRect)
    {
      buttonRect.x -= 160f;
      if (DateTime.Now.Ticks / 10000L < CollabToolbarWindow.s_LastClosedTime + 50L)
        return false;
      if (Event.current.type != EventType.Layout)
        Event.current.Use();
      if ((UnityEngine.Object) CollabToolbarWindow.s_CollabToolbarWindow == (UnityEngine.Object) null)
        CollabToolbarWindow.s_CollabToolbarWindow = ScriptableObject.CreateInstance<CollabToolbarWindow>();
      Vector2 windowSize = new Vector2(320f, 350f);
      CollabToolbarWindow.s_CollabToolbarWindow.initialOpenUrl = "file:///" + EditorApplication.userJavascriptPackagesPath + "unityeditor-collab-toolbar/dist/index.html";
      CollabToolbarWindow.s_CollabToolbarWindow.Init();
      CollabToolbarWindow.s_CollabToolbarWindow.ShowAsDropDown(buttonRect, windowSize);
      CollabToolbarWindow.s_CollabToolbarWindow.OnFocus();
      return true;
    }

    public void OnReceiveTitle(string title)
    {
      this.titleContent.text = title;
    }

    public new void OnInitScripting()
    {
      base.OnInitScripting();
    }

    public override void OnEnable()
    {
      this.minSize = new Vector2(320f, 350f);
      this.maxSize = new Vector2(320f, 350f);
      this.initialOpenUrl = "file:///" + EditorApplication.userJavascriptPackagesPath + "unityeditor-collab-toolbar/dist/index.html";
      base.OnEnable();
      if (!(bool) ((UnityEngine.Object) CollabToolbarWindow.s_CollabToolbarWindow))
        return;
      CollabToolbarWindow.s_ToolbarIsVisible = true;
      this.NotifyVisibility(CollabToolbarWindow.s_ToolbarIsVisible);
    }

    internal void OnDisable()
    {
      CollabToolbarWindow.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      if ((bool) ((UnityEngine.Object) CollabToolbarWindow.s_CollabToolbarWindow))
      {
        CollabToolbarWindow.s_ToolbarIsVisible = false;
        this.NotifyVisibility(CollabToolbarWindow.s_ToolbarIsVisible);
      }
      CollabToolbarWindow.s_CollabToolbarWindow = (CollabToolbarWindow) null;
    }

    public new void OnDestroy()
    {
      this.OnLostFocus();
      base.OnDestroy();
    }
  }
}
