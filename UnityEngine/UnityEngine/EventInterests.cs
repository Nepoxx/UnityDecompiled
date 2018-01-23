// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventInterests
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct EventInterests
  {
    public bool wantsMouseMove { get; set; }

    public bool wantsMouseEnterLeaveWindow { get; set; }

    public bool WantsEvent(EventType type)
    {
      switch (type)
      {
        case EventType.MouseMove:
          return this.wantsMouseMove;
        case EventType.MouseEnterWindow:
        case EventType.MouseLeaveWindow:
          return this.wantsMouseEnterLeaveWindow;
        default:
          return true;
      }
    }
  }
}
