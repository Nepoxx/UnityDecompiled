// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.PurchasingAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using UnityEditor.Connect;
using UnityEditor.Purchasing;
using UnityEngine;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class PurchasingAccess : CloudServiceAccess
  {
    private static readonly Uri kPackageUri = new Uri("https://public-cdn.cloud.unity3d.com/UnityEngine.Cloud.Purchasing.unitypackage");
    private const string kServiceName = "Purchasing";
    private const string kServiceDisplayName = "In App Purchasing";
    private const string kServicePackageName = "com.unity.purchasing";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/purchasing";
    private const string kETagPath = "Assets/Plugins/UnityPurchasing/ETag";
    private const string kUnknownPackageETag = "unknown";
    private bool m_InstallInProgress;

    static PurchasingAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Purchasing", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/purchasing", (CloudServiceAccess) new PurchasingAccess(), "unity/project/cloud/purchasing"));
    }

    public override string GetServiceName()
    {
      return "Purchasing";
    }

    public override string GetServiceDisplayName()
    {
      return "In App Purchasing";
    }

    public override string GetPackageName()
    {
      return "com.unity.purchasing";
    }

    public override bool IsServiceEnabled()
    {
      return PurchasingSettings.enabled;
    }

    public override void EnableService(bool enabled)
    {
      if (PurchasingSettings.enabled == enabled)
        return;
      PurchasingSettings.enabled = enabled;
      EditorAnalytics.SendEventServiceInfo((object) new PurchasingAccess.PurchasingServiceState()
      {
        iap = enabled
      });
    }

    public void InstallUnityPackage()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PurchasingAccess.\u003CInstallUnityPackage\u003Ec__AnonStorey0 packageCAnonStorey0 = new PurchasingAccess.\u003CInstallUnityPackage\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey0.\u0024this = this;
      if (this.m_InstallInProgress)
        return;
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey0.originalCallback = ServicePointManager.ServerCertificateValidationCallback;
      if (Application.platform != RuntimePlatform.OSXEditor)
        ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((a, b, c, d) => true);
      this.m_InstallInProgress = true;
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey0.location = FileUtil.GetUniqueTempPathInProject();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey0.location = Path.ChangeExtension(packageCAnonStorey0.location, ".unitypackage");
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey0.client = new WebClient();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      packageCAnonStorey0.client.DownloadFileCompleted += new AsyncCompletedEventHandler(packageCAnonStorey0.\u003C\u003Em__0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey0.client.DownloadFileAsync(PurchasingAccess.kPackageUri, packageCAnonStorey0.location);
    }

    public string GetInstalledETag()
    {
      if (System.IO.File.Exists("Assets/Plugins/UnityPurchasing/ETag"))
        return System.IO.File.ReadAllText("Assets/Plugins/UnityPurchasing/ETag");
      if (Directory.Exists(Path.GetDirectoryName("Assets/Plugins/UnityPurchasing/ETag")))
        return "unknown";
      return (string) null;
    }

    private void SaveETag(WebClient client)
    {
      string contents = client.ResponseHeaders.Get("ETag");
      if (contents == null)
        return;
      Directory.CreateDirectory(Path.GetDirectoryName("Assets/Plugins/UnityPurchasing/ETag"));
      System.IO.File.WriteAllText("Assets/Plugins/UnityPurchasing/ETag", contents);
    }

    public struct PurchasingServiceState
    {
      public bool iap;
    }
  }
}
