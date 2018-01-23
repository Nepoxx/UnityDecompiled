// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.TroubleshooterAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class TroubleshooterAccess
  {
    private TroubleshooterAccess()
    {
    }

    static TroubleshooterAccess()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("/unity/editor/troubleshooter", (object) new TroubleshooterAccess());
    }

    public string GetUserName()
    {
      UnityConnect instance = UnityConnect.instance;
      if (!instance.GetConnectInfo().loggedIn)
        return "Anonymous";
      return instance.GetUserName();
    }

    public string GetUserId()
    {
      UnityConnect instance = UnityConnect.instance;
      if (!instance.GetConnectInfo().loggedIn)
        return string.Empty;
      return instance.GetUserInfo().userId;
    }

    public void SignIn()
    {
      UnityConnect.instance.ShowLogin();
    }

    public void SignOut()
    {
      UnityConnect.instance.Logout();
    }

    public void StartBugReporter()
    {
      EditorUtility.LaunchBugReporter();
    }
  }
}
