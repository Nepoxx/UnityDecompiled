// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Impl.AchievementDescription
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.SocialPlatforms.Impl
{
  public class AchievementDescription : IAchievementDescription
  {
    private string m_Title;
    private Texture2D m_Image;
    private string m_AchievedDescription;
    private string m_UnachievedDescription;
    private bool m_Hidden;
    private int m_Points;

    public AchievementDescription(string id, string title, Texture2D image, string achievedDescription, string unachievedDescription, bool hidden, int points)
    {
      this.id = id;
      this.m_Title = title;
      this.m_Image = image;
      this.m_AchievedDescription = achievedDescription;
      this.m_UnachievedDescription = unachievedDescription;
      this.m_Hidden = hidden;
      this.m_Points = points;
    }

    public override string ToString()
    {
      return this.id + " - " + this.title + " - " + this.achievedDescription + " - " + this.unachievedDescription + " - " + (object) this.points + " - " + (object) this.hidden;
    }

    public void SetImage(Texture2D image)
    {
      this.m_Image = image;
    }

    public string id { get; set; }

    public string title
    {
      get
      {
        return this.m_Title;
      }
    }

    public Texture2D image
    {
      get
      {
        return this.m_Image;
      }
    }

    public string achievedDescription
    {
      get
      {
        return this.m_AchievedDescription;
      }
    }

    public string unachievedDescription
    {
      get
      {
        return this.m_UnachievedDescription;
      }
    }

    public bool hidden
    {
      get
      {
        return this.m_Hidden;
      }
    }

    public int points
    {
      get
      {
        return this.m_Points;
      }
    }
  }
}
