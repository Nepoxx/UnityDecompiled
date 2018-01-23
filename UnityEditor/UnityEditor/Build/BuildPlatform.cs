// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.BuildPlatform
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Build
{
  internal class BuildPlatform
  {
    public string name;
    public GUIContent title;
    public Texture2D smallIcon;
    public BuildTargetGroup targetGroup;
    public bool forceShowTarget;
    public string tooltip;

    public BuildPlatform(string locTitle, string iconId, BuildTargetGroup targetGroup, bool forceShowTarget)
      : this(locTitle, "", iconId, targetGroup, forceShowTarget)
    {
    }

    public BuildPlatform(string locTitle, string tooltip, string iconId, BuildTargetGroup targetGroup, bool forceShowTarget)
    {
      this.targetGroup = targetGroup;
      this.name = targetGroup == BuildTargetGroup.Unknown ? "" : BuildPipeline.GetBuildTargetGroupName(this.defaultTarget);
      this.title = EditorGUIUtility.TextContentWithIcon(locTitle, iconId);
      this.smallIcon = EditorGUIUtility.IconContent(iconId + ".Small").image as Texture2D;
      this.tooltip = tooltip;
      this.forceShowTarget = forceShowTarget;
    }

    public BuildTarget defaultTarget
    {
      get
      {
        BuildTargetGroup targetGroup = this.targetGroup;
        switch (targetGroup)
        {
          case BuildTargetGroup.WebGL:
            return BuildTarget.WebGL;
          case BuildTargetGroup.WSA:
            return BuildTarget.WSAPlayer;
          case BuildTargetGroup.Tizen:
            return BuildTarget.Tizen;
          case BuildTargetGroup.PSP2:
            return BuildTarget.PSP2;
          case BuildTargetGroup.PS4:
            return BuildTarget.PS4;
          case BuildTargetGroup.XboxOne:
            return BuildTarget.XboxOne;
          case BuildTargetGroup.N3DS:
            return BuildTarget.N3DS;
          case BuildTargetGroup.WiiU:
            return BuildTarget.WiiU;
          case BuildTargetGroup.tvOS:
            return BuildTarget.tvOS;
          case BuildTargetGroup.Facebook:
            return BuildTarget.StandaloneWindows64;
          case BuildTargetGroup.Switch:
            return BuildTarget.Switch;
          default:
            switch (targetGroup - 1)
            {
              case BuildTargetGroup.Unknown:
                return BuildTarget.StandaloneWindows;
              case BuildTargetGroup.Standalone | BuildTargetGroup.WebPlayer:
                return BuildTarget.iOS;
              default:
                return targetGroup == BuildTargetGroup.Android ? BuildTarget.Android : BuildTarget.iPhone;
            }
        }
      }
    }
  }
}
