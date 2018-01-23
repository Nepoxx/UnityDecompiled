// Decompiled with JetBrains decompiler
// Type: UnityEditor.Networking.PlayerConnection.EditorConnection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Scripting;

namespace UnityEditor.Networking.PlayerConnection
{
  /// <summary>
  ///   <para>Handles the connection from the Editor to the Player.</para>
  /// </summary>
  [Serializable]
  public class EditorConnection : ScriptableSingleton<EditorConnection>, IEditorPlayerConnection
  {
    [SerializeField]
    private PlayerEditorConnectionEvents m_PlayerEditorConnectionEvents = new PlayerEditorConnectionEvents();
    [SerializeField]
    private List<ConnectedPlayer> m_connectedPlayers = new List<ConnectedPlayer>();
    internal static IPlayerEditorConnectionNative connectionNative;

    /// <summary>
    ///   <para>A list of the connected players.</para>
    /// </summary>
    public List<ConnectedPlayer> ConnectedPlayers
    {
      get
      {
        return this.m_connectedPlayers;
      }
    }

    /// <summary>
    ///   <para>Initializes the EditorConnection.</para>
    /// </summary>
    public void Initialize()
    {
      this.GetEditorConnectionNativeApi().Initialize();
    }

    private void Cleanup()
    {
      this.UnregisterAllPersistedListeners((UnityEventBase) this.m_PlayerEditorConnectionEvents.connectionEvent);
      this.UnregisterAllPersistedListeners((UnityEventBase) this.m_PlayerEditorConnectionEvents.disconnectionEvent);
      this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Clear();
    }

    private void UnregisterAllPersistedListeners(UnityEventBase connectionEvent)
    {
      int persistentEventCount = connectionEvent.GetPersistentEventCount();
      for (int index = 0; index < persistentEventCount; ++index)
        connectionEvent.UnregisterPersistentListener(index);
    }

    private IPlayerEditorConnectionNative GetEditorConnectionNativeApi()
    {
      return EditorConnection.connectionNative ?? (IPlayerEditorConnectionNative) new EditorConnectionInternal();
    }

    public void Register(Guid messageId, UnityAction<MessageEventArgs> callback)
    {
      if (messageId == Guid.Empty)
        throw new ArgumentException("Cant be Guid.Empty", nameof (messageId));
      if (!this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId)))
        this.GetEditorConnectionNativeApi().RegisterInternal(messageId);
      this.m_PlayerEditorConnectionEvents.AddAndCreate(messageId).AddPersistentListener(callback, UnityEventCallState.EditorAndRuntime);
    }

    public void Unregister(Guid messageId, UnityAction<MessageEventArgs> callback)
    {
      this.m_PlayerEditorConnectionEvents.UnregisterManagedCallback(messageId, callback);
      if (this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>((Func<PlayerEditorConnectionEvents.MessageTypeSubscribers, bool>) (x => x.MessageTypeId == messageId)))
        return;
      this.GetEditorConnectionNativeApi().UnregisterInternal(messageId);
    }

    public void RegisterConnection(UnityAction<int> callback)
    {
      foreach (ConnectedPlayer connectedPlayer in this.m_connectedPlayers)
        callback(connectedPlayer.playerId);
      this.m_PlayerEditorConnectionEvents.connectionEvent.AddPersistentListener(callback, UnityEventCallState.EditorAndRuntime);
    }

    public void RegisterDisconnection(UnityAction<int> callback)
    {
      this.m_PlayerEditorConnectionEvents.disconnectionEvent.AddPersistentListener(callback, UnityEventCallState.EditorAndRuntime);
    }

    /// <summary>
    ///   <para>Sends data to multiple or single Player(s).</para>
    /// </summary>
    /// <param name="messageId">Type ID of the message to send to the Player(s).</param>
    /// <param name="playerId">If set, the message will only send to the Player with this ID.</param>
    /// <param name="data"></param>
    public void Send(Guid messageId, byte[] data, int playerId)
    {
      if (messageId == Guid.Empty)
        throw new ArgumentException("Cant be Guid.Empty", nameof (messageId));
      this.GetEditorConnectionNativeApi().SendMessage(messageId, data, playerId);
    }

    /// <summary>
    ///   <para>Sends data to multiple or single Player(s).</para>
    /// </summary>
    /// <param name="messageId">Type ID of the message to send to the Player(s).</param>
    /// <param name="playerId">If set, the message will only send to the Player with this ID.</param>
    /// <param name="data"></param>
    public void Send(Guid messageId, byte[] data)
    {
      this.Send(messageId, data, 0);
    }

    /// <summary>
    ///   <para>This disconnects all of the active connections.</para>
    /// </summary>
    public void DisconnectAll()
    {
      this.GetEditorConnectionNativeApi().DisconnectAll();
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
      ScriptableSingleton<EditorConnection>.instance.m_PlayerEditorConnectionEvents.InvokeMessageIdSubscribers(new Guid(messageId), numArray, (int) guid);
    }

    [RequiredByNativeCode]
    private static void ConnectedCallbackInternal(int playerId, string playerName)
    {
      ScriptableSingleton<EditorConnection>.instance.m_connectedPlayers.Add(new ConnectedPlayer(playerId, playerName));
      ScriptableSingleton<EditorConnection>.instance.m_PlayerEditorConnectionEvents.connectionEvent.Invoke(playerId);
    }

    [RequiredByNativeCode]
    private static void DisconnectedCallback(int playerId)
    {
      ScriptableSingleton<EditorConnection>.instance.m_connectedPlayers.RemoveAll((Predicate<ConnectedPlayer>) (c => c.playerId == playerId));
      ScriptableSingleton<EditorConnection>.instance.m_PlayerEditorConnectionEvents.disconnectionEvent.Invoke(playerId);
      if (ScriptableSingleton<EditorConnection>.instance.ConnectedPlayers.Any<ConnectedPlayer>())
        return;
      ScriptableSingleton<EditorConnection>.instance.Cleanup();
    }
  }
}
