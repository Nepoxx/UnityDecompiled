// Decompiled with JetBrains decompiler
// Type: UnityEngine.EnumInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  internal class EnumInfo
  {
    public string[] names;
    public int[] values;
    public string[] annotations;
    public bool isFlags;

    [UsedByNativeCode]
    internal static EnumInfo CreateEnumInfoFromNativeEnum(string[] names, int[] values, string[] annotations, bool isFlags)
    {
      return new EnumInfo() { names = names, values = values, annotations = annotations, isFlags = isFlags };
    }
  }
}
