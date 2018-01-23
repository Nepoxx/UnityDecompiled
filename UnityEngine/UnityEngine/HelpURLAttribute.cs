// Decompiled with JetBrains decompiler
// Type: UnityEngine.HelpURLAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class HelpURLAttribute : Attribute
  {
    internal readonly string m_Url;

    public HelpURLAttribute(string url)
    {
      this.m_Url = url;
    }

    public string URL
    {
      get
      {
        return this.m_Url;
      }
    }
  }
}
