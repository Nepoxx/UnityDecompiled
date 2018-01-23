// Decompiled with JetBrains decompiler
// Type: UnityEngine.WebCamDevice
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct WebCamDevice
  {
    internal string m_Name;
    internal int m_Flags;

    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    public bool isFrontFacing
    {
      get
      {
        return (this.m_Flags & 1) == 1;
      }
    }
  }
}
