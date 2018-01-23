// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeploymentTargets.DefaultDeploymentTargetsExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.BuildReporting;

namespace UnityEditor.DeploymentTargets
{
  internal abstract class DefaultDeploymentTargetsExtension : IDeploymentTargetsExtension
  {
    public virtual List<DeploymentTargetIdAndStatus> GetKnownTargets(ProgressHandler progressHandler = null)
    {
      return new List<DeploymentTargetIdAndStatus>();
    }

    public virtual IDeploymentTargetInfo GetTargetInfo(DeploymentTargetId targetId, ProgressHandler progressHandler = null)
    {
      return (IDeploymentTargetInfo) new DefaultDeploymentTargetInfo();
    }

    public virtual void LaunchBuildOnTarget(BuildReport buildReport, DeploymentTargetId targetId, ProgressHandler progressHandler = null)
    {
      throw new NotSupportedException();
    }
  }
}
