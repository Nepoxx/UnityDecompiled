// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.DepthState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
  public struct DepthState
  {
    private byte m_WriteEnabled;
    private sbyte m_CompareFunction;

    public DepthState(bool writeEnabled = true, CompareFunction compareFunction = CompareFunction.Less)
    {
      this.m_WriteEnabled = Convert.ToByte(writeEnabled);
      this.m_CompareFunction = (sbyte) compareFunction;
    }

    public static DepthState Default
    {
      get
      {
        return new DepthState(true, CompareFunction.Less);
      }
    }

    public bool writeEnabled
    {
      get
      {
        return Convert.ToBoolean(this.m_WriteEnabled);
      }
      set
      {
        this.m_WriteEnabled = Convert.ToByte(value);
      }
    }

    public CompareFunction compareFunction
    {
      get
      {
        return (CompareFunction) this.m_CompareFunction;
      }
      set
      {
        this.m_CompareFunction = (sbyte) value;
      }
    }
  }
}
