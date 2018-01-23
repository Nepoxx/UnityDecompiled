// Decompiled with JetBrains decompiler
// Type: UnityEngine.RemoteNotification
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;

namespace UnityEngine
{
  [Obsolete("RemoteNotification is deprecated. Please use iOS.RemoteNotification instead (UnityUpgradable) -> UnityEngine.iOS.RemoteNotification", true)]
  public sealed class RemoteNotification
  {
    public string alertBody
    {
      get
      {
        return (string) null;
      }
    }

    public bool hasAction
    {
      get
      {
        return false;
      }
    }

    public int applicationIconBadgeNumber
    {
      get
      {
        return 0;
      }
    }

    public string soundName
    {
      get
      {
        return (string) null;
      }
    }

    public IDictionary userInfo
    {
      get
      {
        return (IDictionary) null;
      }
    }
  }
}
