// Decompiled with JetBrains decompiler
// Type: UnityEngine.FocusType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Used by GUIUtility.GetControlID to inform the IMGUI system if a given control can get keyboard focus. This allows the IMGUI system to give focus appropriately when a user presses tab for cycling between controls.</para>
  /// </summary>
  public enum FocusType
  {
    [Obsolete("FocusType.Native now behaves the same as FocusType.Passive in all OS cases. (UnityUpgradable) -> Passive", false)] Native,
    Keyboard,
    Passive,
  }
}
