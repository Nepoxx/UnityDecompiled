// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibraryHelpers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class PresetLibraryHelpers
  {
    public static void MoveListItem<T>(List<T> list, int index, int destIndex, bool insertAfterDestIndex)
    {
      if (index < 0 || destIndex < 0)
      {
        Debug.LogError((object) "Invalid preset move");
      }
      else
      {
        if (index == destIndex)
          return;
        if (destIndex > index)
          --destIndex;
        if (insertAfterDestIndex && destIndex < list.Count - 1)
          ++destIndex;
        T obj = list[index];
        list.RemoveAt(index);
        list.Insert(destIndex, obj);
      }
    }
  }
}
