// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.GlobalConfig
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Defines global paramters for network library.</para>
  /// </summary>
  [Serializable]
  public class GlobalConfig
  {
    private const uint g_MaxTimerTimeout = 12000;
    private const uint g_MaxNetSimulatorTimeout = 12000;
    private const ushort g_MaxHosts = 128;
    [SerializeField]
    private uint m_ThreadAwakeTimeout;
    [SerializeField]
    private ReactorModel m_ReactorModel;
    [SerializeField]
    private ushort m_ReactorMaximumReceivedMessages;
    [SerializeField]
    private ushort m_ReactorMaximumSentMessages;
    [SerializeField]
    private ushort m_MaxPacketSize;
    [SerializeField]
    private ushort m_MaxHosts;
    [SerializeField]
    private byte m_ThreadPoolSize;
    [SerializeField]
    private uint m_MinTimerTimeout;
    [SerializeField]
    private uint m_MaxTimerTimeout;
    [SerializeField]
    private uint m_MinNetSimulatorTimeout;
    [SerializeField]
    private uint m_MaxNetSimulatorTimeout;

    /// <summary>
    ///   <para>Create new global config object.</para>
    /// </summary>
    public GlobalConfig()
    {
      this.m_ThreadAwakeTimeout = 1U;
      this.m_ReactorModel = ReactorModel.SelectReactor;
      this.m_ReactorMaximumReceivedMessages = (ushort) 1024;
      this.m_ReactorMaximumSentMessages = (ushort) 1024;
      this.m_MaxPacketSize = (ushort) 2000;
      this.m_MaxHosts = (ushort) 16;
      this.m_ThreadPoolSize = (byte) 1;
      this.m_MinTimerTimeout = 1U;
      this.m_MaxTimerTimeout = 12000U;
      this.m_MinNetSimulatorTimeout = 1U;
      this.m_MaxNetSimulatorTimeout = 12000U;
    }

    /// <summary>
    ///   <para>Defines (1) for select reactor, minimum time period, when system will check if there are any messages for send (2) for fixrate reactor, minimum interval of time, when system will check for sending and receiving messages.</para>
    /// </summary>
    public uint ThreadAwakeTimeout
    {
      get
      {
        return this.m_ThreadAwakeTimeout;
      }
      set
      {
        if ((int) value == 0)
          throw new ArgumentOutOfRangeException("Minimal thread awake timeout should be > 0");
        this.m_ThreadAwakeTimeout = value;
      }
    }

    /// <summary>
    ///   <para>Defines reactor model for the network library.</para>
    /// </summary>
    public ReactorModel ReactorModel
    {
      get
      {
        return this.m_ReactorModel;
      }
      set
      {
        this.m_ReactorModel = value;
      }
    }

    /// <summary>
    ///   <para>This property determines the initial size of the queue that holds messages received by Unity Multiplayer before they are processed.</para>
    /// </summary>
    public ushort ReactorMaximumReceivedMessages
    {
      get
      {
        return this.m_ReactorMaximumReceivedMessages;
      }
      set
      {
        this.m_ReactorMaximumReceivedMessages = value;
      }
    }

    /// <summary>
    ///   <para>Defines the initial size of the send queue. Messages are placed in this queue ready to be sent in packets to their destination.</para>
    /// </summary>
    public ushort ReactorMaximumSentMessages
    {
      get
      {
        return this.m_ReactorMaximumSentMessages;
      }
      set
      {
        this.m_ReactorMaximumSentMessages = value;
      }
    }

    /// <summary>
    ///   <para>Defines maximum possible packet size in bytes for all network connections.</para>
    /// </summary>
    public ushort MaxPacketSize
    {
      get
      {
        return this.m_MaxPacketSize;
      }
      set
      {
        this.m_MaxPacketSize = value;
      }
    }

    /// <summary>
    ///   <para>Defines how many hosts you can use. Default Value = 16. Max value = 128.</para>
    /// </summary>
    public ushort MaxHosts
    {
      get
      {
        return this.m_MaxHosts;
      }
      set
      {
        if ((int) value == 0)
          throw new ArgumentOutOfRangeException(nameof (MaxHosts), "Maximum hosts number should be > 0");
        if ((int) value > 128)
          throw new ArgumentOutOfRangeException(nameof (MaxHosts), "Maximum hosts number should be <= " + (ushort) 128.ToString());
        this.m_MaxHosts = value;
      }
    }

    /// <summary>
    ///   <para>Defines how many worker threads are available to handle incoming and outgoing messages.</para>
    /// </summary>
    public byte ThreadPoolSize
    {
      get
      {
        return this.m_ThreadPoolSize;
      }
      set
      {
        this.m_ThreadPoolSize = value;
      }
    }

    /// <summary>
    ///   <para>Defines the minimum timeout in milliseconds recognised by the system. The default value is 1 ms.</para>
    /// </summary>
    public uint MinTimerTimeout
    {
      get
      {
        return this.m_MinTimerTimeout;
      }
      set
      {
        if (value > this.MaxTimerTimeout)
          throw new ArgumentOutOfRangeException("MinTimerTimeout should be < MaxTimerTimeout");
        if ((int) value == 0)
          throw new ArgumentOutOfRangeException("MinTimerTimeout should be > 0");
        this.m_MinTimerTimeout = value;
      }
    }

    /// <summary>
    ///   <para>Defines the maximum timeout in milliseconds for any configuration. The default value is 12 seconds (12000ms).</para>
    /// </summary>
    public uint MaxTimerTimeout
    {
      get
      {
        return this.m_MaxTimerTimeout;
      }
      set
      {
        if ((int) value == 0)
          throw new ArgumentOutOfRangeException("MaxTimerTimeout should be > 0");
        if (value > 12000U)
          throw new ArgumentOutOfRangeException("MaxTimerTimeout should be <=" + 12000U.ToString());
        this.m_MaxTimerTimeout = value;
      }
    }

    /// <summary>
    ///   <para>Deprecated. Defines the minimal timeout for network simulator. You cannot set up any delay less than this value. See Also: MinTimerTimeout.</para>
    /// </summary>
    public uint MinNetSimulatorTimeout
    {
      get
      {
        return this.m_MinNetSimulatorTimeout;
      }
      set
      {
        if (value > this.MaxNetSimulatorTimeout)
          throw new ArgumentOutOfRangeException("MinNetSimulatorTimeout should be < MaxTimerTimeout");
        if ((int) value == 0)
          throw new ArgumentOutOfRangeException("MinNetSimulatorTimeout should be > 0");
        this.m_MinNetSimulatorTimeout = value;
      }
    }

    /// <summary>
    ///   <para>Deprecated. Defines maximum delay for network simulator. See Also: MaxTimerTimeout.</para>
    /// </summary>
    public uint MaxNetSimulatorTimeout
    {
      get
      {
        return this.m_MaxNetSimulatorTimeout;
      }
      set
      {
        if ((int) value == 0)
          throw new ArgumentOutOfRangeException("MaxNetSimulatorTimeout should be > 0");
        if (value > 12000U)
          throw new ArgumentOutOfRangeException("MaxNetSimulatorTimeout should be <=" + 12000U.ToString());
        this.m_MaxNetSimulatorTimeout = value;
      }
    }
  }
}
