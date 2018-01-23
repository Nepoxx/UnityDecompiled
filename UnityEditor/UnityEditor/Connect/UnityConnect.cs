// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnect
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor.Web;
using UnityEngine.Scripting;

namespace UnityEditor.Connect
{
  [InitializeOnLoad]
  internal sealed class UnityConnect
  {
    private static readonly UnityConnect s_Instance = new UnityConnect();

    private UnityConnect()
    {
      PackageUtils.instance.RetrievePackageInfo();
    }

    static UnityConnect()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("unity/connect", (object) UnityConnect.s_Instance);
    }

    public static extern bool preferencesEnabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool skipMissingUPID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool online { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool loggedIn { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool projectValid { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool workingOffline { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool shouldShowServicesWindow { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern string configuration { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetConfigurationURL(CloudConfigUrl config);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetEnvironment();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetAPIVersion();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetUserId();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetUserName();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetAccessToken();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetProjectGUID();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetProjectName();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetOrganizationId();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetOrganizationName();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetOrganizationForeignKey();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RefreshProject();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearCache();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Logout();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void WorkOffline(bool rememberDecision);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ShowLogin();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void OpenAuthorizedURLInWebBrowser(string url);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void BindProject(string projectGUID, string projectName, string organizationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void UnbindCloudProject();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SetCOPPACompliance(COPPACompliance compliance);

    public extern string lastErrorMessage { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int lastErrorCode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetError(int errorCode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearError(int errorCode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearErrors();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void UnhandledError(string request, int responseCode, string response);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ComputerGoesToSleep();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ComputerDidWakeUp();

    public extern UserInfo userInfo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern ProjectInfo projectInfo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern ConnectInfo connectInfo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool canBuildWithUPID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public event StateChangedDelegate StateChanged;

    public event ProjectStateChangedDelegate ProjectStateChanged;

    public event UserStateChangedDelegate UserStateChanged;

    public void GoToHub(string page)
    {
      UnityConnectServiceCollection.instance.ShowService("Hub", page, true, "goto_hub_method");
    }

    public void UnbindProject()
    {
      this.UnbindCloudProject();
      UnityConnectServiceCollection.instance.UnbindAllServices();
    }

    public ProjectInfo GetProjectInfo()
    {
      return this.projectInfo;
    }

    public UserInfo GetUserInfo()
    {
      return this.userInfo;
    }

    public ConnectInfo GetConnectInfo()
    {
      return this.connectInfo;
    }

    public string GetConfigurationUrlByIndex(int index)
    {
      switch (index)
      {
        case 0:
          return this.GetConfigurationURL(CloudConfigUrl.CloudCore);
        case 1:
          return this.GetConfigurationURL(CloudConfigUrl.CloudCollab);
        case 2:
          return this.GetConfigurationURL(CloudConfigUrl.CloudWebauth);
        case 3:
          return this.GetConfigurationURL(CloudConfigUrl.CloudLogin);
        case 6:
          return this.GetConfigurationURL(CloudConfigUrl.CloudIdentity);
        case 7:
          return this.GetConfigurationURL(CloudConfigUrl.CloudPortal);
        default:
          return "";
      }
    }

    public string GetCoreConfigurationUrl()
    {
      return this.GetConfigurationURL(CloudConfigUrl.CloudCore);
    }

    public bool DisplayDialog(string title, string message, string okBtn, string cancelBtn)
    {
      return EditorUtility.DisplayDialog(title, message, okBtn, cancelBtn);
    }

    public bool SetCOPPACompliance(int compliance)
    {
      return this.SetCOPPACompliance((COPPACompliance) compliance);
    }

    [MenuItem("Window/Unity Connect/Computer GoesToSleep", false, 1000, true)]
    public static void TestComputerGoesToSleep()
    {
      UnityConnect.instance.ComputerGoesToSleep();
    }

    [MenuItem("Window/Unity Connect/Computer DidWakeUp", false, 1000, true)]
    public static void TestComputerDidWakeUp()
    {
      UnityConnect.instance.ComputerDidWakeUp();
    }

    public static UnityConnect instance
    {
      get
      {
        return UnityConnect.s_Instance;
      }
    }

    private static void OnStateChanged()
    {
      // ISSUE: reference to a compiler-generated field
      StateChangedDelegate stateChanged = UnityConnect.instance.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged(UnityConnect.instance.connectInfo);
    }

    private static void OnProjectStateChanged()
    {
      // ISSUE: reference to a compiler-generated field
      ProjectStateChangedDelegate projectStateChanged = UnityConnect.instance.ProjectStateChanged;
      if (projectStateChanged == null)
        return;
      projectStateChanged(UnityConnect.instance.projectInfo);
    }

    private static void OnUserStateChanged()
    {
      // ISSUE: reference to a compiler-generated field
      UserStateChangedDelegate userStateChanged = UnityConnect.instance.UserStateChanged;
      if (userStateChanged == null)
        return;
      userStateChanged(UnityConnect.instance.userInfo);
    }

    [System.Flags]
    internal enum UnityErrorPriority
    {
      Critical = 0,
      Error = 1,
      Warning = 2,
      Info = Warning | Error, // 0x00000003
      None = 4,
    }

    [System.Flags]
    internal enum UnityErrorBehaviour
    {
      Alert = 0,
      Automatic = 1,
      Hidden = 2,
      ConsoleOnly = Hidden | Automatic, // 0x00000003
      Reconnect = 4,
    }

    [System.Flags]
    internal enum UnityErrorFilter
    {
      ByContext = 1,
      ByParent = 2,
      ByChild = 4,
      All = ByChild | ByParent | ByContext, // 0x00000007
    }
  }
}
