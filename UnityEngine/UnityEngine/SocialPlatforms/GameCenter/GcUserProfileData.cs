// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.GameCenter.GcUserProfileData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms.GameCenter
{
  [RequiredByNativeCode]
  internal struct GcUserProfileData
  {
    public string userName;
    public string userID;
    public int isFriend;
    public Texture2D image;

    public UserProfile ToUserProfile()
    {
      return new UserProfile(this.userName, this.userID, this.isFriend == 1, UserState.Offline, this.image);
    }

    public void AddToArray(ref UserProfile[] array, int number)
    {
      if (array.Length > number && number >= 0)
        array[number] = this.ToUserProfile();
      else
        Debug.Log((object) "Index number out of bounds when setting user data");
    }
  }
}
