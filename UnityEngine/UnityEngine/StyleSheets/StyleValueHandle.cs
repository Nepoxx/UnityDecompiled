// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleValueHandle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal struct StyleValueHandle
  {
    [SerializeField]
    private StyleValueType m_ValueType;
    [SerializeField]
    internal int valueIndex;

    internal StyleValueHandle(int valueIndex, StyleValueType valueType)
    {
      this.valueIndex = valueIndex;
      this.m_ValueType = valueType;
    }

    public StyleValueType valueType
    {
      get
      {
        return this.m_ValueType;
      }
      internal set
      {
        this.m_ValueType = value;
      }
    }
  }
}
