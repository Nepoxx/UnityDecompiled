// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmError
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmError : JspmResult
  {
    public string errorClass;
    public string message;

    public JspmError(long messageID, int status, string errorClass, string message)
      : base(messageID, status)
    {
      this.errorClass = errorClass;
      this.message = message;
    }
  }
}
