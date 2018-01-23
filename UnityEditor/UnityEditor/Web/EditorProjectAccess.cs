// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.EditorProjectAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor.Connect;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal sealed class EditorProjectAccess
  {
    private const string kCloudServiceKey = "CloudServices";
    private const string kCloudEnabled = "CloudEnabled";

    static EditorProjectAccess()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("unity/project", (object) new EditorProjectAccess());
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetProjectEditorVersion();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetRESTServiceURI();

    public void OpenLink(string link)
    {
      Help.BrowseURL(link);
    }

    public bool IsOnline()
    {
      return UnityConnect.instance.online;
    }

    public bool IsLoggedIn()
    {
      return UnityConnect.instance.loggedIn;
    }

    public string GetEnvironment()
    {
      return UnityConnect.instance.GetEnvironment();
    }

    public string GetUserName()
    {
      return UnityConnect.instance.userInfo.userName;
    }

    public string GetUserDisplayName()
    {
      return UnityConnect.instance.userInfo.displayName;
    }

    public string GetUserPrimaryOrganizationId()
    {
      return UnityConnect.instance.userInfo.primaryOrg;
    }

    public string GetUserAccessToken()
    {
      return UnityConnect.instance.GetAccessToken();
    }

    public string GetProjectName()
    {
      string projectName = UnityConnect.instance.projectInfo.projectName;
      if (projectName != "")
        return projectName;
      return PlayerSettings.productName;
    }

    public string GetProjectGUID()
    {
      return UnityConnect.instance.projectInfo.projectGUID;
    }

    public string GetProjectPath()
    {
      return Directory.GetCurrentDirectory();
    }

    public string GetProjectIcon()
    {
      return (string) null;
    }

    public string GetOrganizationID()
    {
      return UnityConnect.instance.projectInfo.organizationId;
    }

    public string GetBuildTarget()
    {
      return EditorUserBuildSettings.activeBuildTarget.ToString();
    }

    public bool IsProjectBound()
    {
      return UnityConnect.instance.projectInfo.projectBound;
    }

    public void EnableCloud(bool enable)
    {
      EditorUserSettings.SetConfigValue("CloudServices/CloudEnabled", enable.ToString());
    }

    public void EnterPlayMode()
    {
      EditorApplication.isPlaying = true;
    }

    public bool IsPlayMode()
    {
      return EditorApplication.isPlaying;
    }

    public bool SaveCurrentModifiedScenesIfUserWantsTo()
    {
      return EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }

    public int GetEditorSkinIndex()
    {
      return EditorGUIUtility.skinIndex;
    }

    public void GoToHistory()
    {
      CollabHistoryWindow.ShowHistoryWindow().Focus();
    }

    public static void ShowToolbarDropdown()
    {
      Toolbar.requestShowCollabToolbar = true;
      if (!(bool) ((Object) Toolbar.get))
        return;
      Toolbar.get.Repaint();
    }

    public void CloseToolbarWindow()
    {
      CollabToolbarWindow.CloseToolbarWindows();
    }

    public void CloseToolbarWindowImmediately()
    {
      CollabToolbarWindow.CloseToolbarWindowsImmediately();
    }
  }
}
