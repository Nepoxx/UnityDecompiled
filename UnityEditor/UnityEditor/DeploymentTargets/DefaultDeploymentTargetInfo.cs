// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeploymentTargets.DefaultDeploymentTargetInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.BuildReporting;

namespace UnityEditor.DeploymentTargets
{
  internal class DefaultDeploymentTargetInfo : IDeploymentTargetInfo
  {
    public virtual FlagSet<DeploymentTargetSupportFlags> GetSupportFlags()
    {
      return (FlagSet<DeploymentTargetSupportFlags>) DeploymentTargetSupportFlags.None;
    }

    public virtual BuildCheckResult CheckBuild(BuildReport buildReport)
    {
      return new BuildCheckResult();
    }
  }
}
