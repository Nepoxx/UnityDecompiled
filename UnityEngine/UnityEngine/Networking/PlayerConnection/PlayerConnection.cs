// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.PlayerConnection.PlayerConnection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using UnityEngine.Scripting;

namespace UnityEngine.Networking.PlayerConnection
{
  [Serializable]
  public class PlayerConnection : ScriptableObject, IEditorPlayerConnection
  {
    [SerializeField]
    private PlayerEditorConnectionEvents m_PlayerEditorConnectionEvents = new PlayerEditorConnectionEvents();
    [SerializeField]
    private List<int> m_connectedPlayers = new List<int>();
    internal static IPlayerEditorConnectionNative connectionNative;
    private bool m_IsInitilized;
    private static UnityEngine.Networking.PlayerConnection.PlayerConnection s_Instance;

    public static UnityEngine.Networking.PlayerConnection.PlayerConnection instance
    {
      get
      {
        if ((UnityEngine.Object) UnityEngine.Networking.PlayerConnection.PlayerConnection.s_Instance == (UnityEngine.Object) null)
          return UnityEngine.Networking.PlayerConnection.PlayerConnection.CreateInstance();
        return UnityEngine.Networking.PlayerConnection.PlayerConnection.s_Instance;
      }
    }

    public bool isConnected
    {
      get
      {
        return this.GetConnectionNativeApi().IsConnected();
      }
    }

    private static UnityEngine.Networking.PlayerConnection.PlayerConnection CreateInstance()
    {
      UnityEngine.Networking.PlayerConnection.PlayerConnection.s_Instance = ScriptableObject.CreateInstance<UnityEngine.Networking.PlayerConnection.PlayerConnection>();
      UnityEngine.Networking.PlayerConnection.PlayerConnection.s_Instance.hideFlags = HideFlags.HideAndDontSave;
      return UnityEngine.Networking.PlayerConnection.PlayerConnection.s_Instance;
    }

    public void OnEnable()
    {
      if (this.m_IsInitilized)
        return;
      this.m_IsInitilized = true;
      this.GetConnectionNativeApi().Initialize();
    }

    private IPlayerEditorConnectionNative GetConnectionNativeApi()
    {
      return UnityEngine.Networking.PlayerConnection.PlayerConnection.connectionNative ?? (IPlayerEditorConnectionNative) new PlayerConnectionInternal();
    }

    public void Register(Guid messageId, UnityAction<MessageEventArgs> callback)
    {
      if (messageId == Guid.Empty)
        throw new ArgumentException("Cant be Guid.Empty", nameof (messageId));
      if (!this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId)))
        this.GetConnectionNativeApi().RegisterInternal(messageId);
      this.m_PlayerEditorConnectionEvents.AddAndCreate(messageId).AddListener(callback);
    }

    public void Unregister(Guid messageId, UnityAction<MessageEventArgs> callback)
    {
      this.m_PlayerEditorConnectionEvents.UnregisterManagedCallback(messageId, callback);
      if (this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId)))
        return;
      this.GetConnectionNativeApi().UnregisterInternal(messageId);
    }

    public void RegisterConnection(UnityAction<int> callback)
    {
      foreach (int connectedPlayer in this.m_connectedPlayers)
        callback(connectedPlayer);
      this.m_PlayerEditorConnectionEvents.connectionEvent.AddListener(callback);
    }

    public void RegisterDisconnection(UnityAction<int> callback)
    {
      this.m_PlayerEditorConnectionEvents.disconnectionEvent.AddListener(callback);
    }

    public void Send(Guid messageId, byte[] data)
    {
      if (messageId == Guid.Empty)
        throw new ArgumentException("Cant be Guid.Empty", nameof (messageId));
      this.GetConnectionNativeApi().SendMessage(messageId, data, 0);
    }

    public bool BlockUntilRecvMsg(Guid messageId, int timeout)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UnityEngine.Networking.PlayerConnection.PlayerConnection.\u003CBlockUntilRecvMsg\u003Ec__AnonStorey2 recvMsgCAnonStorey2 = new UnityEngine.Networking.PlayerConnection.PlayerConnection.\u003CBlockUntilRecvMsg\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      recvMsgCAnonStorey2.msgReceived = false;
      // ISSUE: reference to a compiler-generated method
      UnityAction<MessageEventArgs> callback = new UnityAction<MessageEventArgs>(recvMsgCAnonStorey2.\u003C\u003Em__0);
      DateTime now = DateTime.Now;
      this.Register(messageId, callback);
      // ISSUE: reference to a compiler-generated field
      while ((DateTime.Now - now).TotalMilliseconds < (double) timeout && !recvMsgCAnonStorey2.msgReceived)
        this.GetConnectionNativeApi().Poll();
      this.Unregister(messageId, callback);
      // ISSUE: reference to a compiler-generated field
      return recvMsgCAnonStorey2.msgReceived;
    }

    public void DisconnectAll()
    {
      this.GetConnectionNativeApi().DisconnectAll();
    }

    [RequiredByNativeCode]
    private static void MessageCallbackInternal(IntPtr data, ulong size, ulong guid, string messageId)
    {
      byte[] numArray = (byte[]) null;
      if (size > 0UL)
      {
        numArray = new byte[size];
        Marshal.Copy(data, numArray, 0, (int) size);
      }
      UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.m_PlayerEditorConnectionEvents.InvokeMessageIdSubscribers(new Guid(messageId), numArray, (int) guid);
    }

    [RequiredByNativeCode]
    private static void ConnectedCallbackInternal(int playerId)
    {
      UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.m_connectedPlayers.Add(playerId);
      UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.m_PlayerEditorConnectionEvents.connectionEvent.Invoke(playerId);
    }

    [RequiredByNativeCode]
    private static void DisconnectedCallback(int playerId)
    {
      UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.m_connectedPlayers.Remove(playerId);
      UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.m_PlayerEditorConnectionEvents.disconnectionEvent.Invoke(playerId);
    }
  }
}
