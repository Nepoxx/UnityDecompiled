// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IVisualElementScheduler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public interface IVisualElementScheduler
  {
    IVisualElementScheduledItem Execute(Action<TimerState> timerUpdateEvent);

    /// <summary>
    ///   <para>Schedule this action to be executed later.</para>
    /// </summary>
    /// <param name="timerUpdateEvent">The action to be executed.</param>
    /// <param name="updateEvent">The action to be executed.</param>
    /// <returns>
    ///   <para>Reference to the scheduled action.</para>
    /// </returns>
    IVisualElementScheduledItem Execute(Action updateEvent);
  }
}
