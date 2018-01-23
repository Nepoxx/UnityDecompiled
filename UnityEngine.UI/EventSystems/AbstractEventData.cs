// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.AbstractEventData
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>A class that can be used for sending simple events via the event system.</para>
  /// </summary>
  public abstract class AbstractEventData
  {
    protected bool m_Used;

    /// <summary>
    ///   <para>Reset the event.</para>
    /// </summary>
    public virtual void Reset()
    {
      this.m_Used = false;
    }

    /// <summary>
    ///   <para>Use the event.</para>
    /// </summary>
    public virtual void Use()
    {
      this.m_Used = true;
    }

    /// <summary>
    ///   <para>Is the event used?</para>
    /// </summary>
    public virtual bool used
    {
      get
      {
        return this.m_Used;
      }
    }
  }
}
