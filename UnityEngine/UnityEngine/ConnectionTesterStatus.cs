// Decompiled with JetBrains decompiler
// Type: UnityEngine.ConnectionTesterStatus
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The various test results the connection tester may return with.</para>
  /// </summary>
  public enum ConnectionTesterStatus
  {
    Error = -2,
    Undetermined = -1,
    [Obsolete("No longer returned, use newer connection tester enums instead.")] PrivateIPNoNATPunchthrough = 0,
    [Obsolete("No longer returned, use newer connection tester enums instead.")] PrivateIPHasNATPunchThrough = 1,
    PublicIPIsConnectable = 2,
    PublicIPPortBlocked = 3,
    PublicIPNoServerStarted = 4,
    LimitedNATPunchthroughPortRestricted = 5,
    LimitedNATPunchthroughSymmetric = 6,
    NATpunchthroughFullCone = 7,
    NATpunchthroughAddressRestrictedCone = 8,
  }
}
