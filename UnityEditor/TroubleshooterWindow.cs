// Decompiled with JetBrains decompiler
// Type: TroubleshooterWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.Web;
using UnityEngine;

internal class TroubleshooterWindow : WebViewEditorWindow, IHasCustomMenu
{
  private WebView m_WebView;

  protected TroubleshooterWindow()
  {
    this.m_InitialOpenURL = "https://bugservices.unity3d.com/troubleshooter/";
  }

  public new void OnInitScripting()
  {
    base.OnInitScripting();
  }

  internal override WebView webView
  {
    get
    {
      return this.m_WebView;
    }
    set
    {
      this.m_WebView = value;
    }
  }

  [MenuItem("Help/Troubleshoot Issue...")]
  public static void RunTroubleshooter()
  {
    TroubleshooterWindow windowWithRect = EditorWindow.GetWindowWithRect<TroubleshooterWindow>(new Rect(100f, 100f, 990f, 680f), true, "Troubleshooter");
    if (!((Object) windowWithRect != (Object) null))
      return;
    windowWithRect.ShowUtility();
  }
}
