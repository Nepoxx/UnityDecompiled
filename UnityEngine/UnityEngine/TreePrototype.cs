// Decompiled with JetBrains decompiler
// Type: UnityEngine.TreePrototype
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class TreePrototype
  {
    internal GameObject m_Prefab;
    internal float m_BendFactor;

    public GameObject prefab
    {
      get
      {
        return this.m_Prefab;
      }
      set
      {
        this.m_Prefab = value;
      }
    }

    public float bendFactor
    {
      get
      {
        return this.m_BendFactor;
      }
      set
      {
        this.m_BendFactor = value;
      }
    }
  }
}
