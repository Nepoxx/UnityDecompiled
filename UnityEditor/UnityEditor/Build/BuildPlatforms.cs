// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.BuildPlatforms
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.Build
{
  internal class BuildPlatforms
  {
    private static readonly BuildPlatforms s_Instance = new BuildPlatforms();
    public BuildPlatform[] buildPlatforms;

    internal BuildPlatforms()
    {
      List<BuildPlatform> buildPlatformList = new List<BuildPlatform>();
      buildPlatformList.Add(new BuildPlatform("PC, Mac & Linux Standalone", "BuildSettings.Standalone", BuildTargetGroup.Standalone, true));
      buildPlatformList.Add(new BuildPlatform("iOS", "BuildSettings.iPhone", BuildTargetGroup.iPhone, true));
      buildPlatformList.Add(new BuildPlatform("tvOS", "BuildSettings.tvOS", BuildTargetGroup.tvOS, true));
      buildPlatformList.Add(new BuildPlatform("Android", "BuildSettings.Android", BuildTargetGroup.Android, true));
      buildPlatformList.Add(new BuildPlatform("Tizen", "BuildSettings.Tizen", BuildTargetGroup.Tizen, false));
      buildPlatformList.Add(new BuildPlatform("Xbox One", "BuildSettings.XboxOne", BuildTargetGroup.XboxOne, true));
      buildPlatformList.Add(new BuildPlatform("PS Vita", "BuildSettings.PSP2", BuildTargetGroup.PSP2, true));
      buildPlatformList.Add(new BuildPlatform("PS4", "BuildSettings.PS4", BuildTargetGroup.PS4, true));
      buildPlatformList.Add(new BuildPlatform("Wii U", "BuildSettings.WiiU", BuildTargetGroup.WiiU, false));
      buildPlatformList.Add(new BuildPlatform("Universal Windows Platform", "BuildSettings.Metro", BuildTargetGroup.WSA, true));
      buildPlatformList.Add(new BuildPlatform("WebGL", "BuildSettings.WebGL", BuildTargetGroup.WebGL, true));
      buildPlatformList.Add(new BuildPlatform("Nintendo 3DS", "BuildSettings.N3DS", BuildTargetGroup.N3DS, false));
      buildPlatformList.Add(new BuildPlatform("Facebook", "BuildSettings.Facebook", BuildTargetGroup.Facebook, true));
      buildPlatformList.Add(new BuildPlatform("Nintendo Switch", "BuildSettings.Switch", BuildTargetGroup.Switch, false));
      foreach (BuildPlatform buildPlatform in buildPlatformList)
        buildPlatform.tooltip = BuildPipeline.GetBuildTargetGroupDisplayName(buildPlatform.targetGroup) + " settings";
      this.buildPlatforms = buildPlatformList.ToArray();
    }

    public static BuildPlatforms instance
    {
      get
      {
        return BuildPlatforms.s_Instance;
      }
    }

    public string GetBuildTargetDisplayName(BuildTargetGroup group, BuildTarget target)
    {
      foreach (BuildPlatform buildPlatform in this.buildPlatforms)
      {
        if (buildPlatform.defaultTarget == target && buildPlatform.targetGroup == group)
          return buildPlatform.title.text;
      }
      switch (target)
      {
        case BuildTarget.StandaloneOSX:
        case BuildTarget.StandaloneOSXIntel:
label_9:
          return "Mac OS X";
        case BuildTarget.StandaloneWindows:
label_8:
          return "Windows";
        default:
          switch (target - 24)
          {
            case ~BuildTarget.iPhone:
            case ~BuildTarget.NoTarget:
label_10:
              return "Linux";
            case (BuildTarget) 3:
              goto label_9;
            default:
              switch (target - 17)
              {
                case ~BuildTarget.iPhone:
                  goto label_10;
                case BuildTarget.StandaloneOSX:
                  goto label_8;
                default:
                  return "Unsupported Target";
              }
          }
      }
    }

    public string GetModuleDisplayName(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
    {
      if (buildTargetGroup == BuildTargetGroup.Facebook)
        return BuildPipeline.GetBuildTargetGroupDisplayName(buildTargetGroup);
      return this.GetBuildTargetDisplayName(buildTargetGroup, buildTarget);
    }

    public int BuildPlatformIndexFromTargetGroup(BuildTargetGroup group)
    {
      for (int index = 0; index < this.buildPlatforms.Length; ++index)
      {
        if (group == this.buildPlatforms[index].targetGroup)
          return index;
      }
      return -1;
    }

    public BuildPlatform BuildPlatformFromTargetGroup(BuildTargetGroup group)
    {
      int index = this.BuildPlatformIndexFromTargetGroup(group);
      return index == -1 ? (BuildPlatform) null : this.buildPlatforms[index];
    }

    public List<BuildPlatform> GetValidPlatforms(bool includeMetaPlatforms)
    {
      List<BuildPlatform> buildPlatformList = new List<BuildPlatform>();
      foreach (BuildPlatform buildPlatform in this.buildPlatforms)
      {
        if ((buildPlatform.targetGroup == BuildTargetGroup.Standalone || BuildPipeline.IsBuildTargetSupported(buildPlatform.targetGroup, buildPlatform.defaultTarget)) && (buildPlatform.targetGroup != BuildTargetGroup.Facebook || includeMetaPlatforms))
          buildPlatformList.Add(buildPlatform);
      }
      return buildPlatformList;
    }

    public List<BuildPlatform> GetValidPlatforms()
    {
      return this.GetValidPlatforms(false);
    }
  }
}
