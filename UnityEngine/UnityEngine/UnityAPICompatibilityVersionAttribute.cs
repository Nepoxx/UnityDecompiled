// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnityAPICompatibilityVersionAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public class UnityAPICompatibilityVersionAttribute : Attribute
  {
    private string _version;

    public UnityAPICompatibilityVersionAttribute(string version)
    {
      this._version = version;
    }

    public string version
    {
      get
      {
        return this._version;
      }
    }
  }
}
