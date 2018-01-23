// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.ILocalUser
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface ILocalUser : IUserProfile
  {
    void Authenticate(Action<bool> callback);

    void Authenticate(Action<bool, string> callback);

    void LoadFriends(Action<bool> callback);

    IUserProfile[] friends { get; }

    bool authenticated { get; }

    bool underage { get; }
  }
}
