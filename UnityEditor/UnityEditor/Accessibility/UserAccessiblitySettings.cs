// Decompiled with JetBrains decompiler
// Type: UnityEditor.Accessibility.UserAccessiblitySettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.Accessibility
{
  internal static class UserAccessiblitySettings
  {
    private static ColorBlindCondition s_ColorBlindCondition = (ColorBlindCondition) EditorPrefs.GetInt("AccessibilityColorBlindCondition", 0);
    private const string k_ColorBlindConditionPrefKey = "AccessibilityColorBlindCondition";
    public static Action colorBlindConditionChanged;

    public static ColorBlindCondition colorBlindCondition
    {
      get
      {
        return UserAccessiblitySettings.s_ColorBlindCondition;
      }
      set
      {
        if (UserAccessiblitySettings.s_ColorBlindCondition == value)
          return;
        UserAccessiblitySettings.s_ColorBlindCondition = value;
        EditorPrefs.SetInt("AccessibilityColorBlindCondition", (int) value);
        if (UserAccessiblitySettings.colorBlindConditionChanged != null)
          UserAccessiblitySettings.colorBlindConditionChanged();
      }
    }
  }
}
