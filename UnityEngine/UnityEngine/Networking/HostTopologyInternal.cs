// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.HostTopologyInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
  internal sealed class HostTopologyInternal : IDisposable
  {
    internal IntPtr m_Ptr;

    public HostTopologyInternal(HostTopology topology)
    {
      this.InitWrapper(new ConnectionConfigInternal(topology.DefaultConfig), topology.MaxDefaultConnections);
      for (int i = 1; i <= topology.SpecialConnectionConfigsCount; ++i)
        this.AddSpecialConnectionConfig(new ConnectionConfigInternal(topology.GetSpecialConnectionConfig(i)));
      this.InitOtherParameters(topology);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitWrapper(ConnectionConfigInternal config, int maxDefaultConnections);

    private int AddSpecialConnectionConfig(ConnectionConfigInternal config)
    {
      return this.AddSpecialConnectionConfigWrapper(config);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int AddSpecialConnectionConfigWrapper(ConnectionConfigInternal config);

    private void InitOtherParameters(HostTopology topology)
    {
      this.InitReceivedPoolSize(topology.ReceivedMessagePoolSize);
      this.InitSentMessagePoolSize(topology.SentMessagePoolSize);
      this.InitMessagePoolSizeGrowthFactor(topology.MessagePoolSizeGrowthFactor);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitReceivedPoolSize(ushort pool);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitSentMessagePoolSize(ushort pool);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMessagePoolSizeGrowthFactor(float factor);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~HostTopologyInternal()
    {
      this.Dispose();
    }
  }
}
