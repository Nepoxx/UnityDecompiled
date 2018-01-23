// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Impl.LocalUser
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms.Impl
{
  public class LocalUser : UserProfile, ILocalUser, IUserProfile
  {
    private IUserProfile[] m_Friends;
    private bool m_Authenticated;
    private bool m_Underage;

    public LocalUser()
    {
      this.m_Friends = (IUserProfile[]) new UserProfile[0];
      this.m_Authenticated = false;
      this.m_Underage = false;
    }

    public void Authenticate(Action<bool> callback)
    {
      ActivePlatform.Instance.Authenticate((ILocalUser) this, callback);
    }

    public void Authenticate(Action<bool, string> callback)
    {
      ActivePlatform.Instance.Authenticate((ILocalUser) this, callback);
    }

    public void LoadFriends(Action<bool> callback)
    {
      ActivePlatform.Instance.LoadFriends((ILocalUser) this, callback);
    }

    public void SetFriends(IUserProfile[] friends)
    {
      this.m_Friends = friends;
    }

    public void SetAuthenticated(bool value)
    {
      this.m_Authenticated = value;
    }

    public void SetUnderage(bool value)
    {
      this.m_Underage = value;
    }

    public IUserProfile[] friends
    {
      get
      {
        return this.m_Friends;
      }
    }

    public bool authenticated
    {
      get
      {
        return this.m_Authenticated;
      }
    }

    public bool underage
    {
      get
      {
        return this.m_Underage;
      }
    }
  }
}
