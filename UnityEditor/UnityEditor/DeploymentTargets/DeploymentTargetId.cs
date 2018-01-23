// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeploymentTargets.DeploymentTargetId
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.DeploymentTargets
{
  internal struct DeploymentTargetId
  {
    public string id;

    public DeploymentTargetId(string id)
    {
      this.id = id;
    }

    public static implicit operator DeploymentTargetId(string id)
    {
      return new DeploymentTargetId(id);
    }

    public static implicit operator string(DeploymentTargetId id)
    {
      return id.id;
    }
  }
}
