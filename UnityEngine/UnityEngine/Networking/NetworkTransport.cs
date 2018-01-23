// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkTransport
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngine.Networking.Types;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Transport Layer API.</para>
  /// </summary>
  public sealed class NetworkTransport
  {
    private NetworkTransport()
    {
    }

    internal static bool DoesEndPointUsePlatformProtocols(EndPoint endPoint)
    {
      if (endPoint.GetType().FullName == "UnityEngine.PS4.SceEndPoint" || endPoint.GetType().FullName == "UnityEngine.PSVita.SceEndPoint")
      {
        SocketAddress socketAddress = endPoint.Serialize();
        if ((int) socketAddress[8] != 0 || (int) socketAddress[9] != 0)
          return true;
      }
      return false;
    }

    public static int ConnectEndPoint(int hostId, EndPoint endPoint, int exceptionConnectionId, out byte error)
    {
      error = (byte) 0;
      byte[] numArray1 = new byte[4]{ (byte) 95, (byte) 36, (byte) 19, (byte) 246 };
      if (endPoint == null)
        throw new NullReferenceException("Null EndPoint provided");
      if (endPoint.GetType().FullName != "UnityEngine.XboxOne.XboxOneEndPoint" && endPoint.GetType().FullName != "UnityEngine.PS4.SceEndPoint" && endPoint.GetType().FullName != "UnityEngine.PSVita.SceEndPoint")
        throw new ArgumentException("Endpoint of type XboxOneEndPoint or SceEndPoint  required");
      if (endPoint.GetType().FullName == "UnityEngine.XboxOne.XboxOneEndPoint")
      {
        EndPoint endPoint1 = endPoint;
        if (endPoint1.AddressFamily != AddressFamily.InterNetworkV6)
          throw new ArgumentException("XboxOneEndPoint has an invalid family");
        SocketAddress socketAddress = endPoint1.Serialize();
        if (socketAddress.Size != 14)
          throw new ArgumentException("XboxOneEndPoint has an invalid size");
        if ((int) socketAddress[0] != 0 || (int) socketAddress[1] != 0)
          throw new ArgumentException("XboxOneEndPoint has an invalid family signature");
        if ((int) socketAddress[2] != (int) numArray1[0] || (int) socketAddress[3] != (int) numArray1[1] || ((int) socketAddress[4] != (int) numArray1[2] || (int) socketAddress[5] != (int) numArray1[3]))
          throw new ArgumentException("XboxOneEndPoint has an invalid signature");
        byte[] numArray2 = new byte[8];
        for (int index = 0; index < numArray2.Length; ++index)
          numArray2[index] = socketAddress[6 + index];
        IntPtr num = new IntPtr(BitConverter.ToInt64(numArray2, 0));
        if (num == IntPtr.Zero)
          throw new ArgumentException("XboxOneEndPoint has an invalid SOCKET_STORAGE pointer");
        byte[] destination = new byte[2];
        Marshal.Copy(num, destination, 0, destination.Length);
        if (((int) destination[1] << 8) + (int) destination[0] != 23)
          throw new ArgumentException("XboxOneEndPoint has corrupt or invalid SOCKET_STORAGE pointer");
        return NetworkTransport.Internal_ConnectEndPoint(hostId, num, 128, exceptionConnectionId, out error);
      }
      SocketAddress socketAddress1 = endPoint.Serialize();
      if (socketAddress1.Size != 16)
        throw new ArgumentException("EndPoint has an invalid size");
      if ((int) socketAddress1[0] != socketAddress1.Size)
        throw new ArgumentException("EndPoint has an invalid size value");
      if ((int) socketAddress1[1] != 2)
        throw new ArgumentException("EndPoint has an invalid family value");
      byte[] source = new byte[16];
      for (int index = 0; index < source.Length; ++index)
        source[index] = socketAddress1[index];
      IntPtr num1 = Marshal.AllocHGlobal(source.Length);
      Marshal.Copy(source, 0, num1, source.Length);
      int num2 = NetworkTransport.Internal_ConnectEndPoint(hostId, num1, 16, exceptionConnectionId, out error);
      Marshal.FreeHGlobal(num1);
      return num2;
    }

    /// <summary>
    ///   <para>Initializes the NetworkTransport. Should be called before any other operations on the NetworkTransport are done.</para>
    /// </summary>
    public static void Init()
    {
      NetworkTransport.InitWithNoParameters();
    }

    public static void Init(GlobalConfig config)
    {
      NetworkTransport.InitWithParameters(new GlobalConfigInternal(config));
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InitWithNoParameters();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InitWithParameters(GlobalConfigInternal config);

    /// <summary>
    ///   <para>Shut down the NetworkTransport.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Shutdown();

    /// <summary>
    ///   <para>The Unity Multiplayer spawning system uses assetIds to identify what remote objects to spawn. This function allows you to get the assetId for the prefab associated with an object.</para>
    /// </summary>
    /// <param name="go">Target GameObject to get assetId for.</param>
    /// <returns>
    ///   <para>The assetId of the game object's prefab.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAssetId(GameObject go);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AddSceneId(int id);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNextSceneId();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ConnectAsNetworkHost(int hostId, string address, int port, NetworkID network, SourceID source, NodeID node, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DisconnectNetworkHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkEventType ReceiveRelayEventFromHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, int bytesPerSec, float bucketSizeFactor, out byte error);

    public static int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, out byte error)
    {
      return NetworkTransport.ConnectToNetworkPeer(hostId, address, port, exceptionConnectionId, relaySlotId, network, source, node, 0, 0.0f, out error);
    }

    /// <summary>
    ///   <para>Returns the number of unread messages in the read-queue.</para>
    /// </summary>
    [Obsolete("GetCurrentIncomingMessageAmount has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentIncomingMessageAmount();

    /// <summary>
    ///   <para>Returns the total number of messages still in the write-queue.</para>
    /// </summary>
    [Obsolete("GetCurrentOutgoingMessageAmount has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentOutgoingMessageAmount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetIncomingMessageQueueSize(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingMessageQueueSize(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentRTT(int hostId, int connectionId, out byte error);

    [Obsolete("GetCurrentRtt() has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentRtt(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetIncomingPacketLossCount(int hostId, int connectionId, out byte error);

    [Obsolete("GetNetworkLostPacketNum() has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNetworkLostPacketNum(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetIncomingPacketCount(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingPacketNetworkLossPercent(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingPacketOverflowLossPercent(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetMaxAllowedBandwidth(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetAckBufferCount(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>How many packets have been dropped due lack space in incoming queue (absolute value, countinf from start).</para>
    /// </summary>
    /// <returns>
    ///   <para>Dropping packet count.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetIncomingPacketDropCountForAllHosts();

    /// <summary>
    ///   <para>Returns how many packets have been received from start. (from Networking.NetworkTransport.Init call).</para>
    /// </summary>
    /// <returns>
    ///   <para>Packets count received from start for all hosts.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetIncomingPacketCountForAllHosts();

    /// <summary>
    ///   <para>Returns how many packets have been sent from start (from call Networking.NetworkTransport.Init) for all hosts.</para>
    /// </summary>
    /// <returns>
    ///   <para>Packets count sent from networking library start (from call Networking.NetworkTransport.Init)  for all hosts.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingPacketCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingPacketCountForHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingPacketCountForConnection(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>Returns how many messages have been sent from start (from Networking.NetworkTransport.Init call).</para>
    /// </summary>
    /// <returns>
    ///   <para>Messages count sent from start (from call Networking.NetworkTransport.Init) for all hosts.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingMessageCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingMessageCountForHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingMessageCountForConnection(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>Returns how much payload (user) bytes have been sent from start (from Networking.NetworkTransport.Init call).</para>
    /// </summary>
    /// <returns>
    ///   <para>Total payload (in bytes) sent from start for all hosts.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingUserBytesCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingUserBytesCountForHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingUserBytesCountForConnection(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>Returns how much user payload and protocol system headers (in bytes)  have been sent from start (from Networking.NetworkTransport.Init call).</para>
    /// </summary>
    /// <returns>
    ///   <para>Total payload and protocol system headers (in bytes) sent from start for all hosts.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingSystemBytesCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingSystemBytesCountForHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingSystemBytesCountForConnection(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>Returns how much raw data (in bytes) have been sent from start for all hosts (from Networking.NetworkTransport.Init call).</para>
    /// </summary>
    /// <returns>
    ///   <para>Total data (user payload, protocol specific data, ip and udp headers) (in bytes) sent from start for all hosts.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingFullBytesCount();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingFullBytesCountForHost(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetOutgoingFullBytesCountForConnection(int hostId, int connectionId, out byte error);

    [Obsolete("GetPacketSentRate has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPacketSentRate(int hostId, int connectionId, out byte error);

    [Obsolete("GetPacketReceivedRate has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPacketReceivedRate(int hostId, int connectionId, out byte error);

    [Obsolete("GetRemotePacketReceivedRate has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRemotePacketReceivedRate(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>Function returns time spent on network I/O operations in microseconds.</para>
    /// </summary>
    /// <returns>
    ///   <para>Time in micro seconds.</para>
    /// </returns>
    [Obsolete("GetNetIOTimeuS has been deprecated.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNetIOTimeuS();

    public static void GetConnectionInfo(int hostId, int connectionId, out string address, out int port, out NetworkID network, out NodeID dstNode, out byte error)
    {
      ulong network1;
      ushort dstNode1;
      address = NetworkTransport.GetConnectionInfo(hostId, connectionId, out port, out network1, out dstNode1, out error);
      network = (NetworkID) network1;
      dstNode = (NodeID) dstNode1;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetConnectionInfo(int hostId, int connectionId, out int port, out ulong network, out ushort dstNode, out byte error);

    /// <summary>
    ///   <para>Get a network timestamp. Can be used in your messages to investigate network delays together with Networking.GetRemoteDelayTimeMS.</para>
    /// </summary>
    /// <returns>
    ///   <para>Timestamp.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNetworkTimestamp();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRemoteDelayTimeMS(int hostId, int connectionId, int remoteTime, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool StartSendMulticast(int hostId, int channelId, byte[] buffer, int size, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SendMulticast(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool FinishSendMulticast(int hostId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetMaxPacketSize();

    private static void CheckTopology(HostTopology topology)
    {
      int maxPacketSize = NetworkTransport.GetMaxPacketSize();
      if ((int) topology.DefaultConfig.PacketSize > maxPacketSize)
        throw new ArgumentOutOfRangeException("Default config: packet size should be less than packet size defined in global config: " + maxPacketSize.ToString());
      for (int index = 0; index < topology.SpecialConnectionConfigs.Count; ++index)
      {
        if ((int) topology.SpecialConnectionConfigs[index].PacketSize > maxPacketSize)
          throw new ArgumentOutOfRangeException("Special config " + index.ToString() + ": packet size should be less than packet size defined in global config: " + maxPacketSize.ToString());
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddWsHostWrapper(HostTopologyInternal topologyInt, string ip, int port);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddWsHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port);

    /// <summary>
    ///   <para>Created web socket host.</para>
    /// </summary>
    /// <param name="port">Port to bind to.</param>
    /// <param name="topology">The Networking.HostTopology associated with the host.</param>
    /// <param name="ip">IP address to bind to.</param>
    /// <returns>
    ///   <para>Web socket host id.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int AddWebsocketHost(HostTopology topology, int port)
    {
      string ip = (string) null;
      return NetworkTransport.AddWebsocketHost(topology, port, ip);
    }

    /// <summary>
    ///   <para>Created web socket host.</para>
    /// </summary>
    /// <param name="port">Port to bind to.</param>
    /// <param name="topology">The Networking.HostTopology associated with the host.</param>
    /// <param name="ip">IP address to bind to.</param>
    /// <returns>
    ///   <para>Web socket host id.</para>
    /// </returns>
    public static int AddWebsocketHost(HostTopology topology, int port, [DefaultValue("null")] string ip)
    {
      if (topology == null)
        throw new NullReferenceException("topology is not defined");
      NetworkTransport.CheckTopology(topology);
      if (ip == null)
        return NetworkTransport.AddWsHostWrapperWithoutIp(new HostTopologyInternal(topology), port);
      return NetworkTransport.AddWsHostWrapper(new HostTopologyInternal(topology), ip, port);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddHostWrapper(HostTopologyInternal topologyInt, string ip, int port, int minTimeout, int maxTimeout);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port, int minTimeout, int maxTimeout);

    [ExcludeFromDocs]
    public static int AddHost(HostTopology topology, int port)
    {
      string ip = (string) null;
      return NetworkTransport.AddHost(topology, port, ip);
    }

    [ExcludeFromDocs]
    public static int AddHost(HostTopology topology)
    {
      string ip = (string) null;
      int port = 0;
      return NetworkTransport.AddHost(topology, port, ip);
    }

    /// <summary>
    ///   <para>Creates a host based on Networking.HostTopology.</para>
    /// </summary>
    /// <param name="topology">The Networking.HostTopology associated with the host.</param>
    /// <param name="port">Port to bind to (when 0 is selected, the OS will choose a port at random).</param>
    /// <param name="ip">IP address to bind to.</param>
    /// <returns>
    ///   <para>Returns the ID of the host that was created.</para>
    /// </returns>
    public static int AddHost(HostTopology topology, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
    {
      if (topology == null)
        throw new NullReferenceException("topology is not defined");
      NetworkTransport.CheckTopology(topology);
      if (ip == null)
        return NetworkTransport.AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, 0, 0);
      return NetworkTransport.AddHostWrapper(new HostTopologyInternal(topology), ip, port, 0, 0);
    }

    [ExcludeFromDocs]
    public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, int port)
    {
      string ip = (string) null;
      return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
    }

    [ExcludeFromDocs]
    public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout)
    {
      string ip = (string) null;
      int port = 0;
      return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
    }

    /// <summary>
    ///   <para>Create a host and configure them to simulate Internet latency (works on Editor and development build only).</para>
    /// </summary>
    /// <param name="topology">The Networking.HostTopology associated with the host.</param>
    /// <param name="minTimeout">Minimum simulated delay in milliseconds.</param>
    /// <param name="maxTimeout">Maximum simulated delay in milliseconds.</param>
    /// <param name="port">Port to bind to (when 0 is selected, the OS will choose a port at random).</param>
    /// <param name="ip">IP address to bind to.</param>
    /// <returns>
    ///   <para>Returns host ID just created.</para>
    /// </returns>
    public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
    {
      if (topology == null)
        throw new NullReferenceException("topology is not defined");
      if (ip == null)
        return NetworkTransport.AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, minTimeout, maxTimeout);
      return NetworkTransport.AddHostWrapper(new HostTopologyInternal(topology), ip, port, minTimeout, maxTimeout);
    }

    /// <summary>
    ///   <para>Closes the opened socket, and closes all connections belonging to that socket.</para>
    /// </summary>
    /// <param name="hostId">Host ID to remove.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RemoveHost(int hostId);

    /// <summary>
    ///   <para>Deprecated.</para>
    /// </summary>
    public static extern bool IsStarted { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int Connect(int hostId, string address, int port, int exeptionConnectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_ConnectEndPoint(int hostId, IntPtr sockAddrStorage, int sockAddrStorageLen, int exceptionConnectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ConnectWithSimulator(int hostId, string address, int port, int exeptionConnectionId, out byte error, ConnectionSimulatorConfig conf);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Disconnect(int hostId, int connectionId, out byte error);

    public static bool Send(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error)
    {
      if (buffer == null)
        throw new NullReferenceException("send buffer is not initialized");
      return NetworkTransport.SendWrapper(hostId, connectionId, channelId, buffer, size, out error);
    }

    public static bool QueueMessageForSending(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error)
    {
      if (buffer == null)
        throw new NullReferenceException("send buffer is not initialized");
      return NetworkTransport.QueueMessageForSendingWrapper(hostId, connectionId, channelId, buffer, size, out error);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SendQueuedMessages(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SendWrapper(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool QueueMessageForSendingWrapper(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool NotifyConnectionSendable(int hostId, int connectionId, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkEventType Receive(out int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkEventType ReceiveFromHost(int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPacketStat(int direction, int packetStatId, int numMsgs, int numBytes);

    public static bool StartBroadcastDiscovery(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error)
    {
      if (buffer != null)
      {
        if (buffer.Length < size)
          throw new ArgumentOutOfRangeException("Size: " + (object) size + " > buffer.Length " + (object) buffer.Length);
        if (size == 0)
          throw new ArgumentOutOfRangeException("Size is zero while buffer exists, please pass null and 0 as buffer and size parameters");
      }
      if (buffer == null)
        return NetworkTransport.StartBroadcastDiscoveryWithoutData(hostId, broadcastPort, key, version, subversion, timeout, out error);
      return NetworkTransport.StartBroadcastDiscoveryWithData(hostId, broadcastPort, key, version, subversion, buffer, size, timeout, out error);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool StartBroadcastDiscoveryWithoutData(int hostId, int broadcastPort, int key, int version, int subversion, int timeout, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool StartBroadcastDiscoveryWithData(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error);

    /// <summary>
    ///   <para>Stop sending the broadcast discovery message.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopBroadcastDiscovery();

    /// <summary>
    ///   <para>Check if the broadcast discovery sender is running.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if it is running. False if it is not running.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsBroadcastDiscoveryRunning();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBroadcastCredentials(int hostId, int key, int version, int subversion, out byte error);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetBroadcastConnectionInfo(int hostId, out int port, out byte error);

    public static void GetBroadcastConnectionInfo(int hostId, out string address, out int port, out byte error)
    {
      address = NetworkTransport.GetBroadcastConnectionInfo(hostId, out port, out error);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetBroadcastConnectionMessage(int hostId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);
  }
}
