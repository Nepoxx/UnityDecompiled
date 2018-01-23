// Decompiled with JetBrains decompiler
// Type: UnityEngine.AtomicSafetyHandleVersionMask
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  internal enum AtomicSafetyHandleVersionMask
  {
    Read = 1,
    Write = 2,
    Dispose = 4,
    ReadAndWrite = Write | Read, // 0x00000003
    ReadWriteAndDispose = ReadAndWrite | Dispose, // 0x00000007
    WriteInv = -3,
    ReadInv = -2,
    ReadAndWriteInv = -4,
    ReadWriteAndDisposeInv = -8,
  }
}
