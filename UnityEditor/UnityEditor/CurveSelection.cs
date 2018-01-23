// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveSelection : IComparable
  {
    [SerializeField]
    public int curveID = 0;
    [SerializeField]
    public int key = -1;
    [SerializeField]
    public bool semiSelected = false;
    [SerializeField]
    public CurveSelection.SelectionType type;

    internal CurveSelection(int curveID, int key)
    {
      this.curveID = curveID;
      this.key = key;
      this.type = CurveSelection.SelectionType.Key;
    }

    internal CurveSelection(int curveID, int key, CurveSelection.SelectionType type)
    {
      this.curveID = curveID;
      this.key = key;
      this.type = type;
    }

    public int CompareTo(object _other)
    {
      CurveSelection curveSelection = (CurveSelection) _other;
      int num1 = this.curveID - curveSelection.curveID;
      if (num1 != 0)
        return num1;
      int num2 = this.key - curveSelection.key;
      if (num2 != 0)
        return num2;
      return this.type - curveSelection.type;
    }

    public override bool Equals(object _other)
    {
      CurveSelection curveSelection = (CurveSelection) _other;
      return curveSelection.curveID == this.curveID && curveSelection.key == this.key && curveSelection.type == this.type;
    }

    public override int GetHashCode()
    {
      return (int) (this.curveID * 729 + this.key * 27 + this.type);
    }

    internal enum SelectionType
    {
      Key,
      InTangent,
      OutTangent,
      Count,
    }
  }
}
