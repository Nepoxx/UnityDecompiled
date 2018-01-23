// Decompiled with JetBrains decompiler
// Type: UnityEngine.Scripting.APIUpdating.MovedFromAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Scripting.APIUpdating
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate)]
  public class MovedFromAttribute : Attribute
  {
    public MovedFromAttribute(string sourceNamespace)
      : this(sourceNamespace, false)
    {
    }

    public MovedFromAttribute(string sourceNamespace, bool isInDifferentAssembly)
    {
      this.Namespace = sourceNamespace;
      this.IsInDifferentAssembly = isInDifferentAssembly;
    }

    public string Namespace { get; private set; }

    public bool IsInDifferentAssembly { get; private set; }
  }
}
