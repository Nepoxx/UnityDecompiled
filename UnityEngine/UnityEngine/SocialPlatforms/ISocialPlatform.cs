// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.ISocialPlatform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface ISocialPlatform
  {
    ILocalUser localUser { get; }

    void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback);

    void ReportProgress(string achievementID, double progress, Action<bool> callback);

    void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback);

    void LoadAchievements(Action<IAchievement[]> callback);

    IAchievement CreateAchievement();

    void ReportScore(long score, string board, Action<bool> callback);

    void LoadScores(string leaderboardID, Action<IScore[]> callback);

    ILeaderboard CreateLeaderboard();

    void ShowAchievementsUI();

    void ShowLeaderboardUI();

    void Authenticate(ILocalUser user, Action<bool> callback);

    void Authenticate(ILocalUser user, Action<bool, string> callback);

    void LoadFriends(ILocalUser user, Action<bool> callback);

    void LoadScores(ILeaderboard board, Action<bool> callback);

    bool GetLoading(ILeaderboard board);
  }
}
