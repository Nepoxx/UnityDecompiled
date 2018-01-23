// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.CollabAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Collaboration;
using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class CollabAccess : CloudServiceAccess
  {
    private static CollabAccess s_instance = new CollabAccess();
    private const string kServiceName = "Collab";
    private const string kServiceDisplayName = "Unity Collab";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/collab";

    static CollabAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Collab", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/collab", (CloudServiceAccess) CollabAccess.s_instance, "unity/project/cloud/collab"));
    }

    public static CollabAccess Instance
    {
      get
      {
        return CollabAccess.s_instance;
      }
    }

    public override string GetServiceName()
    {
      return "Collab";
    }

    public override string GetServiceDisplayName()
    {
      return "Unity Collab";
    }

    public override void EnableService(bool enabled)
    {
      base.EnableService(enabled);
      Collab.instance.SendNotification();
      Collab.instance.SetCollabEnabledForCurrentProject(enabled);
      AssetDatabase.Refresh();
    }

    public bool IsCollabUIAccessible()
    {
      return true;
    }
  }
}
