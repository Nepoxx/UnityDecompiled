// Decompiled with JetBrains decompiler
// Type: UnityEngine.Network
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The network class is at the heart of the network implementation and provides the core functions.</para>
  /// </summary>
  public sealed class Network
  {
    /// <summary>
    ///   <para>Initialize the server.</para>
    /// </summary>
    /// <param name="connections"></param>
    /// <param name="listenPort"></param>
    /// <param name="useNat"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkConnectionError InitializeServer(int connections, int listenPort, bool useNat);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern NetworkConnectionError Internal_InitializeServerDeprecated(int connections, int listenPort);

    /// <summary>
    ///   <para>Initialize the server.</para>
    /// </summary>
    /// <param name="connections"></param>
    /// <param name="listenPort"></param>
    /// <param name="useNat"></param>
    [Obsolete("Use the IntializeServer(connections, listenPort, useNat) function instead")]
    public static NetworkConnectionError InitializeServer(int connections, int listenPort)
    {
      return Network.Internal_InitializeServerDeprecated(connections, listenPort);
    }

    /// <summary>
    ///   <para>Set the password for the server (for incoming connections).</para>
    /// </summary>
    public static extern string incomingPassword { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the log level for network messages (default is Off).</para>
    /// </summary>
    public static extern NetworkLogLevel logLevel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Initializes security layer.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void InitializeSecurity();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern NetworkConnectionError Internal_ConnectToSingleIP(string IP, int remotePort, int localPort, [DefaultValue("\"\"")] string password);

    [ExcludeFromDocs]
    private static NetworkConnectionError Internal_ConnectToSingleIP(string IP, int remotePort, int localPort)
    {
      string password = "";
      return Network.Internal_ConnectToSingleIP(IP, remotePort, localPort, password);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern NetworkConnectionError Internal_ConnectToGuid(string guid, string password);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern NetworkConnectionError Internal_ConnectToIPs(string[] IP, int remotePort, int localPort, [DefaultValue("\"\"")] string password);

    [ExcludeFromDocs]
    private static NetworkConnectionError Internal_ConnectToIPs(string[] IP, int remotePort, int localPort)
    {
      string password = "";
      return Network.Internal_ConnectToIPs(IP, remotePort, localPort, password);
    }

    /// <summary>
    ///   <para>Connect to the specified host (ip or domain name) and server port.</para>
    /// </summary>
    /// <param name="IP"></param>
    /// <param name="remotePort"></param>
    /// <param name="password"></param>
    [ExcludeFromDocs]
    public static NetworkConnectionError Connect(string IP, int remotePort)
    {
      string password = "";
      return Network.Connect(IP, remotePort, password);
    }

    /// <summary>
    ///   <para>Connect to the specified host (ip or domain name) and server port.</para>
    /// </summary>
    /// <param name="IP"></param>
    /// <param name="remotePort"></param>
    /// <param name="password"></param>
    public static NetworkConnectionError Connect(string IP, int remotePort, [DefaultValue("\"\"")] string password)
    {
      return Network.Internal_ConnectToSingleIP(IP, remotePort, 0, password);
    }

    /// <summary>
    ///   <para>This function is exactly like Network.Connect but can accept an array of IP addresses.</para>
    /// </summary>
    /// <param name="IPs"></param>
    /// <param name="remotePort"></param>
    /// <param name="password"></param>
    [ExcludeFromDocs]
    public static NetworkConnectionError Connect(string[] IPs, int remotePort)
    {
      string password = "";
      return Network.Connect(IPs, remotePort, password);
    }

    /// <summary>
    ///   <para>This function is exactly like Network.Connect but can accept an array of IP addresses.</para>
    /// </summary>
    /// <param name="IPs"></param>
    /// <param name="remotePort"></param>
    /// <param name="password"></param>
    public static NetworkConnectionError Connect(string[] IPs, int remotePort, [DefaultValue("\"\"")] string password)
    {
      return Network.Internal_ConnectToIPs(IPs, remotePort, 0, password);
    }

    /// <summary>
    ///   <para>Connect to a server GUID. NAT punchthrough can only be performed this way.</para>
    /// </summary>
    /// <param name="GUID"></param>
    /// <param name="password"></param>
    [ExcludeFromDocs]
    public static NetworkConnectionError Connect(string GUID)
    {
      string password = "";
      return Network.Connect(GUID, password);
    }

    /// <summary>
    ///   <para>Connect to a server GUID. NAT punchthrough can only be performed this way.</para>
    /// </summary>
    /// <param name="GUID"></param>
    /// <param name="password"></param>
    public static NetworkConnectionError Connect(string GUID, [DefaultValue("\"\"")] string password)
    {
      return Network.Internal_ConnectToGuid(GUID, password);
    }

    /// <summary>
    ///   <para>Connect to the host represented by a HostData structure returned by the Master Server.</para>
    /// </summary>
    /// <param name="hostData"></param>
    /// <param name="password"></param>
    [ExcludeFromDocs]
    public static NetworkConnectionError Connect(HostData hostData)
    {
      string password = "";
      return Network.Connect(hostData, password);
    }

    /// <summary>
    ///   <para>Connect to the host represented by a HostData structure returned by the Master Server.</para>
    /// </summary>
    /// <param name="hostData"></param>
    /// <param name="password"></param>
    public static NetworkConnectionError Connect(HostData hostData, [DefaultValue("\"\"")] string password)
    {
      if (hostData == null)
        throw new NullReferenceException();
      if (hostData.guid.Length > 0 && hostData.useNat)
        return Network.Connect(hostData.guid, password);
      return Network.Connect(hostData.ip, hostData.port, password);
    }

    /// <summary>
    ///   <para>Close all open connections and shuts down the network interface.</para>
    /// </summary>
    /// <param name="timeout"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Disconnect([DefaultValue("200")] int timeout);

    [ExcludeFromDocs]
    public static void Disconnect()
    {
      Network.Disconnect(200);
    }

    /// <summary>
    ///   <para>Close the connection to another system.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="sendDisconnectionNotification"></param>
    public static void CloseConnection(NetworkPlayer target, bool sendDisconnectionNotification)
    {
      Network.INTERNAL_CALL_CloseConnection(ref target, sendDisconnectionNotification);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CloseConnection(ref NetworkPlayer target, bool sendDisconnectionNotification);

    /// <summary>
    ///   <para>All connected players.</para>
    /// </summary>
    public static extern NetworkPlayer[] connections { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetPlayer();

    /// <summary>
    ///   <para>Get the local NetworkPlayer instance.</para>
    /// </summary>
    public static NetworkPlayer player
    {
      get
      {
        NetworkPlayer networkPlayer;
        networkPlayer.index = Network.Internal_GetPlayer();
        return networkPlayer;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_AllocateViewID(out NetworkViewID viewID);

    /// <summary>
    ///   <para>Query for the next available network view ID number and allocate it (reserve).</para>
    /// </summary>
    public static NetworkViewID AllocateViewID()
    {
      NetworkViewID viewID;
      Network.Internal_AllocateViewID(out viewID);
      return viewID;
    }

    /// <summary>
    ///   <para>Network instantiate a prefab.</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="group"></param>
    [TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
    public static Object Instantiate(Object prefab, Vector3 position, Quaternion rotation, int group)
    {
      return Network.INTERNAL_CALL_Instantiate(prefab, ref position, ref rotation, group);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Object INTERNAL_CALL_Instantiate(Object prefab, ref Vector3 position, ref Quaternion rotation, int group);

    /// <summary>
    ///   <para>Destroy the object associated with this view ID across the network.</para>
    /// </summary>
    /// <param name="viewID"></param>
    public static void Destroy(NetworkViewID viewID)
    {
      Network.INTERNAL_CALL_Destroy(ref viewID);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Destroy(ref NetworkViewID viewID);

    /// <summary>
    ///   <para>Destroy the object across the network.</para>
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Destroy(GameObject gameObject)
    {
      if (!((Object) gameObject != (Object) null))
        return;
      NetworkView component = gameObject.GetComponent<NetworkView>();
      if ((Object) component != (Object) null)
        Network.Destroy(component.viewID);
      else
        Debug.LogError((object) "Couldn't destroy game object because no network view is attached to it.", (Object) gameObject);
    }

    /// <summary>
    ///   <para>Destroy all the objects based on view IDs belonging to this player.</para>
    /// </summary>
    /// <param name="playerID"></param>
    public static void DestroyPlayerObjects(NetworkPlayer playerID)
    {
      Network.INTERNAL_CALL_DestroyPlayerObjects(ref playerID);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_DestroyPlayerObjects(ref NetworkPlayer playerID);

    private static void Internal_RemoveRPCs(NetworkPlayer playerID, NetworkViewID viewID, uint channelMask)
    {
      Network.INTERNAL_CALL_Internal_RemoveRPCs(ref playerID, ref viewID, channelMask);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_RemoveRPCs(ref NetworkPlayer playerID, ref NetworkViewID viewID, uint channelMask);

    /// <summary>
    ///   <para>Remove all RPC functions which belong to this player ID.</para>
    /// </summary>
    /// <param name="playerID"></param>
    public static void RemoveRPCs(NetworkPlayer playerID)
    {
      Network.Internal_RemoveRPCs(playerID, NetworkViewID.unassigned, uint.MaxValue);
    }

    /// <summary>
    ///   <para>Remove all RPC functions which belong to this player ID and were sent based on the given group.</para>
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="group"></param>
    public static void RemoveRPCs(NetworkPlayer playerID, int group)
    {
      Network.Internal_RemoveRPCs(playerID, NetworkViewID.unassigned, (uint) (1 << group));
    }

    /// <summary>
    ///   <para>Remove the RPC function calls accociated with this view ID number.</para>
    /// </summary>
    /// <param name="viewID"></param>
    public static void RemoveRPCs(NetworkViewID viewID)
    {
      Network.Internal_RemoveRPCs(NetworkPlayer.unassigned, viewID, uint.MaxValue);
    }

    /// <summary>
    ///   <para>Remove all RPC functions which belong to given group number.</para>
    /// </summary>
    /// <param name="group"></param>
    public static void RemoveRPCsInGroup(int group)
    {
      Network.Internal_RemoveRPCs(NetworkPlayer.unassigned, NetworkViewID.unassigned, (uint) (1 << group));
    }

    /// <summary>
    ///   <para>Returns true if your peer type is client.</para>
    /// </summary>
    public static extern bool isClient { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if your peer type is server.</para>
    /// </summary>
    public static extern bool isServer { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The status of the peer type, i.e. if it is disconnected, connecting, server or client.</para>
    /// </summary>
    public static extern NetworkPeerType peerType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Set the level prefix which will then be prefixed to all network ViewID numbers.</para>
    /// </summary>
    /// <param name="prefix"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetLevelPrefix(int prefix);

    /// <summary>
    ///   <para>The last ping time to the given player in milliseconds.</para>
    /// </summary>
    /// <param name="player"></param>
    public static int GetLastPing(NetworkPlayer player)
    {
      return Network.INTERNAL_CALL_GetLastPing(ref player);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_GetLastPing(ref NetworkPlayer player);

    /// <summary>
    ///   <para>The last average ping time to the given player in milliseconds.</para>
    /// </summary>
    /// <param name="player"></param>
    public static int GetAveragePing(NetworkPlayer player)
    {
      return Network.INTERNAL_CALL_GetAveragePing(ref player);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_GetAveragePing(ref NetworkPlayer player);

    /// <summary>
    ///   <para>The default send rate of network updates for all Network Views.</para>
    /// </summary>
    public static extern float sendRate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable or disable the processing of network messages.</para>
    /// </summary>
    public static extern bool isMessageQueueRunning { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable or disables the reception of messages in a specific group number from a specific player.</para>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="group"></param>
    /// <param name="enabled"></param>
    public static void SetReceivingEnabled(NetworkPlayer player, int group, bool enabled)
    {
      Network.INTERNAL_CALL_SetReceivingEnabled(ref player, group, enabled);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetReceivingEnabled(ref NetworkPlayer player, int group, bool enabled);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetSendingGlobal(int group, bool enabled);

    private static void Internal_SetSendingSpecific(NetworkPlayer player, int group, bool enabled)
    {
      Network.INTERNAL_CALL_Internal_SetSendingSpecific(ref player, group, enabled);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetSendingSpecific(ref NetworkPlayer player, int group, bool enabled);

    /// <summary>
    ///   <para>Enables or disables transmission of messages and RPC calls on a specific network group number.</para>
    /// </summary>
    /// <param name="group"></param>
    /// <param name="enabled"></param>
    public static void SetSendingEnabled(int group, bool enabled)
    {
      Network.Internal_SetSendingGlobal(group, enabled);
    }

    /// <summary>
    ///   <para>Enable or disable transmission of messages and RPC calls based on target network player as well as the network group.</para>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="group"></param>
    /// <param name="enabled"></param>
    public static void SetSendingEnabled(NetworkPlayer player, int group, bool enabled)
    {
      Network.Internal_SetSendingSpecific(player, group, enabled);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_GetTime(out double t);

    /// <summary>
    ///   <para>Get the current network time (seconds).</para>
    /// </summary>
    public static double time
    {
      get
      {
        double t;
        Network.Internal_GetTime(out t);
        return t;
      }
    }

    /// <summary>
    ///   <para>Get or set the minimum number of ViewID numbers in the ViewID pool given to clients by the server.</para>
    /// </summary>
    public static extern int minimumAllocatableViewIDs { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("No longer needed. This is now explicitly set in the InitializeServer function call. It is implicitly set when calling Connect depending on if an IP/port combination is used (useNat=false) or a GUID is used(useNat=true).")]
    public static extern bool useNat { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The IP address of the NAT punchthrough facilitator.</para>
    /// </summary>
    public static extern string natFacilitatorIP { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The port of the NAT punchthrough facilitator.</para>
    /// </summary>
    public static extern int natFacilitatorPort { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Test this machines network connection.</para>
    /// </summary>
    /// <param name="forceTest"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ConnectionTesterStatus TestConnection([DefaultValue("false")] bool forceTest);

    [ExcludeFromDocs]
    public static ConnectionTesterStatus TestConnection()
    {
      return Network.TestConnection(false);
    }

    /// <summary>
    ///   <para>Test the connection specifically for NAT punch-through connectivity.</para>
    /// </summary>
    /// <param name="forceTest"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ConnectionTesterStatus TestConnectionNAT([DefaultValue("false")] bool forceTest);

    [ExcludeFromDocs]
    public static ConnectionTesterStatus TestConnectionNAT()
    {
      return Network.TestConnectionNAT(false);
    }

    /// <summary>
    ///   <para>The IP address of the connection tester used in Network.TestConnection.</para>
    /// </summary>
    public static extern string connectionTesterIP { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The port of the connection tester used in Network.TestConnection.</para>
    /// </summary>
    public static extern int connectionTesterPort { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Check if this machine has a public IP address.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HavePublicAddress();

    /// <summary>
    ///   <para>Set the maximum amount of connections/players allowed.</para>
    /// </summary>
    public static extern int maxConnections { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The IP address of the proxy server.</para>
    /// </summary>
    public static extern string proxyIP { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The port of the proxy server.</para>
    /// </summary>
    public static extern int proxyPort { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Indicate if proxy support is needed, in which case traffic is relayed through the proxy server.</para>
    /// </summary>
    public static extern bool useProxy { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the proxy server password.</para>
    /// </summary>
    public static extern string proxyPassword { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
