// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.MatchDesc
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal class MatchDesc : ResponseBase
  {
    public NetworkID networkId { get; set; }

    public string name { get; set; }

    public int averageEloScore { get; set; }

    public int maxSize { get; set; }

    public int currentSize { get; set; }

    public bool isPrivate { get; set; }

    public Dictionary<string, long> matchAttributes { get; set; }

    public NodeID hostNodeId { get; set; }

    public List<MatchDirectConnectInfo> directConnectInfos { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-networkId:0x{1},name:{2},averageEloScore:{3},maxSize:{4},currentSize:{5},isPrivate:{6},matchAttributes.Count:{7},hostNodeId:{8},directConnectInfos.Count:{9}", (object) base.ToString(), (object) this.networkId.ToString("X"), (object) this.name, (object) this.averageEloScore, (object) this.maxSize, (object) this.currentSize, (object) this.isPrivate, (object) (this.matchAttributes != null ? this.matchAttributes.Count : 0), (object) this.hostNodeId, (object) this.directConnectInfos.Count);
    }

    public override void Parse(object obj)
    {
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.networkId = (NetworkID) this.ParseJSONUInt64("networkId", obj, dictJsonObj);
      this.name = this.ParseJSONString("name", obj, dictJsonObj);
      this.averageEloScore = this.ParseJSONInt32("averageEloScore", obj, dictJsonObj);
      this.maxSize = this.ParseJSONInt32("maxSize", obj, dictJsonObj);
      this.currentSize = this.ParseJSONInt32("currentSize", obj, dictJsonObj);
      this.isPrivate = this.ParseJSONBool("isPrivate", obj, dictJsonObj);
      this.hostNodeId = (NodeID) this.ParseJSONUInt16("hostNodeId", obj, dictJsonObj);
      this.directConnectInfos = this.ParseJSONList<MatchDirectConnectInfo>("directConnectInfos", obj, dictJsonObj);
    }
  }
}
