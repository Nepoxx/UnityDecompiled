// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.DropConnectionResponse
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal class DropConnectionResponse : Response
  {
    public NetworkID networkId { get; set; }

    public NodeID nodeId { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-networkId:{1}", (object) base.ToString(), (object) this.networkId.ToString("X"));
    }

    public override void Parse(object obj)
    {
      base.Parse(obj);
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.networkId = (NetworkID) this.ParseJSONUInt64("networkId", obj, dictJsonObj);
      this.nodeId = (NodeID) this.ParseJSONUInt16("nodeId", obj, dictJsonObj);
    }
  }
}
