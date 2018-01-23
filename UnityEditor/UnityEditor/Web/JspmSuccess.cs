// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmSuccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmSuccess : JspmResult
  {
    public object result;
    public string type;

    public JspmSuccess(long messageID, object result, string type)
      : base(messageID, 0)
    {
      this.result = result;
      this.type = type;
    }
  }
}
