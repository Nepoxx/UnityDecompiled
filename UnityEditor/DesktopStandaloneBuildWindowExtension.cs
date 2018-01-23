// Decompiled with JetBrains decompiler
// Type: DesktopStandaloneBuildWindowExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Modules;
using UnityEngine;

internal class DesktopStandaloneBuildWindowExtension : DefaultBuildWindowExtension
{
  private GUIContent m_StandaloneTarget = EditorGUIUtility.TextContent("Target Platform|Destination platform for standalone build");
  private GUIContent m_Architecture = EditorGUIUtility.TextContent("Architecture|Build m_Architecture for standalone");
  private BuildTarget[] m_StandaloneSubtargets;
  private GUIContent[] m_StandaloneSubtargetStrings;

  public DesktopStandaloneBuildWindowExtension()
  {
    this.SetupStandaloneSubtargets();
  }

  private void SetupStandaloneSubtargets()
  {
    List<BuildTarget> buildTargetList = new List<BuildTarget>();
    List<GUIContent> guiContentList = new List<GUIContent>();
    if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows)))
    {
      buildTargetList.Add(BuildTarget.StandaloneWindows);
      guiContentList.Add(EditorGUIUtility.TextContent("Windows"));
    }
    if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)))
    {
      buildTargetList.Add(BuildTarget.StandaloneOSX);
      guiContentList.Add(EditorGUIUtility.TextContent("Mac OS X"));
    }
    if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)))
    {
      buildTargetList.Add(BuildTarget.StandaloneLinux);
      guiContentList.Add(EditorGUIUtility.TextContent("Linux"));
    }
    this.m_StandaloneSubtargets = buildTargetList.ToArray();
    this.m_StandaloneSubtargetStrings = guiContentList.ToArray();
  }

  internal static BuildTarget GetBestStandaloneTarget(BuildTarget selectedTarget)
  {
    if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(selectedTarget)))
      return selectedTarget;
    if (Application.platform == RuntimePlatform.WindowsEditor && ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows)))
      return BuildTarget.StandaloneWindows;
    if (Application.platform == RuntimePlatform.OSXEditor && ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)))
      return BuildTarget.StandaloneOSX;
    return ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)) ? BuildTarget.StandaloneLinux : BuildTarget.StandaloneWindows;
  }

  private static Dictionary<GUIContent, BuildTarget> GetArchitecturesForPlatform(BuildTarget target)
  {
    switch (target)
    {
      case BuildTarget.StandaloneLinux:
        return new Dictionary<GUIContent, BuildTarget>() { { EditorGUIUtility.TextContent("x86"), BuildTarget.StandaloneLinux }, { EditorGUIUtility.TextContent("x86_64"), BuildTarget.StandaloneLinux64 }, { EditorGUIUtility.TextContent("x86 + x86_64 (Universal)"), BuildTarget.StandaloneLinuxUniversal } };
      case BuildTarget.StandaloneWindows64:
        return new Dictionary<GUIContent, BuildTarget>() { { EditorGUIUtility.TextContent("x86"), BuildTarget.StandaloneWindows }, { EditorGUIUtility.TextContent("x86_64"), BuildTarget.StandaloneWindows64 } };
      default:
        if (target != BuildTarget.StandaloneLinux64 && target != BuildTarget.StandaloneLinuxUniversal)
        {
          if (target != BuildTarget.StandaloneWindows)
            return (Dictionary<GUIContent, BuildTarget>) null;
          goto case BuildTarget.StandaloneWindows64;
        }
        else
          goto case BuildTarget.StandaloneLinux;
    }
  }

  private static BuildTarget DefaultTargetForPlatform(BuildTarget target)
  {
    switch (target)
    {
      case BuildTarget.StandaloneOSX:
      case BuildTarget.StandaloneOSXIntel:
label_5:
        return BuildTarget.StandaloneOSX;
      case BuildTarget.StandaloneWindows:
label_3:
        return BuildTarget.StandaloneWindows;
      default:
        switch (target - 24)
        {
          case ~BuildTarget.iPhone:
          case ~BuildTarget.NoTarget:
label_4:
            return BuildTarget.StandaloneLinux;
          case (BuildTarget) 3:
            goto label_5;
          default:
            switch (target - 17)
            {
              case ~BuildTarget.iPhone:
                goto label_4;
              case BuildTarget.StandaloneOSX:
                goto label_3;
              default:
                return target;
            }
        }
    }
  }

  public override void ShowPlatformBuildOptions()
  {
    BuildTarget standaloneTarget = DesktopStandaloneBuildWindowExtension.GetBestStandaloneTarget(EditorUserBuildSettings.selectedStandaloneTarget);
    BuildTarget buildTarget = EditorUserBuildSettings.selectedStandaloneTarget;
    int selectedIndex1 = Math.Max(0, Array.IndexOf<BuildTarget>(this.m_StandaloneSubtargets, DesktopStandaloneBuildWindowExtension.DefaultTargetForPlatform(standaloneTarget)));
    int index1 = EditorGUILayout.Popup(this.m_StandaloneTarget, selectedIndex1, this.m_StandaloneSubtargetStrings, new GUILayoutOption[0]);
    if (index1 == selectedIndex1)
    {
      Dictionary<GUIContent, BuildTarget> architecturesForPlatform = DesktopStandaloneBuildWindowExtension.GetArchitecturesForPlatform(standaloneTarget);
      if (architecturesForPlatform != null)
      {
        GUIContent[] array = new List<GUIContent>((IEnumerable<GUIContent>) architecturesForPlatform.Keys).ToArray();
        int selectedIndex2 = 0;
        if (index1 == selectedIndex1)
        {
          foreach (KeyValuePair<GUIContent, BuildTarget> keyValuePair in architecturesForPlatform)
          {
            if (keyValuePair.Value == standaloneTarget)
            {
              selectedIndex2 = Math.Max(0, Array.IndexOf<GUIContent>(array, keyValuePair.Key));
              break;
            }
          }
        }
        int index2 = EditorGUILayout.Popup(this.m_Architecture, selectedIndex2, array, new GUILayoutOption[0]);
        buildTarget = architecturesForPlatform[array[index2]];
      }
    }
    else
      buildTarget = this.m_StandaloneSubtargets[index1];
    if (buildTarget == EditorUserBuildSettings.selectedStandaloneTarget)
      return;
    EditorUserBuildSettings.selectedStandaloneTarget = buildTarget;
    GUIUtility.ExitGUI();
  }

  public override bool EnabledBuildButton()
  {
    return true;
  }

  public override bool EnabledBuildAndRunButton()
  {
    return true;
  }
}
