// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ListPool`1
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
  internal static class ListPool<T>
  {
    private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>((UnityAction<List<T>>) null, (UnityAction<List<T>>) (l => l.Clear()));

    public static List<T> Get()
    {
      return ListPool<T>.s_ListPool.Get();
    }

    public static void Release(List<T> toRelease)
    {
      ListPool<T>.s_ListPool.Release(toRelease);
    }
  }
}
