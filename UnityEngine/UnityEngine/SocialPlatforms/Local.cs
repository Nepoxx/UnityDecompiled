// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Local
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms
{
  public class Local : ISocialPlatform
  {
    private static LocalUser m_LocalUser = (LocalUser) null;
    private List<UserProfile> m_Friends = new List<UserProfile>();
    private List<UserProfile> m_Users = new List<UserProfile>();
    private List<AchievementDescription> m_AchievementDescriptions = new List<AchievementDescription>();
    private List<Achievement> m_Achievements = new List<Achievement>();
    private List<Leaderboard> m_Leaderboards = new List<Leaderboard>();
    private Texture2D m_DefaultTexture;

    public ILocalUser localUser
    {
      get
      {
        if (Local.m_LocalUser == null)
          Local.m_LocalUser = new LocalUser();
        return (ILocalUser) Local.m_LocalUser;
      }
    }

    void ISocialPlatform.Authenticate(ILocalUser user, Action<bool> callback)
    {
      LocalUser localUser = (LocalUser) user;
      this.m_DefaultTexture = this.CreateDummyTexture(32, 32);
      this.PopulateStaticData();
      localUser.SetAuthenticated(true);
      localUser.SetUnderage(false);
      localUser.SetUserID("1000");
      localUser.SetUserName("Lerpz");
      localUser.SetImage(this.m_DefaultTexture);
      if (callback == null)
        return;
      callback(true);
    }

    void ISocialPlatform.Authenticate(ILocalUser user, Action<bool, string> callback)
    {
      ((ISocialPlatform) this).Authenticate(user, (Action<bool>) (success => callback(success, (string) null)));
    }

    void ISocialPlatform.LoadFriends(ILocalUser user, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      ((LocalUser) user).SetFriends((IUserProfile[]) this.m_Friends.ToArray());
      if (callback == null)
        return;
      callback(true);
    }

    public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
    {
      List<UserProfile> userProfileList = new List<UserProfile>();
      if (!this.VerifyUser())
        return;
      foreach (string userId in userIDs)
      {
        foreach (UserProfile user in this.m_Users)
        {
          if (user.id == userId)
            userProfileList.Add(user);
        }
        foreach (UserProfile friend in this.m_Friends)
        {
          if (friend.id == userId)
            userProfileList.Add(friend);
        }
      }
      callback((IUserProfile[]) userProfileList.ToArray());
    }

    public void ReportProgress(string id, double progress, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      foreach (Achievement achievement in this.m_Achievements)
      {
        if (achievement.id == id && achievement.percentCompleted <= progress)
        {
          if (progress >= 100.0)
            achievement.SetCompleted(true);
          achievement.SetHidden(false);
          achievement.SetLastReportedDate(DateTime.Now);
          achievement.percentCompleted = progress;
          if (callback == null)
            return;
          callback(true);
          return;
        }
      }
      foreach (AchievementDescription achievementDescription in this.m_AchievementDescriptions)
      {
        if (achievementDescription.id == id)
        {
          bool completed = progress >= 100.0;
          this.m_Achievements.Add(new Achievement(id, progress, completed, false, DateTime.Now));
          if (callback == null)
            return;
          callback(true);
          return;
        }
      }
      Debug.LogError((object) "Achievement ID not found");
      if (callback == null)
        return;
      callback(false);
    }

    public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
    {
      if (!this.VerifyUser() || callback == null)
        return;
      callback((IAchievementDescription[]) this.m_AchievementDescriptions.ToArray());
    }

    public void LoadAchievements(Action<IAchievement[]> callback)
    {
      if (!this.VerifyUser() || callback == null)
        return;
      callback((IAchievement[]) this.m_Achievements.ToArray());
    }

    public void ReportScore(long score, string board, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      foreach (Leaderboard leaderboard in this.m_Leaderboards)
      {
        if (leaderboard.id == board)
        {
          leaderboard.SetScores((IScore[]) new List<Score>((IEnumerable<Score>) leaderboard.scores)
          {
            new Score(board, score, this.localUser.id, DateTime.Now, score.ToString() + " points", 0)
          }.ToArray());
          if (callback == null)
            return;
          callback(true);
          return;
        }
      }
      Debug.LogError((object) "Leaderboard not found");
      if (callback == null)
        return;
      callback(false);
    }

    public void LoadScores(string leaderboardID, Action<IScore[]> callback)
    {
      if (!this.VerifyUser())
        return;
      foreach (Leaderboard leaderboard in this.m_Leaderboards)
      {
        if (leaderboard.id == leaderboardID)
        {
          this.SortScores(leaderboard);
          if (callback == null)
            return;
          callback(leaderboard.scores);
          return;
        }
      }
      Debug.LogError((object) "Leaderboard not found");
      if (callback == null)
        return;
      callback((IScore[]) new Score[0]);
    }

    void ISocialPlatform.LoadScores(ILeaderboard board, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      Leaderboard board1 = (Leaderboard) board;
      foreach (Leaderboard leaderboard in this.m_Leaderboards)
      {
        if (leaderboard.id == board1.id)
        {
          board1.SetTitle(leaderboard.title);
          board1.SetScores(leaderboard.scores);
          board1.SetMaxRange((uint) leaderboard.scores.Length);
        }
      }
      this.SortScores(board1);
      this.SetLocalPlayerScore(board1);
      if (callback == null)
        return;
      callback(true);
    }

    bool ISocialPlatform.GetLoading(ILeaderboard board)
    {
      if (!this.VerifyUser())
        return false;
      return ((Leaderboard) board).loading;
    }

    private void SortScores(Leaderboard board)
    {
      List<Score> scoreList = new List<Score>((IEnumerable<Score>) board.scores);
      scoreList.Sort((Comparison<Score>) ((s1, s2) => s2.value.CompareTo(s1.value)));
      for (int index = 0; index < scoreList.Count; ++index)
        scoreList[index].SetRank(index + 1);
    }

    private void SetLocalPlayerScore(Leaderboard board)
    {
      foreach (Score score in board.scores)
      {
        if (score.userID == this.localUser.id)
        {
          board.SetLocalUserScore((IScore) score);
          break;
        }
      }
    }

    public void ShowAchievementsUI()
    {
      Debug.Log((object) "ShowAchievementsUI not implemented");
    }

    public void ShowLeaderboardUI()
    {
      Debug.Log((object) "ShowLeaderboardUI not implemented");
    }

    public ILeaderboard CreateLeaderboard()
    {
      return (ILeaderboard) new Leaderboard();
    }

    public IAchievement CreateAchievement()
    {
      return (IAchievement) new Achievement();
    }

    private bool VerifyUser()
    {
      if (this.localUser.authenticated)
        return true;
      Debug.LogError((object) "Must authenticate first");
      return false;
    }

    private void PopulateStaticData()
    {
      this.m_Friends.Add(new UserProfile("Fred", "1001", true, UserState.Online, this.m_DefaultTexture));
      this.m_Friends.Add(new UserProfile("Julia", "1002", true, UserState.Online, this.m_DefaultTexture));
      this.m_Friends.Add(new UserProfile("Jeff", "1003", true, UserState.Online, this.m_DefaultTexture));
      this.m_Users.Add(new UserProfile("Sam", "1004", false, UserState.Offline, this.m_DefaultTexture));
      this.m_Users.Add(new UserProfile("Max", "1005", false, UserState.Offline, this.m_DefaultTexture));
      this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement01", "First achievement", this.m_DefaultTexture, "Get first achievement", "Received first achievement", false, 10));
      this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement02", "Second achievement", this.m_DefaultTexture, "Get second achievement", "Received second achievement", false, 20));
      this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement03", "Third achievement", this.m_DefaultTexture, "Get third achievement", "Received third achievement", false, 15));
      Leaderboard leaderboard = new Leaderboard();
      leaderboard.SetTitle("High Scores");
      leaderboard.id = "Leaderboard01";
      leaderboard.SetScores((IScore[]) new List<Score>()
      {
        new Score("Leaderboard01", 300L, "1001", DateTime.Now.AddDays(-1.0), "300 points", 1),
        new Score("Leaderboard01", (long) byte.MaxValue, "1002", DateTime.Now.AddDays(-1.0), "255 points", 2),
        new Score("Leaderboard01", 55L, "1003", DateTime.Now.AddDays(-1.0), "55 points", 3),
        new Score("Leaderboard01", 10L, "1004", DateTime.Now.AddDays(-1.0), "10 points", 4)
      }.ToArray());
      this.m_Leaderboards.Add(leaderboard);
    }

    private Texture2D CreateDummyTexture(int width, int height)
    {
      Texture2D texture2D = new Texture2D(width, height);
      for (int y = 0; y < height; ++y)
      {
        for (int x = 0; x < width; ++x)
        {
          Color color = (x & y) <= 0 ? Color.gray : Color.white;
          texture2D.SetPixel(x, y, color);
        }
      }
      texture2D.Apply();
      return texture2D;
    }
  }
}
