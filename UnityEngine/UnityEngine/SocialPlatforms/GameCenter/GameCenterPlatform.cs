// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms.GameCenter
{
  [RequiredByNativeCode]
  public sealed class GameCenterPlatform : ISocialPlatform
  {
    private static AchievementDescription[] s_adCache = new AchievementDescription[0];
    private static UserProfile[] s_friends = new UserProfile[0];
    private static UserProfile[] s_users = new UserProfile[0];
    private static List<GcLeaderboard> m_GcBoards = new List<GcLeaderboard>();
    private static Action<bool, string> s_AuthenticateCallback;
    private static Action<bool> s_ResetAchievements;
    private static LocalUser m_LocalUser;

    [RequiredByNativeCode]
    private static void ClearAchievementDescriptions(int size)
    {
      if (GameCenterPlatform.s_adCache != null && GameCenterPlatform.s_adCache.Length == size)
        return;
      GameCenterPlatform.s_adCache = new AchievementDescription[size];
    }

    [RequiredByNativeCode]
    private static void SetAchievementDescription(GcAchievementDescriptionData data, int number)
    {
      GameCenterPlatform.s_adCache[number] = data.ToAchievementDescription();
    }

    [RequiredByNativeCode]
    private static void SetAchievementDescriptionImage(Texture2D texture, int number)
    {
      if (GameCenterPlatform.s_adCache.Length <= number || number < 0)
        Debug.Log((object) "Achievement description number out of bounds when setting image");
      else
        GameCenterPlatform.s_adCache[number].SetImage(texture);
    }

    [RequiredByNativeCode]
    private static void TriggerAchievementDescriptionCallback(Action<IAchievementDescription[]> callback)
    {
      if (callback == null || GameCenterPlatform.s_adCache == null)
        return;
      if (GameCenterPlatform.s_adCache.Length == 0)
        Debug.Log((object) "No achievement descriptions returned");
      callback((IAchievementDescription[]) GameCenterPlatform.s_adCache);
    }

    [RequiredByNativeCode]
    private static void AuthenticateCallbackWrapper(int result, string error)
    {
      GameCenterPlatform.PopulateLocalUser();
      if (GameCenterPlatform.s_AuthenticateCallback == null)
        return;
      GameCenterPlatform.s_AuthenticateCallback(result == 1, error);
      GameCenterPlatform.s_AuthenticateCallback = (Action<bool, string>) null;
    }

    [RequiredByNativeCode]
    private static void ClearFriends(int size)
    {
      GameCenterPlatform.SafeClearArray(ref GameCenterPlatform.s_friends, size);
    }

    [RequiredByNativeCode]
    private static void SetFriends(GcUserProfileData data, int number)
    {
      data.AddToArray(ref GameCenterPlatform.s_friends, number);
    }

    [RequiredByNativeCode]
    private static void SetFriendImage(Texture2D texture, int number)
    {
      GameCenterPlatform.SafeSetUserImage(ref GameCenterPlatform.s_friends, texture, number);
    }

    [RequiredByNativeCode]
    private static void TriggerFriendsCallbackWrapper(Action<bool> callback, int result)
    {
      if (GameCenterPlatform.s_friends != null)
        GameCenterPlatform.m_LocalUser.SetFriends((IUserProfile[]) GameCenterPlatform.s_friends);
      if (callback == null)
        return;
      callback(result == 1);
    }

    [RequiredByNativeCode]
    private static void AchievementCallbackWrapper(Action<IAchievement[]> callback, GcAchievementData[] result)
    {
      if (callback == null)
        return;
      if (result.Length == 0)
        Debug.Log((object) "No achievements returned");
      Achievement[] achievementArray = new Achievement[result.Length];
      for (int index = 0; index < result.Length; ++index)
        achievementArray[index] = result[index].ToAchievement();
      callback((IAchievement[]) achievementArray);
    }

    [RequiredByNativeCode]
    private static void ProgressCallbackWrapper(Action<bool> callback, bool success)
    {
      if (callback == null)
        return;
      callback(success);
    }

    [RequiredByNativeCode]
    private static void ScoreCallbackWrapper(Action<bool> callback, bool success)
    {
      if (callback == null)
        return;
      callback(success);
    }

    [RequiredByNativeCode]
    private static void ScoreLoaderCallbackWrapper(Action<IScore[]> callback, GcScoreData[] result)
    {
      if (callback == null)
        return;
      Score[] scoreArray = new Score[result.Length];
      for (int index = 0; index < result.Length; ++index)
        scoreArray[index] = result[index].ToScore();
      callback((IScore[]) scoreArray);
    }

    void ISocialPlatform.LoadFriends(ILocalUser user, Action<bool> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback(false);
      }
      else
        GameCenterPlatform.Internal_LoadFriends((object) callback);
    }

    void ISocialPlatform.Authenticate(ILocalUser user, Action<bool> callback)
    {
      ((ISocialPlatform) this).Authenticate(user, (Action<bool, string>) ((success, error) => callback(success)));
    }

    void ISocialPlatform.Authenticate(ILocalUser user, Action<bool, string> callback)
    {
      GameCenterPlatform.s_AuthenticateCallback = callback;
      GameCenterPlatform.Internal_Authenticate();
    }

    public ILocalUser localUser
    {
      get
      {
        if (GameCenterPlatform.m_LocalUser == null)
          GameCenterPlatform.m_LocalUser = new LocalUser();
        if (GameCenterPlatform.Internal_Authenticated() && GameCenterPlatform.m_LocalUser.id == "0")
          GameCenterPlatform.PopulateLocalUser();
        return (ILocalUser) GameCenterPlatform.m_LocalUser;
      }
    }

    [RequiredByNativeCode]
    private static void PopulateLocalUser()
    {
      GameCenterPlatform.m_LocalUser.SetAuthenticated(GameCenterPlatform.Internal_Authenticated());
      GameCenterPlatform.m_LocalUser.SetUserName(GameCenterPlatform.Internal_UserName());
      GameCenterPlatform.m_LocalUser.SetUserID(GameCenterPlatform.Internal_UserID());
      GameCenterPlatform.m_LocalUser.SetUnderage(GameCenterPlatform.Internal_Underage());
      GameCenterPlatform.m_LocalUser.SetImage(GameCenterPlatform.Internal_UserImage());
    }

    public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback((IAchievementDescription[]) new AchievementDescription[0]);
      }
      else
        GameCenterPlatform.Internal_LoadAchievementDescriptions((object) callback);
    }

    public void ReportProgress(string id, double progress, Action<bool> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback(false);
      }
      else
        GameCenterPlatform.Internal_ReportProgress(id, progress, (object) callback);
    }

    public void LoadAchievements(Action<IAchievement[]> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback((IAchievement[]) new Achievement[0]);
      }
      else
        GameCenterPlatform.Internal_LoadAchievements((object) callback);
    }

    public void ReportScore(long score, string board, Action<bool> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback(false);
      }
      else
        GameCenterPlatform.Internal_ReportScore(score, board, (object) callback);
    }

    public void LoadScores(string category, Action<IScore[]> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback((IScore[]) new Score[0]);
      }
      else
        GameCenterPlatform.Internal_LoadScores(category, (object) callback);
    }

    public void LoadScores(ILeaderboard board, Action<bool> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback(false);
      }
      else
      {
        Leaderboard board1 = (Leaderboard) board;
        GcLeaderboard gcLeaderboard = new GcLeaderboard(board1);
        GameCenterPlatform.m_GcBoards.Add(gcLeaderboard);
        string[] userIDs = board1.GetUserFilter();
        if (userIDs.Length == 0)
          userIDs = (string[]) null;
        gcLeaderboard.Internal_LoadScores(board.id, board.range.from, board.range.count, userIDs, (int) board.userScope, (int) board.timeScope, (object) callback);
      }
    }

    [RequiredByNativeCode]
    private static void LeaderboardCallbackWrapper(Action<bool> callback, bool success)
    {
      if (callback == null)
        return;
      callback(success);
    }

    public bool GetLoading(ILeaderboard board)
    {
      if (!this.VerifyAuthentication())
        return false;
      foreach (GcLeaderboard gcBoard in GameCenterPlatform.m_GcBoards)
      {
        if (gcBoard.Contains((Leaderboard) board))
          return gcBoard.Loading();
      }
      return false;
    }

    private bool VerifyAuthentication()
    {
      if (this.localUser.authenticated)
        return true;
      Debug.Log((object) "Must authenticate first");
      return false;
    }

    public void ShowAchievementsUI()
    {
      if (!this.VerifyAuthentication())
        return;
      GameCenterPlatform.Internal_ShowAchievementsUI();
    }

    public void ShowLeaderboardUI()
    {
      if (!this.VerifyAuthentication())
        return;
      GameCenterPlatform.Internal_ShowLeaderboardUI();
    }

    [RequiredByNativeCode]
    private static void ClearUsers(int size)
    {
      GameCenterPlatform.SafeClearArray(ref GameCenterPlatform.s_users, size);
    }

    [RequiredByNativeCode]
    private static void SetUser(GcUserProfileData data, int number)
    {
      data.AddToArray(ref GameCenterPlatform.s_users, number);
    }

    [RequiredByNativeCode]
    private static void SetUserImage(Texture2D texture, int number)
    {
      GameCenterPlatform.SafeSetUserImage(ref GameCenterPlatform.s_users, texture, number);
    }

    [RequiredByNativeCode]
    private static void TriggerUsersCallbackWrapper(Action<IUserProfile[]> callback)
    {
      if (callback == null)
        return;
      callback((IUserProfile[]) GameCenterPlatform.s_users);
    }

    public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
    {
      if (!this.VerifyAuthentication())
      {
        if (callback == null)
          return;
        callback((IUserProfile[]) new UserProfile[0]);
      }
      else
        GameCenterPlatform.Internal_LoadUsers(userIds, (object) callback);
    }

    [RequiredByNativeCode]
    private static void SafeSetUserImage(ref UserProfile[] array, Texture2D texture, int number)
    {
      if (array.Length <= number || number < 0)
      {
        Debug.Log((object) "Invalid texture when setting user image");
        texture = new Texture2D(76, 76);
      }
      if (array.Length > number && number >= 0)
        array[number].SetImage(texture);
      else
        Debug.Log((object) "User number out of bounds when setting image");
    }

    private static void SafeClearArray(ref UserProfile[] array, int size)
    {
      if (array != null && array.Length == size)
        return;
      array = new UserProfile[size];
    }

    public ILeaderboard CreateLeaderboard()
    {
      return (ILeaderboard) new Leaderboard();
    }

    public IAchievement CreateAchievement()
    {
      return (IAchievement) new Achievement();
    }

    [RequiredByNativeCode]
    private static void TriggerResetAchievementCallback(bool result)
    {
      if (GameCenterPlatform.s_ResetAchievements == null)
        return;
      GameCenterPlatform.s_ResetAchievements(result);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_Authenticate();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Internal_Authenticated();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string Internal_UserName();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string Internal_UserID();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool Internal_Underage();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D Internal_UserImage();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_LoadFriends(object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_LoadAchievementDescriptions(object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_LoadAchievements(object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ReportProgress(string id, double progress, object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ReportScore(long score, string category, object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_LoadScores(string category, object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ShowAchievementsUI();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ShowLeaderboardUI();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_LoadUsers(string[] userIds, object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ResetAllAchievements();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ShowDefaultAchievementBanner(bool value);

    public static void ResetAllAchievements(Action<bool> callback)
    {
      GameCenterPlatform.s_ResetAchievements = callback;
      GameCenterPlatform.Internal_ResetAllAchievements();
      Debug.Log((object) "ResetAllAchievements - no effect in editor");
      if (callback == null)
        return;
      callback(true);
    }

    public static void ShowDefaultAchievementCompletionBanner(bool value)
    {
      GameCenterPlatform.Internal_ShowDefaultAchievementBanner(value);
      Debug.Log((object) "ShowDefaultAchievementCompletionBanner - no effect in editor");
    }

    public static void ShowLeaderboardUI(string leaderboardID, TimeScope timeScope)
    {
      GameCenterPlatform.Internal_ShowSpecificLeaderboardUI(leaderboardID, (int) timeScope);
      Debug.Log((object) "ShowLeaderboardUI - no effect in editor");
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_ShowSpecificLeaderboardUI(string leaderboardID, int timeScope);
  }
}
