// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.GlobalConfigInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
  internal sealed class GlobalConfigInternal : IDisposable
  {
    internal IntPtr m_Ptr;

    public GlobalConfigInternal(GlobalConfig config)
    {
      this.InitWrapper();
      this.InitThreadAwakeTimeout(config.ThreadAwakeTimeout);
      this.InitReactorModel((byte) config.ReactorModel);
      this.InitReactorMaximumReceivedMessages(config.ReactorMaximumReceivedMessages);
      this.InitReactorMaximumSentMessages(config.ReactorMaximumSentMessages);
      this.InitMaxPacketSize(config.MaxPacketSize);
      this.InitMaxHosts(config.MaxHosts);
      if ((int) config.ThreadPoolSize == 0 || (int) config.ThreadPoolSize > 254)
        throw new ArgumentOutOfRangeException("Worker thread pool size should be >= 1 && < 254 (for server only)");
      this.InitThreadPoolSize(config.ThreadPoolSize);
      this.InitMinTimerTimeout(config.MinTimerTimeout);
      this.InitMaxTimerTimeout(config.MaxTimerTimeout);
      this.InitMinNetSimulatorTimeout(config.MinNetSimulatorTimeout);
      this.InitMaxNetSimulatorTimeout(config.MaxNetSimulatorTimeout);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitWrapper();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitThreadAwakeTimeout(uint ms);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitReactorModel(byte model);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitReactorMaximumReceivedMessages(ushort size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitReactorMaximumSentMessages(ushort size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxPacketSize(ushort size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxHosts(ushort size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitThreadPoolSize(byte size);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMinTimerTimeout(uint ms);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxTimerTimeout(uint ms);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMinNetSimulatorTimeout(uint ms);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void InitMaxNetSimulatorTimeout(uint ms);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~GlobalConfigInternal()
    {
      this.Dispose();
    }
  }
}
