// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmStubInfoSuccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmStubInfoSuccess : JspmSuccess
  {
    public string reference;

    public JspmStubInfoSuccess(long messageID, string reference, JspmPropertyInfo[] properties, JspmMethodInfo[] methods, string[] events)
      : base(messageID, (object) new JspmStubInfo(properties, methods, events), "GETSTUBINFO")
    {
      this.reference = reference;
    }
  }
}
