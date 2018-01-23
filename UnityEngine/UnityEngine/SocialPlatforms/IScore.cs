// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.IScore
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface IScore
  {
    void ReportScore(Action<bool> callback);

    string leaderboardID { get; set; }

    long value { get; set; }

    DateTime date { get; }

    string formattedValue { get; }

    string userID { get; }

    int rank { get; }
  }
}
