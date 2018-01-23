// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnectConsentView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Web;
using UnityEngine;

namespace UnityEditor.Connect
{
  [Serializable]
  internal class UnityConnectConsentView : WebViewEditorWindow
  {
    private string code = "";
    private string error = "";

    public string Code
    {
      get
      {
        return this.code;
      }
    }

    public string Error
    {
      get
      {
        return this.error;
      }
    }

    internal override WebView webView { get; set; }

    public static UnityConnectConsentView ShowUnityConnectConsentView(string URL)
    {
      UnityConnectConsentView instance = ScriptableObject.CreateInstance<UnityConnectConsentView>();
      Rect rect = new Rect(100f, 100f, 800f, 605f);
      instance.titleContent = EditorGUIUtility.TextContent("Unity Application Consent Window");
      instance.minSize = new Vector2(rect.width, rect.height);
      instance.maxSize = new Vector2(rect.width, rect.height);
      instance.position = rect;
      instance.m_InitialOpenURL = URL;
      instance.ShowModal();
      instance.m_Parent.window.m_DontSaveToLayout = true;
      return instance;
    }

    public override void OnDestroy()
    {
      this.OnBecameInvisible();
    }

    public override void OnInitScripting()
    {
      this.SetScriptObject();
    }

    public override void OnLocationChanged(string url)
    {
      string query = new Uri(url).Query;
      char[] chArray = new char[1]{ '&' };
      foreach (string str in query.Split(chArray))
      {
        string[] strArray = str.Replace("?", string.Empty).Split('=');
        if (strArray[0] == "code")
        {
          this.code = strArray[1];
          break;
        }
        if (strArray[0] == "error")
        {
          this.error = strArray[1];
          break;
        }
      }
      if (!string.IsNullOrEmpty(this.code) || !string.IsNullOrEmpty(this.error))
        this.Close();
      else
        base.OnLocationChanged(url);
    }
  }
}
