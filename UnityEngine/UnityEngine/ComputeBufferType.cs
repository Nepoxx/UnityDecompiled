// Decompiled with JetBrains decompiler
// Type: UnityEngine.ComputeBufferType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum ComputeBufferType
  {
    Default = 0,
    Raw = 1,
    Append = 2,
    Counter = 4,
    [Obsolete("Enum member DrawIndirect has been deprecated. Use IndirectArguments instead (UnityUpgradable) -> IndirectArguments", false)] DrawIndirect = 256, // 0x00000100
    IndirectArguments = DrawIndirect, // 0x00000100
    [Obsolete("Enum member GPUMemory has been deprecated. All compute buffers now follow the behavior previously defined by this member.", false)] GPUMemory = 512, // 0x00000200
  }
}
