// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.ActivePlatform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.SocialPlatforms
{
  internal static class ActivePlatform
  {
    private static ISocialPlatform _active;

    internal static ISocialPlatform Instance
    {
      get
      {
        if (ActivePlatform._active == null)
          ActivePlatform._active = ActivePlatform.SelectSocialPlatform();
        return ActivePlatform._active;
      }
      set
      {
        ActivePlatform._active = value;
      }
    }

    private static ISocialPlatform SelectSocialPlatform()
    {
      return (ISocialPlatform) new Local();
    }
  }
}
