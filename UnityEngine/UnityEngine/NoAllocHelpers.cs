// Decompiled with JetBrains decompiler
// Type: UnityEngine.NoAllocHelpers
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class NoAllocHelpers
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResizeList(object list, int size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Array ExtractArrayFromList(object list);

    public static int SafeLength(Array values)
    {
      return values == null ? 0 : values.Length;
    }

    public static int SafeLength<T>(List<T> values)
    {
      return values == null ? 0 : values.Count;
    }
  }
}
