// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmStubInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmStubInfo
  {
    public JspmPropertyInfo[] properties = (JspmPropertyInfo[]) null;
    public JspmMethodInfo[] methods = (JspmMethodInfo[]) null;
    public string[] events = (string[]) null;

    public JspmStubInfo(JspmPropertyInfo[] properties, JspmMethodInfo[] methods, string[] events)
    {
      this.methods = methods;
      this.properties = properties;
      this.events = events;
    }
  }
}
