// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShadowInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ShadowInfo
  {
    [SerializeField]
    private float m_Latitude = 60f;
    [SerializeField]
    private float m_Longitude = 299f;
    [SerializeField]
    private float m_ShadowIntensity = 1f;
    [SerializeField]
    private Color m_ShadowColor = Color.white;

    public float shadowIntensity
    {
      get
      {
        return this.m_ShadowIntensity;
      }
      set
      {
        this.m_ShadowIntensity = value;
      }
    }

    public Color shadowColor
    {
      get
      {
        return this.m_ShadowColor;
      }
      set
      {
        this.m_ShadowColor = value;
      }
    }

    public float latitude
    {
      get
      {
        return this.m_Latitude;
      }
      set
      {
        this.m_Latitude = value;
        this.ConformLatLong();
      }
    }

    public float longitude
    {
      get
      {
        return this.m_Longitude;
      }
      set
      {
        this.m_Longitude = value;
        this.ConformLatLong();
      }
    }

    private void ConformLatLong()
    {
      if ((double) this.m_Latitude < -90.0)
        this.m_Latitude = -90f;
      if ((double) this.m_Latitude > 89.0)
        this.m_Latitude = 89f;
      this.m_Longitude %= 360f;
      if ((double) this.m_Longitude >= 0.0)
        return;
      this.m_Longitude = 360f + this.m_Longitude;
    }
  }
}
