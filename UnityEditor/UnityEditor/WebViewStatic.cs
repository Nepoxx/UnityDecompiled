// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebViewStatic
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal class WebViewStatic : ScriptableSingleton<WebViewStatic>
  {
    [SerializeField]
    private WebView m_WebView;

    public static WebView GetWebView()
    {
      return ScriptableSingleton<WebViewStatic>.instance.m_WebView;
    }

    public static void SetWebView(WebView webView)
    {
      ScriptableSingleton<WebViewStatic>.instance.m_WebView = webView;
    }
  }
}
