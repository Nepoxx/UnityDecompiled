// Decompiled with JetBrains decompiler
// Type: UnityEditor.DebugUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  internal class DebugUtils
  {
    internal static string ListToString<T>(IEnumerable<T> list)
    {
      if (list == null)
        return "[null list]";
      string str1 = "[";
      int num = 0;
      foreach (T obj in list)
      {
        if (num != 0)
          str1 += ", ";
        str1 = (object) obj == null ? str1 + "'null'" : str1 + obj.ToString();
        ++num;
      }
      string str2 = str1 + "]";
      if (num == 0)
        return "[empty list]";
      return "(" + (object) num + ") " + str2;
    }
  }
}
