// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeploymentTargets.BuildCheckResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.DeploymentTargets
{
  internal struct BuildCheckResult
  {
    public CategoryCheckResult hardware;
    public CategoryCheckResult sdk;

    public bool Passed()
    {
      return this.hardware.status == CheckStatus.Ok && this.sdk.status == CheckStatus.Ok;
    }
  }
}
