// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkEventType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Event that is returned when calling the Networking.NetworkTransport.Receive and Networking.NetworkTransport.ReceiveFromHost functions.</para>
  /// </summary>
  public enum NetworkEventType
  {
    DataEvent,
    ConnectEvent,
    DisconnectEvent,
    Nothing,
    BroadcastEvent,
  }
}
