// Decompiled with JetBrains decompiler
// Type: UnityEngine.IPlayerEditorConnectionNative
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal interface IPlayerEditorConnectionNative
  {
    void Initialize();

    void DisconnectAll();

    void SendMessage(Guid messageId, byte[] data, int playerId);

    void Poll();

    void RegisterInternal(Guid messageId);

    void UnregisterInternal(Guid messageId);

    bool IsConnected();
  }
}
