// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.HubAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class HubAccess : CloudServiceAccess
  {
    private static HubAccess s_Instance = new HubAccess();
    public const string kServiceName = "Hub";
    private const string kServiceDisplayName = "Services";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/hub";

    static HubAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Hub", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/hub", (CloudServiceAccess) HubAccess.s_Instance, "unity/project/cloud/hub"));
    }

    public static HubAccess instance
    {
      get
      {
        return HubAccess.s_Instance;
      }
    }

    public override string GetServiceName()
    {
      return "Hub";
    }

    public override string GetServiceDisplayName()
    {
      return "Services";
    }

    public UnityConnectServiceCollection.ServiceInfo[] GetServices()
    {
      return UnityConnectServiceCollection.instance.GetAllServiceInfos();
    }

    public void ShowService(string name)
    {
      UnityConnectServiceCollection.instance.ShowService(name, true, "show_service_method");
    }

    public void EnableCloudService(string name, bool enabled)
    {
      UnityConnectServiceCollection.instance.EnableService(name, enabled);
    }

    [MenuItem("Window/Services %0", false, 1999)]
    private static void ShowMyWindow()
    {
      UnityConnectServiceCollection.instance.ShowService("Hub", true, "window_menu_item");
    }
  }
}
