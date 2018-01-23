// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Types.NetworkAccessLevel
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
  /// <summary>
  ///   <para>Describes the access levels granted to this client.</para>
  /// </summary>
  [DefaultValue(NetworkAccessLevel.Invalid)]
  public enum NetworkAccessLevel : ulong
  {
    Invalid = 0,
    User = 1,
    Owner = 2,
    Admin = 4,
  }
}
