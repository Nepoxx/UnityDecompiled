// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collections.NativeArrayDebugView`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Collections
{
  internal sealed class NativeArrayDebugView<T> where T : struct
  {
    private NativeArray<T> array;

    public NativeArrayDebugView(NativeArray<T> array)
    {
      this.array = array;
    }

    public T[] Items
    {
      get
      {
        return this.array.ToArray();
      }
    }
  }
}
