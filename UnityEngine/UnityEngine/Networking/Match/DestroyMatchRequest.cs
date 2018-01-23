// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.DestroyMatchRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal class DestroyMatchRequest : Request
  {
    public NetworkID networkId { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-networkId:0x{1}", (object) base.ToString(), (object) this.networkId.ToString("X"));
    }

    public override bool IsValid()
    {
      return base.IsValid() && this.networkId != NetworkID.Invalid;
    }
  }
}
