// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.GameCenter.GcAchievementDescriptionData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms.GameCenter
{
  [RequiredByNativeCode]
  internal struct GcAchievementDescriptionData
  {
    public string m_Identifier;
    public string m_Title;
    public Texture2D m_Image;
    public string m_AchievedDescription;
    public string m_UnachievedDescription;
    public int m_Hidden;
    public int m_Points;

    public AchievementDescription ToAchievementDescription()
    {
      return new AchievementDescription(this.m_Identifier, this.m_Title, this.m_Image, this.m_AchievedDescription, this.m_UnachievedDescription, this.m_Hidden != 0, this.m_Points);
    }
  }
}
