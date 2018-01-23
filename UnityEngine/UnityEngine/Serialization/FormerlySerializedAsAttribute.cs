// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.FormerlySerializedAsAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Serialization
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
  [RequiredByNativeCode]
  public class FormerlySerializedAsAttribute : Attribute
  {
    private string m_oldName;

    public FormerlySerializedAsAttribute(string oldName)
    {
      this.m_oldName = oldName;
    }

    public string oldName
    {
      get
      {
        return this.m_oldName;
      }
    }
  }
}
