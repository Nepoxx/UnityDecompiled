// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.Request
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  internal abstract class Request
  {
    public static readonly int currentVersion = 3;

    public int version { get; set; }

    public SourceID sourceId { get; set; }

    public string projectId { get; set; }

    public AppID appId { get; set; }

    public string accessTokenString { get; set; }

    public int domain { get; set; }

    public virtual bool IsValid()
    {
      return this.sourceId != SourceID.Invalid;
    }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-SourceID:0x{1},projectId:{2},accessTokenString.IsEmpty:{3},domain:{4}", (object) base.ToString(), (object) this.sourceId.ToString("X"), (object) this.projectId, (object) string.IsNullOrEmpty(this.accessTokenString), (object) this.domain);
    }
  }
}
