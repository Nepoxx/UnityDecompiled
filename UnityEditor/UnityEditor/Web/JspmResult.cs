// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmResult
  {
    public double version;
    public long messageID;
    public int status;

    public JspmResult()
    {
      this.version = 1.0;
      this.messageID = -1L;
      this.status = 0;
    }

    public JspmResult(long messageID, int status)
    {
      this.version = 1.0;
      this.messageID = messageID;
      this.status = status;
    }
  }
}
