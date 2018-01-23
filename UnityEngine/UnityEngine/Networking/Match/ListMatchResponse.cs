// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.ListMatchResponse
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  internal class ListMatchResponse : BasicResponse
  {
    public ListMatchResponse()
    {
      this.matches = new List<MatchDesc>();
    }

    public ListMatchResponse(List<MatchDesc> otherMatches)
    {
      this.matches = otherMatches;
    }

    public List<MatchDesc> matches { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-matches.Count:{1}", (object) base.ToString(), (object) (this.matches != null ? this.matches.Count : 0));
    }

    public override void Parse(object obj)
    {
      base.Parse(obj);
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.matches = this.ParseJSONList<MatchDesc>("matches", obj, dictJsonObj);
    }
  }
}
