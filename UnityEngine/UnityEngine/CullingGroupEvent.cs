// Decompiled with JetBrains decompiler
// Type: UnityEngine.CullingGroupEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Provides information about the current and previous states of one sphere in a CullingGroup.</para>
  /// </summary>
  public struct CullingGroupEvent
  {
    private int m_Index;
    private byte m_PrevState;
    private byte m_ThisState;
    private const byte kIsVisibleMask = 128;
    private const byte kDistanceMask = 127;

    /// <summary>
    ///   <para>The index of the sphere that has changed.</para>
    /// </summary>
    public int index
    {
      get
      {
        return this.m_Index;
      }
    }

    /// <summary>
    ///   <para>Was the sphere considered visible by the most recent culling pass?</para>
    /// </summary>
    public bool isVisible
    {
      get
      {
        return ((int) this.m_ThisState & 128) != 0;
      }
    }

    /// <summary>
    ///   <para>Was the sphere visible before the most recent culling pass?</para>
    /// </summary>
    public bool wasVisible
    {
      get
      {
        return ((int) this.m_PrevState & 128) != 0;
      }
    }

    /// <summary>
    ///   <para>Did this sphere change from being invisible to being visible in the most recent culling pass?</para>
    /// </summary>
    public bool hasBecomeVisible
    {
      get
      {
        return this.isVisible && !this.wasVisible;
      }
    }

    /// <summary>
    ///   <para>Did this sphere change from being visible to being invisible in the most recent culling pass?</para>
    /// </summary>
    public bool hasBecomeInvisible
    {
      get
      {
        return !this.isVisible && this.wasVisible;
      }
    }

    /// <summary>
    ///   <para>The current distance band index of the sphere, after the most recent culling pass.</para>
    /// </summary>
    public int currentDistance
    {
      get
      {
        return (int) this.m_ThisState & (int) sbyte.MaxValue;
      }
    }

    /// <summary>
    ///   <para>The distance band index of the sphere before the most recent culling pass.</para>
    /// </summary>
    public int previousDistance
    {
      get
      {
        return (int) this.m_PrevState & (int) sbyte.MaxValue;
      }
    }
  }
}
