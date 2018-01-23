// Decompiled with JetBrains decompiler
// Type: UnityEngine.ManagedStreamHelpers
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.IO;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal static class ManagedStreamHelpers
  {
    internal static void ValidateLoadFromStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("ManagedStream object must be non-null", nameof (stream));
      if (!stream.CanRead)
        throw new ArgumentException("ManagedStream object must be readable (stream.CanRead must return true)", nameof (stream));
      if (!stream.CanSeek)
        throw new ArgumentException("ManagedStream object must be seekable (stream.CanSeek must return true)", nameof (stream));
    }

    [RequiredByNativeCode]
    internal static unsafe void ManagedStreamRead(byte[] buffer, int offset, int count, Stream stream, IntPtr returnValueAddress)
    {
      if (returnValueAddress == IntPtr.Zero)
        throw new ArgumentException("Return value address cannot be 0.", nameof (returnValueAddress));
      ManagedStreamHelpers.ValidateLoadFromStream(stream);
      *(int*) (void*) returnValueAddress = stream.Read(buffer, offset, count);
    }

    [RequiredByNativeCode]
    internal static unsafe void ManagedStreamSeek(long offset, uint origin, Stream stream, IntPtr returnValueAddress)
    {
      if (returnValueAddress == IntPtr.Zero)
        throw new ArgumentException("Return value address cannot be 0.", nameof (returnValueAddress));
      ManagedStreamHelpers.ValidateLoadFromStream(stream);
      *(long*) (void*) returnValueAddress = stream.Seek(offset, (SeekOrigin) origin);
    }

    [RequiredByNativeCode]
    internal static unsafe void ManagedStreamLength(Stream stream, IntPtr returnValueAddress)
    {
      if (returnValueAddress == IntPtr.Zero)
        throw new ArgumentException("Return value address cannot be 0.", nameof (returnValueAddress));
      ManagedStreamHelpers.ValidateLoadFromStream(stream);
      *(long*) (void*) returnValueAddress = stream.Length;
    }
  }
}
