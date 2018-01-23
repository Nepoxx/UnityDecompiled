// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.UnetAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class UnetAccess : CloudServiceAccess
  {
    private const string kServiceName = "UNet";
    private const string kServiceDisplayName = "Multiplayer";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/unet";

    static UnetAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("UNet", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/unet", (CloudServiceAccess) new UnetAccess(), "unity/project/cloud/networking"));
    }

    public override string GetServiceName()
    {
      return "UNet";
    }

    public override string GetServiceDisplayName()
    {
      return "Multiplayer";
    }

    public override void EnableService(bool enabled)
    {
      if (this.IsServiceEnabled() == enabled)
        return;
      base.EnableService(enabled);
      EditorAnalytics.SendEventServiceInfo((object) new UnetAccess.UnetServiceState()
      {
        unet = enabled
      });
    }

    public void SetMultiplayerId(int id)
    {
    }

    [Serializable]
    public struct UnetServiceState
    {
      public bool unet;
    }
  }
}
