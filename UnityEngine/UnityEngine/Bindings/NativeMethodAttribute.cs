// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeMethodAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
  internal class NativeMethodAttribute : Attribute, IBindingsNameProviderAttribute, IBindingsIsThreadSafeProviderAttribute, IBindingsIsFreeFunctionProviderAttribute, IBindingsThrowsProviderAttribute, IBindingsAttribute
  {
    public NativeMethodAttribute()
    {
    }

    public NativeMethodAttribute(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (name == "")
        throw new ArgumentException("name cannot be empty", nameof (name));
      this.Name = name;
    }

    public NativeMethodAttribute(string name, bool isFreeFunction)
      : this(name)
    {
      this.IsFreeFunction = isFreeFunction;
    }

    public NativeMethodAttribute(string name, bool isFreeFunction, bool isThreadSafe)
      : this(name, isFreeFunction)
    {
      this.IsThreadSafe = isThreadSafe;
    }

    public NativeMethodAttribute(string name, bool isFreeFunction, bool isThreadSafe, bool throws)
      : this(name, isFreeFunction, isThreadSafe)
    {
      this.ThrowsException = throws;
    }

    public string Name { get; set; }

    public bool IsThreadSafe { get; set; }

    public bool IsFreeFunction { get; set; }

    public bool ThrowsException { get; set; }

    public bool HasExplicitThis { get; set; }

    public bool WritableSelf { get; set; }
  }
}
