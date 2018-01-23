// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.ILeaderboard
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface ILeaderboard
  {
    void SetUserFilter(string[] userIDs);

    void LoadScores(Action<bool> callback);

    bool loading { get; }

    string id { get; set; }

    UserScope userScope { get; set; }

    Range range { get; set; }

    TimeScope timeScope { get; set; }

    IScore localUserScore { get; }

    uint maxRange { get; }

    IScore[] scores { get; }

    string title { get; }
  }
}
