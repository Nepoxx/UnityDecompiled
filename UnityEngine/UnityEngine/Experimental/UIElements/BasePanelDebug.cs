// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.BasePanelDebug
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public abstract class BasePanelDebug
  {
    internal bool enabled { get; set; }

    internal virtual bool RecordRepaint(VisualElement visualElement)
    {
      return false;
    }

    internal virtual bool EndRepaint()
    {
      return false;
    }

    internal Func<Event, bool> interceptEvents { get; set; }
  }
}
