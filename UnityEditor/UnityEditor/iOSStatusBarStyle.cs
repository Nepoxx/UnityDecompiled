// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSStatusBarStyle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>iOS status bar style.</para>
  /// </summary>
  public enum iOSStatusBarStyle
  {
    [Obsolete("BlackOpaque has no effect, use LightContent instead (UnityUpgradable) -> LightContent", true)] BlackOpaque = -1,
    [Obsolete("BlackTranslucent has no effect, use LightContent instead (UnityUpgradable) -> LightContent", true)] BlackTranslucent = -1,
    Default = 0,
    LightContent = 1,
  }
}
