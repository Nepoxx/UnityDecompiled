// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightmapsMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;

namespace UnityEngine
{
  public enum LightmapsMode
  {
    NonDirectional = 0,
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Enum member LightmapsMode.Single has been removed. Use NonDirectional instead (UnityUpgradable) -> NonDirectional", true)] Single = 0,
    CombinedDirectional = 1,
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Enum member LightmapsMode.Dual has been removed. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true)] Dual = 1,
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Enum member LightmapsMode.Directional has been removed. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true)] Directional = 2,
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Enum member LightmapsMode.SeparateDirectional has been removed. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true)] SeparateDirectional = 2,
  }
}
