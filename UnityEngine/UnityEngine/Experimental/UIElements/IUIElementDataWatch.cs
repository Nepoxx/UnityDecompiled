// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IUIElementDataWatch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public interface IUIElementDataWatch
  {
    IUIElementDataWatchRequest RegisterWatch(UnityEngine.Object toWatch, Action<UnityEngine.Object> watchNotification);

    /// <summary>
    ///   <para>Unregisters a previously watched request.</para>
    /// </summary>
    /// <param name="requested">The registered request.</param>
    void UnregisterWatch(IUIElementDataWatchRequest requested);
  }
}
