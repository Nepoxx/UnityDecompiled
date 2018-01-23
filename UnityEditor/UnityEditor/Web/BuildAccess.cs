// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.BuildAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class BuildAccess : CloudServiceAccess
  {
    private const string kServiceName = "Build";
    private const string kServiceDisplayName = "Unity Build";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/build";

    static BuildAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Build", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/build", (CloudServiceAccess) new BuildAccess(), "unity/project/cloud/build"));
    }

    public override string GetServiceName()
    {
      return "Build";
    }

    public override string GetServiceDisplayName()
    {
      return "Unity Build";
    }

    public void ShowBuildForCommit(string commitId)
    {
      this.ShowServicePage();
      this.GetWebView().ExecuteJavascript(string.Format("window.unityEvents ? window.unityEvents.broadcast('build.showForCommit', '{0}'): '';", (object) commitId));
    }
  }
}
