// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.WebScriptObject
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Web
{
  internal class WebScriptObject : ScriptableObject
  {
    private WebView m_WebView;

    private WebScriptObject()
    {
      this.m_WebView = (WebView) null;
    }

    public WebView webView
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

    public bool ProcessMessage(string jsonRequest, WebViewV8CallbackCSharp callback)
    {
      if ((Object) this.m_WebView != (Object) null)
        return JSProxyMgr.GetInstance().DoMessage(jsonRequest, (JSProxyMgr.ExecCallback) (result => callback.Callback(JSProxyMgr.GetInstance().Stringify(result))), this.m_WebView);
      return false;
    }

    public bool processMessage(string jsonRequest, WebViewV8CallbackCSharp callback)
    {
      return this.ProcessMessage(jsonRequest, callback);
    }
  }
}
