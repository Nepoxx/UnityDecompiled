// Decompiled with JetBrains decompiler
// Type: UnityEngine.PropertyNameUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
  internal class PropertyNameUtils
  {
    public static PropertyName PropertyNameFromString([Unmarshalled] string name)
    {
      PropertyName ret;
      PropertyNameUtils.PropertyNameFromString_Injected(name, out ret);
      return ret;
    }

    public static string StringFromPropertyName(PropertyName propertyName)
    {
      return PropertyNameUtils.StringFromPropertyName_Injected(ref propertyName);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ConflictCountForID(int id);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void PropertyNameFromString_Injected(string name, out PropertyName ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string StringFromPropertyName_Injected(ref PropertyName propertyName);
  }
}
