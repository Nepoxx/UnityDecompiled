// Decompiled with JetBrains decompiler
// Type: UnityEditor.DoubleCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class DoubleCurve
  {
    [SerializeField]
    private AnimationCurve m_MinCurve;
    [SerializeField]
    private AnimationCurve m_MaxCurve;
    [SerializeField]
    private bool m_SignedRange;

    public DoubleCurve(AnimationCurve minCurve, AnimationCurve maxCurve, bool signedRange)
    {
      if (minCurve != null)
        this.m_MinCurve = new AnimationCurve(minCurve.keys);
      if (maxCurve != null)
        this.m_MaxCurve = new AnimationCurve(maxCurve.keys);
      else
        Debug.LogError((object) "Ensure that maxCurve is not null when creating a double curve. The minCurve can be null for single curves");
      this.m_SignedRange = signedRange;
    }

    public AnimationCurve minCurve
    {
      get
      {
        return this.m_MinCurve;
      }
      set
      {
        this.m_MinCurve = value;
      }
    }

    public AnimationCurve maxCurve
    {
      get
      {
        return this.m_MaxCurve;
      }
      set
      {
        this.m_MaxCurve = value;
      }
    }

    public bool signedRange
    {
      get
      {
        return this.m_SignedRange;
      }
      set
      {
        this.m_SignedRange = value;
      }
    }

    public bool IsSingleCurve()
    {
      return this.minCurve == null || this.minCurve.length == 0;
    }
  }
}
