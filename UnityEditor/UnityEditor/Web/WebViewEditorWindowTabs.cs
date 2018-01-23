// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.WebViewEditorWindowTabs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Web
{
  internal class WebViewEditorWindowTabs : WebViewEditorWindow, IHasCustomMenu, ISerializationCallbackReceiver
  {
    protected object m_GlobalObject = (object) null;
    internal WebView m_WebView;
    [SerializeField]
    private List<string> m_RegisteredViewURLs;
    [SerializeField]
    private List<WebView> m_RegisteredViewInstances;
    private Dictionary<string, WebView> m_RegisteredViews;

    protected WebViewEditorWindowTabs()
    {
      this.m_RegisteredViewURLs = new List<string>();
      this.m_RegisteredViewInstances = new List<WebView>();
      this.m_RegisteredViews = new Dictionary<string, WebView>();
      this.m_GlobalObject = (object) null;
    }

    public override void Init()
    {
      if (this.m_GlobalObject != null || string.IsNullOrEmpty(this.m_GlobalObjectTypeName))
        return;
      System.Type type = System.Type.GetType(this.m_GlobalObjectTypeName);
      if (type != null)
      {
        this.m_GlobalObject = (object) ScriptableObject.CreateInstance(type);
        JSProxyMgr.GetInstance().AddGlobalObject(this.m_GlobalObject.GetType().Name, this.m_GlobalObject);
      }
    }

    public override void OnDestroy()
    {
      if ((UnityEngine.Object) this.webView != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.webView);
      this.m_GlobalObject = (object) null;
      foreach (WebView webView in this.m_RegisteredViews.Values)
      {
        if ((UnityEngine.Object) webView != (UnityEngine.Object) null)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) webView);
      }
      this.m_RegisteredViews.Clear();
      this.m_RegisteredViewURLs.Clear();
      this.m_RegisteredViewInstances.Clear();
    }

    public void OnBeforeSerialize()
    {
      this.m_RegisteredViewURLs = new List<string>();
      this.m_RegisteredViewInstances = new List<WebView>();
      foreach (KeyValuePair<string, WebView> registeredView in this.m_RegisteredViews)
      {
        this.m_RegisteredViewURLs.Add(registeredView.Key);
        this.m_RegisteredViewInstances.Add(registeredView.Value);
      }
    }

    public void OnAfterDeserialize()
    {
      this.m_RegisteredViews = new Dictionary<string, WebView>();
      for (int index = 0; index != Math.Min(this.m_RegisteredViewURLs.Count, this.m_RegisteredViewInstances.Count); ++index)
        this.m_RegisteredViews.Add(this.m_RegisteredViewURLs[index], this.m_RegisteredViewInstances[index]);
    }

    private static string MakeUrlKey(string webViewUrl)
    {
      int length1 = webViewUrl.IndexOf("#");
      string str = length1 == -1 ? webViewUrl : webViewUrl.Substring(0, length1);
      int length2 = str.LastIndexOf("/");
      if (length2 == str.Length - 1)
        return str.Substring(0, length2);
      return str;
    }

    protected void UnregisterWebviewUrl(string webViewUrl)
    {
      this.m_RegisteredViews[WebViewEditorWindowTabs.MakeUrlKey(webViewUrl)] = (WebView) null;
    }

    private void RegisterWebviewUrl(string webViewUrl, WebView view)
    {
      this.m_RegisteredViews[WebViewEditorWindowTabs.MakeUrlKey(webViewUrl)] = view;
    }

    private bool FindWebView(string webViewUrl, out WebView webView)
    {
      webView = (WebView) null;
      return this.m_RegisteredViews.TryGetValue(WebViewEditorWindowTabs.MakeUrlKey(webViewUrl), out webView);
    }

    public WebView GetWebViewFromURL(string url)
    {
      return this.m_RegisteredViews[WebViewEditorWindowTabs.MakeUrlKey(url)];
    }

    public override void OnInitScripting()
    {
      this.SetScriptObject();
    }

    protected override void InitWebView(Rect webViewRect)
    {
      base.InitWebView(webViewRect);
      if (this.m_InitialOpenURL == null || !((UnityEngine.Object) this.webView != (UnityEngine.Object) null))
        return;
      this.RegisterWebviewUrl(this.m_InitialOpenURL, this.webView);
    }

    protected override void LoadPage()
    {
      if (!(bool) this.webView)
        return;
      WebView webView;
      if (!this.FindWebView(this.m_InitialOpenURL, out webView) || (UnityEngine.Object) webView == (UnityEngine.Object) null)
      {
        this.NotifyVisibility(false);
        this.webView.SetHostView((GUIView) null);
        this.webView = (WebView) null;
        this.InitWebView(GUIClip.Unclip(new Rect(0.0f, 0.0f, this.position.width, this.position.height)));
        this.RegisterWebviewUrl(this.m_InitialOpenURL, this.webView);
        this.NotifyVisibility(true);
      }
      else
      {
        if ((UnityEngine.Object) webView != (UnityEngine.Object) this.webView)
        {
          this.NotifyVisibility(false);
          webView.SetHostView((GUIView) this.m_Parent);
          this.webView.SetHostView((GUIView) null);
          this.webView = webView;
          this.NotifyVisibility(true);
          this.webView.Show();
        }
        this.LoadUri();
      }
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
  }
}
