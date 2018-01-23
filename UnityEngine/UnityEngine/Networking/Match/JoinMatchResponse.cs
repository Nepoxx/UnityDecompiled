// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.JoinMatchResponse
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal class JoinMatchResponse : BasicResponse
  {
    public string address { get; set; }

    public int port { get; set; }

    public int domain { get; set; }

    public NetworkID networkId { get; set; }

    public string accessTokenString { get; set; }

    public NodeID nodeId { get; set; }

    public bool usingRelay { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-address:{1},port:{2},networkId:0x{3},accessTokenString.IsEmpty:{4},nodeId:0x{5},usingRelay:{6}", (object) base.ToString(), (object) this.address, (object) this.port, (object) this.networkId.ToString("X"), (object) string.IsNullOrEmpty(this.accessTokenString), (object) this.nodeId.ToString("X"), (object) this.usingRelay);
    }

    public override void Parse(object obj)
    {
      base.Parse(obj);
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.address = this.ParseJSONString("address", obj, dictJsonObj);
      this.port = this.ParseJSONInt32("port", obj, dictJsonObj);
      this.networkId = (NetworkID) this.ParseJSONUInt64("networkId", obj, dictJsonObj);
      this.accessTokenString = this.ParseJSONString("accessTokenString", obj, dictJsonObj);
      this.nodeId = (NodeID) this.ParseJSONUInt16("nodeId", obj, dictJsonObj);
      this.usingRelay = this.ParseJSONBool("usingRelay", obj, dictJsonObj);
    }
  }
}
