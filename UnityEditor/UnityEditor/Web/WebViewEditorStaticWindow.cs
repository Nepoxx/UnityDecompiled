// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.WebViewEditorStaticWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Web
{
  internal abstract class WebViewEditorStaticWindow : WebViewEditorWindow, IHasCustomMenu
  {
    protected object m_GlobalObject = (object) null;

    protected WebViewEditorStaticWindow()
    {
      this.m_GlobalObject = (object) null;
    }

    public override void OnDestroy()
    {
      this.OnBecameInvisible();
      this.m_GlobalObject = (object) null;
    }

    public override void OnInitScripting()
    {
      this.SetScriptObject();
    }
  }
}
