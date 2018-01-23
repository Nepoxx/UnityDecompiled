// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.AnalyticsAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Analytics;
using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class AnalyticsAccess : CloudServiceAccess
  {
    private const string kServiceName = "Analytics";
    private const string kServiceDisplayName = "Analytics";
    private const string kServicePackageName = "com.unity.analytics";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/analytics";

    static AnalyticsAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Analytics", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/analytics", (CloudServiceAccess) new AnalyticsAccess(), "unity/project/cloud/analytics"));
    }

    public override string GetServiceName()
    {
      return "Analytics";
    }

    public override string GetServiceDisplayName()
    {
      return "Analytics";
    }

    public override string GetPackageName()
    {
      return "com.unity.analytics";
    }

    public override bool IsServiceEnabled()
    {
      return AnalyticsSettings.enabled;
    }

    public override void EnableService(bool enabled)
    {
      if (AnalyticsSettings.enabled == enabled)
        return;
      AnalyticsSettings.enabled = enabled;
      EditorAnalytics.SendEventServiceInfo((object) new AnalyticsAccess.AnalyticsServiceState()
      {
        analytics = enabled
      });
    }

    public bool IsTestModeEnabled()
    {
      return AnalyticsSettings.testMode;
    }

    public void SetTestModeEnabled(bool enabled)
    {
      AnalyticsSettings.testMode = enabled;
    }

    [Serializable]
    public struct AnalyticsServiceState
    {
      public bool analytics;
    }
  }
}
