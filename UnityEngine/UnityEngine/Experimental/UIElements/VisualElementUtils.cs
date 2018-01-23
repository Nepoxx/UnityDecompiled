// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualElementUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  internal sealed class VisualElementUtils
  {
    private static readonly HashSet<string> s_usedNames = new HashSet<string>();

    public static string GetUniqueName(string nameBase)
    {
      string str = nameBase;
      int num = 2;
      while (VisualElementUtils.s_usedNames.Contains(str))
      {
        str = nameBase + (object) num;
        ++num;
      }
      VisualElementUtils.s_usedNames.Add(str);
      return str;
    }
  }
}
