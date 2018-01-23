// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.PlayerConnection.IEditorPlayerConnection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Events;

namespace UnityEngine.Networking.PlayerConnection
{
  internal interface IEditorPlayerConnection
  {
    void Register(Guid messageId, UnityAction<MessageEventArgs> callback);

    void Unregister(Guid messageId, UnityAction<MessageEventArgs> callback);

    void DisconnectAll();

    void RegisterConnection(UnityAction<int> callback);

    void RegisterDisconnection(UnityAction<int> callback);

    void Send(Guid messageId, byte[] data);
  }
}
