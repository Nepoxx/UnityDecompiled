// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Types.NetworkID
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
  /// <summary>
  ///   <para>Network ID, used for match making.</para>
  /// </summary>
  [DefaultValue(NetworkID.Invalid)]
  public enum NetworkID : ulong
  {
    Invalid = 18446744073709551615, // 0xFFFFFFFFFFFFFFFF
  }
}
