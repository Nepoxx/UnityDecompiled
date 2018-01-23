// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.RaycasterManager
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
  internal static class RaycasterManager
  {
    private static readonly List<BaseRaycaster> s_Raycasters = new List<BaseRaycaster>();

    public static void AddRaycaster(BaseRaycaster baseRaycaster)
    {
      if (RaycasterManager.s_Raycasters.Contains(baseRaycaster))
        return;
      RaycasterManager.s_Raycasters.Add(baseRaycaster);
    }

    public static List<BaseRaycaster> GetRaycasters()
    {
      return RaycasterManager.s_Raycasters;
    }

    public static void RemoveRaycasters(BaseRaycaster baseRaycaster)
    {
      if (!RaycasterManager.s_Raycasters.Contains(baseRaycaster))
        return;
      RaycasterManager.s_Raycasters.Remove(baseRaycaster);
    }
  }
}
