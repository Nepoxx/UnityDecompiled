// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.MatchDirectConnectInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal class MatchDirectConnectInfo : ResponseBase
  {
    public NodeID nodeId { get; set; }

    public string publicAddress { get; set; }

    public string privateAddress { get; set; }

    public HostPriority hostPriority { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-nodeId:{1},publicAddress:{2},privateAddress:{3},hostPriority:{4}", (object) base.ToString(), (object) this.nodeId, (object) this.publicAddress, (object) this.privateAddress, (object) this.hostPriority);
    }

    public override void Parse(object obj)
    {
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.nodeId = (NodeID) this.ParseJSONUInt16("nodeId", obj, dictJsonObj);
      this.publicAddress = this.ParseJSONString("publicAddress", obj, dictJsonObj);
      this.privateAddress = this.ParseJSONString("privateAddress", obj, dictJsonObj);
      this.hostPriority = (HostPriority) this.ParseJSONInt32("hostPriority", obj, dictJsonObj);
    }
  }
}
