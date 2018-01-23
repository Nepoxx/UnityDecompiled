// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnectServiceCollection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Web;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Connect
{
  internal class UnityConnectServiceCollection
  {
    private string m_CurrentServiceName = "";
    private string m_CurrentPageName = "";
    private static UnityConnectServiceCollection s_UnityConnectEditor;
    private static UnityConnectEditorWindow s_UnityConnectEditorWindow;
    private const string kDrawerContainerTitle = "Services";
    private readonly Dictionary<string, UnityConnectServiceData> m_Services;

    private UnityConnectServiceCollection()
    {
      this.m_Services = new Dictionary<string, UnityConnectServiceData>();
    }

    private void Init()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("UnityConnectEditor", (object) this);
      if (!Application.HasARGV("createProject"))
        return;
      this.ShowService("Hub", true, "init_create_project");
    }

    public bool isDrawerOpen
    {
      get
      {
        UnityConnectEditorWindow[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (UnityConnectEditorWindow)) as UnityConnectEditorWindow[];
        return objectsOfTypeAll != null && ((IEnumerable<UnityConnectEditorWindow>) objectsOfTypeAll).Any<UnityConnectEditorWindow>((Func<UnityConnectEditorWindow, bool>) (win => (UnityEngine.Object) win != (UnityEngine.Object) null));
      }
    }

    private void EnsureDrawerIsVisible(bool forceFocus)
    {
      if ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow == (UnityEngine.Object) null || !UnityConnectServiceCollection.s_UnityConnectEditorWindow.UrlsMatch(this.GetAllServiceUrls()))
      {
        string title = "Services";
        int serviceEnv = UnityConnectPrefs.GetServiceEnv(this.m_CurrentServiceName);
        if (serviceEnv != 0)
          title = title + " [" + UnityConnectPrefs.kEnvironmentFamilies[serviceEnv] + "]";
        UnityConnectServiceCollection.s_UnityConnectEditorWindow = UnityConnectEditorWindow.Create(title, this.GetAllServiceUrls());
        UnityConnectServiceCollection.s_UnityConnectEditorWindow.ErrorUrl = this.m_Services["ErrorHub"].serviceUrl;
        UnityConnectServiceCollection.s_UnityConnectEditorWindow.minSize = new Vector2(275f, 50f);
      }
      string str = this.m_Services[this.m_CurrentServiceName].serviceUrl;
      if (this.m_CurrentPageName.Length > 0)
        str = str + "/#/" + this.m_CurrentPageName;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.currentUrl = str;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.ShowTab();
      if (!InternalEditorUtility.isApplicationActive || !forceFocus)
        return;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.Focus();
    }

    public void CloseServices()
    {
      if ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow != (UnityEngine.Object) null)
      {
        UnityConnectServiceCollection.s_UnityConnectEditorWindow.Close();
        UnityConnectServiceCollection.s_UnityConnectEditorWindow = (UnityConnectEditorWindow) null;
      }
      UnityConnect.instance.ClearCache();
    }

    public void ReloadServices()
    {
      if (!((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow != (UnityEngine.Object) null))
        return;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.Reload();
    }

    public static UnityConnectServiceCollection instance
    {
      get
      {
        if (UnityConnectServiceCollection.s_UnityConnectEditor == null)
        {
          UnityConnectServiceCollection.s_UnityConnectEditor = new UnityConnectServiceCollection();
          UnityConnectServiceCollection.s_UnityConnectEditor.Init();
        }
        return UnityConnectServiceCollection.s_UnityConnectEditor;
      }
    }

    public static void StaticEnableService(string serviceName, bool enabled)
    {
      UnityConnectServiceCollection.instance.EnableService(serviceName, enabled);
    }

    public bool AddService(UnityConnectServiceData cloudService)
    {
      if (this.m_Services.ContainsKey(cloudService.serviceName))
        return false;
      this.m_Services[cloudService.serviceName] = cloudService;
      return true;
    }

    public bool RemoveService(string serviceName)
    {
      if (!this.m_Services.ContainsKey(serviceName))
        return false;
      return this.m_Services.Remove(serviceName);
    }

    public bool ServiceExist(string serviceName)
    {
      return this.m_Services.ContainsKey(serviceName);
    }

    public bool ShowService(string serviceName, bool forceFocus, string atReferrer)
    {
      return this.ShowService(serviceName, "", forceFocus, atReferrer);
    }

    public bool ShowService(string serviceName, string atPage, bool forceFocus, string atReferrer)
    {
      if (!this.m_Services.ContainsKey(serviceName))
        return false;
      ConnectInfo connectInfo = UnityConnect.instance.connectInfo;
      this.m_CurrentServiceName = this.GetActualServiceName(serviceName, connectInfo);
      this.m_CurrentPageName = atPage;
      EditorAnalytics.SendEventShowService((object) new UnityConnectServiceCollection.ShowServiceState()
      {
        service = this.m_CurrentServiceName,
        page = atPage,
        referrer = atReferrer
      });
      this.EnsureDrawerIsVisible(forceFocus);
      return true;
    }

    private string GetActualServiceName(string desiredServiceName, ConnectInfo state)
    {
      if (!state.online)
        return "ErrorHub";
      if (!state.ready)
        return "Hub";
      if (state.maintenance)
        return "ErrorHub";
      if (desiredServiceName != "Hub" && state.online && !state.loggedIn || desiredServiceName == "ErrorHub" && state.online || string.IsNullOrEmpty(desiredServiceName))
        return "Hub";
      return desiredServiceName;
    }

    public void EnableService(string name, bool enabled)
    {
      if (!this.m_Services.ContainsKey(name))
        return;
      this.m_Services[name].EnableService(enabled);
    }

    public string GetUrlForService(string serviceName)
    {
      return !this.m_Services.ContainsKey(serviceName) ? string.Empty : this.m_Services[serviceName].serviceUrl;
    }

    public UnityConnectServiceData GetServiceFromUrl(string searchUrl)
    {
      return this.m_Services.FirstOrDefault<KeyValuePair<string, UnityConnectServiceData>>((Func<KeyValuePair<string, UnityConnectServiceData>, bool>) (kvp => kvp.Value.serviceUrl == searchUrl)).Value;
    }

    public List<string> GetAllServiceNames()
    {
      return this.m_Services.Keys.ToList<string>();
    }

    public List<string> GetAllServiceUrls()
    {
      return this.m_Services.Values.Select<UnityConnectServiceData, string>((Func<UnityConnectServiceData, string>) (unityConnectData => unityConnectData.serviceUrl)).ToList<string>();
    }

    public UnityConnectServiceCollection.ServiceInfo[] GetAllServiceInfos()
    {
      return this.m_Services.Select<KeyValuePair<string, UnityConnectServiceData>, UnityConnectServiceCollection.ServiceInfo>((Func<KeyValuePair<string, UnityConnectServiceData>, UnityConnectServiceCollection.ServiceInfo>) (item => new UnityConnectServiceCollection.ServiceInfo(item.Value.serviceName, item.Value.serviceUrl, item.Value.serviceJsGlobalObjectName, item.Value.serviceJsGlobalObject.IsServiceEnabled()))).ToArray<UnityConnectServiceCollection.ServiceInfo>();
    }

    public WebView GetWebViewFromServiceName(string serviceName)
    {
      if ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow == (UnityEngine.Object) null || !UnityConnectServiceCollection.s_UnityConnectEditorWindow.UrlsMatch(this.GetAllServiceUrls()) || !this.m_Services.ContainsKey(serviceName))
        return (WebView) null;
      ConnectInfo connectInfo = UnityConnect.instance.connectInfo;
      string serviceUrl = this.m_Services[this.GetActualServiceName(serviceName, connectInfo)].serviceUrl;
      return UnityConnectServiceCollection.s_UnityConnectEditorWindow.GetWebViewFromURL(serviceUrl);
    }

    public void UnbindAllServices()
    {
      foreach (UnityConnectServiceData connectServiceData in this.m_Services.Values)
        connectServiceData.OnProjectUnbound();
    }

    [Serializable]
    public struct ShowServiceState
    {
      public string service;
      public string page;
      public string referrer;
    }

    public class ServiceInfo
    {
      public string name;
      public string url;
      public string unityPath;
      public bool enabled;

      public ServiceInfo(string name, string url, string unityPath, bool enabled)
      {
        this.name = name;
        this.url = url;
        this.unityPath = unityPath;
        this.enabled = enabled;
      }
    }
  }
}
