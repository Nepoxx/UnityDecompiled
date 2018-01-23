// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.IUserProfile
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.SocialPlatforms
{
  public interface IUserProfile
  {
    string userName { get; }

    string id { get; }

    bool isFriend { get; }

    UserState state { get; }

    Texture2D image { get; }
  }
}
