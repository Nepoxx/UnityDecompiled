// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.CreateMatchRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  internal class CreateMatchRequest : Request
  {
    public string name { get; set; }

    public uint size { get; set; }

    public string publicAddress { get; set; }

    public string privateAddress { get; set; }

    public int eloScore { get; set; }

    public bool advertise { get; set; }

    public string password { get; set; }

    public Dictionary<string, long> matchAttributes { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-name:{1},size:{2},publicAddress:{3},privateAddress:{4},eloScore:{5},advertise:{6},HasPassword:{7},matchAttributes.Count:{8}", (object) base.ToString(), (object) this.name, (object) this.size, (object) this.publicAddress, (object) this.privateAddress, (object) this.eloScore, (object) this.advertise, !string.IsNullOrEmpty(this.password) ? (object) "YES" : (object) "NO", (object) (this.matchAttributes != null ? this.matchAttributes.Count : 0));
    }

    public override bool IsValid()
    {
      return base.IsValid() && this.size >= 2U && (this.matchAttributes == null || this.matchAttributes.Count <= 10);
    }
  }
}
