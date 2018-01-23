// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.ErrorHubAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class ErrorHubAccess : CloudServiceAccess
  {
    private static string kServiceUrl = "file://" + EditorApplication.userJavascriptPackagesPath + "unityeditor-cloud-hub/dist/index.html?failure=unity_connect";
    public const string kServiceName = "ErrorHub";

    static ErrorHubAccess()
    {
      ErrorHubAccess.instance = new ErrorHubAccess();
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("ErrorHub", ErrorHubAccess.kServiceUrl, (CloudServiceAccess) ErrorHubAccess.instance, "unity/project/cloud/errorhub"));
    }

    public static ErrorHubAccess instance { get; private set; }

    public string errorMessage { get; set; }

    public override string GetServiceName()
    {
      return "ErrorHub";
    }
  }
}
