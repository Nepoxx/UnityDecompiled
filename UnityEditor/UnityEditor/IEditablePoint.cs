// Decompiled with JetBrains decompiler
// Type: UnityEditor.IEditablePoint
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal interface IEditablePoint
  {
    Vector3 GetPosition(int idx);

    void SetPosition(int idx, Vector3 position);

    Color GetDefaultColor();

    Color GetSelectedColor();

    float GetPointScale();

    IEnumerable<Vector3> GetPositions();

    Vector3[] GetUnselectedPositions();

    Vector3[] GetSelectedPositions();

    int Count { get; }
  }
}
