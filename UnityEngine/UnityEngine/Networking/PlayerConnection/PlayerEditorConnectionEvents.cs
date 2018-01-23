// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.PlayerConnection.PlayerEditorConnectionEvents
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UnityEngine.Networking.PlayerConnection
{
  [Serializable]
  internal class PlayerEditorConnectionEvents
  {
    [SerializeField]
    public List<PlayerEditorConnectionEvents.MessageTypeSubscribers> messageTypeSubscribers = new List<PlayerEditorConnectionEvents.MessageTypeSubscribers>();
    [SerializeField]
    public PlayerEditorConnectionEvents.ConnectionChangeEvent connectionEvent = new PlayerEditorConnectionEvents.ConnectionChangeEvent();
    [SerializeField]
    public PlayerEditorConnectionEvents.ConnectionChangeEvent disconnectionEvent = new PlayerEditorConnectionEvents.ConnectionChangeEvent();

    public void InvokeMessageIdSubscribers(Guid messageId, byte[] data, int playerId)
    {
      IEnumerable<PlayerEditorConnectionEvents.MessageTypeSubscribers> source = this.messageTypeSubscribers.Where<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId));
      if (!source.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>())
      {
        Debug.LogError((object) ("No actions found for messageId: " + (object) messageId));
      }
      else
      {
        MessageEventArgs messageEventArgs = new MessageEventArgs() { playerId = playerId, data = data };
        foreach (PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers in source)
          messageTypeSubscribers.messageCallback.Invoke(messageEventArgs);
      }
    }

    public UnityEvent<MessageEventArgs> AddAndCreate(Guid messageId)
    {
      PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers = this.messageTypeSubscribers.SingleOrDefault<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId));
      if (messageTypeSubscribers == null)
      {
        messageTypeSubscribers = new PlayerEditorConnectionEvents.MessageTypeSubscribers()
        {
          MessageTypeId = messageId,
          messageCallback = new PlayerEditorConnectionEvents.MessageEvent()
        };
        this.messageTypeSubscribers.Add(messageTypeSubscribers);
      }
      ++messageTypeSubscribers.subscriberCount;
      return (UnityEvent<MessageEventArgs>) messageTypeSubscribers.messageCallback;
    }

    public void UnregisterManagedCallback(Guid messageId, UnityAction<MessageEventArgs> callback)
    {
      PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers = this.messageTypeSubscribers.SingleOrDefault<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId));
      if (messageTypeSubscribers == null)
        return;
      --messageTypeSubscribers.subscriberCount;
      messageTypeSubscribers.messageCallback.RemoveListener(callback);
      if (messageTypeSubscribers.subscriberCount > 0)
        return;
      this.messageTypeSubscribers.Remove(messageTypeSubscribers);
    }

    [Serializable]
    public class MessageEvent : UnityEvent<MessageEventArgs>
    {
    }

    [Serializable]
    public class ConnectionChangeEvent : UnityEvent<int>
    {
    }

    [Serializable]
    public class MessageTypeSubscribers
    {
      public int subscriberCount = 0;
      public PlayerEditorConnectionEvents.MessageEvent messageCallback = new PlayerEditorConnectionEvents.MessageEvent();
      [SerializeField]
      private string m_messageTypeId;

      public Guid MessageTypeId
      {
        get
        {
          return new Guid(this.m_messageTypeId);
        }
        set
        {
          this.m_messageTypeId = value.ToString();
        }
      }
    }
  }
}
