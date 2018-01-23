// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class LookDevContext
  {
    [SerializeField]
    private LookDevContext.LookDevPropertyValue[] m_Properties = new LookDevContext.LookDevPropertyValue[5];

    public LookDevContext()
    {
      for (int index = 0; index < 5; ++index)
        this.m_Properties[index] = new LookDevContext.LookDevPropertyValue();
      this.m_Properties[0].floatValue = 0.0f;
      this.m_Properties[1].intValue = 0;
      this.m_Properties[2].intValue = -1;
      this.m_Properties[4].intValue = -1;
      this.m_Properties[3].floatValue = 0.0f;
    }

    public float exposureValue
    {
      get
      {
        return this.m_Properties[0].floatValue;
      }
    }

    public float envRotation
    {
      get
      {
        return this.m_Properties[3].floatValue;
      }
      set
      {
        this.m_Properties[3].floatValue = value;
      }
    }

    public int currentHDRIIndex
    {
      get
      {
        return this.m_Properties[1].intValue;
      }
      set
      {
        this.m_Properties[1].intValue = value;
      }
    }

    public int shadingMode
    {
      get
      {
        return this.m_Properties[2].intValue;
      }
    }

    public int lodIndex
    {
      get
      {
        return this.m_Properties[4].intValue;
      }
    }

    public LookDevContext.LookDevPropertyValue GetProperty(LookDevProperty property)
    {
      return this.m_Properties[(int) property];
    }

    public void UpdateProperty(LookDevProperty property, float value)
    {
      this.m_Properties[(int) property].floatValue = value;
    }

    public void UpdateProperty(LookDevProperty property, int value)
    {
      this.m_Properties[(int) property].intValue = value;
    }

    [Serializable]
    public class LookDevPropertyValue
    {
      public float floatValue = 0.0f;
      public int intValue = 0;
    }
  }
}
