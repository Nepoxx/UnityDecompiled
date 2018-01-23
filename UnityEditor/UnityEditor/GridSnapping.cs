// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridSnapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal static class GridSnapping
  {
    public static Func<Vector3, Vector3> snapPosition;
    public static Func<bool> activeFunc;

    public static bool active
    {
      get
      {
        return GridSnapping.activeFunc != null && GridSnapping.activeFunc();
      }
    }

    public static Vector3 Snap(Vector3 position)
    {
      if (GridSnapping.snapPosition != null)
        return GridSnapping.snapPosition(position);
      return position;
    }
  }
}
