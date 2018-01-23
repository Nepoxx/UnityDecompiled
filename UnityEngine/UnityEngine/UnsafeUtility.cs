// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnsafeUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Collections;

namespace UnityEngine
{
  internal static class UnsafeUtility
  {
    public static unsafe void CopyPtrToStructure<T>(IntPtr ptr, out T output) where T : struct
    {
      output = *(T*) ptr;
    }

    public static unsafe void CopyStructureToPtr<T>(ref T output, IntPtr ptr) where T : struct
    {
      *(T*) ptr = output;
    }

    public static unsafe T ReadArrayElement<T>(IntPtr source, int index)
    {
      return *(T*) (source + index * sizeof (T));
    }

    public static unsafe T ReadArrayElementWithStride<T>(IntPtr source, int index, int stride)
    {
      return *(T*) (source + index * stride);
    }

    public static unsafe void WriteArrayElement<T>(IntPtr destination, int index, T value)
    {
      *(T*) (destination + index * sizeof (T)) = value;
    }

    public static unsafe void WriteArrayElementWithStride<T>(IntPtr destination, int index, int stride, T value)
    {
      *(T*) (destination + index * stride) = value;
    }

    public static IntPtr AddressOf<T>(ref T output) where T : struct
    {
      // ISSUE: explicit reference operation
      return (IntPtr) @output;
    }

    public static int SizeOf<T>() where T : struct
    {
      return sizeof (T);
    }

    public static int AlignOf<T>() where T : struct
    {
      return 4;
    }

    public static int OffsetOf<T>(string name) where T : struct
    {
      return (int) Marshal.OffsetOf(typeof (T), name);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern IntPtr Malloc(int size, int alignment, Allocator label);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Free(IntPtr memory, Allocator label);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MemCpy(IntPtr destination, IntPtr source, int size);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MemMove(IntPtr destination, IntPtr source, int size);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MemClear(IntPtr destination, int size);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int SizeOfStruct(System.Type type);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LogError(string msg, string filename, int linenumber);
  }
}
