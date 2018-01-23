// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.Response
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  internal abstract class Response : ResponseBase, IResponse
  {
    public bool success { get; private set; }

    public string extendedInfo { get; private set; }

    public void SetSuccess()
    {
      this.success = true;
      this.extendedInfo = "";
    }

    public void SetFailure(string info)
    {
      this.success = false;
      this.extendedInfo += info;
    }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-success:{1}-extendedInfo:{2}", (object) base.ToString(), (object) this.success, (object) this.extendedInfo);
    }

    public override void Parse(object obj)
    {
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        return;
      this.success = this.ParseJSONBool("success", obj, dictJsonObj);
      this.extendedInfo = this.ParseJSONString("extendedInfo", obj, dictJsonObj);
      if (!this.success)
        throw new FormatException("FAILURE Returned from server: " + this.extendedInfo);
    }
  }
}
