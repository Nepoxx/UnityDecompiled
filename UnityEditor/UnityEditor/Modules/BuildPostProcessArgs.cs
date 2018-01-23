// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.BuildPostProcessArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.BuildReporting;

namespace UnityEditor.Modules
{
  internal struct BuildPostProcessArgs
  {
    public BuildTarget target;
    public string stagingArea;
    public string stagingAreaData;
    public string stagingAreaDataManaged;
    public string playerPackage;
    public string installPath;
    public string companyName;
    public string productName;
    public Guid productGUID;
    public BuildOptions options;
    public BuildReport report;
    internal RuntimeClassRegistry usedClassRegistry;
  }
}
