// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collections.NativeSliceExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Collections
{
  public static class NativeSliceExtensions
  {
    public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray) where T : struct
    {
      return new NativeSlice<T>(thisArray);
    }

    public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int length) where T : struct
    {
      return new NativeSlice<T>(thisArray, length);
    }

    public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int start, int length) where T : struct
    {
      return new NativeSlice<T>(thisArray, start, length);
    }
  }
}
