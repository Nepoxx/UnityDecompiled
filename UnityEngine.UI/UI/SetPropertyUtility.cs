// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.SetPropertyUtility
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.UI
{
  internal static class SetPropertyUtility
  {
    public static bool SetColor(ref Color currentValue, Color newValue)
    {
      if ((double) currentValue.r == (double) newValue.r && (double) currentValue.g == (double) newValue.g && ((double) currentValue.b == (double) newValue.b && (double) currentValue.a == (double) newValue.a))
        return false;
      currentValue = newValue;
      return true;
    }

    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
    {
      if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
        return false;
      currentValue = newValue;
      return true;
    }

    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
    {
      if ((object) currentValue == null && (object) newValue == null || (object) currentValue != null && currentValue.Equals((object) newValue))
        return false;
      currentValue = newValue;
      return true;
    }
  }
}
