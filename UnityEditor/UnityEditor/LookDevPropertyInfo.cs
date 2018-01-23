// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevPropertyInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class LookDevPropertyInfo
  {
    [SerializeField]
    private bool m_Linked = false;
    [SerializeField]
    private LookDevPropertyType m_PropertyType;

    public LookDevPropertyInfo(LookDevPropertyType type)
    {
      this.m_PropertyType = type;
    }

    public LookDevPropertyType propertyType
    {
      get
      {
        return this.m_PropertyType;
      }
    }

    public bool linked
    {
      get
      {
        return this.m_Linked;
      }
      set
      {
        this.m_Linked = value;
      }
    }
  }
}
