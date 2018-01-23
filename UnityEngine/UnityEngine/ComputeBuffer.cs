// Decompiled with JetBrains decompiler
// Type: UnityEngine.ComputeBuffer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>GPU data buffer, mostly for use with compute shaders.</para>
  /// </summary>
  public sealed class ComputeBuffer : IDisposable
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Create a Compute Buffer.</para>
    /// </summary>
    /// <param name="count">Number of elements in the buffer.</param>
    /// <param name="stride">Size of one element in the buffer. Has to match size of buffer type in the shader. See for cross-platform compatibility information.</param>
    /// <param name="type">Type of the buffer, default is ComputeBufferType.Default (structured buffer).</param>
    public ComputeBuffer(int count, int stride)
      : this(count, stride, ComputeBufferType.Default, 3)
    {
    }

    /// <summary>
    ///   <para>Create a Compute Buffer.</para>
    /// </summary>
    /// <param name="count">Number of elements in the buffer.</param>
    /// <param name="stride">Size of one element in the buffer. Has to match size of buffer type in the shader. See for cross-platform compatibility information.</param>
    /// <param name="type">Type of the buffer, default is ComputeBufferType.Default (structured buffer).</param>
    public ComputeBuffer(int count, int stride, ComputeBufferType type)
      : this(count, stride, type, 3)
    {
    }

    internal ComputeBuffer(int count, int stride, ComputeBufferType type, int stackDepth)
    {
      if (count <= 0)
        throw new ArgumentException("Attempting to create a zero length compute buffer", nameof (count));
      if (stride < 0)
        throw new ArgumentException("Attempting to create a compute buffer with a negative stride", nameof (stride));
      this.m_Ptr = IntPtr.Zero;
      ComputeBuffer.InitBuffer(this, count, stride, type);
      this.SaveCallstack(stackDepth);
    }

    ~ComputeBuffer()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (disposing)
        ComputeBuffer.DestroyBuffer(this);
      else if (this.m_Ptr != IntPtr.Zero)
        Debug.LogWarning((object) string.Format("GarbageCollector disposing of ComputeBuffer allocated in {0} at line {1}. Please use ComputeBuffer.Release() or .Dispose() to manually release the buffer.", (object) this.GetFileName(), (object) this.GetLineNumber()));
      this.m_Ptr = IntPtr.Zero;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InitBuffer(ComputeBuffer buf, int count, int stride, ComputeBufferType type);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DestroyBuffer(ComputeBuffer buf);

    /// <summary>
    ///   <para>Release a Compute Buffer.</para>
    /// </summary>
    public void Release()
    {
      this.Dispose();
    }

    /// <summary>
    ///   <para>Number of elements in the buffer (Read Only).</para>
    /// </summary>
    public extern int count { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Size of one element in the buffer (Read Only).</para>
    /// </summary>
    public extern int stride { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Set the buffer with values from an array.</para>
    /// </summary>
    /// <param name="data">Array of values to fill the buffer.</param>
    [SecuritySafeCritical]
    public void SetData(Array data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      this.InternalSetData(data, 0, 0, data.Length, Marshal.SizeOf(data.GetType().GetElementType()));
    }

    [SecuritySafeCritical]
    public void SetData<T>(List<T> data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      this.InternalSetData(NoAllocHelpers.ExtractArrayFromList((object) data), 0, 0, NoAllocHelpers.SafeLength<T>(data), Marshal.SizeOf(typeof (T)));
    }

    /// <summary>
    ///   <para>Partial copy of data values from an array into the buffer.</para>
    /// </summary>
    /// <param name="data">Array of values to fill the buffer.</param>
    /// <param name="managedBufferStartIndex">The first element index in data to copy to the compute buffer.</param>
    /// <param name="computeBufferStartIndex">The first element index in compute buffer to receive the data.</param>
    /// <param name="count">The number of elements to copy.</param>
    [SecuritySafeCritical]
    public void SetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || (count < 0 || managedBufferStartIndex + count > data.Length))
        throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", (object) managedBufferStartIndex, (object) computeBufferStartIndex, (object) count));
      this.InternalSetData(data, managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
    }

    [SecuritySafeCritical]
    public void SetData<T>(List<T> data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || (count < 0 || managedBufferStartIndex + count > data.Count))
        throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", (object) managedBufferStartIndex, (object) computeBufferStartIndex, (object) count));
      this.InternalSetData(NoAllocHelpers.ExtractArrayFromList((object) data), managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(typeof (T)));
    }

    [SecurityCritical]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalSetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count, int elemSize);

    /// <summary>
    ///   <para>Read data values from the buffer into an array.</para>
    /// </summary>
    /// <param name="data">An array to receive the data.</param>
    [SecurityCritical]
    public void GetData(Array data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      this.InternalGetData(data, 0, 0, data.Length, Marshal.SizeOf(data.GetType().GetElementType()));
    }

    /// <summary>
    ///   <para>Partial read of data values from the buffer into an array.</para>
    /// </summary>
    /// <param name="data">An array to receive the data.</param>
    /// <param name="managedBufferStartIndex">The first element index in data where retrieved elements are copied.</param>
    /// <param name="computeBufferStartIndex">The first element index of the compute buffer from which elements are read.</param>
    /// <param name="count">The number of elements to retrieve.</param>
    [SecurityCritical]
    public void GetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || (count < 0 || managedBufferStartIndex + count > data.Length))
        throw new ArgumentOutOfRangeException(string.Format("Bad indices/count argument (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", (object) managedBufferStartIndex, (object) computeBufferStartIndex, (object) count));
      this.InternalGetData(data, managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
    }

    [SecurityCritical]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalGetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count, int elemSize);

    /// <summary>
    ///   <para>Sets counter value of append/consume buffer.</para>
    /// </summary>
    /// <param name="counterValue">Value of the append/consume counter.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetCounterValue(uint counterValue);

    /// <summary>
    ///   <para>Copy counter value of append/consume buffer into another buffer.</para>
    /// </summary>
    /// <param name="src">Append/consume buffer to copy the counter from.</param>
    /// <param name="dst">A buffer to copy the counter to.</param>
    /// <param name="dstOffsetBytes">Target byte offset in dst.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffsetBytes);

    /// <summary>
    ///   <para>Retrieve a native (underlying graphics API) pointer to the buffer.</para>
    /// </summary>
    /// <returns>
    ///   <para>Pointer to the underlying graphics API buffer.</para>
    /// </returns>
    public IntPtr GetNativeBufferPtr()
    {
      IntPtr num;
      ComputeBuffer.INTERNAL_CALL_GetNativeBufferPtr(this, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetNativeBufferPtr(ComputeBuffer self, out IntPtr value);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetFileName();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int GetLineNumber();

    internal void SaveCallstack(int stackDepth)
    {
      StackFrame stackFrame = new StackFrame(stackDepth, true);
      this.SaveCallstack_Internal(stackFrame.GetFileName(), stackFrame.GetFileLineNumber());
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SaveCallstack_Internal(string fileName, int lineNumber);
  }
}
