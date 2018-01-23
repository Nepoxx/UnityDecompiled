// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.JoinMatchRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal class JoinMatchRequest : Request
  {
    public NetworkID networkId { get; set; }

    public string publicAddress { get; set; }

    public string privateAddress { get; set; }

    public int eloScore { get; set; }

    public string password { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-networkId:0x{1},publicAddress:{2},privateAddress:{3},eloScore:{4},HasPassword:{5}", (object) base.ToString(), (object) this.networkId.ToString("X"), (object) this.publicAddress, (object) this.privateAddress, (object) this.eloScore, !string.IsNullOrEmpty(this.password) ? (object) "YES" : (object) "NO");
    }

    public override bool IsValid()
    {
      return base.IsValid() && this.networkId != NetworkID.Invalid;
    }
  }
}
