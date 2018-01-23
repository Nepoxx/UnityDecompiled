// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.BuildLaunchPlayerArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.BuildReporting;

namespace UnityEditor.Modules
{
  internal struct BuildLaunchPlayerArgs
  {
    public BuildTarget target;
    public string playerPackage;
    public string installPath;
    public string productName;
    public BuildOptions options;
    public BuildReport report;
  }
}
