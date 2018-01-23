// Decompiled with JetBrains decompiler
// Type: UnityEngine.RequireComponent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public sealed class RequireComponent : Attribute
  {
    public System.Type m_Type0;
    public System.Type m_Type1;
    public System.Type m_Type2;

    public RequireComponent(System.Type requiredComponent)
    {
      this.m_Type0 = requiredComponent;
    }

    public RequireComponent(System.Type requiredComponent, System.Type requiredComponent2)
    {
      this.m_Type0 = requiredComponent;
      this.m_Type1 = requiredComponent2;
    }

    public RequireComponent(System.Type requiredComponent, System.Type requiredComponent2, System.Type requiredComponent3)
    {
      this.m_Type0 = requiredComponent;
      this.m_Type1 = requiredComponent2;
      this.m_Type2 = requiredComponent3;
    }
  }
}
