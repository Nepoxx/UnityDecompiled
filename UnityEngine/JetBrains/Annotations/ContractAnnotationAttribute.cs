// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ContractAnnotationAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public sealed class ContractAnnotationAttribute : Attribute
  {
    public ContractAnnotationAttribute([NotNull] string contract)
      : this(contract, false)
    {
    }

    public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
    {
      this.Contract = contract;
      this.ForceFullStates = forceFullStates;
    }

    public string Contract { get; private set; }

    public bool ForceFullStates { get; private set; }
  }
}
