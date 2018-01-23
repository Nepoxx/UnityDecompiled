// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ConnectionAcksType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Defines size of the buffer holding reliable messages, before they will be acknowledged.</para>
  /// </summary>
  public enum ConnectionAcksType
  {
    Acks32 = 1,
    Acks64 = 2,
    Acks96 = 3,
    Acks128 = 4,
  }
}
