// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collections.NativeArray`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace UnityEngine.Collections
{
  [DebuggerTypeProxy(typeof (NativeArrayDebugView<>))]
  [NativeContainerSupportsDeallocateOnJobCompletion]
  [DebuggerDisplay("Length = {Length}")]
  [NativeContainerSupportsMinMaxWriteRestriction]
  [NativeContainer]
  public struct NativeArray<T> : IDisposable, IEnumerable<T>, IEnumerable where T : struct
  {
    internal IntPtr m_Buffer;
    internal int m_Length;
    internal int m_MinIndex;
    internal int m_MaxIndex;
    internal AtomicSafetyHandle m_Safety;
    internal DisposeSentinel m_DisposeSentinel;
    private Allocator m_AllocatorLabel;

    public NativeArray(int length, Allocator allocMode)
    {
      NativeArray<T>.Allocate(length, allocMode, out this);
      UnsafeUtility.MemClear(this.m_Buffer, this.Length * UnsafeUtility.SizeOf<T>());
    }

    public NativeArray(T[] array, Allocator allocMode)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      NativeArray<T>.Allocate(array.Length, allocMode, out this);
      this.CopyFrom(array);
    }

    public NativeArray(NativeArray<T> array, Allocator allocMode)
    {
      NativeArray<T>.Allocate(array.Length, allocMode, out this);
      this.CopyFrom(array);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static NativeArray<T> ConvertExistingDataToNativeArrayInternal(IntPtr dataPointer, int length, AtomicSafetyHandle safety, Allocator allocMode)
    {
      return new NativeArray<T>() { m_Buffer = dataPointer, m_Length = length, m_AllocatorLabel = allocMode, m_MinIndex = 0, m_MaxIndex = length - 1, m_Safety = safety, m_DisposeSentinel = (DisposeSentinel) null };
    }

    private static void Allocate(int length, Allocator allocator, out NativeArray<T> array)
    {
      if (allocator <= Allocator.None)
        throw new ArgumentOutOfRangeException("allocMode", "Allocator must be Temp, Job or Persistent");
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), "Length must be >= 0");
      long num = (long) UnsafeUtility.SizeOf<T>() * (long) length;
      if (num > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (length), "Length * sizeof(T) cannot exceed " + (object) int.MaxValue + "bytes");
      array.m_Buffer = UnsafeUtility.Malloc((int) num, UnsafeUtility.AlignOf<T>(), allocator);
      array.m_Length = length;
      array.m_AllocatorLabel = allocator;
      array.m_MinIndex = 0;
      array.m_MaxIndex = length - 1;
      DisposeSentinel.Create(array.m_Buffer, allocator, out array.m_Safety, out array.m_DisposeSentinel, 1, (DisposeSentinel.DeallocateDelegate) null);
    }

    public int Length
    {
      get
      {
        return this.m_Length;
      }
    }

    public unsafe T this[int index]
    {
      get
      {
        if (this.m_Buffer == IntPtr.Zero)
          throw new InvalidOperationException("NativeArray not properly initialized");
        if ((this.m_Safety.version & AtomicSafetyHandleVersionMask.Read) == ~(AtomicSafetyHandleVersionMask.WriteInv | AtomicSafetyHandleVersionMask.Write) && this.m_Safety.version != (*(AtomicSafetyHandleVersionMask*) (void*) this.m_Safety.versionNode & AtomicSafetyHandleVersionMask.WriteInv))
          AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(this.m_Safety);
        if (index < this.m_MinIndex || index > this.m_MaxIndex)
          this.FailOutOfRangeError(index);
        return UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, index);
      }
      set
      {
        if (this.m_Buffer == IntPtr.Zero)
          throw new InvalidOperationException("NativeArray not properly initialized");
        if ((this.m_Safety.version & AtomicSafetyHandleVersionMask.Write) == ~(AtomicSafetyHandleVersionMask.WriteInv | AtomicSafetyHandleVersionMask.Write) && this.m_Safety.version != (*(AtomicSafetyHandleVersionMask*) (void*) this.m_Safety.versionNode & AtomicSafetyHandleVersionMask.ReadInv))
          AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut(this.m_Safety);
        if (index < this.m_MinIndex || index > this.m_MaxIndex)
          this.FailOutOfRangeError(index);
        UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, index, value);
      }
    }

    public bool IsCreated
    {
      get
      {
        return this.m_Buffer != IntPtr.Zero;
      }
    }

    public void Dispose()
    {
      DisposeSentinel.Dispose(this.m_Safety, ref this.m_DisposeSentinel);
      UnsafeUtility.Free(this.m_Buffer, this.m_AllocatorLabel);
      this.m_Buffer = IntPtr.Zero;
      this.m_Length = 0;
    }

    public IntPtr UnsafePtr
    {
      get
      {
        AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
        return this.m_Buffer;
      }
    }

    public IntPtr UnsafeReadOnlyPtr
    {
      get
      {
        AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
        return this.m_Buffer;
      }
    }

    internal void GetUnsafeBufferPointerWithoutChecksInternal(out AtomicSafetyHandle handle, out IntPtr ptr)
    {
      ptr = this.m_Buffer;
      handle = this.m_Safety;
    }

    public void CopyFrom(T[] array)
    {
      AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
      if (this.Length != array.Length)
        throw new ArgumentException("Array length does not match the length of this instance");
      for (int index = 0; index < this.Length; ++index)
        UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, index, array[index]);
    }

    public void CopyFrom(NativeArray<T> array)
    {
      array.CopyTo(this);
    }

    public void CopyTo(T[] array)
    {
      AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
      if (this.Length != array.Length)
        throw new ArgumentException("Array length does not match the length of this instance");
      for (int index = 0; index < this.Length; ++index)
        array[index] = UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, index);
    }

    public void CopyTo(NativeArray<T> array)
    {
      AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
      AtomicSafetyHandle.CheckWriteAndThrow(array.m_Safety);
      if (this.Length != array.Length)
        throw new ArgumentException("Array length does not match the length of this instance");
      UnsafeUtility.MemCpy(array.m_Buffer, this.m_Buffer, this.Length * UnsafeUtility.SizeOf<T>());
    }

    public T[] ToArray()
    {
      T[] array = new T[this.Length];
      this.CopyTo(array);
      return array;
    }

    private void FailOutOfRangeError(int index)
    {
      if (index < this.Length && (this.m_MinIndex != 0 || this.m_MaxIndex != this.Length - 1))
        throw new IndexOutOfRangeException(string.Format("Index {0} is out of restricted IJobParallelFor range [{1}...{2}] in ReadWriteBuffer.\nReadWriteBuffers are restricted to only read & write the element at the job index. You can use double buffering strategies to avoid race conditions due to reading & writing in parallel to the same elements from a job.", (object) index, (object) this.m_MinIndex, (object) this.m_MaxIndex));
      throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", (object) index, (object) this.Length));
    }

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) new NativeArray<T>.Enumerator(ref this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
      private NativeArray<T> array;
      private int index;

      public Enumerator(ref NativeArray<T> array)
      {
        this.array = array;
        this.index = -1;
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        ++this.index;
        return this.index < this.array.Length;
      }

      public void Reset()
      {
        this.index = -1;
      }

      public T Current
      {
        get
        {
          return this.array[this.index];
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }
    }
  }
}
