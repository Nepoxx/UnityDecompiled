// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.DopeLine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class DopeLine
  {
    public static GUIStyle dopekeyStyle = (GUIStyle) "Dopesheetkeyframe";
    private int m_HierarchyNodeID;
    private AnimationWindowCurve[] m_Curves;
    private List<AnimationWindowKeyframe> m_Keys;
    public Rect position;
    public System.Type objectType;
    public bool tallMode;
    public bool hasChildren;
    public bool isMasterDopeline;

    public DopeLine(int hierarchyNodeID, AnimationWindowCurve[] curves)
    {
      this.m_HierarchyNodeID = hierarchyNodeID;
      this.m_Curves = curves;
    }

    public System.Type valueType
    {
      get
      {
        if (this.m_Curves.Length <= 0)
          return (System.Type) null;
        System.Type valueType = this.m_Curves[0].valueType;
        for (int index = 1; index < this.m_Curves.Length; ++index)
        {
          if (this.m_Curves[index].valueType != valueType)
            return (System.Type) null;
        }
        return valueType;
      }
    }

    public bool isPptrDopeline
    {
      get
      {
        if (this.m_Curves.Length <= 0)
          return false;
        for (int index = 0; index < this.m_Curves.Length; ++index)
        {
          if (!this.m_Curves[index].isPPtrCurve)
            return false;
        }
        return true;
      }
    }

    public bool isEditable
    {
      get
      {
        if (this.m_Curves.Length > 0)
          return !Array.Exists<AnimationWindowCurve>(this.m_Curves, (Predicate<AnimationWindowCurve>) (curve => !curve.animationIsEditable));
        return false;
      }
    }

    public int hierarchyNodeID
    {
      get
      {
        return this.m_HierarchyNodeID;
      }
    }

    public AnimationWindowCurve[] curves
    {
      get
      {
        return this.m_Curves;
      }
    }

    public List<AnimationWindowKeyframe> keys
    {
      get
      {
        if (this.m_Keys == null)
        {
          this.m_Keys = new List<AnimationWindowKeyframe>();
          foreach (AnimationWindowCurve curve in this.m_Curves)
          {
            foreach (AnimationWindowKeyframe keyframe in curve.m_Keyframes)
              this.m_Keys.Add(keyframe);
          }
          this.m_Keys.Sort((Comparison<AnimationWindowKeyframe>) ((a, b) => a.time.CompareTo(b.time)));
        }
        return this.m_Keys;
      }
    }

    public void InvalidateKeyframes()
    {
      this.m_Keys = (List<AnimationWindowKeyframe>) null;
    }
  }
}
