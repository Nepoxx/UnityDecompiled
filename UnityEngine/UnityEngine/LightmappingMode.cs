// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightmappingMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("LightmappingMode has been deprecated. Use LightmapBakeType instead (UnityUpgradable) -> LightmapBakeType", true)]
  public enum LightmappingMode
  {
    [Obsolete("LightmappingMode.Mixed has been deprecated. Use LightmapBakeType.Mixed instead (UnityUpgradable) -> LightmapBakeType.Mixed", true)] Mixed = 1,
    [Obsolete("LightmappingMode.Baked has been deprecated. Use LightmapBakeType.Baked instead (UnityUpgradable) -> LightmapBakeType.Baked", true)] Baked = 2,
    [Obsolete("LightmappingMode.Realtime has been deprecated. Use LightmapBakeType.Realtime instead (UnityUpgradable) -> LightmapBakeType.Realtime", true)] Realtime = 4,
  }
}
