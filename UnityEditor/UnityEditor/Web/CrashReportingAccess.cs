// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.CrashReportingAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Connect;
using UnityEditor.CrashReporting;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class CrashReportingAccess : CloudServiceAccess
  {
    private const string kServiceName = "Game Performance";
    private const string kServiceDisplayName = "Game Performance";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/crash";

    static CrashReportingAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Game Performance", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/crash", (CloudServiceAccess) new CrashReportingAccess(), "unity/project/cloud/crashreporting"));
    }

    public override string GetServiceName()
    {
      return "Game Performance";
    }

    public override string GetServiceDisplayName()
    {
      return "Game Performance";
    }

    public override bool IsServiceEnabled()
    {
      return CrashReportingSettings.enabled;
    }

    public override void EnableService(bool enabled)
    {
      if (CrashReportingSettings.enabled == enabled)
        return;
      CrashReportingSettings.enabled = enabled;
      EditorAnalytics.SendEventServiceInfo((object) new CrashReportingAccess.CrashReportingServiceState()
      {
        crash_reporting = enabled
      });
    }

    public bool GetCaptureEditorExceptions()
    {
      return CrashReportingSettings.captureEditorExceptions;
    }

    public void SetCaptureEditorExceptions(bool captureEditorExceptions)
    {
      CrashReportingSettings.captureEditorExceptions = captureEditorExceptions;
    }

    [Serializable]
    public struct CrashReportingServiceState
    {
      public bool crash_reporting;
    }
  }
}
