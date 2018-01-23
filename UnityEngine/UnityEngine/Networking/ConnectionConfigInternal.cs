// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ConnectionConfigInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
  internal sealed class ConnectionConfigInternal : IDisposable
  {
    internal IntPtr m_Ptr;

    private ConnectionConfigInternal()
    {
    }

    public ConnectionConfigInternal(ConnectionConfig config)
    {
      if (config == null)
        throw new NullReferenceException("config is not defined");
      this.InitWrapper();
      this.InitPacketSize(config.PacketSize);
      this.InitFragmentSize(config.FragmentSize);
      this.InitResendTimeout(config.ResendTimeout);
      this.InitDisconnectTimeout(config.DisconnectTimeout);
      this.InitConnectTimeout(config.ConnectTimeout);
      this.InitMinUpdateTimeout(config.MinUpdateTimeout);
      this.InitPingTimeout(config.PingTimeout);
      this.InitReducedPingTimeout(config.ReducedPingTimeout);
      this.InitAllCostTimeout(config.AllCostTimeout);
      this.InitNetworkDropThreshold(config.NetworkDropThreshold);
      this.InitOverflowDropThreshold(config.OverflowDropThreshold);
      this.InitMaxConnectionAttempt(config.MaxConnectionAttempt);
      this.InitAckDelay(config.AckDelay);
      this.InitSendDelay(config.SendDelay);
      this.InitMaxCombinedReliableMessageSize(config.MaxCombinedReliableMessageSize);
      this.InitMaxCombinedReliableMessageCount(config.MaxCombinedReliableMessageCount);
      this.InitMaxSentMessageQueueSize(config.MaxSentMessageQueueSize);
      this.InitAcksType((int) config.AcksType);
      this.InitUsePlatformSpecificProtocols(config.UsePlatformSpecificProtocols);
      this.InitInitialBandwidth(config.InitialBandwidth);
      this.InitBandwidthPeakFactor(config.BandwidthPeakFactor);
      this.InitWebSocketReceiveBufferMaxSize(config.WebSocketReceiveBufferMaxSize);
      this.InitUdpSocketReceiveBufferMaxSize(config.UdpSocketReceiveBufferMaxSize);
      if (config.SSLCertFilePath != null)
      {
        int num = this.InitSSLCertFilePath(config.SSLCertFilePath);
        if (num != 0)
          throw new ArgumentOutOfRangeException("SSLCertFilePath cannot be > than " + num.ToString());
      }
      if (config.SSLPrivateKeyFilePath != null)
      {
        int num = this.InitSSLPrivateKeyFilePath(config.SSLPrivateKeyFilePath);
        if (num != 0)
          throw new ArgumentOutOfRangeException("SSLPrivateKeyFilePath cannot be > than " + num.ToString());
      }
      if (config.SSLCAFilePath != null)
      {
        int num = this.InitSSLCAFilePath(config.SSLCAFilePath);
        if (num != 0)
          throw new ArgumentOutOfRangeException("SSLCAFilePath cannot be > than " + num.ToString());
      }
      for (byte idx = 0; (int) idx < config.ChannelCount; ++idx)
      {
        int num = (int) this.AddChannel(config.GetChannel(idx));
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitWrapper();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern byte AddChannel(QosType value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern QosType GetChannel(int i);

    public extern int ChannelSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitPacketSize(ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitFragmentSize(ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitResendTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitDisconnectTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitConnectTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMinUpdateTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitPingTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitReducedPingTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitAllCostTimeout(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitNetworkDropThreshold(byte value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitOverflowDropThreshold(byte value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxConnectionAttempt(byte value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitAckDelay(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitSendDelay(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxCombinedReliableMessageSize(ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxCombinedReliableMessageCount(ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxSentMessageQueueSize(ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitAcksType(int value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitUsePlatformSpecificProtocols(bool value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitInitialBandwidth(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitBandwidthPeakFactor(float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitWebSocketReceiveBufferMaxSize(ushort value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitUdpSocketReceiveBufferMaxSize(uint value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int InitSSLCertFilePath(string value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int InitSSLPrivateKeyFilePath(string value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int InitSSLCAFilePath(string value);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~ConnectionConfigInternal()
    {
      this.Dispose();
    }
  }
}
