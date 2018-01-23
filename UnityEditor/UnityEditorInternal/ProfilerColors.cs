// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerColors
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Accessibility;
using UnityEngine;
using UnityEngine.Accessibility;

namespace UnityEditorInternal
{
  internal class ProfilerColors
  {
    internal static Color allocationSample = new Color(0.7f, 0.1f, 0.3f, 1f);
    internal static Color internalSample = new Color(0.3921569f, 0.3921569f, 0.3921569f, 0.75f);
    private static readonly Color[] s_DefaultColors = new Color[18]{ new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f), new Color(0.2070592f, 0.5333336f, 0.6556864f, 1f), new Color(0.8f, 0.4423528f, 0.0f, 1f), new Color(0.4486272f, 0.4078432f, 0.050196f, 1f), new Color(0.7749016f, 0.6368624f, 0.0250984f, 1f), new Color(0.5333336f, 0.16f, 0.0282352f, 1f), new Color(0.3827448f, 0.2886272f, 0.5239216f, 1f), new Color(0.4784314f, 0.4823529f, 0.1176471f, 1f), new Color(0.9411765f, 0.5019608f, 0.5019608f, 1f), new Color(0.6627451f, 0.6627451f, 0.6627451f, 1f), new Color(0.5450981f, 0.0f, 0.5450981f, 1f), new Color((float) byte.MaxValue, 0.8941177f, 0.7098039f, 1f), new Color(0.1254902f, 0.6980392f, 0.6666667f, 1f), new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f), new Color(0.3827448f, 0.2886272f, 0.5239216f, 1f), new Color(0.8f, 0.4423528f, 0.0f, 1f), new Color(0.4486272f, 0.4078432f, 0.050196f, 1f), new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f) };
    private static readonly Color[] s_ColorBlindSafeColors = new Color[ProfilerColors.s_DefaultColors.Length];

    static ProfilerColors()
    {
      VisionUtility.GetColorBlindSafePalette(ProfilerColors.s_ColorBlindSafeColors, 0.3f, 1f);
    }

    public static Color[] currentColors
    {
      get
      {
        return UserAccessiblitySettings.colorBlindCondition != ColorBlindCondition.Default ? ProfilerColors.s_ColorBlindSafeColors : ProfilerColors.s_DefaultColors;
      }
    }
  }
}
